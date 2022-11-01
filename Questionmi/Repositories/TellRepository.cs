using Microsoft.EntityFrameworkCore;
using Questionmi.DTOs;
using Questionmi.Filters;
using Questionmi.Helpers.Exceptions;
using Questionmi.Models;
using SellpanderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Questionmi.Repositories
{
    public class TellRepository : ITellRepository
    {
        private readonly DatabaseContext _context;

        public TellRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Tell>> Get(PaginationParams paginationParams, TellsFilter tellsFilter)
        {
            var queryableTells = _context.Tells.AsQueryable();

            if (tellsFilter == null)
            {
                queryableTells = queryableTells
                    .OrderBy(t => t.CreatedAt);
            }

            if (tellsFilter?.Id != null)
            {
                queryableTells = queryableTells
                    .Where(t => t.Id == tellsFilter.Id);
            }

            if (tellsFilter?.Date != null)
            {
                queryableTells = queryableTells
                    .Where(t => t.CreatedAt == tellsFilter.Date);
            }

            if (!string.IsNullOrEmpty(tellsFilter?.Ip))
            {
                queryableTells = queryableTells
                    .Where(t => t.UsersIP == tellsFilter.Ip);
            }

            if (!string.IsNullOrEmpty(tellsFilter?.Text))
            {
                queryableTells = queryableTells
                    .Where(t => t.Text == tellsFilter.Text);
            }

            if (tellsFilter?.IsPosted != null)
            {
                queryableTells = queryableTells
                    .Where(t => t.IsPosted == tellsFilter.IsPosted);
            }

            if(tellsFilter?.IsWaitingForAccept != null)
            {
                queryableTells = queryableTells
                    .Where(t => t.IsWaitingForAccept == tellsFilter.IsWaitingForAccept);
            }

            return await queryableTells
                .Skip((paginationParams.Page - 1) * paginationParams.ItemsPerPage)
                .Take(paginationParams.ItemsPerPage)
                .Select(t => new Tell
                {
                    Id = t.Id,
                    CreatedAt = t.CreatedAt,
                    UsersIP = t.UsersIP,
                    Text = t.Text,
                    IsPosted = t.IsPosted,
                    IsWaitingForAccept = t.IsWaitingForAccept
                }).ToListAsync();
        }

        public async Task<List<Tell>> GetForPost()
        {
            var tells =  await _context.Tells
                .Where(x => !x.IsPosted && !x.IsWaitingForAccept)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            tells.AsParallel().ForAll(x => x.IsPosted = true);
            _context.Tells.UpdateRange(tells);
            _context.SaveChanges();

            return tells;
        }

        public async Task<int> Create(string userIp, TellDto tell)
        {
            if (tell.Text.Length <= 5)
                throw new IncorrectDataException("Tell must have more than 5 characters.");

            if (tell.Text.Length > 1000)
                throw new IncorrectDataException("Tell can't have more than 1000 characters.");

            if (!Regex.IsMatch(tell.Text, "^[a-zA-z0-9 ĄąĘęĆćŁłŃńŚśÓóŹźŻż,.!#$%&*()-={}\"\'|?>]+$"))
                throw new IncorrectDataException("Tell has illegall characters.");

            var similiarTellPosted = _context.Tells
                .Any(t => t.Text.ToLower().Replace(" ", "") == tell.Text.ToLower().Replace(" ", ""));

            if (similiarTellPosted)
                throw new IncorrectDataException("Similar tell was posted.");

            var mappedTell = new Tell
            {
                CreatedAt = DateTime.Now,
                UsersIP = userIp,
                Text = tell.Text
            };

            var badWords = await _context.BadWords.Select(x => x.Words).ToListAsync();
            var badWordsRegex = new Regex(@"\b(" + string.Join("|", badWords.Select(Regex.Escape).ToArray()) + @"\b)");

            if (Regex.IsMatch(tell.Text, badWordsRegex.ToString()))
                mappedTell.IsWaitingForAccept = true;

            _context.Tells.Add(mappedTell);
            await _context.SaveChangesAsync();

            return mappedTell.Id;
        }

        public async Task Update(Tell tell)
        {
            var tellInDb = _context.Tells
                .Where(t => t.Id == tell.Id)
                .FirstOrDefault();

            if (tellInDb == null)
                throw new NotFoundException($"Tell with id {tell.Id} not found");

            _context.Entry(tell).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tellToDelete = await _context.Tells.FindAsync(id);
            _context.Tells.Remove(tellToDelete);
            await _context.SaveChangesAsync();
        }
    }
}

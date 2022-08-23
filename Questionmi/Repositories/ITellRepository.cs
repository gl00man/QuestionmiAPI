using Questionmi.DTOs;
using Questionmi.Filters;
using Questionmi.Models;
using SellpanderAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questionmi.Repositories
{
    public interface ITellRepository
    {
        Task<List<Tell>> Get(PaginationParams paginationParams, TellsFilter tellsFilter);
        Task<List<Tell>> GetForPost();
        Task<int> Create(string userIp, TellDto tell);
        Task Update(Tell tell);
        Task Delete(int id);
    }
}

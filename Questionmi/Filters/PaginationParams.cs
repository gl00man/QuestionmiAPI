using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SellpanderAPI.Models
{
    public class PaginationParams
    {
        private const int _maxItemsPerPage = 100;
        private int itemsPerPage;
        [FromQuery(Name = "page_id"), Required(ErrorMessage = "page_id required.")]
        public int Page { get; set; }
        [FromQuery(Name = "records_per_page"), Required(ErrorMessage = "records_per_page required.")]
        public int ItemsPerPage
        {
            get => itemsPerPage;
            set => itemsPerPage = value > _maxItemsPerPage ? _maxItemsPerPage : value;
        }
    }
}

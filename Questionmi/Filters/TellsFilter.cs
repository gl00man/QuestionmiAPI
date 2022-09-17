using Microsoft.AspNetCore.Mvc;
using System;

namespace Questionmi.Filters
{
    public class TellsFilter
    {
        [FromQuery(Name = "id")]
        public int? Id { get; set; }
        [FromQuery(Name = "date")]
        public DateTime? Date { get; set; }
        [FromQuery(Name = "ip")]
        public string Ip { get; set; }
        [FromQuery(Name = "text")]
        public string Text { get; set; }
        [FromQuery(Name = "is_posted")]
        public bool? IsPosted { get; set; }
        [FromQuery(Name = "is_waiting_for_accept")]
        public bool? IsWaitingForAccept { get; set; }
    }
}

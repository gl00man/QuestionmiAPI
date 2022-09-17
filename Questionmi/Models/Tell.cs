using System;
using System.Text.Json.Serialization;

namespace Questionmi.Models
{
    public class Tell
    {
        public int Id { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("users_ip")]
        public string UsersIP { get; set; }
        public string Text { get; set; }
        public bool IsPosted { get; set; }
        public bool IsWaitingForAccept { get; set; }
    }
}

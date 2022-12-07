using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class EventVoteData
    {
        
        public string? Name { get; set; }
        [Required]
        public string Emoji { get; set; }
        [Required]
        public List<string> DiscordUserId { get; set; }
    }
}

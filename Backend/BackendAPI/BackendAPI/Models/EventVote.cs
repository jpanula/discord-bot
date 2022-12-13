using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class EventVote : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string Emoji { get; set; }
        [Required]
        public List<string> DiscordUserIds { get; set; }
    }
}

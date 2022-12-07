using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public HashSet<EventVote> Votes { get; set; }
    }
}

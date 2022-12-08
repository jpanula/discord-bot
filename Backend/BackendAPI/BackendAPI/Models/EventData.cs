using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class EventData
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string MessageId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class SubCommand : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ExampleText { get; set; }
        public string? ExampleMediaUrl { get; set; }
        public ICollection<CommandParameter>? Parameters { get; set; }
    }
}

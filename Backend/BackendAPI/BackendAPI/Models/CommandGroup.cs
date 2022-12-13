using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class CommandGroup : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public bool IsConfigurable { get; set; }
        public ICollection<SubCommand>? SubCommands { get; set; }

    }
}

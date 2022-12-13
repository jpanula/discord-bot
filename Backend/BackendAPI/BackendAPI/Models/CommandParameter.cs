using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class CommandParameter : IEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}

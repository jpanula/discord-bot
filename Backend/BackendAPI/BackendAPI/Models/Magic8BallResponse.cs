using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Magic8BallResponse : IEntity
    {
        public enum AnswerType
        {
            Affirmative,
            Negative,
            Noncommittal
        }
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        [Range(0, 2, ErrorMessage = "Type value must be between 0 and 2")]
        public AnswerType Type { get; set; }
    }
}

using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    [Table("quiz")]
    public class Quiz
    {
        [Key]
        [Required]
        public int QuizId { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = string.Empty;


        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}

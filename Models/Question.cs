using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    [Table("question")]
    public class Question
    {
        [Key]
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int QuizId { get; set; }

        [StringLength(50)]
        public string Text { get; set; } = string.Empty;

        public bool Answer { get; set; }

        public virtual Quiz? Quiz { get; set; }
    }
}

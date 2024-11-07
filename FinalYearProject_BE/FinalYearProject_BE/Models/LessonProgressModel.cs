using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalYearProject_BE.Models
{
    [Table("LessonProgress")]
    public class LessonProgressModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserModel User { get; set; }

        [Required]
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public LessonModel Lesson { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
    }
}

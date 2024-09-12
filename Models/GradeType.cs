using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentGradesApp.Models
{
    public class GradeType
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Id 1 veya 2 olmalıdır.")]
        public int ExamType { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Ağırlık 0 ile 100 arasında olmalıdır.")]
        public int Weight { get; set; }

    }
}

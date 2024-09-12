using System.ComponentModel.DataAnnotations;

namespace StudentGradesApp.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}

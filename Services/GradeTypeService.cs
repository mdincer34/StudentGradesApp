using StudentGradesApp.Models;

namespace StudentGradesApp.Services
{
    public class GradeTypeService : IGradeTypeService
    {
        public int VIZE => 1;
        public int FINAL => 2;

        public string GetNameById(int id)
        {
            return id switch
            {
                1 => "VIZE",
                2 => "FINAL",
                _ => "Bilinmeyen"
            };
        }

        public List<GradeType> GetDefaultGradeTypes()
        {
            return new List<GradeType>
            {
                new GradeType { ExamType = 1, Name = "VIZE", Weight = 40 },
                new GradeType { ExamType = 2, Name = "FINAL", Weight = 60 }
            };
        }
    }
}

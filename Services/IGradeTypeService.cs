using StudentGradesApp.Models;

namespace StudentGradesApp.Services
{
    public interface IGradeTypeService
    {
        int VIZE { get; }
        int FINAL { get; }
        string GetNameById(int id);
        List<GradeType> GetDefaultGradeTypes();
        
    }
}

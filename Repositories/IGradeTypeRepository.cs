using StudentGradesApp.Models;

namespace StudentGradesApp.Repositories
{
    public interface IGradeTypeRepository
    {
        Task<IEnumerable<GradeType>> GetAllGradeTypesAsync();
        Task<GradeType?> GetGradeTypeByIdAsync(int id);
        Task UpdateGradeTypeAsync(GradeType gradeType);
        Task EnsureDefaultGradeTypesExist();
    }
}

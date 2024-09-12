using StudentGradesApp.Models;

namespace StudentGradesApp.Repositories
{
public interface IGradeRepository
{
    Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);
    Task<Grade> GetGradeAsync(int studentId, int gradeTypeId);
    Task<bool> AddOrUpdateGradeAsync(Grade grade);
    Task DeleteGradeAsync(int studentId, int gradeTypeId);
}
}

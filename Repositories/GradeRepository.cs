using Microsoft.EntityFrameworkCore;
using StudentGradesApp.Data;
using StudentGradesApp.Models;

namespace StudentGradesApp.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentId == studentId)
                .Include(g => g.GradeType)
                .ToListAsync();
        }

        public async Task<Grade?> GetGradeAsync(int studentId, int gradeTypeId)
        {
            var grade = await _context.Grades
                .Include(g => g.GradeType)
                .FirstOrDefaultAsync(g => g.StudentId == studentId && g.GradeTypeId == gradeTypeId);
            return grade;
        }
        public async Task<bool> AddOrUpdateGradeAsync(Grade grade)
        {
            var existingGrade = await _context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == grade.StudentId && g.GradeTypeId == grade.GradeTypeId);
            
            if (existingGrade != null)
            {
                existingGrade.Score = grade.Score;
                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task DeleteGradeAsync(int studentId, int gradeTypeId)
        {
            var grade = await _context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == studentId && g.GradeTypeId == gradeTypeId);

            if (grade != null)
            {
                _context.Grades.Remove(grade);
                await _context.SaveChangesAsync();
            }
        }
    }
}

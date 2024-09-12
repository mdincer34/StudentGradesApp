using Microsoft.EntityFrameworkCore;
using StudentGradesApp.Data;
using StudentGradesApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGradesApp.Repositories
{
    public class FinalResultRepository : IFinalResultRepository
    {
        private readonly ApplicationDbContext _context;

        public FinalResultRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveFinalResultsAsync(List<FinalResult> finalResults)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.FinalResults.RemoveRange(_context.FinalResults);
                    await _context.FinalResults.AddRangeAsync(finalResults);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<FinalResult>> GetAllFinalResultsAsync()
        {
            return await _context.FinalResults.ToListAsync();
        }

        public async Task DeleteFinalResultAsync(FinalResult finalResult)
        {
            _context.FinalResults.Remove(finalResult);
            await _context.SaveChangesAsync();
        }
        public async Task<FinalResult> GetFinalResultByStudentIdAsync(int studentId)
        {
            return await _context.FinalResults.FirstOrDefaultAsync(fr => fr.StudentId == studentId);
        }
    }
}

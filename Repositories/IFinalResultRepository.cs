using System.Collections.Generic;
using System.Threading.Tasks;
using StudentGradesApp.Models;

namespace StudentGradesApp.Repositories
{
    public interface IFinalResultRepository
    {
        Task SaveFinalResultsAsync(List<FinalResult> finalResults);
        Task<List<FinalResult>> GetAllFinalResultsAsync();
        Task<FinalResult> GetFinalResultByStudentIdAsync(int studentId);
        Task DeleteFinalResultAsync(FinalResult finalResult);
        
    }
}

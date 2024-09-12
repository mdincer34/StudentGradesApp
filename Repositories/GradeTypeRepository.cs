using Microsoft.EntityFrameworkCore;
using StudentGradesApp.Data;
using StudentGradesApp.Models;
using StudentGradesApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGradesApp.Repositories
{
    public class GradeTypeRepository : IGradeTypeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IGradeTypeService _gradeTypeService;

        public GradeTypeRepository(ApplicationDbContext context, IGradeTypeService gradeTypeService)
        {
            _context = context;
            _gradeTypeService = gradeTypeService;
        }

        public async Task<IEnumerable<GradeType>> GetAllGradeTypesAsync()
        {
            return await _context.GradeTypes.ToListAsync();
        }

        public async Task<GradeType?> GetGradeTypeByIdAsync(int id)
        {
            return await _context.GradeTypes.FindAsync(id);
        }

        public async Task UpdateGradeTypeAsync(GradeType gradeType)
        {
            _context.GradeTypes.Update(gradeType);
            await _context.SaveChangesAsync();
        }

        public async Task EnsureDefaultGradeTypesExist()
        {
            var requiredGradeTypes = _gradeTypeService.GetDefaultGradeTypes();
            foreach (var gradeType in requiredGradeTypes)
            {
                var existingType = await _context.GradeTypes.FindAsync(gradeType.ExamType);
                if (existingType == null)
                {
                    _context.GradeTypes.Add(gradeType);
                }

                else if (existingType.Name != gradeType.Name)
                {
                    existingType.Name = gradeType.Name;
                    _context.GradeTypes.Update(existingType);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StudentGradesApp.Models;
using StudentGradesApp.Repositories;
using StudentGradesApp.Services;
using System.Threading.Tasks;

namespace StudentGradesApp.Controllers
{
    public class GradeController : BaseController
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IGradeTypeService _gradeTypeService;

        public GradeController(IGradeRepository gradeRepository, IGradeTypeService gradeTypeService)
        {
            _gradeRepository = gradeRepository;
            _gradeTypeService = gradeTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> AddOrUpdateGrade(int studentId, int? gradeTypeId = null)
        {
            var grade = await _gradeRepository.GetGradeAsync(studentId, gradeTypeId ?? 0);
            if (grade == null)
            {
                grade = new Grade { StudentId = studentId };
            }

            var existingGrades = await _gradeRepository.GetGradesByStudentIdAsync(studentId);
            var availableGradeTypes = _gradeTypeService.GetDefaultGradeTypes()
                .Where(gt => !existingGrades.Any(g => g.GradeTypeId == gt.ExamType))
                .ToList();

            ViewBag.AvailableGradeTypes = availableGradeTypes;

            return PartialView("_AddOrUpdateGradePartial", grade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdateGrade(Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz veri", errors = GetModelErrors() });
            }

            return await ExecuteRepositoryAction(async () =>
            {
                var existingGrade = await _gradeRepository.GetGradeAsync(grade.StudentId, grade.GradeTypeId);
                if (existingGrade != null)
                {
                    existingGrade.Score = grade.Score;
                    await _gradeRepository.AddOrUpdateGradeAsync(existingGrade);
                }
                else
                {
                    await _gradeRepository.AddOrUpdateGradeAsync(grade);
                }
                string message = existingGrade == null ? "Not başarıyla eklendi." : "Not başarıyla güncellendi.";
                return Json(new { success = true, message = message });
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGrade(int studentId, int gradeTypeId)
        {
            return await ExecuteRepositoryAction(async () =>
            {
                await _gradeRepository.DeleteGradeAsync(studentId, gradeTypeId);
                return Json(new { success = true, message = "Not başarıyla silindi." });
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditGrade(int studentId, int gradeTypeId)
        {
            var grade = await _gradeRepository.GetGradeAsync(studentId, gradeTypeId);
            if (grade == null)
            {
                return NotFound();
            }
            ViewBag.GradeTypes = _gradeTypeService.GetDefaultGradeTypes();
            return PartialView("_EditGradePartial", grade);
        }
    }
}
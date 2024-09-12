using Microsoft.AspNetCore.Mvc;
using StudentGradesApp.Models;
using StudentGradesApp.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentGradesApp.Services;

namespace StudentGradesApp.Controllers
{
    public class SemesterController : BaseController
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly IFinalResultRepository _finalResultRepository;
        private readonly IPassingGradeService _passingGradeService;
        private readonly IGradeTypeService _gradeTypeService;

        public SemesterController(
            IStudentRepository studentRepository,
            IGradeRepository gradeRepository,
            IFinalResultRepository finalResultRepository,
            IPassingGradeService passingGradeService,
            IGradeTypeService gradeTypeService)
        {
            _studentRepository = studentRepository;
            _gradeRepository = gradeRepository;
            _finalResultRepository = finalResultRepository;
            _passingGradeService = passingGradeService;
            _gradeTypeService = gradeTypeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseSemester()
        {
            return await ExecuteRepositoryAction(async () =>
            {
                var students = await _studentRepository.GetAllStudentsAsync();
                var finalResults = new List<FinalResult>();

                foreach (var student in students)
                {
                    var vizeNotu = await _gradeRepository.GetGradeAsync(student.StudentId, _gradeTypeService.VIZE);
                    var finalNotu = await _gradeRepository.GetGradeAsync(student.StudentId, _gradeTypeService.FINAL);

                    if (vizeNotu?.Score != null && finalNotu?.Score != null)
                    {
                        var gradeTypes = _gradeTypeService.GetDefaultGradeTypes();
                        var vizeWeight = gradeTypes.FirstOrDefault(w => w.ExamType == _gradeTypeService.VIZE)?.Weight ?? 40;
                        var finalWeight = gradeTypes.FirstOrDefault(w => w.ExamType == _gradeTypeService.FINAL)?.Weight ?? 60;

                        var average = ((vizeNotu.Score * vizeWeight) + (finalNotu.Score * finalWeight)) / (vizeWeight + finalWeight);
                        var result = average >= _passingGradeService.GetPassingGrade() ? "Geçti" : "Kaldı";

                        finalResults.Add(new FinalResult
                        {
                            Student = student,
                            StudentId = student.StudentId,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            Result = result
                        });
                    }
                }

                await _finalResultRepository.SaveFinalResultsAsync(finalResults);

                return Json(new { success = true, message = "Dönem başarıyla kapatıldı ve sonuçlar kaydedildi." });
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePassingGrade(int passingGrade)
        {
            _passingGradeService.SetPassingGrade(passingGrade);
            return Json(new { success = true, message = "Geçme notu başarıyla güncellendi.", newPassingGrade = passingGrade });
        }

        [HttpGet]
        public IActionResult GetPassingGrade()
        {
            return Json(new { passingGrade = _passingGradeService.GetPassingGrade() });
        }
    }
}
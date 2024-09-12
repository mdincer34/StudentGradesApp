using Microsoft.AspNetCore.Mvc;
using StudentGradesApp.Models;
using StudentGradesApp.Repositories;
using StudentGradesApp.Services;
using System.Threading.Tasks;

namespace StudentGradesApp.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGradeTypeRepository _gradeTypeRepository;
        private readonly IPassingGradeService _passingGradeService;
        private readonly IGradeTypeService _gradeTypeService;

        public StudentController(IStudentRepository studentRepository, IGradeTypeRepository gradeTypeRepository, IPassingGradeService passingGradeService, IGradeTypeService gradeTypeService)
        {
            _studentRepository = studentRepository;
            _gradeTypeRepository = gradeTypeRepository;
            _passingGradeService = passingGradeService;
            _gradeTypeService = gradeTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await _gradeTypeRepository.EnsureDefaultGradeTypesExist();
            var students = await _studentRepository.GetAllStudentsAsync();
            ViewData["PassingGrade"] = _passingGradeService.GetPassingGrade();
            ViewData["GradeTypes"] = await _gradeTypeRepository.GetAllGradeTypesAsync();
            ViewData["DefaultGradeTypes"] = _gradeTypeService.GetDefaultGradeTypes();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreatePartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName")] Student student)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz veri", errors = GetModelErrors() });
            }

            return await ExecuteRepositoryAction(async () =>
            {
                await _studentRepository.AddStudentAsync(student);
                return Json(new { success = true, message = "Öğrenci başarıyla eklendi." });
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return PartialView("_EditPartial", student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz veri", errors = GetModelErrors() });
            }

            return await ExecuteRepositoryAction(async () =>
            {
                await _studentRepository.UpdateStudentAsync(student);
                return Json(new { success = true, message = "Öğrenci başarıyla güncellendi." });
            });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return PartialView("_DeletePartial", student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await ExecuteRepositoryAction(async () =>
            {
                await _studentRepository.DeleteStudentAsync(id);
                return Json(new { success = true, message = "Öğrenci başarıyla silindi." });
            });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DefaultGradeTypes"] = _gradeTypeService.GetDefaultGradeTypes();
            return PartialView("_DetailsPartial", student);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using StudentGradesApp.Models;
using StudentGradesApp.Repositories;
using System.Threading.Tasks;

namespace StudentGradesApp.Controllers
{
    public class GradeTypeController : BaseController
    {
        private readonly IGradeTypeRepository _gradeTypeRepository;

        public GradeTypeController(IGradeTypeRepository gradeTypeRepository)
        {
            _gradeTypeRepository = gradeTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GradeWeights()
        {
            var gradeTypes = await _gradeTypeRepository.GetAllGradeTypesAsync();
            return PartialView("~/Views/GradeType/_GradeWeightPartial.cshtml", gradeTypes);
        }

        [HttpGet]
        public async Task<IActionResult> EditGradeWeight(int id)
        {
            var gradeType = await _gradeTypeRepository.GetGradeTypeByIdAsync(id);
            if (gradeType == null)
            {
                return NotFound();
            }
            
            var allGradeTypes = await _gradeTypeRepository.GetAllGradeTypesAsync();
            var otherGradeTypesWeight = allGradeTypes.Where(gt => gt.ExamType != id).Sum(gt => gt.Weight);
            
            ViewBag.OtherGradeTypesWeight = otherGradeTypesWeight;
            
            return PartialView("~/Views/GradeType/_EditGradeWeightPartial.cshtml", gradeType);
        }

        [HttpGet]
        public async Task<IActionResult> GetGradeWeights()
        {
            var gradeTypes = await _gradeTypeRepository.GetAllGradeTypesAsync();
            return Json(new { success = true, gradeTypes = gradeTypes });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGradeWeight(int examType, int weight)
        {
            var result = await ExecuteRepositoryAction(async () =>
            {
                var gradeType = await _gradeTypeRepository.GetGradeTypeByIdAsync(examType);
                if (gradeType == null)
                {
                    return Json(new { success = false, message = "Sınav türü bulunamadı." });
                }

                var allGradeTypes = await _gradeTypeRepository.GetAllGradeTypesAsync();
                var totalWeight = allGradeTypes.Where(gt => gt.ExamType != examType).Sum(gt => gt.Weight) + weight;

                if (totalWeight > 100)
                {
                    return Json(new { success = false, message = "Toplam ağırlık 100'ü geçemez." });
                }

                gradeType.Weight = weight;
                await _gradeTypeRepository.UpdateGradeTypeAsync(gradeType);

                var updatedGradeTypes = await _gradeTypeRepository.GetAllGradeTypesAsync();
                var vizeWeight = updatedGradeTypes.FirstOrDefault(gt => gt.Name == "VIZE")?.Weight ?? 40;
                var finalWeight = updatedGradeTypes.FirstOrDefault(gt => gt.Name == "FINAL")?.Weight ?? 60;

                return Json(new { 
                    success = true, 
                    message = "Sınav ağırlığı başarıyla güncellendi.",
                    vizeWeight = vizeWeight,
                    finalWeight = finalWeight
                });
            });

            return result;
        }
    }
}
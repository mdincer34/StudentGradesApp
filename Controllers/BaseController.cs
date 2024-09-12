using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGradesApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected async Task<IActionResult> ExecuteRepositoryAction(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        protected string[] GetModelErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
        }
    }
}
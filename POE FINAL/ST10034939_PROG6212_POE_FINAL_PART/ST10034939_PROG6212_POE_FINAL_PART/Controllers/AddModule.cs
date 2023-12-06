using Microsoft.AspNetCore.Mvc;
using TaskManager.Core;
using ST10034939_PROG6212_POE_FINAL_PART.Models;



namespace ST10034939_PROG6212_POE_FINAL_PART.Controllers
{
    public class AddModuleController : Controller
    {
        [HttpGet]
        public IActionResult AddModule()
        {
            // Usually, you would create a new ViewModel instance or fetch data for the form here
            var model = new ModuleViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddModule(ModuleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new module object
                var module = new Module
                {
                    Code = model.Code,
                    Name = model.Name,
                    Credits = model.Credits,
                    ClassHoursPerWeek = model.ClassHoursPerWeek
                };

                // Calculate self-study hours for the module
                module.SelfStudyHours = module.CalculateSelfStudyHours(model.Weeks);

                // Logic to add module to database

                return RedirectToAction("Index"); // Redirect to the main view
            }

            return View(model); // Return the view with model data if validation fails
        }
    }
}

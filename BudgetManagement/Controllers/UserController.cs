using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManagement.Controllers
{
    public class UserController : Controller
    {
        //Get: Register
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Method to register a new user
        /// </summary>
        /// <param name="model">Model with user´s data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid) {
                return View(model);
            }

            return RedirectToAction("Index", "Deal");
        }
    }
}

using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManagement.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly IAccountTypeRepository _accountTypeRepository;
        private readonly IUserService _userService;

        public AccountTypeController(IAccountTypeRepository accountTypeRepository,
                                     IUserService userService)
        {
            _accountTypeRepository = accountTypeRepository;
            _userService = userService;
        }

        //Get: Index
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            var accountTypes = await _accountTypeRepository.GetByUserId(userId);

            return View(accountTypes);
        }

        //Get: Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Method to create a new Account Type from Form
        /// </summary>
        /// <param name="accountType">model with AccountType data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }

            accountType.UserId = _userService.GetUserId();

            await _accountTypeRepository.Create(accountType);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method that validates if an account type exists by its name for the given user
        /// </summary>
        /// <param name="at_name">Account type name</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AccountTypeExistsValidation(string at_name)
        {
            var userId = _userService.GetUserId();
            var accountTypeExists = await _accountTypeRepository.Exists(at_name, userId);

            if (accountTypeExists)
            {
                return Json($"El nombre {at_name} ya existe");
            }

            return Json(true);
        }
    }
}

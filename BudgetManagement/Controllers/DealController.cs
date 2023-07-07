using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Controllers
{
    public class DealController : Controller
    {
        private readonly IDealRepository _dealRepository;
        private readonly IUserService _userService;
        private readonly IAccountRepository _accountRepository;

        public DealController(
            IDealRepository dealRepository,
            IUserService userService,
            IAccountRepository accountRepository
        )
        {
            _dealRepository = dealRepository;
            _userService = userService;
            _accountRepository = accountRepository;
        }

        //Get: Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _userService.GetUserId();
            var model = new DealCreationViewModel();
            model.Accounts = await GetAccounts(userId);

            return View(model);
        }

        /// <summary>
        /// Method to get all the accounts by the user Id
        /// </summary>
        /// <param name="userId">User´s Id</param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetAccounts(int userId)
        {
            var accounts = await _accountRepository.Search(userId);
            return accounts.Select(x => new SelectListItem(x.A_Name, x.Id.ToString()));
        }
    }
}

using AutoMapper;
using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountTypeRepository _accountTypeRepository;
        private readonly IUserService _userService;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IDealRepository _dealRepository;
        private readonly IReportService _reportService;

        public AccountController(IAccountTypeRepository accountTypeRepository,
                                 IUserService userService, IAccountRepository accountRepository,
                                 IMapper mapper, IDealRepository dealRepository,
                                 IReportService reportService)
        {
            _accountTypeRepository = accountTypeRepository;
            _userService = userService;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _dealRepository = dealRepository;
            _reportService = reportService;
        }

        //Get: Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            var accountsWithAccountType = await _accountRepository.Search(userId);

            var model = accountsWithAccountType
                            .GroupBy(x => x.AccountType)
                            .Select(group => new AccountIndexViewModel
                            {
                                AccountType = group.Key,
                                Accounts = group.AsEnumerable()
                            }).ToList();

            return View(model);
        }

        //Get: Detail
        [HttpGet]
        public async Task<IActionResult> Detail(int id, int month, int year)
        {
            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            ViewBag.Account = account.A_Name;

            var model = await _reportService.GetDetailDealReportByAccount(userId, id, month, year, ViewBag);

            return View(model);
        }

        //Get: Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _userService.GetUserId();

            var model = new AccountCreationViewModel();
            model.AccountTypes = await GetAccountTypes(userId);

            return View(model);
        }

        /// <summary>
        /// Method to create an Account in Database
        /// </summary>
        /// <param name="accountVM">Acount View Model object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(AccountCreationViewModel accountVM)
        {
            var userId = _userService.GetUserId();
            var accountType = await _accountTypeRepository.GetByUserId(userId);

            if (accountType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                accountVM.AccountTypes = await GetAccountTypes(userId);
                return View(accountVM);
            }

            await _accountRepository.Create(accountVM);
            return RedirectToAction("Index");
        }

        //Get: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var model = _mapper.Map<AccountCreationViewModel>(account);

            model.AccountTypes = await GetAccountTypes(userId);

            return View(model);
        }

        /// <summary>
        /// Method to Edit an Account By its Id
        /// </summary>
        /// <param name="accountVM">AccountCreationViewModel object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(AccountCreationViewModel accountVM)
        {
            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(accountVM.Id, userId);

            if(account is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var accountType = await _accountTypeRepository.GetById(accountVM.AccountTypeId, userId);

            if(accountType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _accountRepository.Update(accountVM);

            return RedirectToAction("Index");
        }

        //Get: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            return View(account);
        }

        /// <summary>
        /// Method to delete an Account by its Id
        /// </summary>
        /// <param name="id">Id of the account</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _accountRepository.Delete(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method to get all the account types by user Id As SelectListItems
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountTypes = await _accountTypeRepository.GetByUserId(userId);
            return accountTypes.Select(x => new SelectListItem(x.AT_Name, x.Id.ToString()));
        }
    }
}

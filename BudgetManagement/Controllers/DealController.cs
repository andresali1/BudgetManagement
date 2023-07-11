﻿using AutoMapper;
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
        private readonly IOperationTypeRepository _operationTypeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IReportService _reportService;

        public DealController(
            IDealRepository dealRepository,
            IUserService userService,
            IAccountRepository accountRepository,
            IOperationTypeRepository operationTypeRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IReportService reportService
        )
        {
            _dealRepository = dealRepository;
            _userService = userService;
            _accountRepository = accountRepository;
            _operationTypeRepository = operationTypeRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _reportService = reportService;
        }

        //Get: Index
        [HttpGet]
        public async Task<IActionResult> Index(int month, int year)
        {
            var userId = _userService.GetUserId();

            var model = await _reportService.GetDeateiledDealReport(userId, month, year, ViewBag);

            return View(model);
        }

        //Get: Weekly
        [HttpGet]
        public IActionResult Weekly()
        {
            return View();
        }

        //Get: Monthly
        [HttpGet]
        public IActionResult Monthly()
        {
            return View();
        }

        //Get: ExcelReport
        [HttpGet]
        public IActionResult ExcelReport()
        {
            return View();
        }

        //Get: Calendar
        [HttpGet]
        public IActionResult Calendar()
        {
            return View();
        }

        //Get: Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _userService.GetUserId();
            var model = new DealCreationViewModel();
            model.Accounts = await GetAccounts(userId);
            model.Categories = await GetCategoriesByOperationType(userId, (int)OperationTypeEnum.Ingreso);
            model.OperationTypes = await GetOperationTypes();

            return View(model);
        }

        /// <summary>
        /// Method to save a new DEal in Database
        /// </summary>
        /// <param name="dealVM">DealCreationViewModel object with the data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(DealCreationViewModel dealVM)
        {
            var userId = _userService.GetUserId();

            if (!ModelState.IsValid)
            {
                dealVM.Accounts = await GetAccounts(userId);
                dealVM.OperationTypes = await GetOperationTypes();

                return View(dealVM);
            }

            var account = await _accountRepository.GetById(dealVM.AccountId, userId);

            if (account is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var category = await _categoryRepository.GetById(dealVM.CategoryId, userId);

            if (category is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var operationType = await _operationTypeRepository.GetById(dealVM.OperationTypeId);

            if (operationType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            dealVM.UserId = userId;

            if (dealVM.OperationTypeId == (int)OperationTypeEnum.Gasto)
            {
                dealVM.Price *= -1;
            }

            await _dealRepository.Create(dealVM);

            return RedirectToAction("Index");
        }

        //Get: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            var userId = _userService.GetUserId();
            var deal = await _dealRepository.GetById(id, userId);

            if(deal is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var model = _mapper.Map<DealUpdateViewModel>(deal);

            model.PreviousPrice = model.Price;

            if(model.OperationTypeId == (int)OperationTypeEnum.Gasto)
            {
                model.PreviousPrice = model.Price * -1;
            }

            model.PreviousAccountId = deal.AccountId;
            model.Categories = await GetCategoriesByOperationType(userId, deal.OperationTypeId);
            model.Accounts = await GetAccounts(userId);
            model.OperationTypes = await GetOperationTypes();
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        /// <summary>
        /// Metod to edit a Deal in Database
        /// </summary>
        /// <param name="dealVM">DealUpdateViewModel with given data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(DealUpdateViewModel dealVM)
        {
            var userId = _userService.GetUserId();

            if (!ModelState.IsValid)
            {
                dealVM.Accounts = await GetAccounts(userId);
                dealVM.Categories = await GetCategoriesByOperationType(userId, dealVM.OperationTypeId);
                dealVM.OperationTypes = await GetOperationTypes();

                return View(dealVM);
            }

            var account = await _accountRepository.GetById(dealVM.AccountId, userId);

            if (account is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var category = await _categoryRepository.GetById(dealVM.CategoryId, userId);

            if (category is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var operationType = await _operationTypeRepository.GetById(dealVM.OperationTypeId);

            if (operationType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var deal = _mapper.Map<Deal>(dealVM);

            if(dealVM.OperationTypeId == (int)OperationTypeEnum.Gasto)
            {
                deal.Price *= -1;
            }

            await _dealRepository.Update(deal, dealVM.PreviousPrice, dealVM.PreviousAccountId);

            if (string.IsNullOrEmpty(dealVM.ReturnUrl))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(dealVM.ReturnUrl);
            }
        }

        /// <summary>
        /// Method to delete a Deal from Database
        /// </summary>
        /// <param name="id">Id of the Deal</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string returnUrl = null)
        {
            var userId = _userService.GetUserId();
            var deal = await _dealRepository.GetById(id, userId);

            if (deal is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _dealRepository.Delete(id);

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(returnUrl);
            }
        }

        /// <summary>
        /// Method to get all the categories by the OperationTypeId
        /// </summary>
        /// <param name="operationTypeId">Id of the operation type</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCategories([FromBody] int operationTypeId)
        {
            var userId = _userService.GetUserId();
            var categories = await GetCategoriesByOperationType(userId, operationTypeId);
            return Ok(categories);

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

        /// <summary>
        /// Method to get all the OperationTypes in Database as select List Items
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetOperationTypes()
        {
            var operationTypes = await _operationTypeRepository.GetAll();
            return operationTypes.Select(x => new SelectListItem(x.T_Description, x.Id.ToString()));
        }

        /// <summary>
        /// Method to get all the categories by the user Id
        /// </summary>
        /// <param name="userId">User´s Id</param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetCategoriesByOperationType(int userId, int operationTypeId)
        {
            var categories = await _categoryRepository.GetByOperationType(userId, operationTypeId);
            return categories.Select(x => new SelectListItem(x.C_Name, x.Id.ToString()));
        }
    }
}
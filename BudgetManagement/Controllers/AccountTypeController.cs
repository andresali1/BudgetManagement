﻿using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            var accountTypes = await _accountTypeRepository.GetByUserId(userId);

            return View(accountTypes);
        }

        //Get: Create
        [HttpGet]
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

        //Get: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId();
            var accountType = await _accountTypeRepository.GetById(id, userId);

            if(accountType == null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            return View(accountType);
        }

        /// <summary>
        /// Method to edit an AccountType in Db
        /// </summary>
        /// <param name="accountType">AccountType object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(AccountType accountType)
        {
            var userId = _userService.GetUserId();
            var accountTypeInDb = await _accountTypeRepository.GetById(accountType.Id, userId);

            if(accountTypeInDb is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _accountTypeRepository.Update(accountType);

            return RedirectToAction("Index");
        }

        //Get: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var accountType = await _accountTypeRepository.GetById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            return View(accountType);
        }

        /// <summary>
        /// Method to delete an AccountType from DB
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteAccountType(int id)
        {
            var userId = _userService.GetUserId();
            var accountType = await _accountTypeRepository.GetById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _accountTypeRepository.Delete(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method that validates if an account type exists by its name for the given user
        /// </summary>
        /// <param name="at_name">Account type name</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AccountTypeExistsValidation(string at_name, int id)
        {
            var userId = _userService.GetUserId();
            var accountTypeExists = await _accountTypeRepository.Exists(at_name, userId, id);

            if (accountTypeExists)
            {
                return Json($"El nombre {at_name} ya existe");
            }

            return Json(true);
        }

        /// <summary>
        /// Method to save the given order of the table in DB
        /// </summary>
        /// <param name="ids">ids of the elements in the table to give order</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Order([FromBody] int[] ids)
        {
            var userId = _userService.GetUserId();
            var accountTypes = await _accountTypeRepository.GetByUserId(userId);
            var accountTypesIds = accountTypes.Select(x => x.Id);

            var accountTypesOutOfTheUser = ids.Except(accountTypesIds).ToList();

            if(accountTypesOutOfTheUser.Count > 0)
            {
                return Forbid();
            }

            var orderedAccountTypes = ids.Select((value, index) =>
                                            new AccountType() { Id = value, AT_Order = index + 1 }).AsEnumerable();

            await _accountTypeRepository.Order(orderedAccountTypes);

            return Ok();
        }
    }
}

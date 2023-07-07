using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManagement.Controllers
{
    public class OperationTypeController : Controller
    {
        private readonly IOperationTypeRepository _operationTypeRepository;
        private readonly IUserService _userService;

        public OperationTypeController(IOperationTypeRepository operationTypeRepository,
                                       IUserService userService)
        {
            _operationTypeRepository = operationTypeRepository;
            _userService = userService;
        }

        //Get: Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var operationTypes = await _operationTypeRepository.GetAll();

            return View(operationTypes);
        }

        //Get: Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Method to create a new Operation Type
        /// </summary>
        /// <param name="operationType">OperationType Object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(OperationType operationType)
        {
            if (!ModelState.IsValid)
            {
                return View(operationType);
            }

            await _operationTypeRepository.Create(operationType);

            return RedirectToAction("Index");
        }

        //Get: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var operationType = await _operationTypeRepository.GetById(id);
            if (operationType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            return View(operationType);
        }

        /// <summary>
        /// Method to edit an operation type
        /// </summary>
        /// <param name="operationType">OperationType object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(OperationType operationType)
        {
            var operationTypeInDb = await _operationTypeRepository.GetById(operationType.Id);

            if(operationTypeInDb is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _operationTypeRepository.Update(operationType);

            return RedirectToAction("Index");
        }

        //Get: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var operationType = await _operationTypeRepository.GetById(id);

            if(operationType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            return View(operationType);
        }

        /// <summary>
        /// Method to delete an OperationType
        /// </summary>
        /// <param name="operationType">OperationType object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteOperationType(int id)
        {
            var operationType = await _operationTypeRepository.GetById(id);

            if (operationType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _operationTypeRepository.Delete(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method to validate if a given description already exists
        /// </summary>
        /// <param name="t_description">Description</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OperationTypeExistsValidation(string t_description)
        {
            var operationTypeExists = await _operationTypeRepository.Exists(t_description);

            if (operationTypeExists)
            {
                return Json($"La descripción {t_description} ya existe");
            }

            return Json(true);
        }
    }
}

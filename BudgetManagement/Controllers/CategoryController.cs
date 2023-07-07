using AutoMapper;
using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService;
        private readonly IOperationTypeRepository _operationTypeRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository,
                                  IUserService userService,
                                  IOperationTypeRepository operationTypeRepository,
                                  IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _userService = userService;
            _operationTypeRepository = operationTypeRepository;
            _mapper = mapper;
        }

        //Get: Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CategoryCreationViewModel();
            model.OperationTypes = await GetOperationTypes();

            return View(model);
        }

        /// <summary>
        /// Method to create a new Category
        /// </summary>
        /// <param name="category">Category object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreationViewModel categoryVM)
        {
            var operationType = _operationTypeRepository.GetAll();

            if(operationType is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            if (!ModelState.IsValid)
            {
                categoryVM.OperationTypes = await GetOperationTypes();
                return View(categoryVM);
            }

            var userId = _userService.GetUserId();
            categoryVM.UserId = userId;

            var category = _mapper.Map<Category>(categoryVM);

            await _categoryRepository.Create(category);

            return RedirectToAction("Index");
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
    }
}
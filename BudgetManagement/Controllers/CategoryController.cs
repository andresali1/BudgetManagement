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

        //Get: Index
        [HttpGet]
        public async Task<IActionResult> Index(PaginationViewModel paginationVM)
        {
            var userId = _userService.GetUserId();
            var categories = await _categoryRepository.GetByUserId(userId, paginationVM);
            var categoriesTotal = await _categoryRepository.GetDataAmount(userId);

            var responseVM = new PaginationResponse<Category>
            {
                Elements = categories,
                Page = paginationVM.Page,
                RecordsByPage = paginationVM.RecordsByPage,
                AmountOfRecords = categoriesTotal,
                BaseUrl = Url.Action()
            };

            return View(responseVM);
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

            await _categoryRepository.Create(categoryVM);

            return RedirectToAction("Index");
        }

        //Get: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _categoryRepository.GetById(id, userId);

            if(category is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            var model = _mapper.Map<CategoryCreationViewModel>(category);
            model.OperationTypes = await GetOperationTypes();

            return View(model);
        }

        /// <summary>
        /// Method to edit a Category in Database
        /// </summary>
        /// <param name="categoryVM">CategoryCreationViewModel object with data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryCreationViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                categoryVM.OperationTypes = await GetOperationTypes();
                return View(categoryVM);
            }

            var userId = _userService.GetUserId();
            var category = await _categoryRepository.GetById(categoryVM.Id, userId);

            if (category is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            categoryVM.UserId = userId;

            await _categoryRepository.Update(categoryVM);
            return RedirectToAction("Index");
        }

        //Get: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _categoryRepository.GetById(id, userId);

            if (category is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            return View(category);
        }

        /// <summary>
        /// Method to delete a Category from Database
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _categoryRepository.GetById(id, userId);

            if (category is null)
            {
                return RedirectToAction("ViewNotFound", "Home");
            }

            await _categoryRepository.Delete(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method to get all the OperationTypes in Database as select List Items
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetOperationTypes()
        {
            var operationTypes = await _operationTypeRepository.GetAll();
            var result = operationTypes.Select(x => new SelectListItem(x.T_Description, x.Id.ToString())).ToList();

            var defaultOption = new SelectListItem("-- Seleccione un Tipo de Operación --", "", true);

            result.Insert(0, defaultOption);

            return result;
        }
    }
}
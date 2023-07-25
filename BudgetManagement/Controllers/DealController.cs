using AutoMapper;
using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

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
        public async Task<IActionResult> Weekly(int month, int year)
        {
            var userId = _userService.GetUserId();
            IEnumerable<WeeklyGetResult> dealsByWeek = await _reportService.GetWeeklyReport(userId, month, year, ViewBag);

            var grouped = dealsByWeek.GroupBy(x => x.Week)
                                     .Select(x => new WeeklyGetResult()
                                     {
                                         Week = x.Key,
                                         Income = x.Where(x => x.OperationTypeId == (int)OperationTypeEnum.Ingreso).Select(x => x.Price).FirstOrDefault(),
                                         Expense = x.Where(x => x.OperationTypeId == (int)OperationTypeEnum.Gasto).Select(x => x.Price).FirstOrDefault()
                                     }).ToList();

            if(year == 0 || month == 0)
            {
                var today = DateTime.Today;
                year = today.Year;
                month = today.Month;
            }

            var referenceDate = new DateTime(year, month, 1);
            var monthDays = Enumerable.Range(1, referenceDate.AddMonths(1).AddDays(-1).Day);
            var segmentedDays = monthDays.Chunk(7).ToList();

            for(int i = 0; i < segmentedDays.Count(); i++)
            {
                var week = i + 1;
                var beginDate = new DateTime(year, month, segmentedDays[i].First());
                var endDate = new DateTime(year, month, segmentedDays[i].Last());
                var weekGroup = grouped.FirstOrDefault(x => x.Week == week);

                if(weekGroup is null)
                {
                    grouped.Add(new WeeklyGetResult()
                    {
                        Week = week,
                        BeginDate = beginDate,
                        EndDate = endDate
                    });
                }
                else
                {
                    weekGroup.BeginDate = beginDate;
                    weekGroup.EndDate = endDate;
                }
            }

            grouped.OrderByDescending(x => x.Week).ToList();

            var model = new WeeklyReportViewModel();
            model.DealsByWeek = grouped;
            model.ReferenceDate = referenceDate;

            return View(model);
        }

        //Get: Monthly
        [HttpGet]
        public async Task<IActionResult> Monthly(int year)
        {
            var userId = _userService.GetUserId();

            if(year == 0)
            {
                year = DateTime.Now.Year;
            }

            var dealsByMonth = await _dealRepository.GetByMonth(userId, year);
            var groupedDeals = dealsByMonth.GroupBy(x => x.Month)
                                           .Select(x => new MonthlyGetResult()
                                           {
                                               Month = x.Key,
                                               Income = x.Where(x => x.OperationTypeId == (int)OperationTypeEnum.Ingreso).Select(x => x.Price).FirstOrDefault(),
                                               Expense = x.Where(x => x.OperationTypeId == (int)OperationTypeEnum.Gasto).Select(x => x.Price).FirstOrDefault()
                                           }).ToList();

            for(int month = 1; month <= 12; month++)
            {
                var deal = groupedDeals.FirstOrDefault(x => x.Month == month);
                var referenceDate = new DateTime(year, month, 1);

                if(deal is null)
                {
                    groupedDeals.Add(new MonthlyGetResult()
                    {
                        Month = month,
                        ReferenceDate = referenceDate
                    });
                }
                else
                {
                    deal.ReferenceDate = referenceDate;
                }
            }

            groupedDeals = groupedDeals.OrderByDescending(x => x.Month).ToList();

            var model = new MonthlyReportViewModel();
            model.Year = year;
            model.DealsByMonth = groupedDeals;

            return View(model);
        }

        //Get: ExcelReport
        [HttpGet]
        public IActionResult ExcelReport()
        {
            return View();
        }

        /// <summary>
        /// Method to generate an Excel file of a month
        /// </summary>
        /// <param name="month">Given month</param>
        /// <param name="year">Given year</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileResult> ExportExcelByMonth(int month, int year)
        {
            var beginDate = new DateTime(year, month, 1);
            var endDate = beginDate.AddMonths(1).AddDays(-1);
            var userId = _userService.GetUserId();

            var deals = await _dealRepository.GetByUserId(new DealByUserParameter()
            {
                UserId = userId,
                BeginDate = beginDate,
                EndDate = endDate
            });

            var fileName = $"Manejo_Presupuesto-{beginDate.ToString("MMM yyyy")}.xlsx";
            return GenerateExcel(fileName, deals);
        }

        /// <summary>
        /// Method to generate an Excel file of a year
        /// </summary>
        /// <param name="year">Given year</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileResult> ExportExcelByYear(int year)
        {
            var beginDate = new DateTime(year, 1, 1);
            var endDate = beginDate.AddYears(1).AddDays(-1);
            var userId = _userService.GetUserId();

            var deals = await _dealRepository.GetByUserId(new DealByUserParameter()
            {
                UserId = userId,
                BeginDate = beginDate,
                EndDate = endDate
            });

            var fileName = $"Manejo_Presupuesto-{beginDate.ToString("yyyy")}.xlsx";
            return GenerateExcel(fileName, deals);
        }

        /// <summary>
        /// Method to generate an Excel file with all the data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileResult> ExportExcelAll()
        {
            var beginDate = DateTime.Today.AddYears(-100);
            var endDate = DateTime.Today.AddYears(100);
            var userId = _userService.GetUserId();

            var deals = await _dealRepository.GetByUserId(new DealByUserParameter()
            {
                UserId = userId,
                BeginDate = beginDate,
                EndDate = endDate
            });

            var fileName = $"Manejo_Presupuesto-{DateTime.Today.ToString("dd-MM-yyyy")}.xlsx";
            return GenerateExcel(fileName, deals);
        }

        /// <summary>
        /// Method to generate an Excel File
        /// </summary>
        /// <param name="fileName">Name for the file</param>
        /// <param name="deals">IEnumerable with the collection of data</param>
        /// <returns></returns>
        private FileResult GenerateExcel(string fileName, IEnumerable<Deal> deals)
        {
            DataTable dt = new DataTable("Deals");
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Fecha"),
                new DataColumn("Cuenta"),
                new DataColumn("Categoría"),
                new DataColumn("Nota"),
                new DataColumn("Monto"),
                new DataColumn("Ingreso/Gasto")
            });

            foreach(var deal in deals)
            {
                string operationType = ((OperationTypeEnum)deal.OperationTypeId).ToString();
                dt.Rows.Add(deal.DealDate, deal.Account, deal.Category, deal.Note, deal.Price, operationType);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        //Get: Calendar
        [HttpGet]
        public IActionResult Calendar()
        {
            return View();
        }

        /// <summary>
        /// Method to get all the transactions for the calendar
        /// </summary>
        /// <param name="start">Begin Date</param>
        /// <param name="end">End Date</param>
        /// <returns></returns>
        public async Task<JsonResult> GetCalendarTransactions(DateTime start, DateTime end)
        {
            var userId = _userService.GetUserId();

            var deals = await _dealRepository.GetByUserId(new DealByUserParameter()
            {
                UserId = userId,
                BeginDate = start,
                EndDate = end
            });

            var calendarEvents = deals.Select(deal => new CalendarEvent()
            {
                Title = deal.Price.ToString("N0"),
                Start = deal.DealDate.ToString("yyyy-MM-dd"),
                End = deal.DealDate.ToString("yyyy-MM-dd"),
                Color = deal.OperationTypeId == (int)OperationTypeEnum.Gasto ? "Red" : null
            });

            return Json(calendarEvents);
        }

        /// <summary>
        /// Method to get the transactions made by an user according to date
        /// </summary>
        /// <param name="date">Selected Date</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetDealsByDate(DateTime date)
        {
            var userId = _userService.GetUserId();

            var deals = await _dealRepository.GetByUserId(new DealByUserParameter()
            {
                UserId = userId,
                BeginDate = date,
                EndDate = date
            });

            return Json(deals);
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
            var result = accounts.Select(x => new SelectListItem(x.A_Name, x.Id.ToString())).ToList();

            var defaultOption = new SelectListItem("-- Seleccione una Cuenta --", "", true);

            result.Insert(0, defaultOption);

            return result;
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

        /// <summary>
        /// Method to get all the categories by the user Id
        /// </summary>
        /// <param name="userId">User´s Id</param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetCategoriesByOperationType(int userId, int operationTypeId)
        {
            var categories = await _categoryRepository.GetByOperationType(userId, operationTypeId);
            var result = categories.Select(x => new SelectListItem(x.C_Name, x.Id.ToString())).ToList();

            var defaultOption = new SelectListItem("-- Seleccione una Categoría --", "", true);

            result.Insert(0, defaultOption);

            return result;
        }
    }
}

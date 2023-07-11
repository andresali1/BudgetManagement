using BudgetManagement.Interfaces;
using BudgetManagement.Models;

namespace BudgetManagement.Services
{
    public class ReportService : IReportService
    {
        private readonly IDealRepository _dealRepository;
        private readonly HttpContext _httpContext;

        public ReportService(IDealRepository dealRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dealRepository = dealRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// Method to get all the deals made by week in a month
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="month">Given month</param>
        /// <param name="year">Given year</param>
        /// <param name="ViewBag">The ViewBag</param>
        /// <returns></returns>
        public async Task<IEnumerable<WeeklyGetResult>> GetWeeklyReport(int userId, int month, int year, dynamic ViewBag)
        {
            (DateTime beginDate, DateTime endDate) = GenerateBeginAndEndDate(month, year);

            var parameter = new DealByUserParameter()
            {
                UserId = userId,
                BeginDate = beginDate,
                EndDate = endDate
            };

            AssignValuesToViewBag(ViewBag, beginDate);

            var model = await _dealRepository.GetByWeek(parameter);

            return model;
        }

        /// <summary>
        /// Method to generate a Detailed deal report by user Id
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="month">Given month</param>
        /// <param name="year">Given year</param>
        /// <param name="ViewBag">The ViewBag</param>
        /// <returns></returns>
        public async Task<DetailDealReport> GetDeateiledDealReport(int userId, int month, int year, dynamic ViewBag)
        {
            (DateTime beginDate, DateTime endDate) = GenerateBeginAndEndDate(month, year);

            var parameter = new DealByUserParameter()
            {
                UserId = userId,
                BeginDate = beginDate,
                EndDate = endDate
            };

            var deals = await _dealRepository.GetByUserId(parameter);
            var model = DetailedDealReportGeneration(beginDate, endDate, deals);
            AssignValuesToViewBag(ViewBag, beginDate);

            return model;
        }

        /// <summary>
        /// Method to get the deal report by account id
        /// </summary>
        /// <param name="userId">Id of the User</param>
        /// <param name="accountId">Id of the account</param>
        /// <param name="month">Given month</param>
        /// <param name="year">given year</param>
        /// <param name="ViewBag">Variable to handle VieBags</param>
        /// <returns></returns>
        public async Task<DetailDealReport> GetDetailDealReportByAccount(int userId, int accountId, int month, int year, dynamic ViewBag)
        {
            (DateTime beginDate, DateTime endDate) = GenerateBeginAndEndDate(month, year);

            var getDealsByAccount = new GetDealByAccount()
            {
                AccountId = accountId,
                UserId = userId,
                BeginDate = beginDate,
                EndDate = endDate
            };

            var deals = await _dealRepository.GetByAccountId(getDealsByAccount);
            var model = DetailedDealReportGeneration(beginDate, endDate, deals);
            AssignValuesToViewBag(ViewBag, beginDate);

            return model;
        }

        /// <summary>
        /// Method to generate a Tuple with the begin and end date generated values
        /// </summary>
        /// <param name="month">Given month</param>
        /// <param name="year">Given year</param>
        /// <returns></returns>
        private (DateTime beginDate, DateTime endDate) GenerateBeginAndEndDate(int month, int year)
        {
            DateTime beginDate, endDate;

            if (month <= 0 || month > 12 || year <= 1900)
            {
                var today = DateTime.Today;
                beginDate = new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                beginDate = new DateTime(year, month, 1);
            }

            endDate = beginDate.AddMonths(1).AddDays(-1);

            return (beginDate, endDate);
        }

        /// <summary>
        /// Method to generate an object type DeatilDealReport with info
        /// </summary>
        /// <param name="beginDate">Given begin date</param>
        /// <param name="endDate">Given end date</param>
        /// <param name="deals">List of deals</param>
        /// <returns></returns>
        private static DetailDealReport DetailedDealReportGeneration(DateTime beginDate, DateTime endDate, IEnumerable<Deal> deals)
        {
            var model = new DetailDealReport();

            var dealsByDate = deals.OrderByDescending(x => x.DealDate)
                                   .GroupBy(x => x.DealDate)
                                   .Select(group => new DetailDealReport.DealByDate()
                                   {
                                       DealDate = group.Key,
                                       Deals = group.AsEnumerable()
                                   });

            model.GroupedDeals = dealsByDate;
            model.BeginDate = beginDate;
            model.EndDate = endDate;
            return model;
        }

        /// <summary>
        /// Method to give needed values to the ViewBag
        /// </summary>
        /// <param name="ViewBag">The ViewBag</param>
        /// <param name="beginDate">Given begin date</param>
        private void AssignValuesToViewBag(dynamic ViewBag, DateTime beginDate)
        {
            ViewBag.PreviousMonth = beginDate.AddMonths(-1).Month;
            ViewBag.PreviousYear = beginDate.AddMonths(-1).Year;
            ViewBag.NextMonth = beginDate.AddMonths(1).Month;
            ViewBag.NextYear = beginDate.AddMonths(1).Year;
            ViewBag.ReturnUrl = _httpContext.Request.Path + _httpContext.Request.QueryString;
        }
    }
}

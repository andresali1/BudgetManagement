using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IReportService
    {
        Task<DetailDealReport> GetDeateiledDealReport(int userId, int month, int year, dynamic ViewBag);
        Task<DetailDealReport> GetDetailDealReportByAccount(int userId, int accountId, int month, int year, dynamic ViewBag);
    }
}

using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IDealRepository
    {
        Task Create(Deal deal);
        Task Delete(int id);
        Task<IEnumerable<Deal>> GetByAccountId(GetDealByAccount model);
        Task<Deal> GetById(int id, int userId);
        Task<IEnumerable<Deal>> GetByUserId(TransactionByUserParameter model);
        Task Update(Deal deal, int previousPrice, int previousAccountId);
    }
}

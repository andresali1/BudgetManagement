using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IDealRepository
    {
        Task Create(Deal deal);
        Task Delete(int id);
        Task<Deal> GetById(int id, int userId);
        Task Update(Deal deal, int previousPrice, int previousAccountId);
    }
}

using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IDealRepository
    {
        Task Create(Deal deal);
    }
}

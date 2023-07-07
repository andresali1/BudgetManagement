using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface ICategoryRepository
    {
        Task Create(Category category);
    }
}

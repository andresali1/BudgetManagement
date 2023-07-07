using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface ICategoryRepository
    {
        Task Create(CategoryCreationViewModel categoryVM);
        Task Delete(int id);
        Task<Category> GetById(int id, int userId);
        Task<IEnumerable<Category>> GetByUserId(int userId);
        Task Update(CategoryCreationViewModel categoryVM);
    }
}

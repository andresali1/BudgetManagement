using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface ICategoryRepository
    {
        Task Create(CategoryCreationViewModel categoryVM);
        Task Delete(int id);
        Task<Category> GetById(int id, int userId);
        Task<IEnumerable<Category>> GetByOperationType(int userId, int operationTypeId);
        Task<IEnumerable<Category>> GetByUserId(int userId, PaginationViewModel pagination);
        Task<int> GetDataAmount(int userId);
        Task Update(CategoryCreationViewModel categoryVM);
    }
}

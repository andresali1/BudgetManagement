using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IOperationTypeRepository
    {
        Task Create(OperationType operationType);
        Task Delete(int id);
        Task<bool> Exists(string t_description);
        Task<IEnumerable<OperationType>> GetAll();
        Task<OperationType> GetById(int id);
        Task Update(OperationType operationType);
    }
}

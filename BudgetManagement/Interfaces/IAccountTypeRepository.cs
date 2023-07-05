using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountType accountType);
        Task Delete(int id);
        Task<bool> Exists(string at_name, int userId);
        Task<AccountType> GetById(int id, int userId);
        Task<IEnumerable<AccountType>> GetByUserId(int userId);
        Task Update(AccountType accountType);
    }
}

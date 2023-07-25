using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountType accountType);
        Task Delete(int id);
        Task<bool> Exists(string at_name, int userId, int id = 0);
        Task<AccountType> GetById(int id, int userId);
        Task<IEnumerable<AccountType>> GetByUserId(int userId);
        Task Order(IEnumerable<AccountType> orderedAccountTypes);
        Task Update(AccountType accountType);
    }
}

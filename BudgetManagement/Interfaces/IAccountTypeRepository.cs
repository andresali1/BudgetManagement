using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IAccountTypeRepository
    {
        Task Create(AccountType accountType);
        Task<bool> Exists(string at_name, int userId);
        Task<IEnumerable<AccountType>> GetByUserId(int userId);
    }
}

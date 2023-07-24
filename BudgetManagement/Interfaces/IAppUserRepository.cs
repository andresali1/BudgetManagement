using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IAppUserRepository
    {
        Task<int> CreateUser(AppUser user);
        Task<AppUser> GetUserByEmail(string normalizedEmail);
    }
}

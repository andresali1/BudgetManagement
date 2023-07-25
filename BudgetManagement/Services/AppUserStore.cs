using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace BudgetManagement.Services
{
    public class AppUserStore : IUserStore<AppUser>, IUserEmailStore<AppUser>, IUserPasswordStore<AppUser>
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserStore(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        /// <summary>
        /// Method to create a new user
        /// </summary>
        /// <param name="user">AppUser object with data</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            user.Id = await _appUserRepository.CreateUser(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Method to get an User by his email
        /// </summary>
        /// <param name="normalizedEmail">Given Email</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<AppUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _appUserRepository.GetUserByEmail(normalizedEmail);
        }

        public Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to get an User by his email
        /// </summary>
        /// <param name="normalizedUserName">Given Email</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _appUserRepository.GetUserByEmail(normalizedUserName);
        }

        /// <summary>
        /// Method to get the User´s Email
        /// </summary>
        /// <param name="user">Given User</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to get the user´s Password hash
        /// </summary>
        /// <param name="user">Given User</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Method to get the user's Id
        /// </summary>
        /// <param name="user">Given user</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        /// <summary>
        /// Method to get the user´s Email
        /// </summary>
        /// <param name="user">Given user</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(AppUser user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to set the normalized Email of the user
        /// </summary>
        /// <param name="user">Given user</param>
        /// <param name="normalizedEmail">Normalized Email</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task SetNormalizedEmailAsync(AppUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Since we don't have User Name just return a completed task
        /// </summary>
        /// <param name="user"></param>
        /// <param name="normalizedName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method to set the user´s password hash
        /// </summary>
        /// <param name="user">Given user</param>
        /// <param name="passwordHash">New password hash</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

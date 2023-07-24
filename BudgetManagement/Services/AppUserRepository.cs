using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly string _connectionString;

        public AppUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Method to create a new User
        /// </summary>
        /// <param name="user">AppUser object with data</param>
        /// <returns></returns>
        public async Task<int> CreateUser(AppUser user)
        {
            using var connection = new SqlConnection(_connectionString);

            var id = await connection.QuerySingleAsync<int>(
                                        @"INSERT INTO AppUser (Email, NormalizedEmail, PasswordHash)
                                          VALUES (@Email, @NormalizedEmail, @PasswordHash)",
                                        user
                                      );

            return id;
        }

        /// <summary>
        /// Method to get an User By its email
        /// </summary>
        /// <param name="normalizedEmail">Given Email</param>
        /// <returns></returns>
        public async Task<AppUser> GetUserByEmail(string normalizedEmail)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QuerySingleOrDefaultAsync<AppUser>(
                                        @"SELECT Id, Email, NormalizedEmail, PasswordHash
                                          FROM AppUser
                                          WHERE NormalizedEmail = @NormalizedEmail",
                                        new { normalizedEmail }
                                    );
        }
    }
}

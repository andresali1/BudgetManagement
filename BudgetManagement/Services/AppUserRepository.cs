using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

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

            var userId = await connection.QuerySingleAsync<int>(
                                        @"INSERT INTO AppUser (Email, NormalizedEmail, PasswordHash)
                                          VALUES (@Email, @NormalizedEmail, @PasswordHash);
                                          SELECT SCOPE_IDENTITY();",
                                        user
                                      );

            await connection.ExecuteAsync("SP_NewUserDataCreation", new { userId }, commandType: CommandType.StoredProcedure);

            return userId;
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

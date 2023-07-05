using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class AccountTypeRepository : IAccountTypeRepository
    {
        private readonly string _connectionString;

        public AccountTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Method to Create an Account Type in Database
        /// </summary>
        /// <param name="accountType">AccountType object type with the data</param>
        public async Task Create(AccountType accountType)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                        @"INSERT INTO AccountType (AT_Name, UserId, AT_Order)
                                          VALUES (@AT_Name, @UserId, 0);
                                          SELECT SCOPE_IDENTITY()",
                                        accountType
                                       );

            accountType.Id = id;


        }

        /// <summary>
        /// Method to validate if an account type already exists
        /// </summary>
        /// <param name="at_name">Name of the account type</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task<bool> Exists(string at_name, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(
                                            @"SELECT 1 FROM AccountType
                                              WHERE AT_Name = @AT_Name
                                              AND UserId = @UserId;",
                                            new { at_name, userId }
                                           );

            return exists == 1;
        }

        public async Task<IEnumerable<AccountType>> GetByUserId(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<AccountType>(
                                        @"SELECT Id, AT_Name, AT_Order
                                          FROM AccountType
                                          WHERE UserId = @UserId",
                                        new { userId }
                                    );
        }
    }
}

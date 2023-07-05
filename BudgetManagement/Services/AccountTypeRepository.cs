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

        /// <summary>
        /// Method to get all AccountTypes by the user's Id
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method to update an AccountType by its Id
        /// </summary>
        /// <param name="accountType">AccounType object with data</param>
        /// <returns></returns>
        public async Task Update(AccountType accountType)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                                @"UPDATE AccountType SET
                                  AT_Name = @AT_Name
                                  WHERE Id = @Id",
                                accountType
                             );

        }

        /// <summary>
        /// Method to get an AccountType by its Id
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task<AccountType> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>(
                                        @"SELECT Id, AT_Name, AT_Order
                                          FROM AccountType
                                          WHERE Id = @Id
                                          AND UserId = @UserId",
                                        new { id, userId }
                                    );
        }

        /// <summary>
        /// Method to delete an AccountType by its Id
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection( _connectionString);
            await connection.ExecuteAsync(
                                @"DELETE FROM AccountType
                                  WHERE Id = @Id",
                                new { id }
                             );
        }
    }
}

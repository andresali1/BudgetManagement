using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Method to create a ne Account in DataBase
        /// </summary>
        /// <param name="account">Account object with data</param>
        /// <returns></returns>
        public async Task Create(Account account)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                        @"INSERT INTO Account (A_Name, AccountTypeId, Balance, A_Description)
                                          VALUES (@A_Name, @AccountTypeId, @Balance, @A_Description);
                                          SELECT SCOPE_IDENTITY()",
                                        account
                                      );

            account.Id = id;
        }

        /// <summary>
        /// Method to get all the Accounts related to one user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task<IEnumerable<Account>> Search(int userId)
        {
            using var connection = new SqlConnection( _connectionString);
            return await connection.QueryAsync<Account>(
                                        @"SELECT A.Id, A_Name, Balance, AT_Name[AccountType]
                                          FROM Account A
                                          INNER JOIN AccountType AC ON AC.Id = A.AccountTypeId
                                          WHERE AC.UserId = @UserId
                                          ORDER BY AC.AT_Order",
                                        new { userId }
                                    );
        }

        /// <summary>
        /// Method to get an Account by Id
        /// </summary>
        /// <param name="id">Id of the element</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task<Account> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Account>(
                                        @"SELECT A.Id, A_Name, Balance, A_Description, A.AccountTypeId
                                          FROM Account A
                                          INNER JOIN AccountType AC ON AC.Id = A.AccountTypeId
                                          WHERE AC.UserId = @UserId
                                          AND A.Id = @Id",
                                        new { userId, id }
                                    );
        }

        /// <summary>
        /// Method to update an account by its Id
        /// </summary>
        /// <param name="accountVM">AccountCreationViewModel object with data</param>
        /// <returns></returns>
        public async Task Update(AccountCreationViewModel accountVM)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                                @"UPDATE Account
                                  SET A_Name = @A_Name, Balance = @Balance,
                                  	  A_Description = @A_Description, AccountTypeId = @AccountTypeId
                                  WHERE Id = @Id",
                                accountVM
                             );
        }
        
        /// <summary>
        /// Method to delete an Account by its Id
        /// </summary>
        /// <param name="id">Id of the account</param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Account WHERE Id = @Id", new { id });
        }
    }
}

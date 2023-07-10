using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Method to create a new Category
        /// </summary>
        /// <param name="category">Category object with data</param>
        /// <returns></returns>
        public async Task Create(CategoryCreationViewModel categoryVM)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                        @"INSERT INTO Category (C_Name, OperationTypeId, UserId)
                                          VALUES (@C_Name, @OperationTypeId, @UserId)
                                          SELECT SCOPE_IDENTITY()",
                                        categoryVM
                                      );

            categoryVM.Id = id;
        }

        /// <summary>
        /// Method to get all the categories from an user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetByUserId(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Category>(
                                        @"SELECT C.Id, C_Name, OperationTypeId, T_Description, UserId
                                          FROM Category C
                                          INNER JOIN OperationType OT ON OT.Id = C.OperationTypeId
                                          WHERE UserId = @UserId",
                                        new { userId }
                                    );
        }

        /// <summary>
        /// Method to get all the categories by Operation Type
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="operationTypeId">Id of the operation type</param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetByOperationType(int userId, int operationTypeId)
        {
            using var connection = new SqlConnection( _connectionString);
            return await connection.QueryAsync<Category>(
                                        @"SELECT C.Id, C_Name, OperationTypeId, T_Description, UserId
                                          FROM Category C
                                          INNER JOIN OperationType OT ON OT.Id = C.OperationTypeId
                                          WHERE UserId = @UserId
                                          AND OperationTypeId = @OperationTypeId",
                                        new { userId, operationTypeId }
                                    );
        }

        /// <summary>
        /// Method to get a Category by its Id
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task<Category> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Category>(
                                        @"SELECT Id, C_Name, OperationTypeId, UserId
                                          FROM Category
                                          WHERE Id = @Id
                                          AND UserId = @UserId",
                                        new { id, userId }
                                    );
        }

        /// <summary>
        /// Method to update a Category in Database
        /// </summary>
        /// <param name="category">Category object with data</param>
        /// <returns></returns>
        public async Task Update(CategoryCreationViewModel categoryVM)
        {
            using var connection = new SqlConnection( _connectionString);
            await connection.ExecuteAsync(
                                @"UPDATE Category SET
                                  C_Name = @C_Name,
                                  OperationTypeId = @OperationTypeId
                                  WHERE Id = @Id",
                                categoryVM
                              );
        }

        /// <summary>
        /// Method to delete a Category from Database
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Category WHERE Id = @Id", new { id });
        }
    }
}

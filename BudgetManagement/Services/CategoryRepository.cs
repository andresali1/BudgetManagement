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
        public async Task Create(Category category)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                        @"INSERT INTO Category (C_Name, OperationTypeId, UserId)
                                          VALUES (@C_Name, @OperationTypeId, @UserId)
                                          SELECT SCOPE_IDENTITY()",
                                        category
                                      );

            category.Id = id;
        }
    }
}

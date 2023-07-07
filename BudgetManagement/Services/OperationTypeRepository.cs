using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class OperationTypeRepository : IOperationTypeRepository
    {
        private readonly string _connectionString;

        public OperationTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Method to create a new Operation Type in Database
        /// </summary>
        /// <param name="operationType">OperationType Object with data</param>
        /// <returns></returns>
        public async Task Create(OperationType operationType)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                        @"INSERT INTO OperationType (T_Description)
                                          VALUES (@T_Description)
                                          SELECT SCOPE_IDENTITY()",
                                        new { operationType.T_Description }
                                      );

            operationType.Id = id;
        }

        /// <summary>
        /// Method to validate if a Operation Type already Exists
        /// </summary>
        /// <param name="t_description">Name of the operation type</param>
        /// <returns></returns>
        public async Task<bool> Exists(string t_description)
        {
            using var connection = new SqlConnection( _connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(
                                        @"SELECT 1 FROM OperationType
                                          WHERE T_Description = @T_Description",
                                        new { t_description }
                                    );

            return exists == 1;
        }

        /// <summary>
        /// Method to get all the Operation Types in Database
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<OperationType>> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<OperationType>(
                                @"SELECT Id, T_Description
                                  FROM OperationType"
                              );
        }

        /// <summary>
        /// Method to get an OperationType by its id
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <returns></returns>
        public async Task<OperationType> GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<OperationType>(
                                        @"SELECT Id, T_Description
                                          FROM OperationType
                                          WHERE Id = @Id",
                                        new { id }
                                    );
        }

        /// <summary>
        /// Method to Update an operationType in Database
        /// </summary>
        /// <param name="operationType">OperationType object with data</param>
        /// <returns></returns>
        public async Task Update(OperationType operationType)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                                @"UPDATE OperationType SET
                                  T_Description = @T_Description
                                  WHERE Id = @Id",
                                operationType
                             );
        }

        /// <summary>
        /// Method to delete an OperationType from Database
        /// </summary>
        /// <param name="id">Id of the data</param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                                @"DELETE FROM OperationType
                                  WHERE Id = @Id",
                                new { id }
                             );
        }
    }
}

using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BudgetManagement.Services
{
    public class DealRepository : IDealRepository
    {
        private readonly string _connectionString;

        public DealRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Method to create a Deal in Database
        /// </summary>
        /// <param name="deal">Deal Object with data</param>
        /// <returns></returns>
        public async Task Create(Deal deal)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(
                                        "SP_InsDeal",
                                        new
                                        {
                                            deal.UserId,
                                            deal.DealDate,
                                            deal.Price,
                                            deal.AccountId,
                                            deal.CategoryId,
                                            deal.Note
                                        },
                                        commandType: CommandType.StoredProcedure
                                      );

            deal.Id = id;
        }
    }
}

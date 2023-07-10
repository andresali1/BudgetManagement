﻿using BudgetManagement.Interfaces;
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

        /// <summary>
        /// Method to Update a Deal in Database
        /// </summary>
        /// <param name="deal">Deal object with given data</param>
        /// <param name="previousPrice">Previously saved price for the same Deal</param>
        /// <param name="previousAccount">Previously saved accountId for the same Deal</param>
        /// <returns></returns>
        public async Task Update(Deal deal, int previousPrice, int previousAccountId)
        {
            using var connection = new SqlConnection( _connectionString);
            await connection.ExecuteAsync(
                                "SP_ActDeal",
                                new
                                {
                                    deal.Id,
                                    deal.DealDate,
                                    deal.Price,
                                    deal.CategoryId,
                                    deal.AccountId,
                                    deal.Note,
                                    previousPrice,
                                    previousAccountId
                                },
                                commandType: CommandType.StoredProcedure
                             );
        }

        /// <summary>
        /// Method to get a Deal by its Id
        /// </summary>
        /// <param name="id">Id of the deal</param>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        public async Task<Deal> GetById(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Deal>(
                                        @"SELECT D.Id, D.UserId, DealDate, Price, AccountId,
                                          	     CategoryId, Note, C.OperationTypeId
                                          FROM Deal D
                                          INNER JOIN Category C ON C.Id = D.CategoryId
                                          WHERE D.Id = @Id
                                          AND D.UserId = @UserId",
                                        new { id, userId }
                                    );
        }

        /// <summary>
        /// Method to eliminate a Deal from Database
        /// </summary>
        /// <param name="id">Id of the deal</param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("SP_DelDeal", new { id }, commandType: CommandType.StoredProcedure);
        }
    }
}
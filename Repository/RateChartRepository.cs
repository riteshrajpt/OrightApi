using Dapper;
using RateCharts.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RateCharts.Repository
{
    public class RateChartRepository
    {
        private readonly DBContext _dbContext;

        public RateChartRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertRateChartsAsync(IEnumerable<RateChart> rateCharts)
        {
            try
            {
                using (var connection = _dbContext.GetConnection())
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO RateCharts (ClientId, SNF, FAT, Price) " +
                        "VALUES (@ClientId, @SNF, @FAT, @Price)",
                        rateCharts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred in InsertRateChartsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<decimal?> GetPriceForClientAsync(int clientId, decimal snf, decimal fat)
        {
            try
            {
                using (var connection = _dbContext.GetConnection())
                {
                    var price = await connection.ExecuteScalarAsync<decimal?>(
                        "SELECT Price FROM RateCharts WHERE ClientId = @ClientId AND SNF = @SNF AND FAT = @FAT",
                        new { ClientId = clientId, SNF = snf, FAT = fat });

                    if (price == null)
                    {
                        throw new Exception($"Price not found for ClientId: {clientId}, SNF: {snf}, FAT: {fat}");
                    }

                    return price;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred in GetPriceForClientAsync: {ex.Message}");
                throw new Exception("An error occurred while retrieving the price.", ex);
            }
        }

    }
}

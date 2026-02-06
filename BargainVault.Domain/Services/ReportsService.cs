using BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BargainVault.Domain.Services
{
    public class ReportsService : IReportsService
    {
        private readonly string _connectionString;

        public ReportsService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<List<AuctionProfitDto>> GetAuctionProfitAnalysisAsync()
        {
            var results = new List<AuctionProfitDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT * FROM vw_auction_profit_analysis;",
                conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new AuctionProfitDto
                {
                    AcqId = reader.GetInt32(reader.GetOrdinal("acq_id")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    LotNumber = reader.IsDBNull(reader.GetOrdinal("lot_number"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("lot_number")),

                    AuctionSite = reader.GetString(reader.GetOrdinal("auction_site")),

                    TotalSettlement = reader.IsDBNull(reader.GetOrdinal("total_settlement"))
                        ? 0m
                        : reader.GetDecimal(reader.GetOrdinal("total_settlement")),

                    SuggestedBoothPrice = reader.IsDBNull(reader.GetOrdinal("suggested_booth_price"))
                        ? 0m
                        : reader.GetDecimal(reader.GetOrdinal("suggested_booth_price")),

                    PotentialProfit = reader.IsDBNull(reader.GetOrdinal("potential_profit"))
                        ? 0m
                        : reader.GetDecimal(reader.GetOrdinal("potential_profit"))
                });
            }

            return results;
        }
    }

}

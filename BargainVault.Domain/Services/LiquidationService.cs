using BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BargainVault.Domain.Services
{
    public class LiquidationService : ILiquidationService
    {
        private readonly string _connectionString;

        public LiquidationService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<List<LiquidationCandidateDto>> GetCandidatesAsync()
        {
            var results = new List<LiquidationCandidateDto>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"SELECT *
                    FROM vw_liquidation_candidates
                    ORDER BY ile_score DESC, days_in_inventory DESC";

            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = new LiquidationCandidateDto
                {
                    ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    DaysInInventory = reader.GetInt32(reader.GetOrdinal("days_in_inventory")),
                    AuctionPct = reader.GetDecimal(reader.GetOrdinal("auction_pct")),
                    IleScore = reader.GetInt32(reader.GetOrdinal("ile_score")),
                    Recommendation = reader.GetString(reader.GetOrdinal("recommendation")),
                    RetailPrice = reader.IsDBNull(reader.GetOrdinal("retail_price"))
                        ? null
                        : reader.GetDecimal(reader.GetOrdinal("retail_price")),
                    AuctionEstimate = reader.IsDBNull(reader.GetOrdinal("auction_estimate"))
                        ? null
                        : reader.GetDecimal(reader.GetOrdinal("auction_estimate"))
                };

                results.Add(item);
            }

            return results;
        }

        public Task MarkDisregardAsync(int itemId, bool value)
        {
            throw new NotImplementedException();
        }
    }





}

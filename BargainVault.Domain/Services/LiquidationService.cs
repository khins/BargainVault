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

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                select item_id,
                       title,
                       total_settlement,
                       auction_estimate,
                       recommendation
                from public.vw_liquidation_candidates
                where disregard = false
                  and total_settlement is not null
                order by title;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new LiquidationCandidateDto
                {
                    ItemId = reader.GetInt32(reader.GetOrdinal("item_id")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    TotalSettlement = reader.IsDBNull(reader.GetOrdinal("total_settlement"))
                        ? null
                        : reader.GetDecimal(reader.GetOrdinal("total_settlement")),
                    AuctionEstimate = reader.IsDBNull(reader.GetOrdinal("auction_estimate"))
                        ? null
                        : reader.GetDecimal(reader.GetOrdinal("auction_estimate")),
                    Recommendation = reader.IsDBNull(reader.GetOrdinal("recommendation"))
                        ? string.Empty
                        : reader.GetString(reader.GetOrdinal("recommendation"))
                });
            }

            return results;
        }

        public async Task MarkDisregardAsync(int itemId, bool value)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                update public.items
                set disregard = @disregard
                where item_id = @item_id;";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("disregard", value);
            cmd.Parameters.AddWithValue("item_id", itemId);

            await cmd.ExecuteNonQueryAsync();
        }
    }





}

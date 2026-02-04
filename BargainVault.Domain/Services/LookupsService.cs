using BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BargainVault.Domain.Services
{
    public class LookupsService : ILookupsService
    {
        private readonly string _connectionString;

        public LookupsService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<List<LookupDto>> GetBoothsAsync()
        {
            var results = new List<LookupDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT booth_id, booth_name
            FROM booths
            ORDER BY booth_name;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new LookupDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return results;
        }

        public async Task<List<LookupDto>> GetInventoryStatusesAsync()
        {
            var results = new List<LookupDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT status_id, status_name
            FROM inventory_status
            ORDER BY status_name;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new LookupDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return results;
        }
    }

}

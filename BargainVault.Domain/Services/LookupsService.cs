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

        public async Task<List<LookupDto>> GetAcquisitionStatusesAsync()
        {
            const string sql = @"
                    SELECT status_id, status_name 
                    FROM public.inventory_status
                    ORDER BY status_name;
                ";

            var results = new List<LookupDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

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


        public async Task<List<LookupDto>> GetAuctionSitesAsync()
        {
            const string sql = @"
                    SELECT auction_site_id, name
                    FROM auction_sites
                    ORDER BY name;
                ";

            var results = new List<LookupDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

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

        public async Task<List<ItemDto>> GetItemsAsync()
        {
            var results = new List<ItemDto>();

            const string sql = @"
                    SELECT item_id, title
                    FROM items
                    ORDER BY title;
                ";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new ItemDto
                {
                    ItemId = reader.GetInt32(0),
                    Title = reader.GetString(1)
                });
            }

            return results;
        }

    }

}

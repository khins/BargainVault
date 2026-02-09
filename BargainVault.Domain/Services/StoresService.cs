
using BargainVault.Domain.Models;
using Npgsql;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace BargainVault.Domain.Services
{
    public class StoresService : IStoresService
    {
        private readonly string _connectionString;

        public StoresService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<List<StoreDto>> GetStoresAsync()
        {
            var results = new List<StoreDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT
                    store_id,
                    store_name
                FROM stores
                ORDER BY store_name;", conn);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new StoreDto
                {
                    StoreId = reader.GetInt32(0),
                    StoreName = reader.GetString(1)
                });
            }

            return results;
        }
    }
}

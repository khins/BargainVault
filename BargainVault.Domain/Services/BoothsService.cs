using BargainVault.Domain.Models;
using Npgsql;
using System.Configuration;

namespace BargainVault.Domain.Services
{
    public class BoothsService : IBoothsService
    {
        private readonly string _connectionString;

        public BoothsService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<List<BoothDto>> GetBoothsAsync()
        {
            var results = new List<BoothDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT booth_id, booth_name
            FROM public.booths
            ORDER BY booth_name;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new BoothDto
                {
                    BoothId = reader.GetInt32(0),
                    BoothName = reader.GetString(1)
                });
            }

            return results;
        }
    }

}

using BargainVault.Domain.Models;
using BargainVault.Domain.Models.BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace BargainVault.Domain.Services
{
    public class GlobalSearchService : IGlobalSearchService
    {
        private readonly string _connectionString;

        public GlobalSearchService()
        {
            _connectionString =
                ConfigurationManager.ConnectionStrings["BargainVault"]?.ConnectionString
                ?? throw new InvalidOperationException(
                    "Connection string 'BargainVault' not found.");
        }

        public async Task<List<GlobalSearchResultDto>> SearchAsync(string searchText)
        {
            var results = new List<GlobalSearchResultDto>();

            if (string.IsNullOrWhiteSpace(searchText))
                return results;

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT entity_type, entity_id, display_text, secondary_text " +
                "FROM public.global_search(@search_text);",
                conn);

            cmd.Parameters.AddWithValue("search_text", searchText);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new GlobalSearchResultDto
                {
                    EntityType = Enum.Parse<GlobalSearchEntityType>(
                        reader.GetString(0),
                        ignoreCase: true),

                    EntityId = reader.GetInt32(1),

                    DisplayText = reader.GetString(2),

                    SecondaryText = reader.IsDBNull(3)
                        ? null
                        : reader.GetString(3)
                });
            }

            return results;
        }
    }
}

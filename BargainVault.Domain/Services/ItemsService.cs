using Npgsql;
using System.Configuration;

namespace BargainVault.Domain.Services
{
    public class ItemsService : IItemsService
    {
        private readonly string _connectionString;

        public ItemsService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<int> InsertItemAsync(
            int lotNumber,
            string title,
            string description,
            string imagePath,
            int quantity,
            string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT public.insert_item(
                @lot_number,
                @title,
                @description,
                @image_path,
                @quantity,
                @entered_by
            );
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("lot_number", lotNumber);
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("description", (object?)description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("image_path", (object?)imagePath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("quantity", quantity);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                await using var cmd = new NpgsqlCommand("SELECT 1;", conn);
                var result = await cmd.ExecuteScalarAsync();

                return Convert.ToInt32(result) == 1;
            }
            catch
            {
                return false;
            }
        }

    }

}

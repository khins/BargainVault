using BargainVault.Domain.Models;
using Npgsql;
using System.Configuration;
using System.Data;

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

        public async Task<List<ItemDto>> GetItemsAsync()
        {
            var results = new List<ItemDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT item_id, lot_number, title, description, created_at, image_path
                    FROM public.items
                    ORDER BY title ASC;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new ItemDto
                {
                    ItemId = reader.GetInt32(0),
                    LotNumber = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                    CreatedAt = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    ImagePath = reader.IsDBNull(5) ? null : reader.GetString(5),
                });
            }

            return results;
        }

        public async Task UpdateItemAsync(
                int itemId,
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
                    SELECT public.update_item(
                        @item_id,
                        @lot_number,
                        @title,
                        @description,
                        @image_path,
                        @quantity,
                        @entered_by
                    );
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("item_id", itemId);
            cmd.Parameters.AddWithValue("lot_number", lotNumber);
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("description", (object?)description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("image_path", (object?)imagePath ?? DBNull.Value);
            cmd.Parameters.AddWithValue("quantity", quantity);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<ItemDto> GetItemByIdAsync(int itemId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT
            item_id,
            lot_number,
            title,
            description,
            created_at, 
            image_path
          FROM items
          WHERE item_id = @item_id;",
                conn);

            cmd.Parameters.AddWithValue("item_id", itemId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                throw new InvalidOperationException(
                    $"Item with ID {itemId} not found.");

            return new ItemDto
            {
                ItemId = reader.GetInt32(0),
                LotNumber = reader.GetInt32(1),
                Title = reader.GetString(2),
                Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                CreatedAt = reader.GetDateTime(4),
                ImagePath = reader.GetString(5),
            };
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

using BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BargainVault.Domain.Services
{
    public class InventoryLocationsService : IInventoryLocationsService
    {
        private readonly string _connectionString;

        public InventoryLocationsService()
        {
            _connectionString =
                ConfigurationManager
                    .ConnectionStrings["BargainVault"]
                    ?.ConnectionString
                ?? throw new InvalidOperationException(
                    "Connection string 'BargainVault' not found.");
        }

        public async Task<int> InsertInventoryLocationAsync(
            InventoryLocationDto dto,
            string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT public.insert_inventory_location(
                @item_id,
                @booth_id,
                @status_id,
                @date_placed,
                @asking_price,
                @notes,
                @entered_by
            );";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("item_id", dto.ItemId);
            cmd.Parameters.AddWithValue("booth_id", (object?)dto.BoothId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("status_id", (object?)dto.StatusId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("date_placed", (object?)dto.DatePlaced ?? DBNull.Value);
            cmd.Parameters.AddWithValue("asking_price", (object?)dto.AskingPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("notes", (object?)dto.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            return (int)(await cmd.ExecuteScalarAsync())!;
        }

        public async Task UpdateInventoryLocationAsync(
            InventoryLocationDto dto,
            string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT public.update_inventory_location(
                @inventory_location_id,
                @item_id,
                @booth_id,
                @status_id,
                @date_placed,
                @asking_price,
                @notes,
                @entered_by
            );";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("inventory_location_id", dto.InventoryLocationId);
            cmd.Parameters.AddWithValue("item_id", dto.ItemId);
            cmd.Parameters.AddWithValue("booth_id", (object?)dto.BoothId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("status_id", (object?)dto.StatusId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("date_placed", (object?)dto.DatePlaced ?? DBNull.Value);
            cmd.Parameters.AddWithValue("asking_price", (object?)dto.AskingPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("notes", (object?)dto.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteInventoryLocationAsync(
            int inventoryLocationId,
            string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT public.delete_inventory_location(
                @inventory_location_id,
                @entered_by
            );";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("inventory_location_id", inventoryLocationId);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<InventoryLocationListDto>> GetInventoryLocationsAsync()
        {
            var results = new List<InventoryLocationListDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                SELECT
                    il.inventory_location_id,
                    i.item_id,
                    i.title,
                    b.booth_name,
                   s.status_name,
                    il.date_placed,
                    il.asking_price,
                    il.created_at
                FROM inventory_locations il
                JOIN items i ON i.item_id = il.item_id
                LEFT JOIN booths b ON b.booth_id = il.booth_id
                LEFT JOIN inventory_status s ON s.status_id = il.status_id
                ORDER BY i.title;
        ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new InventoryLocationListDto
                {
                    InventoryLocationId = reader.GetInt32(0),
                    ItemId = reader.GetInt32(1),
                    ItemTitle = reader.GetString(2),
                    BoothName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    StatusName = reader.IsDBNull(4) ? null : reader.GetString(4),
                    DatePlaced = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                    AskingPrice = reader.IsDBNull(6) ? null : reader.GetDecimal(6)
                });
            }

            return results;
        }

        public async Task<InventoryLocationDto?> GetInventoryLocationByIdAsync(
            int inventoryLocationId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
            SELECT
                inventory_location_id,
                item_id,
                booth_id,
                status_id,
                date_placed,
                asking_price,
                notes,
                created_at
            FROM inventory_locations
            WHERE inventory_location_id = @id;
             ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", inventoryLocationId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new InventoryLocationDto
            {
                InventoryLocationId = reader.GetInt32(0),
                ItemId = reader.GetInt32(1),
                BoothId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                StatusId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                DatePlaced = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                AskingPrice = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }

        public async Task DeleteInventoryLocationByItemIdAsync(
                int itemId,
                string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.delete_inventory_location_by_item(@item_id, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("item_id", itemId);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

    }


}

using global::BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace BargainVault.Domain.Services
{
    public class RetailPricesService : IRetailPricesService
    {
        private readonly string _connectionString;

        public RetailPricesService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<int> InsertRetailPriceAsync(
            RetailPriceDto dto,
            string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT public.insert_retail_price(
            @item_id,
            @store_id,
            @retail_price,
            @price_date,
            @is_sale_price,
            @notes,
            @entered_by
        );", conn);

            cmd.Parameters.AddWithValue("item_id", dto.ItemId);
            cmd.Parameters.AddWithValue("store_id", dto.StoreId);
            cmd.Parameters.AddWithValue("retail_price", dto.RetailPrice);
            cmd.Parameters.AddWithValue("price_date", (object?)dto.PriceDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("is_sale_price", dto.IsSalePrice);
            cmd.Parameters.AddWithValue("notes", (object?)dto.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            return (int)await cmd.ExecuteScalarAsync();
        }



        public async Task UpdateRetailPriceAsync(RetailPriceDto dto, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT public.update_retail_price(
                    @retail_price_id,
                    @item_id,
                    @store_id,
                    @retail_price,
                    @price_date,
                    @is_sale_price,
                    @notes,
                    @entered_by
                );", conn);

            cmd.Parameters.AddWithValue("retail_price_id", dto.RetailPriceId);
            cmd.Parameters.AddWithValue("item_id", dto.ItemId);
            cmd.Parameters.AddWithValue("store_id", dto.StoreId);
            cmd.Parameters.AddWithValue("retail_price", dto.RetailPrice);
            cmd.Parameters.AddWithValue("price_date", (object?)dto.PriceDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("is_sale_price", dto.IsSalePrice);
            cmd.Parameters.AddWithValue("notes", (object?)dto.Notes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteRetailPriceAsync(int retailPriceId, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT public.delete_retail_price(
                    @retail_price_id,
                    @entered_by
                );", conn);

            cmd.Parameters.AddWithValue("retail_price_id", retailPriceId);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<RetailPriceDto> GetRetailPriceByIdAsync(int retailPriceId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT
                    retail_price_id,
                    item_id,
                    store_id,
                    retail_price,
                    price_date,
                    is_sale_price,
                    notes
                FROM retail_prices
                WHERE retail_price_id = @id;", conn);

            cmd.Parameters.AddWithValue("id", retailPriceId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                throw new InvalidOperationException("Retail price not found.");

            return new RetailPriceDto
            {
                RetailPriceId = reader.GetInt32(0),
                ItemId = reader.GetInt32(1),
                StoreId = reader.GetInt32(2),
                RetailPrice = reader.GetDecimal(3),
                PriceDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                IsSalePrice = reader.GetBoolean(5),
                Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }

        public async Task<List<RetailPriceDto>> GetRetailPricesForItemAsync(int itemId)
        {
            var results = new List<RetailPriceDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT
                    retail_price_id,
                    item_id,
                    store_id,
                    retail_price,
                    price_date,
                    is_sale_price,
                    notes
                FROM retail_prices
                WHERE item_id = @item_id
                ORDER BY price_date DESC NULLS LAST;", conn);

            cmd.Parameters.AddWithValue("item_id", itemId);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new RetailPriceDto
                {
                    RetailPriceId = reader.GetInt32(0),
                    ItemId = reader.GetInt32(1),
                    StoreId = reader.GetInt32(2),
                    RetailPrice = reader.GetDecimal(3),
                    PriceDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    IsSalePrice = reader.GetBoolean(5),
                    Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
                });
            }

            return results;
        }

        public async Task<List<RetailPriceListDto>> GetRetailPricesAsync()
        {
            var results = new List<RetailPriceListDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT
                        rp.retail_price_id,
                        rp.item_id,
                        i.title,
                        rp.store_id,
                        s.store_name,
                        rp.retail_price,
                        rp.price_date,
                        rp.is_sale_price
                    FROM retail_prices rp
                    JOIN items i ON i.item_id = rp.item_id
                    JOIN stores s ON s.store_id = rp.store_id
                    ORDER BY rp.retail_price_id DESC;
                    ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new RetailPriceListDto
                {
                    RetailPriceId = reader.GetInt32(0),
                    ItemId = reader.GetInt32(1),
                    ItemTitle = reader.GetString(2),
                    StoreId = reader.GetInt32(3),
                    StoreName = reader.GetString(4),
                    RetailPrice = reader.GetDecimal(5),
                    PriceDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    IsSalePrice = reader.GetBoolean(7)
                });
            }

            return results;
        }
    }
}

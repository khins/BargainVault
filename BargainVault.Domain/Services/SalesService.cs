using BargainVault.Domain.Models;
using Npgsql;
using System.Data;
using System.Configuration;

namespace BargainVault.Domain.Services
{
    public class SalesService : ISalesService
    {
        private readonly string _connectionString;

        public SalesService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<int> InsertSaleAsync(SaleDto dto, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.insert_sale(@item_id, @date_sold, @qty_sold, @channel_type, @booth_id, @unit_sale_price, @discounted_rate, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("item_id", dto.ItemId);
            cmd.Parameters.AddWithValue("date_sold", dto.DateSold);
            cmd.Parameters.AddWithValue("qty_sold", dto.QtySold);
            cmd.Parameters.AddWithValue("channel_type", (object?)dto.ChannelType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("booth_id", (object?)dto.BoothId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("unit_sale_price", dto.UnitSalePrice);
            cmd.Parameters.AddWithValue("discounted_rate", (object?)dto.DiscountedRate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            return (int)(await cmd.ExecuteScalarAsync())!;
        }

        public async Task UpdateSaleAsync(SaleDto dto, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.update_sale(@sale_id, @item_id, @date_sold, @qty_sold, @channel_type, @booth_id, @unit_sale_price, @discounted_rate, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("sale_id", dto.SaleId);
            cmd.Parameters.AddWithValue("item_id", dto.ItemId);
            cmd.Parameters.AddWithValue("date_sold", dto.DateSold);
            cmd.Parameters.AddWithValue("qty_sold", dto.QtySold);
            cmd.Parameters.AddWithValue("channel_type", (object?)dto.ChannelType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("booth_id", (object?)dto.BoothId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("unit_sale_price", dto.UnitSalePrice);
            cmd.Parameters.AddWithValue("discounted_rate", (object?)dto.DiscountedRate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteSaleAsync(int saleId, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.delete_sale(@sale_id, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("sale_id", saleId);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<SaleDto?> GetSaleByIdAsync(int saleId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                @"SELECT sale_id, item_id, date_sold, qty_sold, channel_type,
                     booth_id, unit_sale_price, discounted_rate
              FROM public.sales
              WHERE sale_id = @sale_id",
                conn);

            cmd.Parameters.AddWithValue("sale_id", saleId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read())
                return null;

            return new SaleDto
            {
                SaleId = reader.GetInt32(0),
                ItemId = reader.GetInt32(1),
                DateSold = reader.GetDateTime(2),
                QtySold = reader.GetInt32(3),
                ChannelType = reader.IsDBNull(4) ? null : reader.GetString(4),
                BoothId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                UnitSalePrice = reader.GetDecimal(6),
                DiscountedRate = reader.IsDBNull(7) ? null : reader.GetDecimal(7)
            };
        }
    }

}

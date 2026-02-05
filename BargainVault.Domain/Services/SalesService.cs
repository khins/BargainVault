using BargainVault.Domain.Models;
using Npgsql;
using System.Data;
using System.Configuration;

namespace BargainVault.Domain.Services
{
    public class SalesService : ISalesService
    {
        private readonly string _connectionString;
        private readonly IInventoryLocationsService _inventoryLocationsService;


        public SalesService(IInventoryLocationsService inventoryLocationsService)
        {
            _inventoryLocationsService = inventoryLocationsService;
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<int> InsertSaleAsync(SaleDto dto, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var tx = await conn.BeginTransactionAsync();

            try
            {
                // 1️⃣ Insert sale
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

                // 2️⃣ Remove inventory
                await _inventoryLocationsService
                    .DeleteInventoryLocationByItemIdAsync(
                        dto.ItemId,
                        enteredBy);

                await tx.CommitAsync();

                return (int)(await cmd.ExecuteScalarAsync())!;
            }
            catch 
            {
                await tx.RollbackAsync();
                throw;
            }            
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

        public async Task<List<SalesMonthlySummaryDto>> GetMonthlySalesSummaryAsync()
        {
            var results = new List<SalesMonthlySummaryDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                        SELECT
                            EXTRACT(YEAR FROM date_sold)::int AS year,
                            EXTRACT(MONTH FROM date_sold)::int AS month,
                            SUM(qty_sold * unit_sale_price) AS total_sales,
                            SUM(qty_sold) AS total_quantity
                        FROM sales
                        GROUP BY 1, 2
                        ORDER BY 1 DESC, 2 DESC;
                    ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new SalesMonthlySummaryDto
                {
                    Year = reader.GetInt32(0),
                    Month = reader.GetInt32(1),
                    TotalSales = reader.GetDecimal(2),
                    TotalQuantity = reader.GetInt32(3)
                });
            }

            return results;
        }

        public async Task<List<SalesMonthlyDetailDto>> GetSalesForMonthAsync(int year, int month)
        {
            var results = new List<SalesMonthlyDetailDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT
                        s.sale_id,
                        s.date_sold,
                        i.title,
                        s.qty_sold,
                        s.unit_sale_price,
                        s.channel_type,
                        b.booth_name
                    FROM sales s
                    JOIN items i ON i.item_id = s.item_id
                    LEFT JOIN booths b ON b.booth_id = s.booth_id
                    WHERE
                        EXTRACT(YEAR FROM s.date_sold) = @year
                        AND EXTRACT(MONTH FROM s.date_sold) = @month
                    ORDER BY s.date_sold;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("year", year);
            cmd.Parameters.AddWithValue("month", month);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new SalesMonthlyDetailDto
                {
                    SaleId = reader.GetInt32(0),
                    DateSold = reader.GetDateTime(1),
                    ItemTitle = reader.GetString(2),
                    QtySold = reader.GetInt32(3),
                    UnitSalePrice = reader.GetDecimal(4),
                    ChannelType = reader.IsDBNull(5) ? null : reader.GetString(5),
                    BoothName = reader.IsDBNull(6) ? null : reader.GetString(6)
                });
            }

            return results;
        }


    }

}

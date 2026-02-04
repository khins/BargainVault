using BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace BargainVault.Domain.Services
{
    public class AcquisitionsService : IAcquisitionsService
    {
        private readonly string _connectionString;

        public AcquisitionsService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<int> InsertAcquisitionAsync(
            int itemId,
            string sourceType,
            int? auctionSiteId,
            DateTime? dateAcquired,
            int? qtyAcquired,
            decimal? unitHammerPrice,
            decimal? buyerPremium,
            decimal? taxRate,
            decimal? salesTaxPaid,
            decimal? totalSettlement,
            int? statusId,
            bool personal,
            bool businessExpense,
            string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.insert_acquisition(@item_id, @source_type, @auction_site_id, " +
                "@date_acquired, @qty_acquired, @unit_hammer_price, @buyer_premium, @tax_rate, " +
                "@sales_tax_paid, @total_settlement, @status_id, @personal, @business_expense, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("item_id", itemId);
            cmd.Parameters.AddWithValue("source_type", (object?)sourceType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("auction_site_id", (object?)auctionSiteId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("date_acquired", (object?)dateAcquired ?? DBNull.Value);
            cmd.Parameters.AddWithValue("qty_acquired", (object?)qtyAcquired ?? DBNull.Value);
            cmd.Parameters.AddWithValue("unit_hammer_price", (object?)unitHammerPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("buyer_premium", (object?)buyerPremium ?? DBNull.Value);
            cmd.Parameters.AddWithValue("tax_rate", (object?)taxRate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("sales_tax_paid", (object?)salesTaxPaid ?? DBNull.Value);
            cmd.Parameters.AddWithValue("total_settlement", (object?)totalSettlement ?? DBNull.Value);
            cmd.Parameters.AddWithValue("status_id", (object?)statusId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("personal", personal);
            cmd.Parameters.AddWithValue("business_expense", businessExpense);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task UpdateAcquisitionAsync(AcquisitionDto acquisition)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT public.update_acquisition(
                        @acq_id,
                        @item_id,
                        @source_type,
                        @auction_site_id,
                        @date_acquired,
                        @qty_acquired,
                        @unit_hammer_price,
                        @buyer_premium,
                        @tax_rate,
                        @sales_tax_paid,
                        @total_settlement,
                        @status_id,
                        @personal,
                        @business_expense,
                        @entered_by
                    );
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("acq_id", acquisition.AcqId);
            cmd.Parameters.AddWithValue("item_id", acquisition.ItemId);
            cmd.Parameters.AddWithValue("source_type", acquisition.SourceType ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("auction_site_id", acquisition.AuctionSiteId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("date_acquired", acquisition.DateAcquired ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("qty_acquired", acquisition.QtyAcquired);
            cmd.Parameters.AddWithValue("unit_hammer_price", acquisition.UnitHammerPrice ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("buyer_premium", acquisition.BuyerPremium ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("tax_rate", acquisition.TaxRate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("sales_tax_paid", acquisition.SalesTaxPaid ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("total_settlement", acquisition.TotalSettlement ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("status_id", acquisition.StatusId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("personal", acquisition.Personal);
            cmd.Parameters.AddWithValue("business_expense", acquisition.BusinessExpense);
            cmd.Parameters.AddWithValue("entered_by", Environment.UserName);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAcquisitionAsync(int acqId, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.delete_acquisition(@acq_id, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("acq_id", acqId);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IList<AcquisitionListDto>> GetAcquisitionsAsync()
        {
            var results = new List<AcquisitionListDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                   SELECT
                       a.acq_id,
                       a.item_id,
                       i.title AS item_title,
                       a.date_acquired,
                       a.total_settlement,
                       s.status_name ,
                       a.personal,
                       a.business_expense
                   FROM acquisitions a
                   JOIN items i ON i.item_id = a.item_id
                   LEFT JOIN inventory_status s ON s.status_id = a.status_id
                   ORDER BY a.acq_id DESC;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new AcquisitionListDto
                {
                    AcqId = reader.GetInt32(0),
                    ItemId = reader.GetInt32(1),
                    ItemTitle = reader.GetString(2),
                    DateAcquired = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    TotalSettlement = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                    StatusName = reader.IsDBNull(5) ? null : reader.GetString(5),
                    Personal = reader.GetBoolean(6),
                    BusinessExpense = reader.GetBoolean(7)
                });
            }

            return results;
        }

        public async Task<AcquisitionDto> GetAcquisitionByIdAsync(int acqId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT
                        acq_id,
                        item_id,
                        source_type,
                        auction_site_id,
                        date_acquired,
                        qty_acquired,
                        unit_hammer_price,
                        buyer_premium,
                        tax_rate,
                        sales_tax_paid,
                        total_settlement,
                        status_id,
                        personal,
                        business_expense
                    FROM acquisitions
                    WHERE acq_id = @acq_id;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("acq_id", acqId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new AcquisitionDto
            {
                AcqId = reader.GetInt32(0),
                ItemId = reader.GetInt32(1),
                SourceType = reader.IsDBNull(2) ? null : reader.GetString(2),
                AuctionSiteId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                DateAcquired = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                QtyAcquired = reader.IsDBNull(5) ? 1 : reader.GetInt32(5),
                UnitHammerPrice = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                BuyerPremium = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                TaxRate = reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                SalesTaxPaid = reader.IsDBNull(9) ? null : reader.GetDecimal(9),
                TotalSettlement = reader.IsDBNull(10) ? null : reader.GetDecimal(10),
                StatusId = reader.IsDBNull(11) ? null : reader.GetInt32(11),
                Personal = reader.GetBoolean(12),
                BusinessExpense = reader.GetBoolean(13)
            };
        }

        public async Task<List<AcquisitionLookupDto>> GetAcquisitionLookupAsync()
        {
            var results = new List<AcquisitionLookupDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT
                        a.acq_id,
                        i.title || ' (#' || a.acq_id || ')' AS display_text
                    FROM acquisitions a
                    JOIN items i ON i.item_id = a.item_id
                    ORDER BY i.title;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new AcquisitionLookupDto
                {
                    AcqId = reader.GetInt32(0),
                    DisplayText = reader.GetString(1)
                });
            }

            return results;
        }



    }
}


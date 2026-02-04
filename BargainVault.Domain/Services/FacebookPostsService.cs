using BargainVault.Domain.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BargainVault.Domain.Services
{
    public class FacebookPostsService : IFacebookPostsService
    {
        private readonly string _connectionString;

        public FacebookPostsService()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["BargainVault"]
                ?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'BargainVault' not found.");
        }

        public async Task<int> InsertFacebookPostAsync(FacebookPostDto dto, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.insert_facebook_post(@acq_id, @post_date, @post_title, @post_description, @asking_price, @boosted, @mark_as_sold, @renew_date, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("acq_id", dto.AcqId);
            cmd.Parameters.AddWithValue("post_date", (object?)dto.PostDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("post_title", (object?)dto.PostTitle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("post_description", (object?)dto.PostDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("asking_price", (object?)dto.AskingPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("boosted", dto.Boosted);
            cmd.Parameters.AddWithValue("mark_as_sold", dto.MarkAsSold);
            cmd.Parameters.AddWithValue("renew_date", (object?)dto.RenewDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            return (int)(await cmd.ExecuteScalarAsync())!;
        }

        public async Task UpdateFacebookPostAsync(FacebookPostDto dto, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.update_facebook_post(@post_id, @post_date, @post_title, @post_description, @asking_price, @boosted, @mark_as_sold, @renew_date, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("post_id", dto.PostId);
            cmd.Parameters.AddWithValue("post_date", (object?)dto.PostDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("post_title", (object?)dto.PostTitle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("post_description", (object?)dto.PostDescription ?? DBNull.Value);
            cmd.Parameters.AddWithValue("asking_price", (object?)dto.AskingPrice ?? DBNull.Value);
            cmd.Parameters.AddWithValue("boosted", dto.Boosted);
            cmd.Parameters.AddWithValue("mark_as_sold", dto.MarkAsSold);
            cmd.Parameters.AddWithValue("renew_date", (object?)dto.RenewDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteScalarAsync();
        }

        public async Task DeleteFacebookPostAsync(int postId, string enteredBy)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(
                "SELECT public.delete_facebook_post(@post_id, @entered_by)",
                conn);

            cmd.Parameters.AddWithValue("post_id", postId);
            cmd.Parameters.AddWithValue("entered_by", enteredBy);

            await cmd.ExecuteScalarAsync();
        }

        public async Task<List<FacebookPostListDto>> GetFacebookPostsAsync()
        {
            var results = new List<FacebookPostListDto>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT
                        fp.post_id,
                        fp.acq_id,
                        i.title,
                        fp.post_date,
                        fp.asking_price,
                        fp.boosted,
                        fp.mark_as_sold
                    FROM facebook_posts fp
                    JOIN acquisitions a ON a.acq_id = fp.acq_id
                    JOIN items i ON i.item_id = a.item_id
                    ORDER BY fp.post_date DESC NULLS LAST;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new FacebookPostListDto
                {
                    PostId = reader.GetInt32(0),
                    AcqId = reader.GetInt32(1),
                    ItemTitle = reader.GetString(2),
                    PostDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    AskingPrice = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                    Boosted = reader.GetBoolean(5),
                    MarkAsSold = reader.GetBoolean(6)
                });
            }

            return results;
        }

        public async Task<FacebookPostDto?> GetFacebookPostByIdAsync(int postId)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            const string sql = @"
                    SELECT
                        post_id,
                        acq_id,
                        post_date,
                        post_title,
                        post_description,
                        asking_price,
                        boosted,
                        mark_as_sold,
                        renew_date
                    FROM facebook_posts
                    WHERE post_id = @post_id;
                ";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("post_id", postId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new FacebookPostDto
            {
                PostId = reader.GetInt32(0),
                AcqId = reader.GetInt32(1),
                PostDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                PostTitle = reader.IsDBNull(3) ? null : reader.GetString(3),
                PostDescription = reader.IsDBNull(4) ? null : reader.GetString(4),
                AskingPrice = reader.IsDBNull(5) ? null : reader.GetDecimal(5),
                Boosted = reader.GetBoolean(6),
                MarkAsSold = reader.GetBoolean(7),
                RenewDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
            };
        }


    }

}

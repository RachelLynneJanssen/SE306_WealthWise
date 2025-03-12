using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WealthWise_RCD.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "AdvisorId", "Content", "ModificationDate", "Price", "PublicationDate", "RecommendationScore", "Title", "Topic" },
                values: new object[,]
                {
                    { 1, "7a87ca4c-6633-4748-9915-0613a5fae389", "This is the content of Blog Post 1.", null, 0m, new DateTime(2025, 3, 11, 21, 48, 22, 508, DateTimeKind.Local).AddTicks(487), 0, "Blog Post 1", "Topic" },
                    { 2, "7a87ca4c-6633-4748-9915-0613a5fae389", "This is the content of Blog Post 2.", null, 0m, new DateTime(2025, 3, 11, 21, 48, 22, 508, DateTimeKind.Local).AddTicks(532), 0, "Blog Post 2", "Topic" },
                    { 3, "7a87ca4c-6633-4748-9915-0613a5fae389", "This is the content of Blog Post 3.", null, 0m, new DateTime(2025, 3, 11, 21, 48, 22, 508, DateTimeKind.Local).AddTicks(535), 0, "Blog Post 3", "Topic" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

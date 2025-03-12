using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WealthWise_RCD.Migrations
{
    /// <inheritdoc />
    public partial class BlogDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "AdvisorId", "Content", "ModificationDate", "Price", "PublicationDate", "RecommendationScore", "Title", "Topic" },
                values: new object[,]
                {
                    { 1, "7d948a2b-f258-4cf8-b8c8-913806968f8f", "Pulled from the database (Quote by Sigmund Freud)!", null, 0m, new DateTime(2025, 3, 12, 10, 31, 1, 831, DateTimeKind.Local).AddTicks(8885), 0, "Time spent with cats is never wasted.", "Topic" },
                    { 2, "7d948a2b-f258-4cf8-b8c8-913806968f8f", "Pulled from the database (Quote by Mark Twain)!", null, 0m, new DateTime(2025, 3, 12, 10, 31, 1, 831, DateTimeKind.Local).AddTicks(8929), 0, "You can never be truly at home without a cat.", "Topic" },
                    { 3, "7d948a2b-f258-4cf8-b8c8-913806968f8f", "Soft as twilight, sleek as night,\\n\r\n                                       A shadow drifts in silver light.\\n\r\n                                       Silent steps on wooden floors,\\n\r\n                                       A ghost that slips through open doors.\\n\r\n                                       \\n\r\n                                       Eyes like lanterns, gleam and glow,\\n\r\n                                       Holding secrets none may know.\\n\r\n                                       A fleeting brush, a velvet sigh,\\n\r\n                                       Then gone—like wind, like lullabies.\\n\r\n                                       \\n\r\n                                       Curled in sunlight, lost in dreams,\\n\r\n                                       Of silent hunts by moonlit streams.\\n\r\n                                       No chains, no ties—just fleeting grace,\\n\r\n                                       A traveler in time and space.\\n\r\n                                       \\n\r\n                                       And when you sleep, beneath the stars,\\n\r\n                                       A whisper hums from realms afar.\\n\r\n                                       A cat’s soft purr, a sacred song,\\n\r\n                                       Reminding you—you do belong.", null, 0m, new DateTime(2025, 3, 12, 10, 31, 1, 831, DateTimeKind.Local).AddTicks(8931), 0, "The smallest feline is a masterpiece. - Leonardo Da Vinci", "Topic" }
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

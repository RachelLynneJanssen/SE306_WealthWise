using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthWise_RCD.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDefaultBlogPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "PublicationDate", "Title" },
                values: new object[] { "Pulled from the database (Quote by Sigmund Freud)!", new DateTime(2025, 3, 11, 22, 16, 26, 578, DateTimeKind.Local).AddTicks(4427), "Time spent with cats is never wasted." });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Content", "PublicationDate", "Title" },
                values: new object[] { "Pulled from the database (Quote by Mark Twain)!", new DateTime(2025, 3, 11, 22, 16, 26, 578, DateTimeKind.Local).AddTicks(4475), "You can never be truly at home without a cat." });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Content", "PublicationDate", "Title" },
                values: new object[] { "Soft as twilight, sleek as night,\\n\r\n                                       A shadow drifts in silver light.\\n\r\n                                       Silent steps on wooden floors,\\n\r\n                                       A ghost that slips through open doors.\\n\r\n                                       \\n\r\n                                       Eyes like lanterns, gleam and glow,\\n\r\n                                       Holding secrets none may know.\\n\r\n                                       A fleeting brush, a velvet sigh,\\n\r\n                                       Then gone—like wind, like lullabies.\\n\r\n                                       \\n\r\n                                       Curled in sunlight, lost in dreams,\\n\r\n                                       Of silent hunts by moonlit streams.\\n\r\n                                       No chains, no ties—just fleeting grace,\\n\r\n                                       A traveler in time and space.\\n\r\n                                       \\n\r\n                                       And when you sleep, beneath the stars,\\n\r\n                                       A whisper hums from realms afar.\\n\r\n                                       A cat’s soft purr, a sacred song,\\n\r\n                                       Reminding you—you do belong.", new DateTime(2025, 3, 11, 22, 16, 26, 578, DateTimeKind.Local).AddTicks(4477), "The smallest feline is a masterpiece. - Leonardo Da Vinci" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Content", "PublicationDate", "Title" },
                values: new object[] { "This is the content of Blog Post 1.", new DateTime(2025, 3, 11, 21, 48, 22, 508, DateTimeKind.Local).AddTicks(487), "Blog Post 1" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Content", "PublicationDate", "Title" },
                values: new object[] { "This is the content of Blog Post 2.", new DateTime(2025, 3, 11, 21, 48, 22, 508, DateTimeKind.Local).AddTicks(532), "Blog Post 2" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Content", "PublicationDate", "Title" },
                values: new object[] { "This is the content of Blog Post 3.", new DateTime(2025, 3, 11, 21, 48, 22, 508, DateTimeKind.Local).AddTicks(535), "Blog Post 3" });
        }
    }
}

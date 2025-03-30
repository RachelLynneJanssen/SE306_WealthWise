using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthWise_RCD.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 23, 12, 26, 58, 267, DateTimeKind.Local).AddTicks(9545));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 23, 12, 26, 58, 267, DateTimeKind.Local).AddTicks(9611));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 23, 12, 26, 58, 267, DateTimeKind.Local).AddTicks(9616));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 12, 10, 31, 1, 831, DateTimeKind.Local).AddTicks(8885));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 12, 10, 31, 1, 831, DateTimeKind.Local).AddTicks(8929));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 12, 10, 31, 1, 831, DateTimeKind.Local).AddTicks(8931));
        }
    }
}

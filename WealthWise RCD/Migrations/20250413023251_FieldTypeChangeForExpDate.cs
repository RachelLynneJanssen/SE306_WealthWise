using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthWise_RCD.Migrations
{
    /// <inheritdoc />
    public partial class FieldTypeChangeForExpDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpDate",
                table: "Payments",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "ExpDate",
                table: "Payments",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}

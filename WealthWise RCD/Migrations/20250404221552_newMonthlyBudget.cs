using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthWise_RCD.Migrations
{
    /// <inheritdoc />
    public partial class newMonthlyBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MonthlyBudgets_BudgetId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BudgetId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MonthlyBudgets",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "MonthlyBudgets",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyBudgets_UserID",
                table: "MonthlyBudgets",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyBudgets_AspNetUsers_UserID",
                table: "MonthlyBudgets",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyBudgets_AspNetUsers_UserID",
                table: "MonthlyBudgets");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyBudgets_UserID",
                table: "MonthlyBudgets");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MonthlyBudgets");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "MonthlyBudgets");

            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BudgetId",
                table: "AspNetUsers",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MonthlyBudgets_BudgetId",
                table: "AspNetUsers",
                column: "BudgetId",
                principalTable: "MonthlyBudgets",
                principalColumn: "Id");
        }
    }
}

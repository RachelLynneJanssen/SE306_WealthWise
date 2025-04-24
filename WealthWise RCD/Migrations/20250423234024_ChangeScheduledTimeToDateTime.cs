using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WealthWise_RCD.Migrations
{
    /// <inheritdoc />
    public partial class ChangeScheduledTimeToDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledTime",
                table: "Appointments",
                type: "datetime", // or "datetime(6)" if you want microseconds
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ScheduledTime",
                table: "Appointments",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime"
            );
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingLot1._0.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AggregatedPaymentMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyPassId",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonthlyPassId",
                table: "Payments",
                type: "int",
                nullable: true);
        }
    }
}

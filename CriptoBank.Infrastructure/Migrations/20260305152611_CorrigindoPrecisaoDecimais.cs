using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CriptoBank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoPrecisaoDecimais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AveragePrice",
                table: "Holdings",
                type: "numeric(18,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AveragePrice",
                table: "Holdings",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)");
        }
    }
}

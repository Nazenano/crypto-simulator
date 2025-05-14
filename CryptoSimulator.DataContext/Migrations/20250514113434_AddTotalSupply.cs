using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoSimulator.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalSupply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalSupply",
                table: "CryptoCurrencies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSupply",
                table: "CryptoCurrencies");
        }
    }
}

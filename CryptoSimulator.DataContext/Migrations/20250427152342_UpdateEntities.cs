using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoSimulator.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CryptoCurrencyId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CryptoCurrencyId",
                table: "Portfolios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Portfolios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CryptoCurrencyId",
                table: "Transactions",
                column: "CryptoCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId1",
                table: "Transactions",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_CryptoCurrencyId",
                table: "Portfolios",
                column: "CryptoCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_UserId1",
                table: "Portfolios",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_CryptoCurrencies_CryptoCurrencyId",
                table: "Portfolios",
                column: "CryptoCurrencyId",
                principalTable: "CryptoCurrencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Users_UserId1",
                table: "Portfolios",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_CryptoCurrencies_CryptoCurrencyId",
                table: "Transactions",
                column: "CryptoCurrencyId",
                principalTable: "CryptoCurrencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserId1",
                table: "Transactions",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_CryptoCurrencies_CryptoCurrencyId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Users_UserId1",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_CryptoCurrencies_CryptoCurrencyId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserId1",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CryptoCurrencyId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_UserId1",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_CryptoCurrencyId",
                table: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_Portfolios_UserId1",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "CryptoCurrencyId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CryptoCurrencyId",
                table: "Portfolios");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Portfolios");
        }
    }
}

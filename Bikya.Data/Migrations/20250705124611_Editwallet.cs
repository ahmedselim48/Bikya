using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bikya.Data.Migrations
{
    /// <inheritdoc />
    public partial class Editwallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Wallets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LinkedPaymentMethod",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "LinkedPaymentMethod",
                table: "Wallets");
        }
    }
}

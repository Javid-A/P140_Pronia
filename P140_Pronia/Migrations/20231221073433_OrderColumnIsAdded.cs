using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P140_Pronia.Migrations
{
    public partial class OrderColumnIsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Order",
                table: "Informations",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Informations");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderTable_CustomerID",
                table: "OrderTable",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTable_Users_CustomerID",
                table: "OrderTable",
                column: "CustomerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTable_Users_CustomerID",
                table: "OrderTable");

            migrationBuilder.DropIndex(
                name: "IX_OrderTable_CustomerID",
                table: "OrderTable");
        }
    }
}

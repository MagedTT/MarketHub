using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameShippingAddressColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressEntityId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressEntityId",
                table: "Orders",
                newName: "ShippingAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingAddressEntityId",
                table: "Orders",
                newName: "IX_Orders_ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressId",
                table: "Orders",
                column: "ShippingAddressId",
                principalTable: "ShippingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingAddressId",
                table: "Orders",
                newName: "ShippingAddressEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingAddressId",
                table: "Orders",
                newName: "IX_Orders_ShippingAddressEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingAddresses_ShippingAddressEntityId",
                table: "Orders",
                column: "ShippingAddressEntityId",
                principalTable: "ShippingAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

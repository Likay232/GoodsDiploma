using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OperationTableProductKeysEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_operations_products_ProductId",
                table: "inventory_operations");

            migrationBuilder.DropForeignKey(
                name: "FK_refund_operations_products_ProductId",
                table: "refund_operations");

            migrationBuilder.DropForeignKey(
                name: "FK_supply_operations_products_ProductId",
                table: "supply_operations");

            migrationBuilder.DropForeignKey(
                name: "FK_write_off_operations_products_ProductId",
                table: "write_off_operations");

            migrationBuilder.DropIndex(
                name: "IX_refund_operations_ProductId",
                table: "refund_operations");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "refund_operations");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "write_off_operations",
                newName: "ProductInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_write_off_operations_ProductId",
                table: "write_off_operations",
                newName: "IX_write_off_operations_ProductInfoId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "supply_operations",
                newName: "ProductInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_supply_operations_ProductId",
                table: "supply_operations",
                newName: "IX_supply_operations_ProductInfoId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "inventory_operations",
                newName: "ProductInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_operations_ProductId",
                table: "inventory_operations",
                newName: "IX_inventory_operations_ProductInfoId");

            migrationBuilder.AddColumn<bool>(
                name: "IsShipped",
                table: "sale_operations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_operations_product_information_ProductInfoId",
                table: "inventory_operations",
                column: "ProductInfoId",
                principalTable: "product_information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supply_operations_product_information_ProductInfoId",
                table: "supply_operations",
                column: "ProductInfoId",
                principalTable: "product_information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_write_off_operations_product_information_ProductInfoId",
                table: "write_off_operations",
                column: "ProductInfoId",
                principalTable: "product_information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventory_operations_product_information_ProductInfoId",
                table: "inventory_operations");

            migrationBuilder.DropForeignKey(
                name: "FK_supply_operations_product_information_ProductInfoId",
                table: "supply_operations");

            migrationBuilder.DropForeignKey(
                name: "FK_write_off_operations_product_information_ProductInfoId",
                table: "write_off_operations");

            migrationBuilder.DropColumn(
                name: "IsShipped",
                table: "sale_operations");

            migrationBuilder.RenameColumn(
                name: "ProductInfoId",
                table: "write_off_operations",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_write_off_operations_ProductInfoId",
                table: "write_off_operations",
                newName: "IX_write_off_operations_ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductInfoId",
                table: "supply_operations",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_supply_operations_ProductInfoId",
                table: "supply_operations",
                newName: "IX_supply_operations_ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductInfoId",
                table: "inventory_operations",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_inventory_operations_ProductInfoId",
                table: "inventory_operations",
                newName: "IX_inventory_operations_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "refund_operations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_refund_operations_ProductId",
                table: "refund_operations",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventory_operations_products_ProductId",
                table: "inventory_operations",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_refund_operations_products_ProductId",
                table: "refund_operations",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supply_operations_products_ProductId",
                table: "supply_operations",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_write_off_operations_products_ProductId",
                table: "write_off_operations",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

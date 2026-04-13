using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSaleOperationProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sale_operations_products_ProductId",
                table: "sale_operations");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "sale_operations",
                newName: "ProductInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_sale_operations_ProductId",
                table: "sale_operations",
                newName: "IX_sale_operations_ProductInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_sale_operations_product_information_ProductInfoId",
                table: "sale_operations",
                column: "ProductInfoId",
                principalTable: "product_information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sale_operations_product_information_ProductInfoId",
                table: "sale_operations");

            migrationBuilder.RenameColumn(
                name: "ProductInfoId",
                table: "sale_operations",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_sale_operations_ProductInfoId",
                table: "sale_operations",
                newName: "IX_sale_operations_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_sale_operations_products_ProductId",
                table: "sale_operations",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

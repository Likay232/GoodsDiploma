using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PathToFileForSaleOperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathToFile",
                table: "sale_operations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathToFile",
                table: "sale_operations");
        }
    }
}

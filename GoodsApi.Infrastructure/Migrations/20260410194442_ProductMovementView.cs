using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductMovementView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
create view public.product_movement_view
            (operationdate, operationtype, productname, ""Article"", ""Size"", ""Color"", quantity, counteragent) as
SELECT s.""AcceptanceDate"" AS operationdate,
       'Supply'::text     AS operationtype,
       prod.""Name""        AS productname,
       pi.""Article"",
       pi.""Size"",
       pi.""Color"",
       s.""Amount""         AS quantity,
       prov.""Name""        AS counteragent
FROM supply_operations s
         JOIN product_information pi ON s.""ProductInfoId"" = pi.""Id""
         JOIN products prod ON pi.""ProductId"" = prod.""Id""
         LEFT JOIN providers prov ON s.""ProviderId"" = prov.""Id""
UNION ALL
SELECT s.""SaleDate"" AS operationdate,
       'Sale'::text AS operationtype,
       prod.""Name""  AS productname,
       pi.""Article"",
       pi.""Size"",
       pi.""Color"",
       - s.""Amount"" AS quantity,
       u.""Username"" AS counteragent
FROM sale_operations s
         JOIN product_information pi ON s.""ProductInfoId"" = pi.""Id""
         JOIN products prod ON pi.""ProductId"" = prod.""Id""
         JOIN users u ON s.""UserId"" = u.""Id""
UNION ALL
SELECT w.""WriteOffDate"" AS operationdate,
       'WriteOff'::text AS operationtype,
       prod.""Name""      AS productname,
       pi.""Article"",
       pi.""Size"",
       pi.""Color"",
       - w.""Amount""     AS quantity,
       u.""Username""     AS counteragent
FROM write_off_operations w
         JOIN product_information pi ON w.""ProductInfoId"" = pi.""Id""
         JOIN products prod ON pi.""ProductId"" = prod.""Id""
         JOIN users u ON w.""UserId"" = u.""Id""
UNION ALL
SELECT r.""RefundDate"" AS operationdate,
       'Refund'::text AS operationtype,
       prod.""Name""    AS productname,
       pi.""Article"",
       pi.""Size"",
       pi.""Color"",
       r.""Amount""     AS quantity,
       u.""Username""   AS counteragent
FROM refund_operations r
         JOIN sale_operations s ON r.""SaleOperationId"" = s.""Id""
         JOIN product_information pi ON s.""ProductInfoId"" = pi.""Id""
         JOIN products prod ON pi.""ProductId"" = prod.""Id""
         JOIN users u ON r.""UserId"" = u.""Id""
UNION ALL
SELECT i.""InventoryOperationDate"" AS operationdate,
       'Inventory'::text          AS operationtype,
       prod.""Name""                AS productname,
       pi.""Article"",
       pi.""Size"",
       pi.""Color"",
       i.""MismatchAmount""         AS quantity,
       u.""Username""               AS counteragent
FROM inventory_operations i
         JOIN product_information pi ON i.""ProductInfoId"" = pi.""Id""
         JOIN products prod ON pi.""ProductId"" = prod.""Id""
         JOIN users u ON i.""UserId"" = u.""Id"";

alter table public.product_movement_view
    owner to postgres;

        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view public.product_movement_view;");

        }
    }
}

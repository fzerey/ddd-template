using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FzereyDDDStarter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemUnitPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItem",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(
                """
                UPDATE "OrderItem" AS oi
                SET "UnitPrice" = i."Price"
                FROM "Item" AS i
                WHERE oi."ItemId" = i."Id";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderItem");
        }
    }
}

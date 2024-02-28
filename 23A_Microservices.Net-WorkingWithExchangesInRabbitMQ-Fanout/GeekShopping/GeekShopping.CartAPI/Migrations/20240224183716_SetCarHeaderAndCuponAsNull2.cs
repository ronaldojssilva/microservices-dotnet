using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.CartAPI.Migrations
{
    /// <inheritdoc />
    public partial class SetCarHeaderAndCuponAsNull2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_detail_cart_header_CartHeaderId",
                table: "cart_detail");

            migrationBuilder.AlterColumn<long>(
                name: "CartHeaderId",
                table: "cart_detail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_detail_cart_header_CartHeaderId",
                table: "cart_detail",
                column: "CartHeaderId",
                principalTable: "cart_header",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_detail_cart_header_CartHeaderId",
                table: "cart_detail");

            migrationBuilder.AlterColumn<long>(
                name: "CartHeaderId",
                table: "cart_detail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_detail_cart_header_CartHeaderId",
                table: "cart_detail",
                column: "CartHeaderId",
                principalTable: "cart_header",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

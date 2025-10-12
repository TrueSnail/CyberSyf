using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Book_Store.Migrations
{
    /// <inheritdoc />
    public partial class EBookPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EBooks",
                table: "EBooks");

            migrationBuilder.DropColumn(
                name: "EBookId",
                table: "EBooks");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "EBooks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EBooks",
                table: "EBooks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EBookPurchases",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EBookId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    PurchaseTimestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EBookPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EBookPurchases_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EBookPurchases_EBooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "EBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EBookPurchases_EBookId",
                table: "EBookPurchases",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_EBookPurchases_UserId",
                table: "EBookPurchases",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EBookPurchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EBooks",
                table: "EBooks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EBooks");

            migrationBuilder.AddColumn<Guid>(
                name: "EBookId",
                table: "EBooks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EBooks",
                table: "EBooks",
                column: "EBookId");
        }
    }
}

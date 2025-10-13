using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Book_Store.Migrations
{
    /// <inheritdoc />
    public partial class ebany2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnforcePasswordPolicy",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PasswordExpiryDays",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnforcePasswordPolicy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordExpiryDays",
                table: "AspNetUsers");
        }
    }
}

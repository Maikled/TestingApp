using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Users_OwnerID",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Sources_OwnerID",
                table: "Sources");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Sources",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Sources");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_OwnerID",
                table: "Sources",
                column: "OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sources_Users_OwnerID",
                table: "Sources",
                column: "OwnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApp.Migrations
{
    /// <inheritdoc />
    public partial class Add_test_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sources_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerID = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    InputData = table.Column<string>(type: "TEXT", nullable: false),
                    OutputData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tests_Sources_SourceID",
                        column: x => x.SourceID,
                        principalTable: "Sources",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tests_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sources_OwnerID",
                table: "Sources",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_OwnerID",
                table: "Tests",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_SourceID",
                table: "Tests",
                column: "SourceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}

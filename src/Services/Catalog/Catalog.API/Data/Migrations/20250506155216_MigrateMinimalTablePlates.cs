using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    public partial class MigrateMinimalTablePlates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservedPlates");

            migrationBuilder.DropTable(
                name: "SoldPlates");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Plates",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Plates");

            migrationBuilder.CreateTable(
                name: "ReservedPlates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reserved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedPlates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldPlates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sold = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldPlates", x => x.Id);
                });
        }
    }
}

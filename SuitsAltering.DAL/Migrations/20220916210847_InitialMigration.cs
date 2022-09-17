using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuitsAltering.DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alterings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClothingType = table.Column<int>(type: "int", nullable: false),
                    LeftAdjustment = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RightAdjustment = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AlteringStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alterings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alterings");
        }
    }
}

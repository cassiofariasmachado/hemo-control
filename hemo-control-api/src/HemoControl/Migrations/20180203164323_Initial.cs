using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HemoControl.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(maxLength: 30, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Weigth = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Infusion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BleedingLocal = table.Column<string>(maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FactorBrand = table.Column<string>(maxLength: 50, nullable: false),
                    FactorLot = table.Column<string>(maxLength: 20, nullable: false),
                    FactorUnity = table.Column<int>(nullable: false),
                    IsBleeding = table.Column<bool>(nullable: false),
                    IsHemarthrosis = table.Column<bool>(nullable: false),
                    IsTreatmentContinuation = table.Column<bool>(nullable: false),
                    TreatmentLocal = table.Column<string>(maxLength: 50, nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    UserWeigth = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infusion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infusion_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Infusion_Id",
                table: "Infusion",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Infusion_UserId",
                table: "Infusion",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Infusion");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

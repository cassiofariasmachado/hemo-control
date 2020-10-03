﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(maxLength: 30, nullable: false),
                    Username = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 60, nullable: false),
                    Weigth = table.Column<decimal>(type: "decimal(19,5)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Infusions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    UserWeigth = table.Column<decimal>(type: "decimal(19,5)", nullable: false),
                    FactorUnity = table.Column<int>(nullable: false),
                    FactorBrand = table.Column<string>(maxLength: 50, nullable: false),
                    FactorLot = table.Column<string>(maxLength: 20, nullable: false),
                    IsHemarthrosis = table.Column<bool>(nullable: false),
                    IsBleeding = table.Column<bool>(nullable: false),
                    IsTreatmentContinuation = table.Column<bool>(nullable: false),
                    BleedingLocal = table.Column<string>(maxLength: 50, nullable: false),
                    TreatmentLocal = table.Column<string>(maxLength: 50, nullable: false),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infusions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Infusions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Infusions_UserId",
                table: "Infusions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Infusions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
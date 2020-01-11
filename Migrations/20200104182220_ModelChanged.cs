using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorywcza.Migrations
{
    public partial class ModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Categories_CategoryId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Employees_EmployeeId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Employers_EmployerId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_EmployeeId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "JobOffers");

            migrationBuilder.AlterColumn<int>(
                name: "EmployerId",
                table: "JobOffers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "JobOffers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "JobOfferEmployees",
                columns: table => new
                {
                    JobOfferId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferEmployees", x => new { x.JobOfferId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_JobOfferEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOfferEmployees_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "JobOfferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobOfferEmployees",
                columns: new[] { "JobOfferId", "EmployeeId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                column: "AddDate",
                value: new DateTime(2020, 1, 4, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferEmployees_EmployeeId",
                table: "JobOfferEmployees",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Categories_CategoryId",
                table: "JobOffers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Employers_EmployerId",
                table: "JobOffers",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "EmployerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Categories_CategoryId",
                table: "JobOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Employers_EmployerId",
                table: "JobOffers");

            migrationBuilder.DropTable(
                name: "JobOfferEmployees");

            migrationBuilder.AlterColumn<int>(
                name: "EmployerId",
                table: "JobOffers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "JobOffers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "JobOffers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                columns: new[] { "AddDate", "EmployeeId" },
                values: new object[] { new DateTime(2020, 1, 2, 0, 0, 0, 0, DateTimeKind.Local), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_EmployeeId",
                table: "JobOffers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Categories_CategoryId",
                table: "JobOffers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Employees_EmployeeId",
                table: "JobOffers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Employers_EmployerId",
                table: "JobOffers",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "EmployerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

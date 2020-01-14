using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorywcza.Migrations
{
    public partial class ModelSlightlyAdjusted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                columns: new[] { "AddDate", "TimeFrame" },
                values: new object[] { new DateTime(2019, 12, 26, 0, 0, 0, 0, DateTimeKind.Local), "12.12.2019 - 14.12.2019" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                columns: new[] { "AddDate", "TimeFrame" },
                values: new object[] { new DateTime(2019, 12, 20, 0, 0, 0, 0, DateTimeKind.Local), "12.12.2019-14.12.2019" });
        }
    }
}

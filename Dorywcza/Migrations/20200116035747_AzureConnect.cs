using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorywcza.Migrations
{
    public partial class AzureConnect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                column: "AddDate",
                value: new DateTime(2020, 1, 16, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                column: "AddDate",
                value: new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}

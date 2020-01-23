using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorywcza.Migrations
{
    public partial class CategoriesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "TypeOfJob", "Workplace" },
                values: new object[,]
                {
                    { 2, "Prace biurowe", "Katowice" },
                    { 3, "Prace transportowe", "Katowice" },
                    { 4, "Opieka", "Katowice" }
                });

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                column: "AddDate",
                value: new DateTime(2019, 12, 27, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                column: "AddDate",
                value: new DateTime(2019, 12, 26, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}

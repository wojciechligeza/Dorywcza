using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorywcza.Migrations
{
    public partial class InitialValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TimeFrame",
                table: "JobOffers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "TypeOfJob", "Workplace" },
                values: new object[] { 1, "Prace budowlane", "Katowice" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "AgreementRodo", "Comment", "Email", "Experience", "Gender", "Name", "Phone", "Qualification" },
                values: new object[] { 1, 22, true, "Czy jest przerwa na piwo?", "example@example", "3 lata na budowie", "M", "Jan Kowalski", "+48 000 000 000", "Certyfikat QWERTY, Ukończone technikum budowlane" });

            migrationBuilder.InsertData(
                table: "Employers",
                columns: new[] { "EmployerId", "CompanyName", "Description" },
                values: new object[] { 1, "ConstructNext", "Zajmujemy się budową obiektów różnego przeznaczenia" });

            migrationBuilder.InsertData(
                table: "JobOffers",
                columns: new[] { "JobOfferId", "AddDate", "AmountOfPlaces", "CategoryId", "Description", "EmployeeId", "EmployerId", "Name", "QualificationIsRequired", "Salary", "State", "TimeFrame" },
                values: new object[] { 1, new DateTime(2019, 12, 20, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, "Praca na budowie sklepu spożywczego w 5-osobowym zespole", 1, 1, "Praca budowlana na Zawodziu", false, 6000m, true, "12.12.2019-14.12.2019" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "EmployerId",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeFrame",
                table: "JobOffers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

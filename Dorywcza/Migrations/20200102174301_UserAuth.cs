using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorywcza.Migrations
{
    public partial class UserAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                column: "AddDate",
                value: new DateTime(2020, 1, 2, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "JobOffers",
                keyColumn: "JobOfferId",
                keyValue: 1,
                column: "AddDate",
                value: new DateTime(2019, 12, 27, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}

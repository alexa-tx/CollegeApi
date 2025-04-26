using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToStudentProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "StudentProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "StudentProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "StudentProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "StudentProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudentProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramUsername",
                table: "StudentProfiles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "TelegramUsername",
                table: "StudentProfiles");
        }
    }
}

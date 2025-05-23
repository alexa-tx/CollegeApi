using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEditProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentProfiles");

            migrationBuilder.RenameColumn(
                name: "TelegramUsername",
                table: "StudentProfiles",
                newName: "TelegramLink");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "StudentProfiles",
                newName: "BirthDate");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "TeacherProfiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "TeacherProfiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "TeacherProfiles",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramLink",
                table: "TeacherProfiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "TeacherProfiles");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "TeacherProfiles");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "TeacherProfiles");

            migrationBuilder.DropColumn(
                name: "TelegramLink",
                table: "TeacherProfiles");

            migrationBuilder.RenameColumn(
                name: "TelegramLink",
                table: "StudentProfiles",
                newName: "TelegramUsername");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "StudentProfiles",
                newName: "DateOfBirth");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "StudentProfiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "StudentProfiles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

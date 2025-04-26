using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetupFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "NewsPosts",
                newName: "DatePosted");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "ScheduleItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_CourseId",
                table: "ScheduleItems",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_Courses_CourseId",
                table: "ScheduleItems",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleItems_Courses_CourseId",
                table: "ScheduleItems");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleItems_CourseId",
                table: "ScheduleItems");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "ScheduleItems");

            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "NewsPosts",
                newName: "CreatedAt");
        }
    }
}

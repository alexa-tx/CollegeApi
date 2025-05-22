using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGradeandHomework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Courses_CourseId",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Homeworks",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_CourseId",
                table: "Homeworks",
                newName: "IX_Homeworks_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Grades",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_CourseId",
                table: "Grades",
                newName: "IX_Grades_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Subjects_SubjectId",
                table: "Grades",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Subjects_SubjectId",
                table: "Homeworks",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Subjects_SubjectId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Subjects_SubjectId",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Homeworks",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_SubjectId",
                table: "Homeworks",
                newName: "IX_Homeworks_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Grades",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_SubjectId",
                table: "Grades",
                newName: "IX_Grades_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Courses_CourseId",
                table: "Homeworks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

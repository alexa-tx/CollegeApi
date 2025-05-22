using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleItems_Courses_CourseId",
                table: "ScheduleItems");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "ScheduleItems",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleItems_CourseId",
                table: "ScheduleItems",
                newName: "IX_ScheduleItems_SubjectId");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubjectId",
                table: "Courses",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_Subjects_SubjectId",
                table: "ScheduleItems",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleItems_Subjects_SubjectId",
                table: "ScheduleItems");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SubjectId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "ScheduleItems",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleItems_SubjectId",
                table: "ScheduleItems",
                newName: "IX_ScheduleItems_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleItems_Courses_CourseId",
                table: "ScheduleItems",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class RemoveNameColumnFromCourseContentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CourseContents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CourseContents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class AddTitleToVideoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Videos");
        }
    }
}

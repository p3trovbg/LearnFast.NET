using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class AddDescriptionColumnInVideoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Videos");
        }
    }
}

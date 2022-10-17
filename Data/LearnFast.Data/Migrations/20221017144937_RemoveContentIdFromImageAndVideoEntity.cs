using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class RemoveContentIdFromImageAndVideoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CourseContents_ContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_CourseContents_ContentId",
                table: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "ContentId",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ContentId",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_CourseContents_ContentId",
                table: "Images",
                column: "ContentId",
                principalTable: "CourseContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_CourseContents_ContentId",
                table: "Videos",
                column: "ContentId",
                principalTable: "CourseContents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CourseContents_ContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_CourseContents_ContentId",
                table: "Videos");

            migrationBuilder.AlterColumn<int>(
                name: "ContentId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContentId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_CourseContents_ContentId",
                table: "Images",
                column: "ContentId",
                principalTable: "CourseContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_CourseContents_ContentId",
                table: "Videos",
                column: "ContentId",
                principalTable: "CourseContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

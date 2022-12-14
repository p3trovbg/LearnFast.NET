using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class RemoveContentFromImageAndVideoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CourseContents_ContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_CourseContents_ContentId",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Videos",
                newName: "CourseContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_ContentId",
                table: "Videos",
                newName: "IX_Videos_CourseContentId");

            migrationBuilder.RenameColumn(
                name: "ContentId",
                table: "Images",
                newName: "CourseContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ContentId",
                table: "Images",
                newName: "IX_Images_CourseContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_CourseContents_CourseContentId",
                table: "Images",
                column: "CourseContentId",
                principalTable: "CourseContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_CourseContents_CourseContentId",
                table: "Videos",
                column: "CourseContentId",
                principalTable: "CourseContents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_CourseContents_CourseContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_CourseContents_CourseContentId",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "CourseContentId",
                table: "Videos",
                newName: "ContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_CourseContentId",
                table: "Videos",
                newName: "IX_Videos_ContentId");

            migrationBuilder.RenameColumn(
                name: "CourseContentId",
                table: "Images",
                newName: "ContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_CourseContentId",
                table: "Images",
                newName: "IX_Images_ContentId");

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
    }
}

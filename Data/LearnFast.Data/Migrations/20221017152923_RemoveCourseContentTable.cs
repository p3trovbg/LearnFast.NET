using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class RemoveCourseContentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Images_ImageId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_CourseContents_CourseContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_CourseContents_CourseContentId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "CourseContents");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ImageId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CourseContentId",
                table: "Videos",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_CourseContentId",
                table: "Videos",
                newName: "IX_Videos_CourseId");

            migrationBuilder.RenameColumn(
                name: "CourseContentId",
                table: "Images",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_CourseContentId",
                table: "Images",
                newName: "IX_Images_CourseId");

            migrationBuilder.AddColumn<string>(
                name: "MainImageUrl",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Courses_CourseId",
                table: "Images",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Courses_CourseId",
                table: "Videos",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Courses_CourseId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Courses_CourseId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "MainImageUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MainImageUrl",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Videos",
                newName: "CourseContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_CourseId",
                table: "Videos",
                newName: "IX_Videos_CourseContentId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Images",
                newName: "CourseContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_CourseId",
                table: "Images",
                newName: "IX_Images_CourseContentId");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseContents_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ImageId",
                table: "Courses",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseContents_CourseId",
                table: "CourseContents",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseContents_IsDeleted",
                table: "CourseContents",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Images_ImageId",
                table: "Courses",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

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
    }
}

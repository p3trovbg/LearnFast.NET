using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnFast.Data.Migrations
{
    public partial class RemoveContentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Contents_ContentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Contents_ContentId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Contents_ContentId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Videos_ContentId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Images_ContentId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ContentId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Videos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_CourseId",
                table: "Videos",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CourseId",
                table: "Images",
                column: "CourseId");

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

            migrationBuilder.DropIndex(
                name: "IX_Videos_CourseId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Images_CourseId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ContentId",
                table: "Videos",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ContentId",
                table: "Images",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ContentId",
                table: "Courses",
                column: "ContentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_IsDeleted",
                table: "Contents",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Contents_ContentId",
                table: "Courses",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Contents_ContentId",
                table: "Images",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Contents_ContentId",
                table: "Videos",
                column: "ContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

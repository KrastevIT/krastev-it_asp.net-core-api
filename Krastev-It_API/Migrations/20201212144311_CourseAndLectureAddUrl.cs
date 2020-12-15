using Microsoft.EntityFrameworkCore.Migrations;

namespace Krastev_It_API.Migrations
{
    public partial class CourseAndLectureAddUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Courses");
        }
    }
}

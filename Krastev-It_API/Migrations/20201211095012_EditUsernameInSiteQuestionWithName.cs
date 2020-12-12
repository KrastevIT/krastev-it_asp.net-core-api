using Microsoft.EntityFrameworkCore.Migrations;

namespace Krastev_It_API.Migrations
{
    public partial class EditUsernameInSiteQuestionWithName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "SiteQuestions",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SiteQuestions",
                newName: "Username");
        }
    }
}

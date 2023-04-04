using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Infrastructure.Migrations
{
    public partial class AddBioToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "User");
        }
    }
}

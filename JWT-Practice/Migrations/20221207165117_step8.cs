using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWT_Practice.Migrations
{
    public partial class step8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userJoined",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                rowVersion: true,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userJoined",
                table: "AspNetUsers");
        }
    }
}

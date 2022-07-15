using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHubApp.Data.Migrations
{
    public partial class RemovePhotoPathPropertyFromCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Cars");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

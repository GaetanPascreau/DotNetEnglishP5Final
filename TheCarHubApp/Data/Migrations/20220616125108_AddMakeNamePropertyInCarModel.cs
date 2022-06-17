using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHubApp.Data.Migrations
{
    public partial class AddMakeNamePropertyInCarModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MakeName",
                table: "CarModels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MakeName",
                table: "CarModels");
        }
    }
}

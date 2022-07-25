using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHubApp.Data.Migrations
{
    public partial class AddingContactInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekDayOpeningHour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeekDayClosingHour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaturdarOpeningHour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaturdayClosingHour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MapLat = table.Column<double>(type: "float", nullable: false),
                    MapLong = table.Column<double>(type: "float", nullable: false),
                    MapZoom = table.Column<int>(type: "int", nullable: false),
                    MapTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInfos");
        }
    }
}

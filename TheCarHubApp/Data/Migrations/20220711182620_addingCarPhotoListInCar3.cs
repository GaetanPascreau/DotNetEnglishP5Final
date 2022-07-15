using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHubApp.Data.Migrations
{
    public partial class addingCarPhotoListInCar3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarCarPhoto");

            migrationBuilder.AddColumn<int>(
                name: "CarPhotoId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarPhotoId",
                table: "Cars",
                column: "CarPhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarPhotos_CarPhotoId",
                table: "Cars",
                column: "CarPhotoId",
                principalTable: "CarPhotos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarPhotos_CarPhotoId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarPhotoId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarPhotoId",
                table: "Cars");

            migrationBuilder.CreateTable(
                name: "CarCarPhoto",
                columns: table => new
                {
                    CarPhotoesId = table.Column<int>(type: "int", nullable: false),
                    CarsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarCarPhoto", x => new { x.CarPhotoesId, x.CarsId });
                    table.ForeignKey(
                        name: "FK_CarCarPhoto_CarPhotos_CarPhotoesId",
                        column: x => x.CarPhotoesId,
                        principalTable: "CarPhotos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarCarPhoto_Cars_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarCarPhoto_CarsId",
                table: "CarCarPhoto",
                column: "CarsId");
        }
    }
}

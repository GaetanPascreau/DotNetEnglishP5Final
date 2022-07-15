using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHubApp.Data.Migrations
{
    public partial class addingListOfCarPhotoesInCAr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CarPhotos_CarId",
                table: "CarPhotos",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarPhotos_Cars_CarId",
                table: "CarPhotos",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarPhotos_Cars_CarId",
                table: "CarPhotos");

            migrationBuilder.DropIndex(
                name: "IX_CarPhotos_CarId",
                table: "CarPhotos");
        }
    }
}

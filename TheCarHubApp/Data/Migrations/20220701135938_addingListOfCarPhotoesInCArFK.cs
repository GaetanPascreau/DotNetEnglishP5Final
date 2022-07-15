using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHubApp.Data.Migrations
{
    public partial class addingListOfCarPhotoesInCArFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarPhotos_Cars_CarId",
                table: "CarPhotos");

            migrationBuilder.DropIndex(
                name: "IX_CarPhotos_CarId",
                table: "CarPhotos");

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
    }
}

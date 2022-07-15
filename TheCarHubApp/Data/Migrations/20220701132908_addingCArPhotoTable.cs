using Microsoft.EntityFrameworkCore.Migrations;

namespace TheCarHubApp.Data.Migrations
{
    public partial class addingCArPhotoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarPhotos_CarDetails_CarDetailId",
                table: "CarPhotos");

            migrationBuilder.DropTable(
                name: "CarDetails");

            migrationBuilder.DropIndex(
                name: "IX_CarPhotos_CarDetailId",
                table: "CarPhotos");

            migrationBuilder.RenameColumn(
                name: "CarDetailId",
                table: "CarPhotos",
                newName: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "CarPhotos",
                newName: "CarDetailId");

            migrationBuilder.CreateTable(
                name: "CarDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarPhotos_CarDetailId",
                table: "CarPhotos",
                column: "CarDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarPhotos_CarDetails_CarDetailId",
                table: "CarPhotos",
                column: "CarDetailId",
                principalTable: "CarDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

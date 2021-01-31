using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoListAPI.Migrations
{
    public partial class SeventhForUploader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImgUploaders",
                columns: table => new
                {
                    imgId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImgUploaders", x => x.imgId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImgUploaders");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoListAPI.Migrations
{
    public partial class SecondChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userBlockStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "userStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "todoStatus",
                table: "TodoItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userBlockStatus",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userStatus",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsComplete",
                table: "TodoItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "todoStatus",
                table: "TodoItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

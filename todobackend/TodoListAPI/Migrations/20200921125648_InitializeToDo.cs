using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoListAPI.Migrations
{
    public partial class InitializeToDo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    todoId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    todoUserId = table.Column<long>(nullable: false),
                    todoTitle = table.Column<string>(nullable: true),
                    todoDesc = table.Column<string>(nullable: true),
                    todoPriority = table.Column<string>(nullable: true),
                    todoStatus = table.Column<int>(nullable: false),
                    todoCreateDate = table.Column<DateTime>(nullable: false),
                    todoDate = table.Column<DateTime>(nullable: false),
                    todoImage = table.Column<string>(nullable: true),
                    IsComplete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.todoId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userUserName = table.Column<string>(nullable: true),
                    userEmail = table.Column<string>(nullable: true),
                    userPassword = table.Column<string>(nullable: true),
                    userMobile = table.Column<long>(nullable: false),
                    userStatus = table.Column<int>(nullable: false),
                    userBlockStatus = table.Column<int>(nullable: false),
                    userCreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

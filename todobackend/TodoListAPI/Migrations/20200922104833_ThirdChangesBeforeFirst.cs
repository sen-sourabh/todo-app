using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoListAPI.Migrations
{
    public partial class ThirdChangesBeforeFirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userBlockStatus",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "userCreateDate",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "userStatus",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsComplete",
                table: "TodoItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "todoCreateDate",
                table: "TodoItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "todoStatus",
                table: "TodoItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userBlockStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "userCreateDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "userStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "todoCreateDate",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "todoStatus",
                table: "TodoItems");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class _011701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoleses",
                table: "UserRoleses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "UserRoleses",
                newName: "UserRole");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 549, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 103, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 547, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 102, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "UserRole",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 105, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "UserRole",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 105, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 104, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 104, DateTimeKind.Local));

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "UserRoleses");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "UserRoleses",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 105, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "UserRoleses",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 105, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 103, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 549, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 102, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 547, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Roles",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 104, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Roles",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 104, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoleses",
                table: "UserRoleses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class _20190129 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "UserRole",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "UserRole",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 638, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 549, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 635, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 547, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local));

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsValid = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsValid = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOrgs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsValid = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    OrgId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOrgs_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrgs_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsValid = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ResourceId",
                table: "Permissions",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrgs_OrgId",
                table: "UserOrgs",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrgs_UserId",
                table: "UserOrgs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "UserOrgs");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "UserRole",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "UserRole",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 549, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 638, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 547, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 635, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Role",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 17, 14, 2, 59, 550, DateTimeKind.Local),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 1, 29, 15, 55, 45, 639, DateTimeKind.Local));
        }
    }
}

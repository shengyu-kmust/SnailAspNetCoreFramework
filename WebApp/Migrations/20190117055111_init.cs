using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 104, DateTimeKind.Local)),
                    UpdateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 104, DateTimeKind.Local)),
                    IsValid = table.Column<int>(nullable: false, defaultValue: 1),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 105, DateTimeKind.Local)),
                    UpdateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 105, DateTimeKind.Local)),
                    IsValid = table.Column<int>(nullable: false, defaultValue: 1),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 102, DateTimeKind.Local)),
                    UpdateTime = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 1, 17, 13, 51, 11, 103, DateTimeKind.Local)),
                    IsValid = table.Column<int>(nullable: false, defaultValue: 1),
                    LoginName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Pwd = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserRoleses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

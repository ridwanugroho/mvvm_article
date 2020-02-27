using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Article.Migrations
{
    public partial class adduserroletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    Created_at = table.Column<DateTime>(nullable: false),
                    Edited_at = table.Column<DateTime>(nullable: false),
                    Deleted_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role");
        }
    }
}

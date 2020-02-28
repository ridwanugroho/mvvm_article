using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Article.Migrations
{
    public partial class addfileimagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    Created_at = table.Column<DateTime>(nullable: false),
                    Edited_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");
        }
    }
}

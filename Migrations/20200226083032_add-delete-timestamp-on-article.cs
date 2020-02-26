using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Article.Migrations
{
    public partial class adddeletetimestamponarticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted_at",
                table: "article",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted_at",
                table: "article");
        }
    }
}

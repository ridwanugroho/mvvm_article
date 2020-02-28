using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Article.Migrations
{
    public partial class restructurefiletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "File",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArticleID",
                table: "File",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleID",
                table: "File");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "File",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

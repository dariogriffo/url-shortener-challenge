using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Url_Shortener.Data.Migrations
{
    public partial class AddShorturlcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortUrl",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortUrl",
                table: "Urls");
        }
    }
}

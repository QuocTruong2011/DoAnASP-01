using Microsoft.EntityFrameworkCore.Migrations;

namespace DoAnASP.Migrations
{
    public partial class EditProductType_AddAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "ProductType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "ProductType");
        }
    }
}

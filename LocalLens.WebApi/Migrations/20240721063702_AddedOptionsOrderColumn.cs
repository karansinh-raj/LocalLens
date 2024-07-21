using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalLens.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedOptionsOrderColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OptionOrder",
                table: "Options",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionOrder",
                table: "Options");
        }
    }
}

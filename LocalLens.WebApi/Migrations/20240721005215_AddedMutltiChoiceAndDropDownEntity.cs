using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalLens.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedMutltiChoiceAndDropDownEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDropDown",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMultiChoice",
                table: "Questions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDropDown",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsMultiChoice",
                table: "Questions");
        }
    }
}

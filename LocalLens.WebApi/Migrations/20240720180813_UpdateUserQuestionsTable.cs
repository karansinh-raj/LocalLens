using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalLens.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserQuestionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestions_Questions_OptionId",
                table: "UserQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestions_Options_OptionId",
                table: "UserQuestions",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuestions_Options_OptionId",
                table: "UserQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuestions_Questions_OptionId",
                table: "UserQuestions",
                column: "OptionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

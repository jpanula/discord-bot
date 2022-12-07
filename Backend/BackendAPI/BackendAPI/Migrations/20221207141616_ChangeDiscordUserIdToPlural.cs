using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDiscordUserIdToPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscordUserId",
                table: "EventVote",
                newName: "DiscordUserIds");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EventVote",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscordUserIds",
                table: "EventVote",
                newName: "DiscordUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EventVote",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

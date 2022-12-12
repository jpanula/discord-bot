using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCommandModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsConfigurable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCommand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExampleText = table.Column<string>(type: "text", nullable: true),
                    ExampleMediaUrl = table.Column<string>(type: "text", nullable: true),
                    CommandGroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCommand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCommand_CommandGroup_CommandGroupId",
                        column: x => x.CommandGroupId,
                        principalTable: "CommandGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommandParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    SubCommandId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandParameter_SubCommand_SubCommandId",
                        column: x => x.SubCommandId,
                        principalTable: "SubCommand",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandParameter_SubCommandId",
                table: "CommandParameter",
                column: "SubCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCommand_CommandGroupId",
                table: "SubCommand",
                column: "CommandGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandParameter");

            migrationBuilder.DropTable(
                name: "SubCommand");

            migrationBuilder.DropTable(
                name: "CommandGroup");
        }
    }
}

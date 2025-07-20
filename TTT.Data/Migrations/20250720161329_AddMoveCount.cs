using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTT.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoveCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoveCount",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoveCount",
                table: "Games");
        }
    }
}

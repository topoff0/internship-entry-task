using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTT.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixMoveNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoveCount",
                table: "Games",
                newName: "MoveNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoveNumber",
                table: "Games",
                newName: "MoveCount");
        }
    }
}

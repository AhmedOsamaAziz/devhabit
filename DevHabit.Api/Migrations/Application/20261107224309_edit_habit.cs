using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevHabit.Api.Migrations.Application
{
    /// <inheritdoc />
    public partial class edit_habit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_compleated_at_utc",
                schema: "devhabit",
                table: "habits",
                newName: "last_completed_at_utc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_completed_at_utc",
                schema: "devhabit",
                table: "habits",
                newName: "last_compleated_at_utc");
        }
    }
}

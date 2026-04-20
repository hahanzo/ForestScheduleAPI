using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForestSchedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lessons_DayOfWeek",
                table: "Lessons",
                column: "DayOfWeek");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_DayOfWeek",
                table: "Lessons");
        }
    }
}

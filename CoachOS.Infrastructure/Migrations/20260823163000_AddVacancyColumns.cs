using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVacancyColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationFee",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EligibilityDetails",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationPdfUrl",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationSent",
                table: "Vacancies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SalaryRange",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalPosts",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationFee",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "EligibilityDetails",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "NotificationPdfUrl",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "SalaryRange",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "TotalPosts",
                table: "Vacancies");
        }
    }
}

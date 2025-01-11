using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceHub.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutMeToServiceProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutMe",
                table: "ServiceProviders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutMe",
                table: "ServiceProviders");
        }
    }
}

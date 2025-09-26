using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PmEngine.Vk.Migrations
{
    /// <inheritdoc />
    public partial class vkUserDataExt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "VkDataUserEntity",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VkDataUserEntity",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "VkDataUserEntity");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VkDataUserEntity");
        }
    }
}

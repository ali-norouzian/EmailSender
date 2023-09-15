using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailSender.Data.Migrations
{
    public partial class IsPendingAddedToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "EmailSendingGroup",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "EmailSendingGroup");
        }
    }
}

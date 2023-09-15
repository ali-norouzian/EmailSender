using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailSender.Data.Migrations
{
    public partial class EmailSendingGroupAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "EmailSendingStatus",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmailSendingGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSendingGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailSendingStatus_GroupId",
                table: "EmailSendingStatus",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailSendingStatus_EmailSendingGroup_GroupId",
                table: "EmailSendingStatus",
                column: "GroupId",
                principalTable: "EmailSendingGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailSendingStatus_EmailSendingGroup_GroupId",
                table: "EmailSendingStatus");

            migrationBuilder.DropTable(
                name: "EmailSendingGroup");

            migrationBuilder.DropIndex(
                name: "IX_EmailSendingStatus_GroupId",
                table: "EmailSendingStatus");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "EmailSendingStatus");
        }
    }
}

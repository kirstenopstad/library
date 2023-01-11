using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    public partial class AddUserToCheckOutRemoveCheckOutDateValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CheckOuts",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_UserId",
                table: "CheckOuts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckOuts_AspNetUsers_UserId",
                table: "CheckOuts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckOuts_AspNetUsers_UserId",
                table: "CheckOuts");

            migrationBuilder.DropIndex(
                name: "IX_CheckOuts_UserId",
                table: "CheckOuts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CheckOuts");
        }
    }
}

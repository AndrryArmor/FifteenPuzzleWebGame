using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetBack.Migrations
{
    /// <inheritdoc />
    public partial class NicknameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGameResults",
                table: "UserGameResults");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserGameResults");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "UserGameResults",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGameResults",
                table: "UserGameResults",
                column: "Nickname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGameResults",
                table: "UserGameResults");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "UserGameResults",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserGameResults",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGameResults",
                table: "UserGameResults",
                column: "Id");
        }
    }
}

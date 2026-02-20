using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjPlatinaSteam.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conquistas_Jogos_Jogoid",
                table: "Conquistas");

            migrationBuilder.RenameColumn(
                name: "Jogoid",
                table: "Conquistas",
                newName: "JogoId");

            migrationBuilder.RenameIndex(
                name: "IX_Conquistas_Jogoid",
                table: "Conquistas",
                newName: "IX_Conquistas_JogoId");

            migrationBuilder.AlterColumn<int>(
                name: "JogoId",
                table: "Conquistas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Conquistas_Jogos_JogoId",
                table: "Conquistas",
                column: "JogoId",
                principalTable: "Jogos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conquistas_Jogos_JogoId",
                table: "Conquistas");

            migrationBuilder.RenameColumn(
                name: "JogoId",
                table: "Conquistas",
                newName: "Jogoid");

            migrationBuilder.RenameIndex(
                name: "IX_Conquistas_JogoId",
                table: "Conquistas",
                newName: "IX_Conquistas_Jogoid");

            migrationBuilder.AlterColumn<int>(
                name: "Jogoid",
                table: "Conquistas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Conquistas_Jogos_Jogoid",
                table: "Conquistas",
                column: "Jogoid",
                principalTable: "Jogos",
                principalColumn: "id");
        }
    }
}

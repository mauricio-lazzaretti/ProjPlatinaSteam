using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjPlatinaSteam.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosSteam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    steamId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    avatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosSteam", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jogos",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gameHours = table.Column<int>(type: "int", nullable: false),
                    UsuarioSteamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Jogos_UsuariosSteam_UsuarioSteamId",
                        column: x => x.UsuarioSteamId,
                        principalTable: "UsuariosSteam",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Conquistas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    apiNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    conquistada = table.Column<bool>(type: "bit", nullable: false),
                    unlockTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ehDLC = table.Column<bool>(type: "bit", nullable: false),
                    Jogoid = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conquistas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conquistas_Jogos_Jogoid",
                        column: x => x.Jogoid,
                        principalTable: "Jogos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conquistas_Jogoid",
                table: "Conquistas",
                column: "Jogoid");

            migrationBuilder.CreateIndex(
                name: "IX_Jogos_UsuarioSteamId",
                table: "Jogos",
                column: "UsuarioSteamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conquistas");

            migrationBuilder.DropTable(
                name: "Jogos");

            migrationBuilder.DropTable(
                name: "UsuariosSteam");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjPlatinaSteam.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCampoAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "UsuariosSteam",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "UsuariosSteam");
        }
    }
}

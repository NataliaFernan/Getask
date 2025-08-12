using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Getask.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddPosicaoTarefa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Posicao",
                table: "Tarefas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Posicao",
                table: "Tarefas");
        }
    }
}

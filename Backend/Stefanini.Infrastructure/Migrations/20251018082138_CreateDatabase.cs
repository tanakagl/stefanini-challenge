using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stefanini.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NomeCompleto = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Sexo = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Nacionalidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Naturalidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Cpf = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    Endereco_Rua = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Endereco_Numero = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Endereco_Complemento = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Endereco_Bairro = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Endereco_Cidade = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Endereco_Estado = table.Column<string>(type: "TEXT", maxLength: 2, nullable: true),
                    Endereco_Cep = table.Column<string>(type: "TEXT", maxLength: 8, nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataUltimaAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Cpf",
                table: "Users",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

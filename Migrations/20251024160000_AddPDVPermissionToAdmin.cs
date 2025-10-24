using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPDVPermissionToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionar permissão PDV_ACESSO se não existir
            migrationBuilder.Sql(@"
                INSERT INTO Permissoes (Nome, Codigo, DataCriacao)
                SELECT 'Acessar PDV', 'PDV_ACESSO', CURRENT_TIMESTAMP
                WHERE NOT EXISTS (SELECT 1 FROM Permissoes WHERE Codigo = 'PDV_ACESSO');
            ");

            // Adicionar permissão ao usuário administrador (ID 1)
            migrationBuilder.Sql(@"
                INSERT INTO UsuarioPermissoes (UsuarioId, PermissaoId, Concedida, DataConcessao)
                SELECT 1, Id, 1, CURRENT_TIMESTAMP FROM Permissoes 
                WHERE Codigo = 'PDV_ACESSO'
                AND NOT EXISTS (
                    SELECT 1 FROM UsuarioPermissoes up
                    JOIN Permissoes p ON up.PermissaoId = p.Id
                    WHERE p.Codigo = 'PDV_ACESSO' AND up.UsuarioId = 1
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover a permissão do usuário administrador
            migrationBuilder.Sql(@"
                DELETE FROM UsuarioPermissoes
                WHERE UsuarioId = 1 AND PermissaoId IN (
                    SELECT Id FROM Permissoes WHERE Codigo = 'PDV_ACESSO'
                );
            ");
        }
    }
}
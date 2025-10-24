using Microsoft.EntityFrameworkCore;
using System;
using WebApp.Models;

namespace WebApp
{
    public class AddNFCeMenuItem
    {
        public static void Main(string[] args)
        {
            using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=WebApp.db")
                .Options))
            {
                // Adicionar o item de menu para NFC-e
                var menuItem = new MenuItem
                {
                    Id = 15,
                    Titulo = "NFC-e",
                    Icone = "bi-receipt",
                    Ordem = 6,
                    Ativo = true,
                    AbrirNovaAba = false,
                    Descricao = "Nota Fiscal de Consumidor Eletrônica",
                    Controller = "PDV",
                    Action = "Index",
                    DataCriacao = DateTime.Now,
                    EMenuPai = false
                };

                context.MenuItems.Add(menuItem);
                
                // Adicionar permissão para NFC-e
                var permissao = new Permissao
                {
                    Nome = "Acesso a NFC-e",
                    Descricao = "Permite acesso às funcionalidades de NFC-e",
                    Chave = "NFCe"
                };
                
                context.Permissoes.Add(permissao);
                context.SaveChanges();
                
                // Adicionar permissão ao usuário admin (assumindo que o ID 1 é o admin)
                var admin = context.Users.Find(1);
                var nfcePermissao = context.Permissoes.FirstOrDefault(p => p.Chave == "NFCe");
                
                if (admin != null && nfcePermissao != null)
                {
                    context.UsuarioPermissoes.Add(new UsuarioPermissao
                    {
                        UsuarioId = admin.Id,
                        PermissaoId = nfcePermissao.Id
                    });
                    
                    context.SaveChanges();
                }
                
                Console.WriteLine("Item de menu NFC-e adicionado com sucesso!");
            }
        }
    }
}
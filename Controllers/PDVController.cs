using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PDVController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PDVController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PDV
        public IActionResult Index()
        {
            // Verificar se há uma venda em aberto
            var vendaAberta = _context.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(v => v.Status == "Aberta");

            if (vendaAberta == null)
            {
                // Criar uma nova venda
                vendaAberta = new Venda
                {
                    DataVenda = DateTime.Now,
                    Status = "Aberta",
                    UsuarioId = 1 // Substituir pelo usuário logado
                };
                _context.Vendas.Add(vendaAberta);
                _context.SaveChanges();
            }

            // Obter categorias para filtro
            ViewBag.Categorias = _context.Categorias.ToList();
            
            // Obter produtos para exibição
            ViewBag.Produtos = _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Imagens)
                .ToList();

            return View(vendaAberta);
        }

        // POST: PDV/AdicionarItem
        [HttpPost]
        public IActionResult AdicionarItem(int vendaId, int produtoId, int quantidade)
        {
            var venda = _context.Vendas.Find(vendaId);
            var produto = _context.Produtos.Find(produtoId);

            if (venda == null || produto == null)
            {
                return NotFound();
            }

            // Verificar se o item já existe na venda
            var itemExistente = _context.ItensVenda
                .FirstOrDefault(i => i.VendaId == vendaId && i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                // Atualizar quantidade
                itemExistente.Quantidade += quantidade;
                itemExistente.Subtotal = itemExistente.Quantidade * itemExistente.PrecoUnitario;
                _context.Update(itemExistente);
            }
            else
            {
                // Criar novo item
                var novoItem = new ItemVenda
                {
                    VendaId = vendaId,
                    ProdutoId = produtoId,
                    Quantidade = quantidade,
                    PrecoUnitario = produto.Preco,
                    Subtotal = quantidade * produto.Preco
                };
                _context.ItensVenda.Add(novoItem);
            }

            _context.SaveChanges();

            // Atualizar valor total da venda
            AtualizarTotalVenda(vendaId);

            return RedirectToAction(nameof(Index));
        }

        // POST: PDV/RemoverItem
        [HttpPost]
        public IActionResult RemoverItem(int itemId)
        {
            var item = _context.ItensVenda.Find(itemId);
            if (item == null)
            {
                return NotFound();
            }

            int vendaId = item.VendaId;
            _context.ItensVenda.Remove(item);
            _context.SaveChanges();

            // Atualizar valor total da venda
            AtualizarTotalVenda(vendaId);

            return RedirectToAction(nameof(Index));
        }

        // POST: PDV/AplicarDesconto
        [HttpPost]
        public IActionResult AplicarDesconto(int vendaId, decimal desconto)
        {
            var venda = _context.Vendas.Find(vendaId);
            if (venda == null)
            {
                return NotFound();
            }

            venda.Desconto = desconto;
            venda.ValorFinal = venda.ValorTotal - desconto;
            _context.Update(venda);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // POST: PDV/FinalizarVenda
        [HttpPost]
        public IActionResult FinalizarVenda(int vendaId, string formaPagamento, int? clienteId)
        {
            var venda = _context.Vendas
                .Include(v => v.Itens)
                .FirstOrDefault(v => v.Id == vendaId);

            if (venda == null || venda.Itens.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            venda.Status = "Finalizada";
            venda.FormaPagamento = formaPagamento;
            venda.ClienteId = clienteId;
            _context.Update(venda);
            _context.SaveChanges();

            return RedirectToAction(nameof(Comprovante), new { id = vendaId });
        }

        // GET: PDV/Comprovante/5
        public IActionResult Comprovante(int id)
        {
            var venda = _context.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .Include(v => v.Cliente)
                .Include(v => v.Usuario)
                .FirstOrDefault(v => v.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // Método auxiliar para atualizar o valor total da venda
        private void AtualizarTotalVenda(int vendaId)
        {
            var venda = _context.Vendas
                .Include(v => v.Itens)
                .FirstOrDefault(v => v.Id == vendaId);

            if (venda != null)
            {
                venda.ValorTotal = venda.Itens.Sum(i => i.Subtotal);
                venda.ValorFinal = venda.ValorTotal - venda.Desconto;
                _context.Update(venda);
                _context.SaveChanges();
            }
        }

        // GET: PDV/BuscarProdutos
        [HttpGet]
        public IActionResult BuscarProdutos(string termo, int? categoriaId)
        {
            var query = _context.Produtos.AsQueryable();

            if (!string.IsNullOrEmpty(termo))
            {
                //query = query.Where(p => !(!p.Nome.Contains(termo) && !p.CodigoBarras.Contains(termo)));
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaId == categoriaId);
            }

            var produtos = query
                .Include(p => p.Categoria)
                .Include(p => p.Imagens)
                .ToList();

            return PartialView("_ProdutosPartial", produtos);
        }

        // GET: PDV/BuscarClientes
        [HttpGet]
        public IActionResult BuscarClientes(string termo)
        {
            var clientes = _context.Clientes
                .Where(c => c.Nome.Contains(termo) || c.CPF.Contains(termo))
                .Take(10)
                .ToList();

            return Json(clientes);
        }
    }
}
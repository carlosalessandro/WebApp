using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class PDVController : Controller
    {
        // Método para garantir que o item de menu NFC-e exista
        private void EnsureNFCeMenuItemExists()
        {
            if (_context.MenuItems.Any(m => m.Titulo == "NFC-e"))
                return;

            var menuItem = new MenuItem
            {
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

            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();
        }
        private readonly ApplicationDbContext _context;
        private readonly INFCeService _nfceService;

        public PDVController(ApplicationDbContext context, INFCeService nfceService)
        {
            _context = context;
            _nfceService = nfceService;
        }

        // GET: PDV
        public IActionResult Index()
        {
            // Garante que o item de menu NFC-e exista
            EnsureNFCeMenuItemExists();
            
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
                //_context.SaveChanges();
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

        // GET: PDV/AplicarDesconto
        [HttpGet]
        public IActionResult AplicarDesconto(int? vendaId)
        {
            // Se não há vendaId, buscar venda aberta
            if (!vendaId.HasValue)
            {
                var vendaAberta = _context.Vendas
                    .FirstOrDefault(v => v.Status == "Aberta");
                
                if (vendaAberta != null)
                {
                    vendaId = vendaAberta.Id;
                }
            }

            if (!vendaId.HasValue)
            {
                TempData["Error"] = "Nenhuma venda encontrada para aplicar desconto.";
                return RedirectToAction(nameof(Index));
            }

            var venda = _context.Vendas
                .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(v => v.Id == vendaId.Value);

            if (venda == null)
            {
                TempData["Error"] = "Venda não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            return View(venda);
        }

        // POST: PDV/AplicarDesconto
        [HttpPost]
        public IActionResult AplicarDesconto(int vendaId, decimal desconto, string? tipoDesconto = "valor")
        {
            var venda = _context.Vendas.Find(vendaId);
            if (venda == null)
            {
                TempData["Error"] = "Venda não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            // Calcular desconto baseado no tipo
            decimal valorDesconto = desconto;
            if (tipoDesconto == "percentual")
            {
                if (desconto > 100)
                {
                    TempData["Error"] = "Desconto percentual não pode ser maior que 100%.";
                    return RedirectToAction(nameof(AplicarDesconto), new { vendaId });
                }
                valorDesconto = (venda.ValorTotal * desconto) / 100;
            }

            if (valorDesconto > venda.ValorTotal)
            {
                TempData["Error"] = "Desconto não pode ser maior que o valor total da venda.";
                return RedirectToAction(nameof(AplicarDesconto), new { vendaId });
            }

            venda.Desconto = valorDesconto;
            venda.ValorFinal = venda.ValorTotal - valorDesconto;
            _context.Update(venda);
            _context.SaveChanges();

            TempData["Success"] = $"Desconto de {valorDesconto:C} aplicado com sucesso!";
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

        // POST: PDV/GerarNFCe
        [HttpPost]
        public async Task<IActionResult> GerarNFCe(int vendaId)
        {
            var nfce = await _nfceService.GerarNFCeAsync(vendaId);
            if (nfce == null)
            {
                TempData["Erro"] = "Não foi possível gerar a NFC-e. Verifique os dados da venda.";
                return RedirectToAction(nameof(Comprovante), new { id = vendaId });
            }

            TempData["Sucesso"] = $"NFC-e gerada com sucesso. Chave de acesso: {nfce.ChaveAcesso}";
            return RedirectToAction(nameof(DetalhesNFCe), new { id = nfce.Id });
        }

        // GET: PDV/DetalhesNFCe/5
        public async Task<IActionResult> DetalhesNFCe(int id)
        {
            var nfce = await _context.NFCes
                .Include(n => n.Venda)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (nfce == null)
            {
                return NotFound();
            }

            return View(nfce);
        }

        // GET: PDV/ImportarNFCe
        public IActionResult ImportarNFCe()
        {
            return View();
        }

        // POST: PDV/ImportarNFCe
        [HttpPost]
        public async Task<IActionResult> ImportarNFCe(string chaveAcesso)
        {
            if (string.IsNullOrEmpty(chaveAcesso))
            {
                TempData["Erro"] = "A chave de acesso é obrigatória.";
                return View();
            }

            var nfce = await _nfceService.ImportarNFCeAsync(chaveAcesso);
            if (nfce == null)
            {
                TempData["Erro"] = "Não foi possível importar a NFC-e. Verifique a chave de acesso.";
                return View();
            }

            TempData["Sucesso"] = "NFC-e importada com sucesso.";
            return RedirectToAction(nameof(DetalhesNFCe), new { id = nfce.Id });
        }

        // POST: PDV/CancelarNFCe
        [HttpPost]
        public async Task<IActionResult> CancelarNFCe(int nfceId, string justificativa)
        {
            if (string.IsNullOrEmpty(justificativa))
            {
                TempData["Erro"] = "A justificativa é obrigatória para cancelar a NFC-e.";
                return RedirectToAction(nameof(DetalhesNFCe), new { id = nfceId });
            }

            var resultado = await _nfceService.CancelarNFCeAsync(nfceId, justificativa);
            if (!resultado)
            {
                TempData["Erro"] = "Não foi possível cancelar a NFC-e.";
                return RedirectToAction(nameof(DetalhesNFCe), new { id = nfceId });
            }

            TempData["Sucesso"] = "NFC-e cancelada com sucesso.";
            return RedirectToAction(nameof(DetalhesNFCe), new { id = nfceId });
        }
    }
}
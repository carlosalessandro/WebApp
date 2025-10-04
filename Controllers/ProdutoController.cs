using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.IO;

namespace WebApp.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProdutoController> _logger;
        private readonly IWebHostEnvironment _environment;

        public ProdutoController(ApplicationDbContext context, ILogger<ProdutoController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        // GET: Produto
        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Imagens.Where(i => i.Principal))
                .Where(p => p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync();

            return View(produtos);
        }

        // GET: Produto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Imagens.OrderBy(i => i.Ordem))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: Produto/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriaDropdown();
            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Preco,PrecoCusto,QuantidadeEstoque,QuantidadeMinima,Codigo,Marca,Modelo,Cor,Tamanho,Peso,Dimensoes,CategoriaId,Ativo,Destaque")] Produto produto, IFormFileCollection? imagens)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    produto.DataCriacao = DateTime.Now;
                    _context.Produtos.Add(produto);
                    await _context.SaveChangesAsync();

                    // Upload das imagens
                    if (imagens != null && imagens.Count > 0)
                    {
                        await UploadImagens(produto.Id, imagens);
                    }

                    TempData["SuccessMessage"] = "Produto criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar produto");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar o produto. Tente novamente.");
                }
            }

            await PopulateCategoriaDropdown();
            return View(produto);
        }

        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Imagens.OrderBy(i => i.Ordem))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            await PopulateCategoriaDropdown();
            return View(produto);
        }

        // POST: Produto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Preco,PrecoCusto,QuantidadeEstoque,QuantidadeMinima,Codigo,Marca,Modelo,Cor,Tamanho,Peso,Dimensoes,CategoriaId,Ativo,Destaque,DataCriacao")] Produto produto, IFormFileCollection? imagens)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    produto.DataAtualizacao = DateTime.Now;
                    _context.Produtos.Update(produto);

                    // Upload de novas imagens
                    if (imagens != null && imagens.Count > 0)
                    {
                        await UploadImagens(produto.Id, imagens);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Produto atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar produto");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar o produto. Tente novamente.");
                }
            }

            await PopulateCategoriaDropdown();
            return View(produto);
        }

        // GET: Produto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Imagens)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var produto = await _context.Produtos
                    .Include(p => p.Imagens)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (produto != null)
                {
                    // Remover arquivos de imagem do servidor
                    foreach (var imagem in produto.Imagens)
                    {
                        var caminhoCompleto = Path.Combine(_environment.WebRootPath, imagem.Caminho.TrimStart('/'));
                        if (System.IO.File.Exists(caminhoCompleto))
                        {
                            System.IO.File.Delete(caminhoCompleto);
                        }
                    }

                    _context.Produtos.Remove(produto);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Produto excluído com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Produto não encontrado.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir produto");
                TempData["ErrorMessage"] = "Ocorreu um erro ao excluir o produto. Tente novamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Produto/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var produto = await _context.Produtos.FindAsync(id);
                if (produto != null)
                {
                    produto.Ativo = !produto.Ativo;
                    produto.DataAtualizacao = DateTime.Now;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Status do produto alterado com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Produto não encontrado.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alterar status do produto");
                TempData["ErrorMessage"] = "Ocorreu um erro ao alterar o status do produto. Tente novamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Produto/DeleteImage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int id)
        {
            try
            {
                var imagem = await _context.ProdutoImagens.FindAsync(id);
                if (imagem != null)
                {
                    // Remover arquivo do servidor
                    var caminhoCompleto = Path.Combine(_environment.WebRootPath, imagem.Caminho.TrimStart('/'));
                    if (System.IO.File.Exists(caminhoCompleto))
                    {
                        System.IO.File.Delete(caminhoCompleto);
                    }

                    _context.ProdutoImagens.Remove(imagem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Imagem excluída com sucesso!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Imagem não encontrada.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir imagem");
                TempData["ErrorMessage"] = "Ocorreu um erro ao excluir a imagem. Tente novamente.";
            }

            return RedirectToAction(nameof(Edit), new { id = Request.Form["produtoId"] });
        }

        private async Task PopulateCategoriaDropdown()
        {
            var categorias = await _context.Categorias
                .Where(c => c.Ativa)
                .OrderBy(c => c.Nome)
                .Select(c => new { c.Id, c.Nome })
                .ToListAsync();

            ViewBag.CategoriaId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categorias, "Id", "Nome");
        }

        private async Task UploadImagens(int produtoId, IFormFileCollection imagens)
        {
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", "produtos");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var produtoImagens = new List<ProdutoImagem>();
            var ordem = 0;

            foreach (var imagem in imagens)
            {
                if (imagem.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
                    var filePath = Path.Combine(uploadsPath, fileName);
                    var relativePath = $"/uploads/produtos/{fileName}";

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }

                    produtoImagens.Add(new ProdutoImagem
                    {
                        ProdutoId = produtoId,
                        NomeArquivo = fileName,
                        Caminho = relativePath,
                        Titulo = Path.GetFileNameWithoutExtension(imagem.FileName),
                        Principal = ordem == 0, // Primeira imagem é principal
                        Ordem = ordem,
                        DataCriacao = DateTime.Now
                    });

                    ordem++;
                }
            }

            if (produtoImagens.Any())
            {
                _context.ProdutoImagens.AddRange(produtoImagens);
                await _context.SaveChangesAsync();
            }
        }
    }
}

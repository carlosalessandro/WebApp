using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using OfficeOpenXml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using System.Text;

namespace WebApp.Controllers
{
    public class RelatorioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RelatorioController> _logger;

        public RelatorioController(ApplicationDbContext context, ILogger<RelatorioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Relatorio
        public IActionResult Index()
        {
            return View();
        }

        // GET: Relatorio/Clientes
        public async Task<IActionResult> Clientes()
        {
            var clientes = await _context.Clientes
                .OrderBy(c => c.Nome)
                .ToListAsync();
            
            return View(clientes);
        }

        // GET: Relatorio/ExportarExcel
        public async Task<IActionResult> ExportarExcel()
        {
            try
            {
                var clientes = await _context.Clientes
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Clientes");

                // Cabeçalhos
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Nome";
                worksheet.Cells[1, 3].Value = "CPF";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "Telefone";
                worksheet.Cells[1, 6].Value = "Endereço";
                worksheet.Cells[1, 7].Value = "Bairro";
                worksheet.Cells[1, 8].Value = "Cidade";
                worksheet.Cells[1, 9].Value = "Estado Civil";
                worksheet.Cells[1, 10].Value = "Data Cadastro";

                // Estilizar cabeçalhos
                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }

                // Dados
                for (int i = 0; i < clientes.Count; i++)
                {
                    var cliente = clientes[i];
                    int row = i + 2;

                    worksheet.Cells[row, 1].Value = cliente.Id;
                    worksheet.Cells[row, 2].Value = cliente.Nome;
                    worksheet.Cells[row, 3].Value = cliente.CPF;
                    worksheet.Cells[row, 4].Value = cliente.Email;
                    worksheet.Cells[row, 5].Value = cliente.Telefone;
                    worksheet.Cells[row, 6].Value = cliente.Endereco;
                    worksheet.Cells[row, 7].Value = cliente.Bairro;
                    worksheet.Cells[row, 8].Value = cliente.Cidade;
                    worksheet.Cells[row, 9].Value = cliente.EstadoCivil;
                    worksheet.Cells[row, 10].Value = cliente.DataCadastro.ToString("dd/MM/yyyy HH:mm");
                }

                // Auto-ajustar colunas
                worksheet.Cells.AutoFitColumns();

                // Adicionar bordas
                using (var range = worksheet.Cells[1, 1, clientes.Count + 1, 10])
                {
                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                var fileName = $"Relatorio_Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                var fileBytes = package.GetAsByteArray();

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao exportar relatório Excel");
                TempData["ErrorMessage"] = "Erro ao gerar relatório Excel. Tente novamente.";
                return RedirectToAction(nameof(Clientes));
            }
        }

        // GET: Relatorio/ExportarPdf
        public async Task<IActionResult> ExportarPdf()
        {
            try
            {
                var clientes = await _context.Clientes
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                using var memoryStream = new MemoryStream();
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Configurar fontes
                var titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var dataFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                var footerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // Título
                var title = new Paragraph("RELATÓRIO DE CLIENTES")
                    .SetFont(titleFont)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20);
                document.Add(title);

                // Data de geração
                var dateParagraph = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(dataFont)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginBottom(20);
                document.Add(dateParagraph);

                // Tabela
                var table = new Table(5)
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetMarginTop(10);

                // Cabeçalhos da tabela
                var headerCells = new[]
                {
                    new Cell().Add(new Paragraph("Nome").SetFont(headerFont).SetFontSize(10)),
                    new Cell().Add(new Paragraph("CPF").SetFont(headerFont).SetFontSize(10)),
                    new Cell().Add(new Paragraph("Email").SetFont(headerFont).SetFontSize(10)),
                    new Cell().Add(new Paragraph("Telefone").SetFont(headerFont).SetFontSize(10)),
                    new Cell().Add(new Paragraph("Data Cadastro").SetFont(headerFont).SetFontSize(10))
                };

                foreach (var cell in headerCells)
                {
                    cell.SetBackgroundColor(ColorConstants.DARK_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetPadding(8);
                    table.AddCell(cell);
                }

                // Dados dos clientes
                foreach (var cliente in clientes)
                {
                    table.AddCell(new Cell().Add(new Paragraph(cliente.Nome).SetFont(dataFont).SetFontSize(9)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(cliente.CPF).SetFont(dataFont).SetFontSize(9)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(cliente.Email).SetFont(dataFont).SetFontSize(9)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(cliente.Telefone).SetFont(dataFont).SetFontSize(9)).SetPadding(6));
                    table.AddCell(new Cell().Add(new Paragraph(cliente.DataCadastro.ToString("dd/MM/yyyy")).SetFont(dataFont).SetFontSize(9)).SetPadding(6));
                }

                document.Add(table);

                // Rodapé com total
                var footer = new Paragraph($"Total de clientes: {clientes.Count}")
                    .SetFont(footerFont)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginTop(20);
                document.Add(footer);

                document.Close();

                var fileName = $"Relatorio_Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao exportar relatório PDF");
                TempData["ErrorMessage"] = "Erro ao gerar relatório PDF. Tente novamente.";
                return RedirectToAction(nameof(Clientes));
            }
        }

        // GET: Relatorio/ExportarCsv
        public async Task<IActionResult> ExportarCsv()
        {
            try
            {
                var clientes = await _context.Clientes
                    .OrderBy(c => c.Nome)
                    .ToListAsync();

                var csv = new StringBuilder();
                csv.AppendLine("ID,Nome,CPF,Email,Telefone,Endereco,Bairro,Cidade,Estado Civil,Data Cadastro");

                foreach (var cliente in clientes)
                {
                    csv.AppendLine($"{cliente.Id},{cliente.Nome},{cliente.CPF},{cliente.Email},{cliente.Telefone},{cliente.Endereco},{cliente.Bairro},{cliente.Cidade},{cliente.EstadoCivil},{cliente.DataCadastro:dd/MM/yyyy HH:mm}");
                }

                var fileName = $"Relatorio_Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                
                return File(bytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao exportar relatório CSV");
                TempData["ErrorMessage"] = "Erro ao gerar relatório CSV. Tente novamente.";
                return RedirectToAction(nameof(Clientes));
            }
        }
    }
}

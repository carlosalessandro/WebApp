using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public class NFCeService : INFCeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NFCeService> _logger;

        public NFCeService(ApplicationDbContext context, ILogger<NFCeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<NFCe> GerarNFCeAsync(int vendaId)
        {
            try
            {
                var venda = await _context.Vendas
                    .Include(v => v.Itens)
                    .ThenInclude(i => i.Produto)
                    .Include(v => v.Cliente)
                    .FirstOrDefaultAsync(v => v.Id == vendaId);

                if (venda == null)
                {
                    _logger.LogError($"Venda {vendaId} não encontrada");
                    return null;
                }

                // Simula a geração de uma NFC-e
                var numeroNFCe = new Random().Next(100000, 999999).ToString();
                var serieNFCe = "001";
                var chaveAcesso = GerarChaveAcesso();

                var nfce = new NFCe
                {
                    Numero = numeroNFCe,
                    Serie = serieNFCe,
                    ChaveAcesso = chaveAcesso,
                    DataEmissao = DateTime.Now,
                    Status = "Autorizada",
                    Xml = $"<nfeProc xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"4.00\"><NFe><infNFe Id=\"NFe{chaveAcesso}\"></infNFe></NFe></nfeProc>",
                    ProtocoloAutorizacao = new Random().Next(100000000, 999999999).ToString(),
                    DataAutorizacao = DateTime.Now,
                    MensagemRetorno = "NFC-e autorizada com sucesso",
                    UrlConsulta = $"https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx?chNFe={chaveAcesso}",
                    Ambiente = "Homologação",
                    TipoEmissao = "Normal",
                    VendaId = vendaId,
                    DataCriacao = DateTime.Now,
                    UltimaAtualizacao = DateTime.Now
                };

                _context.NFCes.Add(nfce);
                await _context.SaveChangesAsync();

                // Atualiza a venda com as informações da NFC-e
                venda.PossuiNFCe = true;
                venda.StatusNFCe = "Autorizada";
                venda.NumeroNFCe = numeroNFCe;
                venda.SerieNFCe = serieNFCe;
                venda.ChaveAcessoNFCe = chaveAcesso;
                venda.DataEmissaoNFCe = DateTime.Now;
                venda.NFCeId = nfce.Id;

                _context.Vendas.Update(venda);
                await _context.SaveChangesAsync();

                return nfce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao gerar NFC-e para venda {vendaId}");
                return null;
            }
        }

        public async Task<NFCe> ImportarNFCeAsync(string chaveAcesso)
        {
            try
            {
                // Simula a importação de uma NFC-e
                var nfceExistente = await _context.NFCes.FirstOrDefaultAsync(n => n.ChaveAcesso == chaveAcesso);
                if (nfceExistente != null)
                {
                    return nfceExistente;
                }

                // Cria uma nova venda para a NFC-e importada
                var venda = new Venda
                {
                    DataVenda = DateTime.Now,
                    Status = "Finalizada",
                    ValorTotal = 100.00m,
                    ValorFinal = 100.00m,
                    FormaPagamento = "Dinheiro",
                    UsuarioId = 1 // Usuário padrão
                };

                _context.Vendas.Add(venda);
                await _context.SaveChangesAsync();

                // Cria a NFC-e importada
                var nfce = new NFCe
                {
                    Numero = new Random().Next(100000, 999999).ToString(),
                    Serie = "001",
                    ChaveAcesso = chaveAcesso,
                    DataEmissao = DateTime.Now,
                    Status = "Importada",
                    Xml = $"<nfeProc xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"4.00\"><NFe><infNFe Id=\"NFe{chaveAcesso}\"></infNFe></NFe></nfeProc>",
                    ProtocoloAutorizacao = new Random().Next(100000000, 999999999).ToString(),
                    DataAutorizacao = DateTime.Now,
                    MensagemRetorno = "NFC-e importada com sucesso",
                    UrlConsulta = $"https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx?chNFe={chaveAcesso}",
                    Ambiente = "Produção",
                    TipoEmissao = "Normal",
                    VendaId = venda.Id,
                    DataCriacao = DateTime.Now,
                    UltimaAtualizacao = DateTime.Now
                };

                _context.NFCes.Add(nfce);
                await _context.SaveChangesAsync();

                // Atualiza a venda com as informações da NFC-e
                venda.PossuiNFCe = true;
                venda.StatusNFCe = "Importada";
                venda.NumeroNFCe = nfce.Numero;
                venda.SerieNFCe = nfce.Serie;
                venda.ChaveAcessoNFCe = chaveAcesso;
                venda.DataEmissaoNFCe = DateTime.Now;
                venda.NFCeId = nfce.Id;

                _context.Vendas.Update(venda);
                await _context.SaveChangesAsync();

                return nfce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao importar NFC-e com chave {chaveAcesso}");
                return null;
            }
        }

        public async Task<NFCe> ConsultarNFCeAsync(string chaveAcesso)
        {
            try
            {
                var nfce = await _context.NFCes.FirstOrDefaultAsync(n => n.ChaveAcesso == chaveAcesso);
                if (nfce == null)
                {
                    _logger.LogWarning($"NFC-e com chave {chaveAcesso} não encontrada");
                    return null;
                }

                return nfce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao consultar NFC-e com chave {chaveAcesso}");
                return null;
            }
        }

        public async Task<bool> CancelarNFCeAsync(int nfceId, string justificativa)
        {
            try
            {
                var nfce = await _context.NFCes.FindAsync(nfceId);
                if (nfce == null)
                {
                    _logger.LogWarning($"NFC-e {nfceId} não encontrada");
                    return false;
                }

                // Simula o cancelamento da NFC-e
                nfce.Status = "Cancelada";
                nfce.MensagemRetorno = $"NFC-e cancelada: {justificativa}";
                nfce.UltimaAtualizacao = DateTime.Now;

                _context.NFCes.Update(nfce);
                await _context.SaveChangesAsync();

                // Atualiza o status na venda
                var venda = await _context.Vendas.FirstOrDefaultAsync(v => v.NFCeId == nfceId);
                if (venda != null)
                {
                    venda.StatusNFCe = "Cancelada";
                    _context.Vendas.Update(venda);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao cancelar NFC-e {nfceId}");
                return false;
            }
        }

        public async Task<string> GerarXmlNFCeAsync(int vendaId)
        {
            try
            {
                var venda = await _context.Vendas
                    .Include(v => v.NFCe)
                    .FirstOrDefaultAsync(v => v.Id == vendaId);

                if (venda == null || venda.NFCe == null)
                {
                    _logger.LogWarning($"Venda {vendaId} não possui NFC-e");
                    return null;
                }

                return venda.NFCe.Xml;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao gerar XML da NFC-e para venda {vendaId}");
                return null;
            }
        }

        public async Task<string> GerarQRCodeNFCeAsync(int nfceId)
        {
            try
            {
                var nfce = await _context.NFCes.FindAsync(nfceId);
                if (nfce == null)
                {
                    _logger.LogWarning($"NFC-e {nfceId} não encontrada");
                    return null;
                }

                // Simula a geração do QR Code
                return $"https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx?chNFe={nfce.ChaveAcesso}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao gerar QR Code para NFC-e {nfceId}");
                return null;
            }
        }

        // Método auxiliar para gerar uma chave de acesso aleatória
        private string GerarChaveAcesso()
        {
            var random = new Random();
            var chave = "43"; // UF RS
            chave += DateTime.Now.ToString("yyMM"); // Ano e mês
            chave += "12345678"; // CNPJ (simplificado)
            chave += "65"; // Modelo 65 = NFC-e
            chave += "001"; // Série
            chave += random.Next(100000000, 999999999).ToString(); // Número
            chave += "1"; // Tipo de emissão
            chave += random.Next(10000000, 99999999).ToString(); // Código

            // Dígito verificador (simplificado)
            chave += "0";

            return chave;
        }
    }
}
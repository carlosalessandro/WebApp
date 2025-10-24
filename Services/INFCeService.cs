using System;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface INFCeService
    {
        Task<NFCe> GerarNFCeAsync(int vendaId);
        Task<NFCe> ImportarNFCeAsync(string chaveAcesso);
        Task<NFCe> ConsultarNFCeAsync(string chaveAcesso);
        Task<bool> CancelarNFCeAsync(int nfceId, string justificativa);
        Task<string> GerarXmlNFCeAsync(int vendaId);
        Task<string> GerarQRCodeNFCeAsync(int nfceId);
    }
}
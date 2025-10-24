using System.Threading.Tasks;

namespace WebApp.Services
{
    public interface IWhatsAppService
    {
        Task<bool> EnviarMensagemTexto(string numeroDestino, string mensagem);
        Task<bool> VerificarConexao();
    }
}
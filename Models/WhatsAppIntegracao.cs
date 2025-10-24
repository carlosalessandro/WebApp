using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class WhatsAppIntegracao
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O token de acesso é obrigatório")]
        [Display(Name = "Token de Acesso")]
        public string TokenAcesso { get; set; }
        
        [Required(ErrorMessage = "O número de telefone é obrigatório")]
        [Display(Name = "Número de Telefone")]
        public string NumeroTelefone { get; set; }
        
        [Display(Name = "ID da Conta de Negócios")]
        public string BusinessAccountId { get; set; }
        
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
        
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        
        [Display(Name = "Última Atualização")]
        public DateTime? UltimaAtualizacao { get; set; }
        
        [Display(Name = "Webhook URL")]
        public string WebhookUrl { get; set; }
        
        [Display(Name = "Webhook Secret")]
        public string WebhookSecret { get; set; }
    }
}
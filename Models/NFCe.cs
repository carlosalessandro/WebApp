using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class NFCe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Required]
        [Display(Name = "Série")]
        public string Serie { get; set; }

        [Required]
        [Display(Name = "Chave de Acesso")]
        public string ChaveAcesso { get; set; }

        [Required]
        [Display(Name = "Data de Emissão")]
        public DateTime DataEmissao { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "XML")]
        public string Xml { get; set; }

        [Display(Name = "Protocolo de Autorização")]
        public string ProtocoloAutorizacao { get; set; }

        [Display(Name = "Data de Autorização")]
        public DateTime? DataAutorizacao { get; set; }

        [Display(Name = "Mensagem de Retorno")]
        public string MensagemRetorno { get; set; }

        [Display(Name = "URL de Consulta")]
        public string UrlConsulta { get; set; }

        [Display(Name = "Ambiente")]
        public string Ambiente { get; set; } = "Homologação"; // Homologação ou Produção

        [Display(Name = "Tipo de Emissão")]
        public string TipoEmissao { get; set; } = "Normal"; // Normal, Contingência, etc.

        [Display(Name = "Venda")]
        public int VendaId { get; set; }

        [ForeignKey("VendaId")]
        public Venda Venda { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Última Atualização")]
        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;
    }
}
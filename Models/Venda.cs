using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DataVenda { get; set; } = DateTime.Now;

        [Display(Name = "Cliente")]
        public int? ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public User Usuario { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Valor Total")]
        public decimal ValorTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Desconto")]
        public decimal Desconto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Valor Final")]
        public decimal ValorFinal { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = "Aberta";

        // Campos para NFC-e
        [Display(Name = "Possui NFC-e")]
        public bool PossuiNFCe { get; set; } = false;

        [Display(Name = "Status NFC-e")]
        public string StatusNFCe { get; set; }

        [Display(Name = "Número NFC-e")]
        public string NumeroNFCe { get; set; }

        [Display(Name = "Série NFC-e")]
        public string SerieNFCe { get; set; }

        [Display(Name = "Chave de Acesso NFC-e")]
        public string ChaveAcessoNFCe { get; set; }

        [Display(Name = "Data Emissão NFC-e")]
        public DateTime? DataEmissaoNFCe { get; set; }

        // Relacionamento com NFCe detalhada
        public int? NFCeId { get; set; }

        [ForeignKey("NFCeId")]
        public NFCe NFCe { get; set; }

        public virtual ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
}
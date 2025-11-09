using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class MovimentacaoFinanceira
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required]
        public TipoMovimentacao Tipo { get; set; }

        [Required]
        public DateTime DataMovimentacao { get; set; }

        [Required]
        public int ContaBancariaId { get; set; }

        [ForeignKey("ContaBancariaId")]
        public virtual ContaBancaria? ContaBancaria { get; set; }

        [Required]
        public int CategoriaFinanceiraId { get; set; }

        [ForeignKey("CategoriaFinanceiraId")]
        public virtual CategoriaFinanceira? CategoriaFinanceira { get; set; }

        public int? ContaPagarId { get; set; }

        [ForeignKey("ContaPagarId")]
        public virtual ContaPagar? ContaPagar { get; set; }

        public int? ContaReceberId { get; set; }

        [ForeignKey("ContaReceberId")]
        public virtual ContaReceber? ContaReceber { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Required]
        public int CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual User? CriadoPor { get; set; }
    }

    public enum TipoMovimentacao
    {
        Entrada = 1,
        Saida = 2,
        Transferencia = 3
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class ContaPagar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string NumeroDocumento { get; set; }

        [Required]
        [StringLength(200)]
        public required string Descricao { get; set; }

        [Required]
        public int FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        public virtual Fornecedor Fornecedor { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorOriginal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorPago { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDesconto { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorJuros { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorMulta { get; set; } = 0;

        [Required]
        public DateTime DataVencimento { get; set; }

        public DateTime? DataPagamento { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public StatusConta Status { get; set; } = StatusConta.Aberta;

        [StringLength(500)]
        public string Observacoes { get; set; }

        [Required]
        public int CategoriaFinanceiraId { get; set; }

        [ForeignKey("CategoriaFinanceiraId")]
        public virtual CategoriaFinanceira CategoriaFinanceira { get; set; }

        public int? ContaBancariaId { get; set; }

        [ForeignKey("ContaBancariaId")]
        public virtual ContaBancaria ContaBancaria { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        [Required]
        public int CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual User CriadoPor { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public decimal ValorSaldo => ValorOriginal - ValorPago;

        [NotMapped]
        public decimal ValorTotal => ValorOriginal + ValorJuros + ValorMulta - ValorDesconto;

        [NotMapped]
        public bool Vencida => Status == StatusConta.Aberta && DataVencimento < DateTime.Today;

        [NotMapped]
        public int DiasVencimento => Status == StatusConta.Aberta ? (DateTime.Today - DataVencimento).Days : 0;
    }

    public enum StatusConta
    {
        Aberta = 1,
        Paga = 2,
        Cancelada = 3,
        Parcial = 4
    }
}

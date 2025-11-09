using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class ContaReceber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorOriginal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorRecebido { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDesconto { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorJuros { get; set; } = 0;

        [Required]
        public DateTime DataVencimento { get; set; }

        public DateTime? DataRecebimento { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public StatusConta Status { get; set; } = StatusConta.Aberta;

        [StringLength(500)]
        public string? Observacoes { get; set; }

        [Required]
        public int CategoriaFinanceiraId { get; set; }

        [ForeignKey("CategoriaFinanceiraId")]
        public virtual CategoriaFinanceira? CategoriaFinanceira { get; set; }

        public int? ContaBancariaId { get; set; }

        [ForeignKey("ContaBancariaId")]
        public virtual ContaBancaria? ContaBancaria { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        [Required]
        public int CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual User? CriadoPor { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public decimal ValorSaldo => ValorOriginal - ValorRecebido;

        [NotMapped]
        public decimal ValorTotal => ValorOriginal + ValorJuros - ValorDesconto;

        [NotMapped]
        public bool Vencida => Status == StatusConta.Aberta && DataVencimento < DateTime.Today;

        [NotMapped]
        public int DiasVencimento => Status == StatusConta.Aberta ? (DateTime.Today - DataVencimento).Days : 0;
    }
}

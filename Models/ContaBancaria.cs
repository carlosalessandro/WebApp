using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class ContaBancaria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Banco { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Agencia { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Conta { get; set; } = string.Empty;

        [StringLength(2)]
        public string? DigitoVerificador { get; set; }

        [Required]
        public TipoConta Tipo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoAtual { get; set; } = 0;

        public bool Ativa { get; set; } = true;

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        // Navegação
        public virtual ICollection<ContaPagar>? ContasPagar { get; set; }
        public virtual ICollection<ContaReceber>? ContasReceber { get; set; }
        public virtual ICollection<MovimentacaoFinanceira>? Movimentacoes { get; set; }
    }

    public enum TipoConta
    {
        ContaCorrente = 1,
        Poupanca = 2,
        ContaInvestimento = 3,
        Caixa = 4
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class CategoriaFinanceira
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        public TipoCategoria Tipo { get; set; }

        public bool Ativa { get; set; } = true;

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        // Navegação
        public virtual ICollection<ContaPagar>? ContasPagar { get; set; }
        public virtual ICollection<ContaReceber>? ContasReceber { get; set; }
        public virtual ICollection<MovimentacaoFinanceira>? Movimentacoes { get; set; }
    }

    public enum TipoCategoria
    {
        Receita = 1,
        Despesa = 2,
        Transferencia = 3
    }
}

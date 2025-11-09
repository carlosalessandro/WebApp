using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Estoque
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto? Produto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal QuantidadeAtual { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal QuantidadeMinima { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal QuantidadeMaxima { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CustoMedio { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UltimoCusto { get; set; } = 0;

        [StringLength(50)]
        public string? Localizacao { get; set; }

        [Required]
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        // Propriedades calculadas
        [NotMapped]
        public bool EstoqueBaixo => QuantidadeAtual <= QuantidadeMinima;

        [NotMapped]
        public bool EstoqueAlto => QuantidadeAtual >= QuantidadeMaxima;

        [NotMapped]
        public decimal ValorEstoque => QuantidadeAtual * CustoMedio;

        // Navegação
        public virtual ICollection<MovimentacaoEstoque>? Movimentacoes { get; set; }
    }

    public class MovimentacaoEstoque
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstoqueId { get; set; }

        [ForeignKey("EstoqueId")]
        public virtual Estoque? Estoque { get; set; }

        [Required]
        public TipoMovimentacaoEstoque Tipo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantidade { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CustoUnitario { get; set; }

        [Required]
        [StringLength(200)]
        public string Motivo { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Observacoes { get; set; }

        [Required]
        public DateTime DataMovimentacao { get; set; } = DateTime.Now;

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual User? Usuario { get; set; }

        // Referências opcionais
        public int? VendaId { get; set; }
        public int? PedidoCompraId { get; set; }
        public int? OrdemProducaoId { get; set; }

        [NotMapped]
        public decimal? ValorTotal => Quantidade * (CustoUnitario ?? 0);
    }

    public enum TipoMovimentacaoEstoque
    {
        Entrada = 1,
        Saida = 2,
        Ajuste = 3,
        Transferencia = 4,
        Producao = 5,
        Venda = 6,
        Compra = 7,
        Devolucao = 8
    }
}

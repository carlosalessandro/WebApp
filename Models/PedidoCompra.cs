using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class PedidoCompra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        public int FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        public virtual Fornecedor? Fornecedor { get; set; }

        [Required]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        public DateTime? DataEntrega { get; set; }

        [Required]
        public StatusPedidoCompra Status { get; set; } = StatusPedidoCompra.Pendente;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorDesconto { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorFrete { get; set; } = 0;

        [StringLength(500)]
        public string? Observacoes { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        [Required]
        public int CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual User? CriadoPor { get; set; }

        // Navegação
        public virtual ICollection<ItemPedidoCompra>? Itens { get; set; }

        [NotMapped]
        public decimal ValorFinal => ValorTotal + ValorFrete - ValorDesconto;
    }

    public class ItemPedidoCompra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PedidoCompraId { get; set; }

        [ForeignKey("PedidoCompraId")]
        public virtual PedidoCompra? PedidoCompra { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto? Produto { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantidade { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Desconto { get; set; } = 0;

        [Column(TypeName = "decimal(18,3)")]
        public decimal QuantidadeRecebida { get; set; } = 0;

        [StringLength(200)]
        public string? Observacoes { get; set; }

        [NotMapped]
        public decimal ValorTotal => Quantidade * PrecoUnitario - Desconto;

        [NotMapped]
        public decimal QuantidadePendente => Quantidade - QuantidadeRecebida;

        [NotMapped]
        public bool TotalmenteRecebido => QuantidadeRecebida >= Quantidade;
    }

    public enum StatusPedidoCompra
    {
        Pendente = 1,
        Aprovado = 2,
        Enviado = 3,
        RecebidoParcial = 4,
        Recebido = 5,
        Cancelado = 6
    }
}

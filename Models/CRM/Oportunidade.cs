using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.CRM
{
    public class Oportunidade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public int LeadId { get; set; }

        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; } = null!;

        [Required]
        public StatusOportunidade Status { get; set; } = StatusOportunidade.Identificacao;

        [Required]
        public int ResponsavelId { get; set; }

        [ForeignKey("ResponsavelId")]
        public virtual User Responsavel { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorEstimado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ProbabilidadeSucesso { get; set; } = 10;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataFechamentoPrevista { get; set; }

        public DateTime? DataFechamentoReal { get; set; }

        [StringLength(1000)]
        public string? ProdutosServicos { get; set; }

        [StringLength(2000)]
        public string? Observacoes { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public decimal ValorEsperado => ValorEstimado * (ProbabilidadeSucesso / 100);

        [NotMapped]
        public int DiasParaFechamento => DataFechamentoPrevista.HasValue ? 
            (DataFechamentoPrevista.Value - DateTime.Today).Days : 0;

        [NotMapped]
        public bool Urgente => DiasParaFechamento <= 7 && DiasParaFechamento >= 0;

        // Navegação
        public virtual ICollection<AtividadeOportunidade> Atividades { get; set; } = new List<AtividadeOportunidade>();
        public virtual ICollection<PropostaComercial> Propostas { get; set; } = new List<PropostaComercial>();
    }

    public enum StatusOportunidade
    {
        Identificacao,
        Qualificacao,
        Analise,
        Proposta,
        Negociacao,
        FechadaGanha,
        FechadaPerdida,
        Cancelada
    }

    public class PropostaComercial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        public int OportunidadeId { get; set; }

        [ForeignKey("OportunidadeId")]
        public virtual Oportunidade Oportunidade { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Desconto { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorFinal { get; set; }

        [Required]
        public StatusProposta Status { get; set; } = StatusProposta.Rascunho;

        public DateTime DataEmissao { get; set; } = DateTime.Now;

        public DateTime? DataValidade { get; set; }

        public DateTime? DataAceite { get; set; }

        [StringLength(2000)]
        public string? TermosCondicoes { get; set; }

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        public int? CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual User? CriadoPor { get; set; }

        // Navegação
        public virtual ICollection<ItemProposta> Itens { get; set; } = new List<ItemProposta>();
    }

    public enum StatusProposta
    {
        Rascunho,
        Enviada,
        EmAnalise,
        Aceita,
        Rejeitada,
        Vencida
    }

    public class ItemProposta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropostaId { get; set; }

        [ForeignKey("PropostaId")]
        public virtual PropostaComercial Proposta { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantidade { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorUnitario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Desconto { get; set; } = 0;

        [StringLength(50)]
        public string Unidade { get; set; } = "UN";

        // Propriedades calculadas
        [NotMapped]
        public decimal ValorTotal => Quantidade * ValorUnitario - Desconto;
    }
}

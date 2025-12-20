using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.ERP
{
    public class PlanoContas
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        public TipoConta Tipo { get; set; }

        public int? PaiId { get; set; }

        [ForeignKey("PaiId")]
        public virtual PlanoContas? Pai { get; set; }

        [Required]
        public int Nivel { get; set; } = 1;

        [Required]
        public bool Ativa { get; set; } = true;

        [Required]
        public bool Analitica { get; set; } = false;

        [StringLength(100)]
        public string? ClassificacaoFiscal { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataUltimaAlteracao { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public string CodigoCompleto => Pai?.CodigoCompleto + "." + Codigo ?? Codigo;

        [NotMapped]
        public string NomeCompleto => Pai?.NomeCompleto + " > " + Nome ?? Nome;

        // Navegação
        public virtual ICollection<PlanoContas> Filhos { get; set; } = new List<PlanoContas>();
        public virtual ICollection<LancamentoContabil> LancamentosDebito { get; set; } = new List<LancamentoContabil>();
        public virtual ICollection<LancamentoContabil> LancamentosCredito { get; set; } = new List<LancamentoContabil>();
    }

    public enum TipoConta
    {
        Ativo,
        Passivo,
        PatrimonioLiquido,
        Receita,
        Despesa
    }

    public class LancamentoContabil
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required]
        public DateTime DataLancamento { get; set; }

        [Required]
        [StringLength(500)]
        public string Historico { get; set; } = string.Empty;

        [Required]
        public TipoLancamento Tipo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required]
        public int ContaDebitoId { get; set; }

        [ForeignKey("ContaDebitoId")]
        public virtual PlanoContas ContaDebito { get; set; } = null!;

        [Required]
        public int ContaCreditoId { get; set; }

        [ForeignKey("ContaCreditoId")]
        public virtual PlanoContas ContaCredito { get; set; } = null!;

        public int? CentroCustoId { get; set; }

        [ForeignKey("CentroCustoId")]
        public virtual CentroCusto? CentroCusto { get; set; }

        public int? ProjetoId { get; set; }

        [ForeignKey("ProjetoId")]
        public virtual Projeto? Projeto { get; set; }

        public int? CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual User? CriadoPor { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Required]
        public StatusLancamento Status { get; set; } = StatusLancamento.Pendente;

        public DateTime? DataAprovacao { get; set; }

        public int? AprovadoPorId { get; set; }

        [ForeignKey("AprovadoPorId")]
        public virtual User? AprovadoPor { get; set; }

        [StringLength(1000)]
        public string? Observacoes { get; set; }
    }

    public enum TipoLancamento
    {
        Manual,
        Automatico,
        Integracao
    }

    public enum StatusLancamento
    {
        Pendente,
        Aprovado,
        Rejeitado,
        Cancelado
    }

    public class CentroCusto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        public int? GerenteId { get; set; }

        [ForeignKey("GerenteId")]
        public virtual User? Gerente { get; set; }

        public int? DepartamentoId { get; set; }

        [ForeignKey("DepartamentoId")]
        public virtual Departamento? Departamento { get; set; }

        [Required]
        public bool Ativo { get; set; } = true;

        [Column(TypeName = "decimal(18,2)")]
        public decimal OrcamentoAnual { get; set; } = 0;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navegação
        public virtual ICollection<LancamentoContabil> Lancamentos { get; set; } = new List<LancamentoContabil>();
    }

    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descricao { get; set; }

        public int? GerenteId { get; set; }

        [ForeignKey("GerenteId")]
        public virtual User? Gerente { get; set; }

        [Required]
        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navegação
        public virtual ICollection<CentroCusto> CentrosCusto { get; set; } = new List<CentroCusto>();
    }
}

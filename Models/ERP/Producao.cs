using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.ERP
{
    public class OrdemProducaoCompleta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NumeroOP { get; set; } = string.Empty;

        [Required]
        public int ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadePlanejada { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeProduzida { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeDefeitos { get; set; } = 0;

        [Required]
        public PrioridadeOP Prioridade { get; set; }

        [Required]
        public StatusOP Status { get; set; } = StatusOP.Planejada;

        public int? GerenteProducaoId { get; set; }

        [ForeignKey("GerenteProducaoId")]
        public virtual User? GerenteProducao { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; } = DateTime.Now;

        public DateTime? DataInicioPlanejada { get; set; }

        public DateTime? DataFimPlanejada { get; set; }

        public DateTime? DataInicioReal { get; set; }

        public DateTime? DataFimReal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CustoEstimado { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CustoReal { get; set; } = 0;

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        public int? ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        public int? PedidoVendaId { get; set; }

        [StringLength(1000)]
        public string? EspecificacoesTecnicas { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public decimal PercentualConclusao => QuantidadePlanejada > 0 ? 
            (QuantidadeProduzida / QuantidadePlanejada) * 100 : 0;

        [NotMapped]
        public decimal TaxaDefeitos => QuantidadeProduzida > 0 ? 
            (QuantidadeDefeitos / QuantidadeProduzida) * 100 : 0;

        [NotMapped]
        public bool Atrasada => DataFimPlanejada.HasValue && 
                                DataFimPlanejada!.Value < DateTime.Today && 
                                Status != StatusOP.Concluida;

        [NotMapped]
        public int DiasAtraso => Atrasada ? 
            (DateTime.Today - DataFimPlanejada!.Value).Days : 0;

        // Navegação
        public virtual ICollection<ApontamentoProducaoCompleto> Apontamentos { get; set; } = new List<ApontamentoProducaoCompleto>();
        public virtual ICollection<RecursoAlocadoOP> RecursosAlocados { get; set; } = new List<RecursoAlocadoOP>();
        public virtual ICollection<InspecaoQualidade> Inspecoes { get; set; } = new List<InspecaoQualidade>();
    }

    public enum PrioridadeOP
    {
        Baixa,
        Normal,
        Alta,
        Urgente,
        Critica
    }

    public enum StatusOP
    {
        Planejada,
        Liberada,
        EmProducao,
        Pausada,
        Concluida,
        Cancelada
    }

    public class ApontamentoProducaoCompleto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrdemProducaoId { get; set; }

        [ForeignKey("OrdemProducaoId")]
        public virtual OrdemProducaoCompleta OrdemProducao { get; set; } = null!;

        [Required]
        public int RecursoId { get; set; }

        [ForeignKey("RecursoId")]
        public virtual RecursoProducao Recurso { get; set; } = null!;

        [Required]
        public int? GerenteProducaoId { get; set; }

        [ForeignKey("GerenteProducaoId")]
        public virtual User? GerenteProducao { get; set; }

        [Required]
        public int OperadorId { get; set; }

        [ForeignKey("OperadorId")]
        public virtual User Operador { get; set; } = null!;

        [Required]
        public DateTime DataHoraInicio { get; set; }

        public DateTime? DataHoraFim { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeProduzida { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeDefeitos { get; set; } = 0;

        [Required]
        public StatusApontamento Status { get; set; } = StatusApontamento.EmAndamento;

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HorasTrabalhadas => DataHoraFim.HasValue ? 
            (decimal)(DataHoraFim.Value - DataHoraInicio).TotalHours : 0;

        // Propriedades calculadas
        [NotMapped]
        public decimal ProdutividadeHoraria => HorasTrabalhadas > 0 ? 
            QuantidadeProduzida / HorasTrabalhadas : 0;
    }

    public enum StatusApontamento
    {
        EmAndamento,
        Concluido,
        Pausado,
        Cancelado
    }

    public class RecursoProducao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        public TipoRecurso Tipo { get; set; }

        [Required]
        public StatusRecurso Status { get; set; } = StatusRecurso.Disponivel;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CapacidadePorHora { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CustoPorHora { get; set; } = 0;

        [StringLength(500)]
        public string? Descricao { get; set; }

        [StringLength(100)]
        public string? Localizacao { get; set; }

        public DateTime? UltimaManutencao { get; set; }

        public DateTime? ProximaManutencao { get; set; }

        [Required]
        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Propriedades calculadas
        [NotMapped]
        public bool NecessitaManutencao => ProximaManutencao.HasValue && 
                                         ProximaManutencao.Value <= DateTime.Today;

        [NotMapped]
        public int DiasParaManutencao => ProximaManutencao.HasValue ? 
            (ProximaManutencao.Value - DateTime.Today).Days : 0;

        // Navegação
        public virtual ICollection<ApontamentoProducaoCompleto> Apontamentos { get; set; } = new List<ApontamentoProducaoCompleto>();
        public virtual ICollection<RecursoAlocadoOP> Alocacoes { get; set; } = new List<RecursoAlocadoOP>();
    }

    public enum TipoRecurso
    {
        Maquina,
        Equipamento,
        Ferramenta,
        MateriaPrima,
        MaoDeObra,
        Outro
    }

    public enum StatusRecurso
    {
        Disponivel,
        Ocupado,
        Manutencao,
        Inativo
    }

    public class RecursoAlocadoOP
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrdemProducaoId { get; set; }

        [ForeignKey("OrdemProducaoId")]
        public virtual OrdemProducaoCompleta OrdemProducao { get; set; } = null!;

        [Required]
        public int RecursoId { get; set; }

        [ForeignKey("RecursoId")]
        public virtual RecursoProducao Recurso { get; set; } = null!;

        public DateTime DataAlocacao { get; set; } = DateTime.Now;

        public DateTime? DataDesalocacao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PercentualAlocacao { get; set; } = 100;

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }

    public class InspecaoQualidade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrdemProducaoId { get; set; }

        [ForeignKey("OrdemProducaoId")]
        public virtual OrdemProducaoCompleta OrdemProducao { get; set; } = null!;

        [Required]
        public DateTime DataInspecao { get; set; } = DateTime.Now;

        [Required]
        public int InspetorId { get; set; }

        [ForeignKey("InspetorId")]
        public virtual User Inspetor { get; set; } = null!;

        [Required]
        public TipoInspecao Tipo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeInspecionada { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeAprovada { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeRejeitada { get; set; }

        [Required]
        public ResultadoInspecao Resultado { get; set; }

        [StringLength(2000)]
        public string? Observacoes { get; set; }

        [StringLength(1000)]
        public string? DefeitosEncontrados { get; set; }

        public bool Liberada { get; set; } = false;

        public DateTime? DataLiberacao { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public decimal TaxaAprovacao => QuantidadeInspecionada > 0 ? 
            (QuantidadeAprovada / QuantidadeInspecionada) * 100 : 0;
    }

    public enum TipoInspecao
    {
        RecebimentoMateriaPrima,
        ProcessoProducao,
        ProdutoFinal,
        Amostragem,
        Completa
    }

    public enum ResultadoInspecao
    {
        Aprovado,
        Reprovado,
        AprovadoComRestricoes,
        Pendente
    }
}

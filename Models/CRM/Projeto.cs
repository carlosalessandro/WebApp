using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Projeto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public StatusProjeto Status { get; set; } = StatusProjeto.Planejamento;

        [Required]
        public PrioridadeProjeto Prioridade { get; set; } = PrioridadeProjeto.Media;

        public DateTime DataInicio { get; set; } = DateTime.Now;

        public DateTime? DataFimPrevista { get; set; }

        public DateTime? DataFimReal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Orcamento { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CustoAtual { get; set; } = 0;

        public int? GerenteId { get; set; }

        [ForeignKey("GerenteId")]
        public virtual User? Gerente { get; set; }

        public int? ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataUltimaAtualizacao { get; set; }

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public decimal PercentualConclusao => 0; // Será calculado com base nas tarefas

        [NotMapped]
        public bool Atrasado => DataFimPrevista.HasValue && 
                                DataFimPrevista.Value < DateTime.Today && 
                                Status != StatusProjeto.Concluido;

        [NotMapped]
        public int DiasAtraso => Atrasado ? 
            (DateTime.Today - DataFimPrevista!.Value).Days : 0;

        [NotMapped]
        public decimal VariacaoOrcamento => Orcamento > 0 ? 
            ((CustoAtual - Orcamento) / Orcamento) * 100 : 0;
    }

    public enum StatusProjeto
    {
        Planejamento,
        EmAndamento,
        Pausado,
        Concluido,
        Cancelado
    }

    public enum PrioridadeProjeto
    {
        Baixa,
        Media,
        Alta,
        Critica
    }
}

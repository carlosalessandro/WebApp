using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.CRM
{
    public class CampanhaMarketing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        [Required]
        public TipoCampanha Tipo { get; set; }

        [Required]
        public StatusCampanha Status { get; set; } = StatusCampanha.Rascunho;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Orcamento { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal InvestimentoAtual { get; set; } = 0;

        [StringLength(1000)]
        public string? PublicoAlvo { get; set; }

        [StringLength(2000)]
        public string? Mensagem { get; set; }

        public int? CriadoPorId { get; set; }

        [ForeignKey("CriadoPorId")]
        public virtual User? CriadoPor { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public bool Ativa => Status == StatusCampanha.Ativa && 
                           DataInicio <= DateTime.Today && 
                           DataFim >= DateTime.Today;

        [NotMapped]
        public decimal ROI => InvestimentoAtual > 0 ? (RetornoEstimado - InvestimentoAtual) / InvestimentoAtual * 100 : 0;

        [NotMapped]
        public decimal RetornoEstimado => 0; // Será calculado com base nos resultados

        // Navegação
        public virtual ICollection<AtividadeCampanha> Atividades { get; set; } = new List<AtividadeCampanha>();
        public virtual ICollection<LeadCampanha> Leads { get; set; } = new List<LeadCampanha>();
    }

    public enum TipoCampanha
    {
        Email,
        SMS,
        WhatsApp,
        SocialMedia,
        GoogleAds,
        FacebookAds,
        LinkedInAds,
        Evento,
        Outros
    }

    public enum StatusCampanha
    {
        Rascunho,
        Ativa,
        Pausada,
        Concluida,
        Cancelada
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.CRM
{
    public class AtividadeCRM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public TipoAtividade Tipo { get; set; }

        [Required]
        public StatusAtividade Status { get; set; } = StatusAtividade.Pendente;

        [Required]
        public DateTime DataAgendamento { get; set; }

        public DateTime? DataConclusao { get; set; }

        [Required]
        public int ResponsavelId { get; set; }

        [ForeignKey("ResponsavelId")]
        public virtual User Responsavel { get; set; } = null!;

        public int? LeadId { get; set; }

        [ForeignKey("LeadId")]
        public virtual Lead? Lead { get; set; }

        public int? OportunidadeId { get; set; }

        [ForeignKey("OportunidadeId")]
        public virtual Oportunidade? Oportunidade { get; set; }

        public int? ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente? Cliente { get; set; }

        [StringLength(1000)]
        public string? Resultado { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public bool Atrasada => Status == StatusAtividade.Pendente && DataAgendamento < DateTime.Today;

        [NotMapped]
        public bool Hoje => Status == StatusAtividade.Pendente && DataAgendamento.Date == DateTime.Today;

        [NotMapped]
        public int DiasAtraso => Atrasada ? (DateTime.Today - DataAgendamento).Days : 0;
    }

    public enum TipoAtividade
    {
        Ligacao,
        Email,
        Reuniao,
        Visita,
        Apresentacao,
        Seguimento,
        TarefaInterna,
        Outro
    }

    public enum StatusAtividade
    {
        Pendente,
        EmAndamento,
        Concluida,
        Cancelada
    }

    // Classes especializadas para cada tipo de entidade
    public class AtividadeLead : AtividadeCRM
    {
        [Required]
        public int LeadEspecificoId { get; set; }

        [ForeignKey("LeadEspecificoId")]
        public virtual Lead LeadEspecifico { get; set; } = null!;
    }

    public class AtividadeOportunidade : AtividadeCRM
    {
        [Required]
        public int OportunidadeEspecificaId { get; set; }

        [ForeignKey("OportunidadeEspecificaId")]
        public virtual Oportunidade OportunidadeEspecifica { get; set; } = null!;
    }

    public class AtividadeCampanha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CampanhaId { get; set; }

        [ForeignKey("CampanhaId")]
        public virtual CampanhaMarketing Campanha { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public TipoAtividadeCampanha Tipo { get; set; }

        [Required]
        public StatusAtividadeCampanha Status { get; set; } = StatusAtividadeCampanha.Pendente;

        public DateTime DataAgendamento { get; set; }

        public DateTime? DataExecucao { get; set; }

        [StringLength(2000)]
        public string? Descricao { get; set; }

        [StringLength(2000)]
        public string? Resultado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Custo { get; set; }

        public int? ContatosAlcancados { get; set; }

        public int? ConversoesGeradas { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public enum TipoAtividadeCampanha
    {
        EnvioEmail,
        EnvioSMS,
        PostSocialMedia,
        AnuncioOnline,
        Evento,
        Pesquisa,
        Outro
    }

    public enum StatusAtividadeCampanha
    {
        Pendente,
        Executando,
        Concluida,
        Cancelada
    }

    public class LeadCampanha
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CampanhaId { get; set; }

        [ForeignKey("CampanhaId")]
        public virtual CampanhaMarketing Campanha { get; set; } = null!;

        [Required]
        public int LeadId { get; set; }

        [ForeignKey("LeadId")]
        public virtual Lead Lead { get; set; } = null!;

        public DateTime DataAssociacao { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? OrigemAssociacao { get; set; }

        public bool Convertido { get; set; } = false;

        public DateTime? DataConversao { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.CRM
{
    public class Lead
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Empresa { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(500)]
        public string? Cargo { get; set; }

        [StringLength(1000)]
        public string? Descricao { get; set; }

        [Required]
        public StatusLead Status { get; set; } = StatusLead.Novo;

        [Required]
        public int OrigemId { get; set; }

        [ForeignKey("OrigemId")]
        public virtual OrigemLead Origem { get; set; } = null!;

        public int? CampanhaId { get; set; }

        [ForeignKey("CampanhaId")]
        public virtual CampanhaMarketing? Campanha { get; set; }

        public int? ResponsavelId { get; set; }

        [ForeignKey("ResponsavelId")]
        public virtual User? Responsavel { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorEstimado { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataPrimeiroContato { get; set; }

        public DateTime? DataUltimoContato { get; set; }

        public DateTime? DataConversao { get; set; }

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        // Propriedades calculadas
        [NotMapped]
        public int DiasNoFunil => (DateTime.Today - DataCriacao).Days;

        [NotMapped]
        public bool Quente => Status == StatusLead.Qualificado && DiasNoFunil <= 30;

        [NotMapped]
        public string Score => CalcularScore();

        // Navegação
        public virtual ICollection<AtividadeLead> Atividades { get; set; } = new List<AtividadeLead>();
        public virtual ICollection<Oportunidade> Oportunidades { get; set; } = new List<Oportunidade>();

        private string CalcularScore()
        {
            var score = 0;
            
            // Baseado no status
            score += Status switch
            {
                StatusLead.Novo => 20,
                StatusLead.Contatado => 40,
                StatusLead.Qualificado => 60,
                StatusLead.Proposta => 80,
                StatusLead.Negociacao => 90,
                StatusLead.Fechado => 100,
                StatusLead.Perdido => 0,
                _ => 0
            };

            // Baseado no valor estimado
            if (ValorEstimado.HasValue)
            {
                if (ValorEstimado >= 10000) score += 20;
                else if (ValorEstimado >= 5000) score += 15;
                else if (ValorEstimado >= 1000) score += 10;
                else score += 5;
            }

            // Baseado no tempo no funil
            if (DiasNoFunil <= 7) score += 10;
            else if (DiasNoFunil <= 30) score += 5;

            return score.ToString();
        }
    }

    public enum StatusLead
    {
        Novo,
        Contatado,
        Qualificado,
        Proposta,
        Negociacao,
        Fechado,
        Perdido
    }

    public class OrigemLead
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class SprintReview
    {
        public int Id { get; set; }
        
        [Required]
        public int SprintId { get; set; }
        public virtual Sprint Sprint { get; set; } = null!;
        
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;
        
        [StringLength(2000)]
        public string? Descricao { get; set; }
        
        public TipoReview Tipo { get; set; } = TipoReview.SprintReview;
        
        public DateTime DataReview { get; set; } = DateTime.Now;
        
        [StringLength(100)]
        public string? Facilitador { get; set; }
        
        [StringLength(1000)]
        public string? Participantes { get; set; }
        
        [StringLength(2000)]
        public string? PontosPositivos { get; set; }
        
        [StringLength(2000)]
        public string? PontosNegativos { get; set; }
        
        [StringLength(2000)]
        public string? AcoesDeImelhoria { get; set; }
        
        public int? NotaGeral { get; set; } // 1-10
        
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
    
    public enum TipoReview
    {
        [Display(Name = "Sprint Review")]
        SprintReview = 1,
        
        [Display(Name = "Sprint Retrospective")]
        SprintRetrospective = 2,
        
        [Display(Name = "Daily Standup")]
        DailyStandup = 3
    }
}

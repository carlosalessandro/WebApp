using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class TaskUserStory
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Descricao { get; set; }
        
        public StatusTask Status { get; set; } = StatusTask.AFazer;
        
        [StringLength(100)]
        public string? Responsavel { get; set; }
        
        public int? HorasEstimadas { get; set; }
        
        public int? HorasRealizadas { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        
        public DateTime? DataInicio { get; set; }
        
        public DateTime? DataConclusao { get; set; }
        
        // Relacionamentos
        [Required]
        public int UserStoryId { get; set; }
        public virtual UserStory UserStory { get; set; } = null!;
        
        // Propriedades calculadas
        public bool EstaAtrasada => DataInicio.HasValue && Status != StatusTask.Concluida && 
                                   DateTime.Now > DataInicio.Value.AddHours(HorasEstimadas ?? 8);
    }
    
    public enum StatusTask
    {
        [Display(Name = "A Fazer")]
        AFazer = 1,
        
        [Display(Name = "Em Progresso")]
        EmProgresso = 2,
        
        [Display(Name = "Em Teste")]
        EmTeste = 3,
        
        [Display(Name = "Concluída")]
        Concluida = 4,
        
        [Display(Name = "Bloqueada")]
        Bloqueada = 5
    }
}

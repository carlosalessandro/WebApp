using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserStory
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Descricao { get; set; }
        
        [StringLength(1000)]
        public string? CriteriosAceitacao { get; set; }
        
        public int? StoryPoints { get; set; }
        
        public PrioridadeUserStory Prioridade { get; set; } = PrioridadeUserStory.Media;
        
        public StatusUserStory Status { get; set; } = StatusUserStory.Backlog;
        
        [StringLength(100)]
        public string? Responsavel { get; set; }
        
        [StringLength(50)]
        public string? Epic { get; set; }
        
        [StringLength(200)]
        public string? Tags { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        
        public DateTime? DataAtualizacao { get; set; }
        
        public DateTime? DataInicio { get; set; }
        
        public DateTime? DataConclusao { get; set; }
        
        // Relacionamentos
        public int? SprintId { get; set; }
        public virtual Sprint? Sprint { get; set; }
        
        public virtual ICollection<TaskUserStory> Tasks { get; set; } = new List<TaskUserStory>();
        
        // Propriedades calculadas
        public int TasksConcluidas => Tasks.Count(t => t.Status == StatusTask.Concluida);
        
        public int TasksTotal => Tasks.Count;
        
        public decimal ProgressoPercentual
        {
            get
            {
                if (TasksTotal == 0) return 0;
                return Math.Round((decimal)TasksConcluidas / TasksTotal * 100, 1);
            }
        }
        
        public bool EstaAtrasada => DataInicio.HasValue && Status != StatusUserStory.Concluida && DateTime.Now > DataInicio.Value.AddDays(StoryPoints ?? 1);
    }
    
    public enum StatusUserStory
    {
        [Display(Name = "Backlog")]
        Backlog = 1,
        
        [Display(Name = "Sprint Backlog")]
        SprintBacklog = 2,
        
        [Display(Name = "Em Desenvolvimento")]
        EmDesenvolvimento = 3,
        
        [Display(Name = "Em Teste")]
        EmTeste = 4,
        
        [Display(Name = "Em Revisão")]
        EmRevisao = 5,
        
        [Display(Name = "Concluída")]
        Concluida = 6,
        
        [Display(Name = "Cancelada")]
        Cancelada = 7
    }
    
    public enum PrioridadeUserStory
    {
        [Display(Name = "Baixa")]
        Baixa = 1,
        
        [Display(Name = "Média")]
        Media = 2,
        
        [Display(Name = "Alta")]
        Alta = 3,
        
        [Display(Name = "Crítica")]
        Critica = 4
    }
}

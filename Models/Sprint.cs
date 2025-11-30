using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Sprint
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Descricao { get; set; }
        
        [Required]
        public DateTime DataInicio { get; set; }
        
        [Required]
        public DateTime DataFim { get; set; }
        
        public StatusSprint Status { get; set; } = StatusSprint.Planejamento;
        
        [StringLength(1000)]
        public string? Objetivo { get; set; }
        
        public int? VelocidadeAlvo { get; set; }
        
        public int? VelocidadeReal { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        
        public DateTime? DataAtualizacao { get; set; }
        
        // Relacionamentos
        public virtual ICollection<UserStory> UserStories { get; set; } = new List<UserStory>();
        
        public virtual ICollection<SprintReview> Reviews { get; set; } = new List<SprintReview>();
        
        // Propriedades calculadas
        public int DuracaoEmDias => (DataFim - DataInicio).Days + 1;
        
        public int DiasRestantes => Status == StatusSprint.Ativo ? Math.Max(0, (DataFim - DateTime.Today).Days) : 0;
        
        public decimal ProgressoPercentual
        {
            get
            {
                if (!UserStories.Any()) return 0;
                var concluidas = UserStories.Count(us => us.Status == StatusUserStory.Concluida);
                return Math.Round((decimal)concluidas / UserStories.Count * 100, 1);
            }
        }
        
        public int StoryPointsTotal => UserStories.Sum(us => us.StoryPoints ?? 0);
        
        public int StoryPointsConcluidos => UserStories.Where(us => us.Status == StatusUserStory.Concluida).Sum(us => us.StoryPoints ?? 0);
    }
    
    public enum StatusSprint
    {
        [Display(Name = "Planejamento")]
        Planejamento = 1,
        
        [Display(Name = "Ativo")]
        Ativo = 2,
        
        [Display(Name = "Concluído")]
        Concluido = 3,
        
        [Display(Name = "Cancelado")]
        Cancelado = 4
    }
}

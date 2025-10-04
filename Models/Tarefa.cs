using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo {1} caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O status é obrigatório")]
        [Display(Name = "Status")]
        public StatusTarefa Status { get; set; } = StatusTarefa.ToDo;

        [Display(Name = "Prioridade")]
        public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;

        [Display(Name = "Data de Vencimento")]
        [DataType(DataType.Date)]
        public DateTime? DataVencimento { get; set; }

        [Display(Name = "Responsável")]
        [StringLength(100, ErrorMessage = "O responsável deve ter no máximo {1} caracteres")]
        public string? Responsavel { get; set; }

        [Display(Name = "Estimativa (horas)")]
        [Range(0.5, 100, ErrorMessage = "A estimativa deve estar entre 0,5 e 100 horas")]
        public decimal? EstimativaHoras { get; set; }

        [Display(Name = "Tempo Gasto (horas)")]
        [Range(0, 1000, ErrorMessage = "O tempo gasto deve estar entre 0 e 1000 horas")]
        public decimal? TempoGasto { get; set; }

        [Display(Name = "Tags")]
        [StringLength(200, ErrorMessage = "As tags devem ter no máximo {1} caracteres")]
        public string? Tags { get; set; }

        [Display(Name = "Cor")]
        [StringLength(7, ErrorMessage = "A cor deve ter 7 caracteres")]
        public string? Cor { get; set; } = "#007bff";

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        // Propriedades calculadas
        [Display(Name = "Progresso")]
        public int Progresso => Status switch
        {
            StatusTarefa.ToDo => 0,
            StatusTarefa.InProgress => 50,
            StatusTarefa.InReview => 75,
            StatusTarefa.Done => 100,
            _ => 0
        };

        [Display(Name = "Está Atrasada")]
        public bool EstaAtrasada => DataVencimento.HasValue && DataVencimento.Value < DateTime.Now && Status != StatusTarefa.Done;
    }

    public enum StatusTarefa
    {
        [Display(Name = "A Fazer")]
        ToDo = 1,
        
        [Display(Name = "Em Progresso")]
        InProgress = 2,
        
        [Display(Name = "Em Revisão")]
        InReview = 3,
        
        [Display(Name = "Concluída")]
        Done = 4
    }

    public enum PrioridadeTarefa
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

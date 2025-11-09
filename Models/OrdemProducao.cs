using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class OrdemProducao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O número da ordem é obrigatório")]
        [StringLength(50)]
        [Display(Name = "Número da Ordem")]
        public string NumeroOrdem { get; set; } = string.Empty;

        [Required(ErrorMessage = "O produto é obrigatório")]
        [Display(Name = "Produto")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Display(Name = "Quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }

        [Display(Name = "Quantidade Produzida")]
        public int QuantidadeProduzida { get; set; } = 0;

        [Display(Name = "Data Início Prevista")]
        [DataType(DataType.Date)]
        public DateTime? DataInicioPrevista { get; set; }

        [Display(Name = "Data Fim Prevista")]
        [DataType(DataType.Date)]
        public DateTime? DataFimPrevista { get; set; }

        [Display(Name = "Data Início Real")]
        [DataType(DataType.DateTime)]
        public DateTime? DataInicioReal { get; set; }

        [Display(Name = "Data Fim Real")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFimReal { get; set; }

        [Required]
        [Display(Name = "Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Planejada"; // Planejada, EmAndamento, Pausada, Concluída, Cancelada

        [Display(Name = "Prioridade")]
        [Range(1, 5)]
        public int Prioridade { get; set; } = 3; // 1 = Baixa, 5 = Urgente

        [Display(Name = "Observações")]
        [StringLength(1000)]
        public string? Observacoes { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Criado Por")]
        public int? CriadoPorId { get; set; }

        // Navegação
        public Produto? Produto { get; set; }
        public User? CriadoPor { get; set; }
        public ICollection<ApontamentoProducao> Apontamentos { get; set; } = new List<ApontamentoProducao>();
        public ICollection<RecursoAlocado> RecursosAlocados { get; set; } = new List<RecursoAlocado>();
    }

    public enum StatusOrdemProducao
    {
        Planejada,
        EmAndamento,
        Pausada,
        Concluida,
        Cancelada
    }
}

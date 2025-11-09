using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RecursoAlocado
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ordem de Produção")]
        public int OrdemProducaoId { get; set; }

        [Required]
        [Display(Name = "Recurso")]
        public int RecursoId { get; set; }

        [Display(Name = "Data Início Alocação")]
        [DataType(DataType.DateTime)]
        public DateTime DataInicioAlocacao { get; set; }

        [Display(Name = "Data Fim Alocação")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFimAlocacao { get; set; }

        [Display(Name = "Horas Planejadas")]
        public decimal HorasPlanejadas { get; set; }

        [Display(Name = "Horas Utilizadas")]
        public decimal HorasUtilizadas { get; set; } = 0;

        [Display(Name = "Observações")]
        [StringLength(500)]
        public string? Observacoes { get; set; }

        // Navegação
        public OrdemProducao? OrdemProducao { get; set; }
        public Recurso? Recurso { get; set; }
    }
}

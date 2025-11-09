using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ApontamentoProducao
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ordem de Produção")]
        public int OrdemProducaoId { get; set; }

        [Required(ErrorMessage = "A quantidade produzida é obrigatória")]
        [Display(Name = "Quantidade Produzida")]
        [Range(0, int.MaxValue)]
        public int QuantidadeProduzida { get; set; }

        [Display(Name = "Quantidade Refugo")]
        [Range(0, int.MaxValue)]
        public int QuantidadeRefugo { get; set; } = 0;

        [Display(Name = "Quantidade Retrabalho")]
        [Range(0, int.MaxValue)]
        public int QuantidadeRetrabalho { get; set; } = 0;

        [Required]
        [Display(Name = "Data/Hora do Apontamento")]
        [DataType(DataType.DateTime)]
        public DateTime DataHoraApontamento { get; set; } = DateTime.Now;

        [Display(Name = "Operador")]
        public int? OperadorId { get; set; }

        [Display(Name = "Tempo de Produção (minutos)")]
        public int TempoProducaoMinutos { get; set; }

        [Display(Name = "Tempo de Setup (minutos)")]
        public int TempoSetupMinutos { get; set; } = 0;

        [Display(Name = "Tempo Parado (minutos)")]
        public int TempoParadoMinutos { get; set; } = 0;

        [Display(Name = "Motivo Parada")]
        [StringLength(500)]
        public string? MotivoParada { get; set; }

        [Display(Name = "Observações")]
        [StringLength(1000)]
        public string? Observacoes { get; set; }

        // Navegação
        public OrdemProducao? OrdemProducao { get; set; }
        public User? Operador { get; set; }
    }
}

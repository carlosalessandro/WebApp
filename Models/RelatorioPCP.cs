using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RelatorioPCP
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200)]
        [Display(Name = "Nome do Relatório")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Descrição")]
        [StringLength(1000)]
        public string? Descricao { get; set; }

        [Required]
        [Display(Name = "Tipo de Relatório")]
        [StringLength(50)]
        public string TipoRelatorio { get; set; } = "Personalizado";

        [Display(Name = "Configuração JSON")]
        public string? ConfiguracaoJson { get; set; }

        [Display(Name = "Campos Selecionados")]
        public string? CamposSelecionados { get; set; }

        [Display(Name = "Filtros")]
        public string? Filtros { get; set; }

        [Display(Name = "Ordenação")]
        public string? Ordenacao { get; set; }

        [Display(Name = "Agrupamento")]
        public string? Agrupamento { get; set; }

        [Display(Name = "Público")]
        public bool Publico { get; set; } = false;

        [Display(Name = "Criado Por")]
        public int CriadoPorId { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        // Navegação
        public User? CriadoPor { get; set; }
    }
}

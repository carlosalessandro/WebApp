using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Recurso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O código é obrigatório")]
        [StringLength(50)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = "Maquina"; // Maquina, Ferramenta, MaoDeObra, Equipamento

        [Display(Name = "Capacidade por Hora")]
        public decimal CapacidadePorHora { get; set; }

        [Display(Name = "Custo por Hora")]
        [DataType(DataType.Currency)]
        public decimal CustoPorHora { get; set; }

        [Display(Name = "Disponível")]
        public bool Disponivel { get; set; } = true;

        [Display(Name = "Em Manutenção")]
        public bool EmManutencao { get; set; } = false;

        [Display(Name = "Localização")]
        [StringLength(200)]
        public string? Localizacao { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(1000)]
        public string? Descricao { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Navegação
        public ICollection<RecursoAlocado> Alocacoes { get; set; } = new List<RecursoAlocado>();
    }

    public enum TipoRecurso
    {
        Maquina,
        Ferramenta,
        MaoDeObra,
        Equipamento
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Permissao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da permissão é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [StringLength(50, ErrorMessage = "O código deve ter no máximo {1} caracteres")]
        [Display(Name = "Código")]
        public string? Codigo { get; set; }

        [Display(Name = "Controller")]
        [StringLength(50, ErrorMessage = "O controller deve ter no máximo {1} caracteres")]
        public string? Controller { get; set; }

        [Display(Name = "Action")]
        [StringLength(50, ErrorMessage = "A action deve ter no máximo {1} caracteres")]
        public string? Action { get; set; }

        [Display(Name = "Ativa")]
        public bool Ativa { get; set; } = true;

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Required(ErrorMessage = "A categoria é obrigatória")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        // Propriedade de navegação
        public Categoria Categoria { get; set; } = null!;
        public ICollection<UsuarioPermissao> UsuarioPermissoes { get; set; } = new List<UsuarioPermissao>();
    }
}

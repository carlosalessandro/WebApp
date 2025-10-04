using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo {1} caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "A URL deve ter no máximo {1} caracteres")]
        [Display(Name = "URL")]
        public string? Url { get; set; }

        [StringLength(50, ErrorMessage = "O ícone deve ter no máximo {1} caracteres")]
        [Display(Name = "Ícone")]
        public string? Icone { get; set; }

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Abrir em nova aba")]
        public bool AbrirNovaAba { get; set; } = false;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Controller")]
        [StringLength(50, ErrorMessage = "O controller deve ter no máximo {1} caracteres")]
        public string? Controller { get; set; }

        [Display(Name = "Action")]
        [StringLength(50, ErrorMessage = "A action deve ter no máximo {1} caracteres")]
        public string? Action { get; set; }

        [Display(Name = "Área")]
        [StringLength(50, ErrorMessage = "A área deve ter no máximo {1} caracteres")]
        public string? Area { get; set; }

        [Display(Name = "Menu Pai")]
        public int? MenuPaiId { get; set; }

        [Display(Name = "É Menu Pai")]
        public bool EMenuPai { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        // Propriedade de navegação
        public MenuItem? MenuPai { get; set; }
        public ICollection<MenuItem> SubMenus { get; set; } = new List<MenuItem>();
    }
}

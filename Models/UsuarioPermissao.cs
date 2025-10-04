using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UsuarioPermissao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O usuário é obrigatório")]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "A permissão é obrigatória")]
        [Display(Name = "Permissão")]
        public int PermissaoId { get; set; }

        [Display(Name = "Concedida")]
        public bool Concedida { get; set; } = true;

        [Display(Name = "Data de Concessão")]
        public DateTime DataConcessao { get; set; } = DateTime.Now;

        [Display(Name = "Data de Expiração")]
        public DateTime? DataExpiracao { get; set; }

        [StringLength(200, ErrorMessage = "O comentário deve ter no máximo {1} caracteres")]
        [Display(Name = "Comentário")]
        public string? Comentario { get; set; }

        // Propriedades de navegação
        public User Usuario { get; set; } = null!;
        public Permissao Permissao { get; set; } = null!;
    }
}

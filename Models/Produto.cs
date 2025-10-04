using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        [Display(Name = "Preço")]
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }

        [Display(Name = "Preço de Custo")]
        [DataType(DataType.Currency)]
        public decimal? PrecoCusto { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a zero")]
        [Display(Name = "Quantidade em Estoque")]
        public int QuantidadeEstoque { get; set; }

        [Display(Name = "Quantidade Mínima")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade mínima deve ser maior ou igual a zero")]
        public int? QuantidadeMinima { get; set; }

        [StringLength(50, ErrorMessage = "O código deve ter no máximo {1} caracteres")]
        [Display(Name = "Código do Produto")]
        public string? Codigo { get; set; }

        [StringLength(100, ErrorMessage = "A marca deve ter no máximo {1} caracteres")]
        [Display(Name = "Marca")]
        public string? Marca { get; set; }

        [StringLength(100, ErrorMessage = "O modelo deve ter no máximo {1} caracteres")]
        [Display(Name = "Modelo")]
        public string? Modelo { get; set; }

        [StringLength(50, ErrorMessage = "A cor deve ter no máximo {1} caracteres")]
        [Display(Name = "Cor")]
        public string? Cor { get; set; }

        [StringLength(20, ErrorMessage = "O tamanho deve ter no máximo {1} caracteres")]
        [Display(Name = "Tamanho")]
        public string? Tamanho { get; set; }

        [StringLength(100, ErrorMessage = "O peso deve ter no máximo {1} caracteres")]
        [Display(Name = "Peso")]
        public string? Peso { get; set; }

        [StringLength(50, ErrorMessage = "As dimensões devem ter no máximo {1} caracteres")]
        [Display(Name = "Dimensões")]
        public string? Dimensoes { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Destaque")]
        public bool Destaque { get; set; } = false;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        // Propriedades de navegação
        public Categoria? Categoria { get; set; }
        public ICollection<ProdutoImagem> Imagens { get; set; } = new List<ProdutoImagem>();
    }

    public class ProdutoImagem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Nome do Arquivo")]
        public string NomeArquivo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Display(Name = "Caminho")]
        public string Caminho { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Título")]
        public string? Titulo { get; set; }

        [StringLength(200)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Principal")]
        public bool Principal { get; set; } = false;

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Chave estrangeira
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }
    }
}

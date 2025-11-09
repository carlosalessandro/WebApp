using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Fornecedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string RazaoSocial { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NomeFantasia { get; set; }

        [Required]
        [StringLength(18)]
        public string CnpjCpf { get; set; } = string.Empty;

        [StringLength(20)]
        public string? InscricaoEstadual { get; set; }

        [StringLength(20)]
        public string? InscricaoMunicipal { get; set; }

        [StringLength(200)]
        public string? Endereco { get; set; }

        [StringLength(10)]
        public string? Numero { get; set; }

        [StringLength(100)]
        public string? Complemento { get; set; }

        [StringLength(100)]
        public string? Bairro { get; set; }

        [StringLength(100)]
        public string? Cidade { get; set; }

        [StringLength(2)]
        public string? Estado { get; set; }

        [StringLength(10)]
        public string? Cep { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(20)]
        public string? Celular { get; set; }

        [StringLength(200)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Site { get; set; }

        [StringLength(100)]
        public string? Contato { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        public bool Ativo { get; set; } = true;

        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime? DataAtualizacao { get; set; }

        // Navegação
        public virtual ICollection<ContaPagar>? ContasPagar { get; set; }
        public virtual ICollection<PedidoCompra>? PedidosCompra { get; set; }
    }
}

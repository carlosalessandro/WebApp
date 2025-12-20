using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class ExcelChatbotSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SessionId { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [StringLength(500)]
        public string? FileName { get; set; }

        [StringLength(1000)]
        public string? FilePath { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime2")]
        public DateTime LastActivity { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navegação
        public virtual ICollection<ExcelChatbotMessage> Messages { get; set; } = new List<ExcelChatbotMessage>();
        public virtual ICollection<ExcelChatbotOperation> Operations { get; set; } = new List<ExcelChatbotOperation>();
    }

    public class ExcelChatbotMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual ExcelChatbotSession Session { get; set; } = null!;

        [Required]
        public MessageType Type { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Column(TypeName = "datetime2")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? Metadata { get; set; } // JSON com dados adicionais
    }

    public enum MessageType
    {
        User,
        Bot,
        System,
        Error
    }

    public class ExcelChatbotOperation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public virtual ExcelChatbotSession Session { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string OperationType { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Parameters { get; set; } // JSON com parâmetros da operação

        [StringLength(1000)]
        public string? Result { get; set; } // JSON com resultado da operação

        public bool Success { get; set; } = true;

        [Column(TypeName = "datetime2")]
        public DateTime ExecutedAt { get; set; } = DateTime.Now;

        public string? ErrorMessage { get; set; }
    }

    public class ExcelFileData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? FilePath { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public string? SheetNames { get; set; } // JSON com lista de nomes das planilhas

        public string? ColumnHeaders { get; set; } // JSON com cabeçalhos das colunas

        public string? SampleData { get; set; } // JSON com dados de exemplo

        public long FileSize { get; set; }

        [StringLength(10)]
        public string? FileExtension { get; set; }

        public bool IsProcessed { get; set; } = false;

        public string? ProcessingError { get; set; }
    }
}

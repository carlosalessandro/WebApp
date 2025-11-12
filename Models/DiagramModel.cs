using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class DiagramModel
    {
        [Key]
        public string Id { get; set; } = "";

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = "";

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "TEXT")]
        public string Components { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string Connections { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string CanvasSettings { get; set; } = "{}";

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = "";

        [MaxLength(20)]
        public string Version { get; set; } = "1.0.0";

        [MaxLength(500)]
        public string Tags { get; set; } = "";

        public bool IsPublic { get; set; } = false;
    }

    public class SqlQueryModel
    {
        [Key]
        public string Id { get; set; } = "";

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = "";

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = "SELECT";

        [Column(TypeName = "TEXT")]
        public string Tables { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string Fields { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string Joins { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string Conditions { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string GroupBy { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string Having { get; set; } = "[]";

        [Column(TypeName = "TEXT")]
        public string OrderBy { get; set; } = "[]";

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        [Column(TypeName = "TEXT")]
        public string? RawSql { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = "";

        public int? ExecutionTime { get; set; }

        public int? ResultCount { get; set; }

        public bool IsValid { get; set; } = false;

        [Column(TypeName = "TEXT")]
        public string ValidationErrors { get; set; } = "[]";
    }

    public class ComponentPaletteModel
    {
        [Key]
        public string Id { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = "";

        [Column(TypeName = "TEXT")]
        public string Components { get; set; } = "[]";

        public bool IsCustom { get; set; } = false;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}

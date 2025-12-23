using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ThemeConfig
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(7)]
        public string PrimaryColor { get; set; } = "#9acd32";

        [Required]
        [MaxLength(7)]
        public string SecondaryColor { get; set; } = "#ccff00";

        [Required]
        [MaxLength(7)]
        public string DarkColor { get; set; } = "#6b8e23";

        [Required]
        [MaxLength(7)]
        public string LightColor { get; set; } = "#e6ff99";

        [Required]
        [MaxLength(7)]
        public string HoverColor { get; set; } = "#b3e600";

        [Required]
        [MaxLength(7)]
        public string TextDark { get; set; } = "#1a3309";

        [Required]
        [MaxLength(7)]
        public string TextMedium { get; set; } = "#2d5016";

        [Required]
        [MaxLength(7)]
        public string BackgroundColor { get; set; } = "#f8f9fa";

        public bool IsActive { get; set; } = false;

        public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}

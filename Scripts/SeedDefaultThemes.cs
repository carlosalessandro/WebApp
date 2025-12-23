using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Scripts
{
    public static class SeedDefaultThemes
    {
        public static async Task SeedThemes(ApplicationDbContext context)
        {
            // Verifica se já existem temas
            if (await context.ThemeConfigs.AnyAsync())
            {
                Console.WriteLine("Temas já existem no banco de dados.");
                return;
            }

            var themes = new List<ThemeConfig>
            {
                new ThemeConfig
                {
                    Name = "Verde Louro (Padrão)",
                    PrimaryColor = "#9acd32",
                    SecondaryColor = "#ccff00",
                    DarkColor = "#6b8e23",
                    LightColor = "#e6ff99",
                    HoverColor = "#b3e600",
                    TextDark = "#1a3309",
                    TextMedium = "#2d5016",
                    BackgroundColor = "#f8f9fa",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new ThemeConfig
                {
                    Name = "Azul Oceano",
                    PrimaryColor = "#0077be",
                    SecondaryColor = "#00a8e8",
                    DarkColor = "#003f5c",
                    LightColor = "#b3e5fc",
                    HoverColor = "#0096d6",
                    TextDark = "#001f3f",
                    TextMedium = "#003d5c",
                    BackgroundColor = "#f0f8ff",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                },
                new ThemeConfig
                {
                    Name = "Roxo Moderno",
                    PrimaryColor = "#7b2cbf",
                    SecondaryColor = "#9d4edd",
                    DarkColor = "#5a189a",
                    LightColor = "#e0aaff",
                    HoverColor = "#8b3dcc",
                    TextDark = "#240046",
                    TextMedium = "#3c096c",
                    BackgroundColor = "#f8f4ff",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                },
                new ThemeConfig
                {
                    Name = "Laranja Vibrante",
                    PrimaryColor = "#ff6b35",
                    SecondaryColor = "#ff9f1c",
                    DarkColor = "#d84315",
                    LightColor = "#ffccbc",
                    HoverColor = "#ff7b45",
                    TextDark = "#4a1504",
                    TextMedium = "#7a2504",
                    BackgroundColor = "#fff8f5",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                },
                new ThemeConfig
                {
                    Name = "Escuro Profissional",
                    PrimaryColor = "#2c3e50",
                    SecondaryColor = "#34495e",
                    DarkColor = "#1a252f",
                    LightColor = "#95a5a6",
                    HoverColor = "#3d5a73",
                    TextDark = "#ecf0f1",
                    TextMedium = "#bdc3c7",
                    BackgroundColor = "#1e1e1e",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                },
                new ThemeConfig
                {
                    Name = "Verde Esmeralda",
                    PrimaryColor = "#27ae60",
                    SecondaryColor = "#2ecc71",
                    DarkColor = "#1e8449",
                    LightColor = "#a9dfbf",
                    HoverColor = "#32c96e",
                    TextDark = "#0b5329",
                    TextMedium = "#186a3b",
                    BackgroundColor = "#f0fff4",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.ThemeConfigs.AddRangeAsync(themes);
            await context.SaveChangesAsync();

            Console.WriteLine($"{themes.Count} temas padrão foram inseridos com sucesso!");
        }
    }
}

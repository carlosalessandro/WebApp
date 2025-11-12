using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using System.Text.Json;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagramController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DiagramController> _logger;

        public DiagramController(ApplicationDbContext context, ILogger<DiagramController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagramModel>>> GetDiagrams()
        {
            try
            {
                var diagrams = await _context.Diagrams
                    .OrderByDescending(d => d.UpdatedAt)
                    .ToListAsync();

                return Ok(diagrams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving diagrams");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiagramModel>> GetDiagram(string id)
        {
            try
            {
                var diagram = await _context.Diagrams.FindAsync(id);

                if (diagram == null)
                {
                    return NotFound($"Diagram with ID {id} not found");
                }

                return Ok(diagram);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving diagram {DiagramId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DiagramModel>> CreateDiagram([FromBody] CreateDiagramRequest request)
        {
            try
            {
                var diagram = new DiagramModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = request.Name,
                    Description = request.Description,
                    Components = "[]",
                    Connections = "[]",
                    CanvasSettings = JsonSerializer.Serialize(new
                    {
                        width = 1200,
                        height = 800,
                        backgroundColor = "#ffffff",
                        gridEnabled = true,
                        gridSize = 20,
                        gridColor = "#e0e0e0",
                        snapToGrid = true,
                        zoom = 1,
                        panX = 0,
                        panY = 0
                    }),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = GetCurrentUserId(),
                    Version = "1.0.0",
                    Tags = request.Tags ?? "",
                    IsPublic = request.IsPublic
                };

                _context.Diagrams.Add(diagram);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created new diagram {DiagramId} with name {DiagramName}", diagram.Id, diagram.Name);

                return CreatedAtAction(nameof(GetDiagram), new { id = diagram.Id }, diagram);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating diagram");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DiagramModel>> UpdateDiagram(string id, [FromBody] UpdateDiagramRequest request)
        {
            try
            {
                var diagram = await _context.Diagrams.FindAsync(id);

                if (diagram == null)
                {
                    return NotFound($"Diagram with ID {id} not found");
                }

                diagram.Name = request.Name ?? diagram.Name;
                diagram.Description = request.Description ?? diagram.Description;
                diagram.Components = request.Components ?? diagram.Components;
                diagram.Connections = request.Connections ?? diagram.Connections;
                diagram.CanvasSettings = request.CanvasSettings ?? diagram.CanvasSettings;
                diagram.UpdatedAt = DateTime.UtcNow;
                diagram.Tags = request.Tags ?? diagram.Tags;
                diagram.IsPublic = request.IsPublic ?? diagram.IsPublic;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Updated diagram {DiagramId}", id);

                return Ok(diagram);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating diagram {DiagramId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiagram(string id)
        {
            try
            {
                var diagram = await _context.Diagrams.FindAsync(id);

                if (diagram == null)
                {
                    return NotFound($"Diagram with ID {id} not found");
                }

                _context.Diagrams.Remove(diagram);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted diagram {DiagramId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting diagram {DiagramId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}/export")]
        public async Task<ActionResult> ExportDiagram(string id, [FromBody] ExportDiagramRequest request)
        {
            try
            {
                var diagram = await _context.Diagrams.FindAsync(id);

                if (diagram == null)
                {
                    return NotFound($"Diagram with ID {id} not found");
                }

                switch (request.Format.ToLower())
                {
                    case "json":
                        var jsonContent = JsonSerializer.Serialize(diagram, new JsonSerializerOptions 
                        { 
                            WriteIndented = true 
                        });
                        return File(System.Text.Encoding.UTF8.GetBytes(jsonContent), 
                                  "application/json", 
                                  $"{diagram.Name}.json");

                    case "svg":
                        var svgContent = GenerateSvgFromDiagram(diagram);
                        return File(System.Text.Encoding.UTF8.GetBytes(svgContent), 
                                  "image/svg+xml", 
                                  $"{diagram.Name}.svg");

                    default:
                        return BadRequest("Unsupported export format. Supported formats: json, svg");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting diagram {DiagramId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private string GetCurrentUserId()
        {
            return "system-user";
        }

        private string GenerateSvgFromDiagram(DiagramModel diagram)
        {
            return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<svg width=""1200"" height=""800"" xmlns=""http://www.w3.org/2000/svg"">
    <rect width=""100%"" height=""100%"" fill=""white""/>
    <text x=""50%"" y=""50%"" text-anchor=""middle"" font-family=""Arial"" font-size=""24"" fill=""black"">
        {diagram.Name}
    </text>
    <text x=""50%"" y=""60%"" text-anchor=""middle"" font-family=""Arial"" font-size=""14"" fill=""gray"">
        Generated from No-Code Builder
    </text>
</svg>";
        }
    }

    public class CreateDiagramRequest
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public bool IsPublic { get; set; } = false;
    }

    public class UpdateDiagramRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Components { get; set; }
        public string? Connections { get; set; }
        public string? CanvasSettings { get; set; }
        public string? Tags { get; set; }
        public bool? IsPublic { get; set; }
    }

    public class ExportDiagramRequest
    {
        public string Format { get; set; } = "json";
    }
}

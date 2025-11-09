using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using System.Text.Json;
using System.Data.Common;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SqlBuilderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SqlBuilderController> _logger;

        public SqlBuilderController(ApplicationDbContext context, ILogger<SqlBuilderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/SqlBuilder/tables
        [HttpGet("tables")]
        public async Task<ActionResult<IEnumerable<DatabaseTableDto>>> GetTables()
        {
            try
            {
                var tables = new List<DatabaseTableDto>();

                // Obter informações das tabelas do banco de dados
                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = GetTablesQuery();

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tables.Add(new DatabaseTableDto
                    {
                        Name = reader.GetString("TABLE_NAME"),
                        Schema = reader.IsDBNull("TABLE_SCHEMA") ? null : reader.GetString("TABLE_SCHEMA"),
                        Columns = new List<DatabaseColumnDto>()
                    });
                }

                return Ok(tables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter tabelas do banco de dados");
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        // GET: api/SqlBuilder/tables/{tableName}/columns
        [HttpGet("tables/{tableName}/columns")]
        public async Task<ActionResult<IEnumerable<DatabaseColumnDto>>> GetTableColumns(string tableName)
        {
            try
            {
                var columns = new List<DatabaseColumnDto>();

                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = $"PRAGMA table_info({tableName})";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    columns.Add(new DatabaseColumnDto
                    {
                        Name = reader.GetString("name"),
                        Type = reader.GetString("type"),
                        Nullable = reader.GetInt32("notnull") == 0,
                        IsPrimaryKey = reader.GetInt32("pk") > 0,
                        IsForeignKey = false, // Será determinado em uma consulta separada
                        Table = tableName
                    });
                }

                // Chaves primárias já identificadas acima
                // Chaves estrangeiras podem ser implementadas posteriormente se necessário

                return Ok(columns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter colunas da tabela {TableName}", tableName);
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        // POST: api/SqlBuilder/execute
        [HttpPost("execute")]
        public async Task<ActionResult<QueryExecutionResultDto>> ExecuteQuery([FromBody] ExecuteQueryRequest request)
        {
            try
            {
                // Validar SQL antes de executar
                if (!IsValidQuery(request.Sql))
                {
                    return BadRequest(new { message = "Query SQL inválida ou perigosa" });
                }

                var startTime = DateTime.Now;
                var result = new QueryExecutionResultDto
                {
                    Success = true,
                    Data = new List<Dictionary<string, object>>(),
                    Columns = new List<string>(),
                    RowCount = 0,
                    ExecutionTime = 0
                };

                var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = request.Sql;
                command.CommandTimeout = 30; // 30 segundos timeout

                using var reader = await command.ExecuteReaderAsync();
                
                // Obter nomes das colunas
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Columns.Add(reader.GetName(i));
                }

                // Ler dados
                while (await reader.ReadAsync() && result.RowCount < 1000) // Limitar a 1000 registros
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[reader.GetName(i)] = value;
                    }
                    result.Data.Add(row);
                    result.RowCount++;
                }

                result.ExecutionTime = (DateTime.Now - startTime).TotalMilliseconds;

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar query SQL: {Sql}", request.Sql);
                return Ok(new QueryExecutionResultDto
                {
                    Success = false,
                    Data = new List<Dictionary<string, object>>(),
                    Columns = new List<string>(),
                    RowCount = 0,
                    ExecutionTime = 0,
                    Error = ex.Message
                });
            }
        }

        // POST: api/SqlBuilder/validate
        [HttpPost("validate")]
        public ActionResult<SqlValidationResult> ValidateQuery([FromBody] ValidateQueryRequest request)
        {
            try
            {
                var result = new SqlValidationResult
                {
                    IsValid = IsValidQuery(request.Sql),
                    Errors = new List<string>()
                };

                if (!result.IsValid)
                {
                    result.Errors.Add("Query contém comandos não permitidos ou sintaxe perigosa");
                }

                // Validações adicionais podem ser implementadas aqui
                if (string.IsNullOrWhiteSpace(request.Sql))
                {
                    result.IsValid = false;
                    result.Errors.Add("Query não pode estar vazia");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar query SQL");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        private string GetTablesQuery()
        {
            // Query para SQLite (ajustar conforme o banco)
            return @"
                SELECT name as TABLE_NAME, NULL as TABLE_SCHEMA
                FROM sqlite_master 
                WHERE type='table' AND name NOT LIKE 'sqlite_%'
                ORDER BY name";
        }

        private string GetColumnsQuery()
        {
            // Query para SQLite (ajustar conforme o banco)
            return "PRAGMA table_info({0})";
        }

        private async Task IdentifyForeignKeys(List<DatabaseColumnDto> columns, string tableName, DbConnection connection)
        {
            try
            {
                // Identificar chaves primárias (SQLite)
                using var pkCommand = connection.CreateCommand();
                pkCommand.CommandText = $"PRAGMA table_info({tableName})";
                
                using var reader = await pkCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var columnName = reader.GetString("name");
                    var isPk = reader.GetInt32("pk") > 0;
                    
                    var column = columns.FirstOrDefault(c => c.Name == columnName);
                    if (column != null)
                    {
                        column.IsPrimaryKey = isPk;
                    }
                }
                reader.Close();

                // Identificar chaves estrangeiras (SQLite)
                using var fkCommand = connection.CreateCommand();
                fkCommand.CommandText = $"PRAGMA foreign_key_list({tableName})";
                
                using var fkReader = await fkCommand.ExecuteReaderAsync();
                while (await fkReader.ReadAsync())
                {
                    var columnName = fkReader.GetString("from");
                    
                    var column = columns.FirstOrDefault(c => c.Name == columnName);
                    if (column != null)
                    {
                        column.IsForeignKey = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro ao identificar chaves para a tabela {TableName}", tableName);
            }
        }

        private bool IsValidQuery(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return false;

            // Lista de comandos perigosos
            var dangerousCommands = new[]
            {
                "DROP", "DELETE", "INSERT", "UPDATE", "ALTER", "CREATE", "TRUNCATE",
                "EXEC", "EXECUTE", "GRANT", "REVOKE", "BACKUP", "RESTORE"
            };

            var upperSql = sql.ToUpper();
            
            // Verificar se contém comandos perigosos
            foreach (var command in dangerousCommands)
            {
                if (upperSql.Contains(command))
                    return false;
            }

            // Verificar se é uma query SELECT válida
            if (!upperSql.TrimStart().StartsWith("SELECT"))
                return false;

            return true;
        }
    }

    // DTOs
    public class DatabaseTableDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Schema { get; set; }
        public string? Alias { get; set; }
        public List<DatabaseColumnDto> Columns { get; set; } = new();
    }

    public class DatabaseColumnDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Nullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string Table { get; set; } = string.Empty;
        public string? Alias { get; set; }
    }

    public class ExecuteQueryRequest
    {
        public string Sql { get; set; } = string.Empty;
    }

    public class ValidateQueryRequest
    {
        public string Sql { get; set; } = string.Empty;
    }

    public class QueryExecutionResultDto
    {
        public bool Success { get; set; }
        public List<Dictionary<string, object>> Data { get; set; } = new();
        public List<string> Columns { get; set; } = new();
        public int RowCount { get; set; }
        public double ExecutionTime { get; set; }
        public string? Error { get; set; }
    }

    public class SqlValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}

import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { DatabaseTable, DatabaseColumn } from '../../../models/sql-builder.model';
import { SqlBuilderService } from '../../../services/sql-builder.service';

@Component({
  selector: 'app-database-explorer',
  templateUrl: './database-explorer.component.html',
  styleUrls: ['./database-explorer.component.css']
})
export class DatabaseExplorerComponent implements OnInit {
  @Input() tables: DatabaseTable[] = [];
  @Output() dragStart = new EventEmitter<{event: DragEvent, item: any, type: string}>();

  expandedTables: Set<string> = new Set();
  searchTerm: string = '';
  filteredTables: DatabaseTable[] = [];

  constructor(private sqlBuilderService: SqlBuilderService) {}

  ngOnInit(): void {
    this.filteredTables = this.tables;
  }

  ngOnChanges(): void {
    this.filterTables();
  }

  toggleTable(tableName: string): void {
    if (this.expandedTables.has(tableName)) {
      this.expandedTables.delete(tableName);
    } else {
      this.expandedTables.add(tableName);
      this.loadTableColumns(tableName);
    }
  }

  isTableExpanded(tableName: string): boolean {
    return this.expandedTables.has(tableName);
  }

  private loadTableColumns(tableName: string): void {
    const table = this.tables.find(t => t.name === tableName);
    if (table && (!table.columns || table.columns.length === 0)) {
      this.sqlBuilderService.getTableColumns(tableName).subscribe({
        next: (columns) => {
          table.columns = columns;
        },
        error: (error) => {
          console.error('Erro ao carregar colunas:', error);
        }
      });
    }
  }

  onDragStart(event: DragEvent, item: any, type: string): void {
    this.dragStart.emit({ event, item, type });
  }

  filterTables(): void {
    if (!this.searchTerm.trim()) {
      this.filteredTables = this.tables;
      return;
    }

    const term = this.searchTerm.toLowerCase();
    this.filteredTables = this.tables.filter(table => 
      table.name.toLowerCase().includes(term) ||
      (table.columns && table.columns.some(col => 
        col.name.toLowerCase().includes(term)
      ))
    );
  }

  getColumnIcon(column: DatabaseColumn): string {
    if (column.isPrimaryKey) {
      return 'bi-key-fill text-warning';
    }
    if (column.isForeignKey) {
      return 'bi-link-45deg text-info';
    }
    
    switch (column.type.toLowerCase()) {
      case 'int':
      case 'integer':
      case 'bigint':
      case 'smallint':
      case 'tinyint':
        return 'bi-123 text-primary';
      case 'varchar':
      case 'char':
      case 'text':
      case 'nvarchar':
        return 'bi-fonts text-success';
      case 'datetime':
      case 'date':
      case 'timestamp':
        return 'bi-calendar-date text-secondary';
      case 'decimal':
      case 'float':
      case 'double':
        return 'bi-calculator text-info';
      case 'bit':
      case 'boolean':
        return 'bi-toggle-on text-warning';
      default:
        return 'bi-circle text-muted';
    }
  }

  getColumnTooltip(column: DatabaseColumn): string {
    let tooltip = `${column.name} (${column.type})`;
    if (column.isPrimaryKey) tooltip += ' - Chave Primária';
    if (column.isForeignKey) tooltip += ' - Chave Estrangeira';
    if (!column.nullable) tooltip += ' - Não Nulo';
    return tooltip;
  }
}

import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SqlBuilderService } from '../../services/sql-builder.service';
import { 
  DatabaseTable, 
  SqlQuery, 
  QueryExecutionResult,
  DatabaseType 
} from '../../models/sql-builder.model';

@Component({
  selector: 'app-sql-builder',
  templateUrl: './sql-builder.component.html',
  styleUrls: ['./sql-builder.component.css']
})
export class SqlBuilderComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  
  // Estado do componente
  tables: DatabaseTable[] = [];
  currentQuery: SqlQuery;
  generatedSql: string = '';
  queryResult: QueryExecutionResult | null = null;
  isExecuting: boolean = false;
  selectedDatabaseType: DatabaseType = DatabaseType.MYSQL;
  
  // UI State
  activeTab: string = 'select';
  showSqlPreview: boolean = true;
  showResults: boolean = false;
  
  // Drag and Drop
  draggedItem: any = null;
  draggedItemType: string = '';

  constructor(private sqlBuilderService: SqlBuilderService) {
    this.currentQuery = this.sqlBuilderService.getCurrentQuery();
  }

  ngOnInit(): void {
    this.loadTables();
    this.subscribeToQueryChanges();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadTables(): void {
    this.sqlBuilderService.getDatabaseTables()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (tables) => {
          this.tables = tables;
        },
        error: (error) => {
          console.error('Erro ao carregar tabelas:', error);
        }
      });
  }

  private subscribeToQueryChanges(): void {
    this.sqlBuilderService.currentQuery$
      .pipe(takeUntil(this.destroy$))
      .subscribe(query => {
        this.currentQuery = query;
        this.generateSql();
      });
  }

  // Geração de SQL
  generateSql(): void {
    this.generatedSql = this.sqlBuilderService.generateSql(this.currentQuery, this.selectedDatabaseType);
  }

  // Execução de query
  executeQuery(): void {
    if (!this.generatedSql.trim()) {
      return;
    }

    this.isExecuting = true;
    this.sqlBuilderService.executeQuery(this.generatedSql)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          this.queryResult = result;
          this.showResults = true;
          this.isExecuting = false;
        },
        error: (error) => {
          console.error('Erro ao executar query:', error);
          this.queryResult = {
            success: false,
            data: [],
            columns: [],
            rowCount: 0,
            executionTime: 0,
            error: error.message || 'Erro desconhecido'
          };
          this.showResults = true;
          this.isExecuting = false;
        }
      });
  }

  // Drag and Drop handlers
  onDragStart(event: DragEvent, item: any, itemType: string): void {
    this.draggedItem = item;
    this.draggedItemType = itemType;
    
    if (event.dataTransfer) {
      event.dataTransfer.effectAllowed = 'copy';
      event.dataTransfer.setData('text/plain', JSON.stringify({ item, itemType }));
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    if (event.dataTransfer) {
      event.dataTransfer.dropEffect = 'copy';
    }
  }

  onDrop(event: DragEvent, dropZone: string): void {
    event.preventDefault();
    
    if (!this.draggedItem || !this.draggedItemType) {
      return;
    }

    this.handleDrop(this.draggedItem, this.draggedItemType, dropZone);
    this.draggedItem = null;
    this.draggedItemType = '';
  }

  private handleDrop(item: any, itemType: string, dropZone: string): void {
    switch (dropZone) {
      case 'select':
        if (itemType === 'column') {
          this.addSelectColumn(item);
        }
        break;
      case 'from':
        if (itemType === 'table') {
          this.setFromTable(item);
        }
        break;
      case 'where':
        if (itemType === 'column') {
          this.addWhereCondition(item);
        }
        break;
      case 'orderby':
        if (itemType === 'column') {
          this.addOrderByColumn(item);
        }
        break;
      case 'groupby':
        if (itemType === 'column') {
          this.addGroupByColumn(item);
        }
        break;
    }
  }

  // Métodos para manipular a query
  addSelectColumn(column: any): void {
    const selectedColumn = {
      column: column,
      alias: '',
      aggregateFunction: undefined,
      expression: undefined
    };
    this.sqlBuilderService.addSelectColumn(selectedColumn);
  }

  removeSelectColumn(index: number): void {
    this.sqlBuilderService.removeSelectColumn(index);
  }

  setFromTable(table: DatabaseTable): void {
    this.sqlBuilderService.setFromTable(table);
  }

  addWhereCondition(column: any): void {
    const condition = {
      column: column,
      operator: '=' as any,
      value: '',
      valueType: 'LITERAL' as any,
      logicalOperator: 'AND' as any
    };
    this.sqlBuilderService.addWhereCondition(condition);
  }

  removeWhereCondition(index: number): void {
    this.sqlBuilderService.removeWhereCondition(index);
  }

  addOrderByColumn(column: any): void {
    const orderBy = {
      column: column,
      direction: 'ASC' as any
    };
    this.sqlBuilderService.addOrderBy(orderBy);
  }

  removeOrderByColumn(index: number): void {
    this.sqlBuilderService.removeOrderBy(index);
  }

  addGroupByColumn(column: any): void {
    const query = this.sqlBuilderService.getCurrentQuery();
    query.groupBy.columns.push(column);
    this.sqlBuilderService.updateQuery(query);
  }

  removeGroupByColumn(index: number): void {
    const query = this.sqlBuilderService.getCurrentQuery();
    query.groupBy.columns.splice(index, 1);
    this.sqlBuilderService.updateQuery(query);
  }

  // Utilitários
  resetQuery(): void {
    this.sqlBuilderService.resetQuery();
    this.queryResult = null;
    this.showResults = false;
  }

  copyToClipboard(): void {
    navigator.clipboard.writeText(this.generatedSql).then(() => {
      // Mostrar notificação de sucesso
      console.log('SQL copiado para a área de transferência');
    });
  }

  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  toggleSqlPreview(): void {
    this.showSqlPreview = !this.showSqlPreview;
  }

  onDatabaseTypeChange(): void {
    this.generateSql();
  }

  // Getters para o template
  get hasSelectColumns(): boolean {
    return this.currentQuery.select.columns.length > 0;
  }

  get hasFromTable(): boolean {
    return !!this.currentQuery.from.table;
  }

  get hasWhereConditions(): boolean {
    return this.currentQuery.where.conditions.length > 0;
  }

  get hasOrderBy(): boolean {
    return this.currentQuery.orderBy.columns.length > 0;
  }

  get hasGroupBy(): boolean {
    return this.currentQuery.groupBy.columns.length > 0;
  }

  get canExecute(): boolean {
    return this.generatedSql.trim().length > 0 && !this.isExecuting;
  }
}

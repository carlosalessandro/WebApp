import { Component, Input, Output, EventEmitter } from '@angular/core';
import { SelectedColumn, AggregateFunction } from '../../../models/sql-builder.model';

@Component({
  selector: 'app-select-builder',
  templateUrl: './select-builder.component.html',
  styleUrls: ['./select-builder.component.css']
})
export class SelectBuilderComponent {
  @Input() columns: SelectedColumn[] = [];
  @Input() distinct: boolean = false;
  
  @Output() columnAdded = new EventEmitter<SelectedColumn>();
  @Output() columnRemoved = new EventEmitter<number>();
  @Output() distinctChanged = new EventEmitter<boolean>();
  @Output() drop = new EventEmitter<DragEvent>();
  @Output() dragover = new EventEmitter<DragEvent>();

  aggregateFunctions = Object.values(AggregateFunction);

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.drop.emit(event);
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.dragover.emit(event);
  }

  onDragEnter(event: DragEvent): void {
    event.preventDefault();
    (event.currentTarget as HTMLElement).classList.add('drag-over');
  }

  onDragLeave(event: DragEvent): void {
    (event.currentTarget as HTMLElement).classList.remove('drag-over');
  }

  removeColumn(index: number): void {
    this.columnRemoved.emit(index);
  }

  updateColumnAlias(index: number, alias: string): void {
    if (this.columns[index]) {
      this.columns[index].alias = alias;
    }
  }

  updateColumnAggregateFunction(index: number, func: AggregateFunction | undefined): void {
    if (this.columns[index]) {
      this.columns[index].aggregateFunction = func;
    }
  }

  updateColumnExpression(index: number, expression: string): void {
    if (this.columns[index]) {
      this.columns[index].expression = expression;
    }
  }

  toggleDistinct(): void {
    this.distinct = !this.distinct;
    this.distinctChanged.emit(this.distinct);
  }

  addAllColumns(): void {
    // Esta funcionalidade seria implementada para adicionar SELECT *
    console.log('Add all columns functionality');
  }

  clearAllColumns(): void {
    for (let i = this.columns.length - 1; i >= 0; i--) {
      this.columnRemoved.emit(i);
    }
  }

  getColumnDisplayName(column: SelectedColumn): string {
    if (column.expression) {
      return column.expression;
    }
    
    let name = `${column.column.table}.${column.column.name}`;
    
    if (column.aggregateFunction) {
      name = `${column.aggregateFunction}(${name})`;
    }
    
    return name;
  }

  isValidExpression(expression: string): boolean {
    // Validação básica de expressão SQL
    if (!expression.trim()) return false;
    
    // Verificar se contém caracteres perigosos
    const dangerousPatterns = /DROP|DELETE|INSERT|UPDATE|ALTER|CREATE|TRUNCATE/i;
    return !dangerousPatterns.test(expression);
  }
}

import { Component, Input, Output, EventEmitter } from '@angular/core';
import { WhereCondition, ComparisonOperator, LogicalOperator, ValueType } from '../../../models/sql-builder.model';

@Component({
  selector: 'app-where-builder',
  templateUrl: './where-builder.component.html',
  styleUrls: ['./where-builder.component.css']
})
export class WhereBuilderComponent {
  @Input() conditions: WhereCondition[] = [];
  @Input() logicalOperator: LogicalOperator = LogicalOperator.AND;
  
  @Output() conditionAdded = new EventEmitter<WhereCondition>();
  @Output() conditionRemoved = new EventEmitter<number>();
  @Output() logicalOperatorChanged = new EventEmitter<LogicalOperator>();
  @Output() drop = new EventEmitter<DragEvent>();
  @Output() dragover = new EventEmitter<DragEvent>();

  comparisonOperators = Object.values(ComparisonOperator);
  logicalOperators = Object.values(LogicalOperator);
  valueTypes = Object.values(ValueType);

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

  removeCondition(index: number): void {
    this.conditionRemoved.emit(index);
  }

  updateConditionOperator(index: number, operator: ComparisonOperator): void {
    if (this.conditions[index]) {
      this.conditions[index].operator = operator;
      
      // Reset value if operator doesn't need one
      if (operator === ComparisonOperator.IS_NULL || operator === ComparisonOperator.IS_NOT_NULL) {
        this.conditions[index].value = '';
      }
    }
  }

  updateConditionValue(index: number, value: any): void {
    if (this.conditions[index]) {
      this.conditions[index].value = value;
    }
  }

  updateConditionValueType(index: number, valueType: ValueType): void {
    if (this.conditions[index]) {
      this.conditions[index].valueType = valueType;
    }
  }

  updateConditionLogicalOperator(index: number, operator: LogicalOperator): void {
    if (this.conditions[index]) {
      this.conditions[index].logicalOperator = operator;
    }
  }

  changeGlobalLogicalOperator(operator: LogicalOperator): void {
    this.logicalOperatorChanged.emit(operator);
  }

  addConditionGroup(): void {
    const groupCondition: WhereCondition = {
      column: null as any,
      operator: ComparisonOperator.EQUALS,
      value: '',
      valueType: ValueType.LITERAL,
      isGroup: true,
      groupConditions: [],
      logicalOperator: LogicalOperator.AND
    };
    this.conditionAdded.emit(groupCondition);
  }

  addConditionToGroup(groupIndex: number, condition: WhereCondition): void {
    if (this.conditions[groupIndex] && this.conditions[groupIndex].isGroup) {
      if (!this.conditions[groupIndex].groupConditions) {
        this.conditions[groupIndex].groupConditions = [];
      }
      this.conditions[groupIndex].groupConditions!.push(condition);
    }
  }

  removeConditionFromGroup(groupIndex: number, conditionIndex: number): void {
    if (this.conditions[groupIndex] && this.conditions[groupIndex].isGroup) {
      this.conditions[groupIndex].groupConditions!.splice(conditionIndex, 1);
    }
  }

  clearAllConditions(): void {
    for (let i = this.conditions.length - 1; i >= 0; i--) {
      this.conditionRemoved.emit(i);
    }
  }

  getConditionDisplayText(condition: WhereCondition): string {
    if (condition.isGroup) {
      return `Grupo (${condition.groupConditions?.length || 0} condições)`;
    }
    
    if (!condition.column) {
      return 'Condição incompleta';
    }

    let text = `${condition.column.table}.${condition.column.name} ${condition.operator}`;
    
    if (condition.operator !== ComparisonOperator.IS_NULL && 
        condition.operator !== ComparisonOperator.IS_NOT_NULL) {
      text += ` ${condition.value || '?'}`;
    }
    
    return text;
  }

  needsValue(operator: ComparisonOperator): boolean {
    return operator !== ComparisonOperator.IS_NULL && 
           operator !== ComparisonOperator.IS_NOT_NULL;
  }

  needsMultipleValues(operator: ComparisonOperator): boolean {
    return operator === ComparisonOperator.IN || 
           operator === ComparisonOperator.NOT_IN ||
           operator === ComparisonOperator.BETWEEN;
  }

  getOperatorDescription(operator: ComparisonOperator): string {
    const descriptions: { [key: string]: string } = {
      [ComparisonOperator.EQUALS]: 'Igual a',
      [ComparisonOperator.NOT_EQUALS]: 'Diferente de',
      [ComparisonOperator.GREATER_THAN]: 'Maior que',
      [ComparisonOperator.GREATER_THAN_OR_EQUAL]: 'Maior ou igual a',
      [ComparisonOperator.LESS_THAN]: 'Menor que',
      [ComparisonOperator.LESS_THAN_OR_EQUAL]: 'Menor ou igual a',
      [ComparisonOperator.LIKE]: 'Contém (LIKE)',
      [ComparisonOperator.NOT_LIKE]: 'Não contém (NOT LIKE)',
      [ComparisonOperator.IN]: 'Está em (IN)',
      [ComparisonOperator.NOT_IN]: 'Não está em (NOT IN)',
      [ComparisonOperator.IS_NULL]: 'É nulo',
      [ComparisonOperator.IS_NOT_NULL]: 'Não é nulo',
      [ComparisonOperator.BETWEEN]: 'Entre (BETWEEN)'
    };
    return descriptions[operator] || operator;
  }

  isValidCondition(condition: WhereCondition): boolean {
    if (condition.isGroup) {
      return condition.groupConditions !== undefined && condition.groupConditions.length > 0;
    }
    
    if (!condition.column) {
      return false;
    }
    
    if (this.needsValue(condition.operator) && !condition.value) {
      return false;
    }
    
    return true;
  }
}

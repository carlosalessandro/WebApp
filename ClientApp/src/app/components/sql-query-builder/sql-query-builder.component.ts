import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, BehaviorSubject } from 'rxjs';
import { 
  SqlQuery, 
  QueryType, 
  TableReference, 
  FieldSelection, 
  JoinClause, 
  WhereCondition, 
  OrderByClause,
  ComparisonOperator,
  LogicalOperator,
  JoinType,
  AggregateFunction,
  QueryExecutionResult,
  DatabaseSchema,
  TableSchema,
  SqlCommand,
  SqlCommandCategory
} from '../../models/sql-builder.models';
import { SqlBuilderService } from '../../services/sql-builder.service';

@Component({
  selector: 'app-sql-query-builder',
  templateUrl: './sql-query-builder.component.html',
  styleUrls: ['./sql-query-builder.component.css']
})
export class SqlQueryBuilderComponent implements OnInit {
  @Input() isReadOnly: boolean = false;
  @Input() showTutorial: boolean = true;
  @Output() queryExecuted = new EventEmitter<QueryExecutionResult>();
  @Output() queryChanged = new EventEmitter<SqlQuery>();

  queryForm: FormGroup;
  currentQuery: SqlQuery;
  availableTables: TableSchema[] = [];
  selectedTables: TableReference[] = [];
  executionResult: QueryExecutionResult | null = null;
  isExecuting = false;
  showSqlPreview = true;
  generatedSql = '';
  
  // Tutorial and help
  sqlCommands: SqlCommand[] = [];
  selectedCommand: SqlCommand | null = null;
  showCommandHelp = false;
  
  // Enums for templates
  QueryType = QueryType;
  ComparisonOperator = ComparisonOperator;
  LogicalOperator = LogicalOperator;
  JoinType = JoinType;
  AggregateFunction = AggregateFunction;

  private querySubject = new BehaviorSubject<SqlQuery | null>(null);

  constructor(
    private fb: FormBuilder,
    private sqlBuilderService: SqlBuilderService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {
    this.initializeForm();
    this.currentQuery = this.createEmptyQuery();
  }

  ngOnInit(): void {
    this.loadDatabaseSchema();
    this.loadSqlCommands();
    this.setupFormSubscriptions();
  }

  private initializeForm(): void {
    this.queryForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      type: [QueryType.SELECT, Validators.required],
      tables: this.fb.array([]),
      fields: this.fb.array([]),
      joins: this.fb.array([]),
      conditions: this.fb.array([]),
      groupBy: this.fb.array([]),
      orderBy: this.fb.array([]),
      limit: [null],
      offset: [null]
    });
  }

  private setupFormSubscriptions(): void {
    this.queryForm.valueChanges.subscribe(() => {
      this.updateCurrentQuery();
      this.generateSql();
    });
  }

  private createEmptyQuery(): SqlQuery {
    return {
      id: '',
      name: 'New Query',
      type: QueryType.SELECT,
      tables: [],
      fields: [],
      joins: [],
      conditions: [],
      groupBy: [],
      having: [],
      orderBy: [],
      metadata: {
        createdAt: new Date(),
        updatedAt: new Date(),
        createdBy: 'current-user',
        isValid: false,
        validationErrors: []
      }
    };
  }

  private loadDatabaseSchema(): void {
    this.sqlBuilderService.getDatabaseSchema().subscribe({
      next: (schema) => {
        this.availableTables = schema.tables;
      },
      error: (error) => {
        console.error('Error loading database schema:', error);
        this.snackBar.open('Failed to load database schema', 'Close', { duration: 3000 });
      }
    });
  }

  private loadSqlCommands(): void {
    this.sqlBuilderService.getSqlCommands().subscribe({
      next: (commands) => {
        this.sqlCommands = commands;
      },
      error: (error) => {
        console.error('Error loading SQL commands:', error);
      }
    });
  }

  // Form array getters
  get tablesFormArray(): FormArray {
    return this.queryForm.get('tables') as FormArray;
  }

  get fieldsFormArray(): FormArray {
    return this.queryForm.get('fields') as FormArray;
  }

  get joinsFormArray(): FormArray {
    return this.queryForm.get('joins') as FormArray;
  }

  get conditionsFormArray(): FormArray {
    return this.queryForm.get('conditions') as FormArray;
  }

  get orderByFormArray(): FormArray {
    return this.queryForm.get('orderBy') as FormArray;
  }

  // Table management
  addTable(table: TableSchema): void {
    if (this.selectedTables.find(t => t.name === table.name)) {
      this.snackBar.open('Table already added', 'Close', { duration: 2000 });
      return;
    }

    const tableRef: TableReference = {
      id: `table_${Date.now()}`,
      name: table.name,
      alias: table.name.toLowerCase(),
      schema: table.schema,
      columns: table.columns,
      position: { x: this.selectedTables.length * 200, y: 50 }
    };

    this.selectedTables.push(tableRef);
    this.tablesFormArray.push(this.createTableFormGroup(tableRef));
    
    // Add available fields
    this.updateAvailableFields();
  }

  removeTable(index: number): void {
    const table = this.selectedTables[index];
    this.selectedTables.splice(index, 1);
    this.tablesFormArray.removeAt(index);
    
    // Remove related fields, joins, and conditions
    this.removeFieldsForTable(table.alias!);
    this.removeJoinsForTable(table.alias!);
    this.removeConditionsForTable(table.alias!);
    
    this.updateAvailableFields();
  }

  private createTableFormGroup(table: TableReference): FormGroup {
    return this.fb.group({
      id: [table.id],
      name: [table.name],
      alias: [table.alias, Validators.required],
      schema: [table.schema]
    });
  }

  // Field management
  addField(tableAlias: string, columnName: string): void {
    const field: FieldSelection = {
      id: `field_${Date.now()}`,
      tableAlias,
      columnName,
      isCalculated: false
    };

    this.fieldsFormArray.push(this.createFieldFormGroup(field));
  }

  removeField(index: number): void {
    this.fieldsFormArray.removeAt(index);
  }

  private createFieldFormGroup(field: FieldSelection): FormGroup {
    return this.fb.group({
      id: [field.id],
      tableAlias: [field.tableAlias, Validators.required],
      columnName: [field.columnName, Validators.required],
      alias: [field.alias],
      aggregateFunction: [field.aggregateFunction],
      expression: [field.expression],
      isCalculated: [field.isCalculated]
    });
  }

  // Join management
  addJoin(): void {
    if (this.selectedTables.length < 2) {
      this.snackBar.open('Need at least 2 tables to create a join', 'Close', { duration: 3000 });
      return;
    }

    const join: JoinClause = {
      id: `join_${Date.now()}`,
      type: JoinType.INNER,
      leftTable: this.selectedTables[0].alias!,
      rightTable: this.selectedTables[1].alias!,
      conditions: []
    };

    this.joinsFormArray.push(this.createJoinFormGroup(join));
  }

  removeJoin(index: number): void {
    this.joinsFormArray.removeAt(index);
  }

  private createJoinFormGroup(join: JoinClause): FormGroup {
    return this.fb.group({
      id: [join.id],
      type: [join.type, Validators.required],
      leftTable: [join.leftTable, Validators.required],
      rightTable: [join.rightTable, Validators.required],
      leftColumn: ['', Validators.required],
      rightColumn: ['', Validators.required]
    });
  }

  // Condition management
  addCondition(): void {
    const condition: WhereCondition = {
      id: `condition_${Date.now()}`,
      field: '',
      operator: ComparisonOperator.EQUALS,
      value: '',
      logicalOperator: LogicalOperator.AND,
      isNested: false
    };

    this.conditionsFormArray.push(this.createConditionFormGroup(condition));
  }

  removeCondition(index: number): void {
    this.conditionsFormArray.removeAt(index);
  }

  private createConditionFormGroup(condition: WhereCondition): FormGroup {
    return this.fb.group({
      id: [condition.id],
      field: [condition.field, Validators.required],
      operator: [condition.operator, Validators.required],
      value: [condition.value, Validators.required],
      logicalOperator: [condition.logicalOperator]
    });
  }

  // Order By management
  addOrderBy(): void {
    const orderBy: OrderByClause = {
      field: '',
      direction: 'ASC',
      tableAlias: ''
    };

    this.orderByFormArray.push(this.createOrderByFormGroup(orderBy));
  }

  removeOrderBy(index: number): void {
    this.orderByFormArray.removeAt(index);
  }

  private createOrderByFormGroup(orderBy: OrderByClause): FormGroup {
    return this.fb.group({
      field: [orderBy.field, Validators.required],
      direction: [orderBy.direction, Validators.required],
      tableAlias: [orderBy.tableAlias, Validators.required]
    });
  }

  // Helper methods
  private updateAvailableFields(): void {
    // This would update the available fields based on selected tables
  }

  private removeFieldsForTable(tableAlias: string): void {
    const fieldsToRemove: number[] = [];
    this.fieldsFormArray.controls.forEach((control, index) => {
      if (control.get('tableAlias')?.value === tableAlias) {
        fieldsToRemove.push(index);
      }
    });
    
    fieldsToRemove.reverse().forEach(index => {
      this.fieldsFormArray.removeAt(index);
    });
  }

  private removeJoinsForTable(tableAlias: string): void {
    const joinsToRemove: number[] = [];
    this.joinsFormArray.controls.forEach((control, index) => {
      const leftTable = control.get('leftTable')?.value;
      const rightTable = control.get('rightTable')?.value;
      if (leftTable === tableAlias || rightTable === tableAlias) {
        joinsToRemove.push(index);
      }
    });
    
    joinsToRemove.reverse().forEach(index => {
      this.joinsFormArray.removeAt(index);
    });
  }

  private removeConditionsForTable(tableAlias: string): void {
    const conditionsToRemove: number[] = [];
    this.conditionsFormArray.controls.forEach((control, index) => {
      const field = control.get('field')?.value;
      if (field.startsWith(tableAlias + '.')) {
        conditionsToRemove.push(index);
      }
    });
    
    conditionsToRemove.reverse().forEach(index => {
      this.conditionsFormArray.removeAt(index);
    });
  }

  private updateCurrentQuery(): void {
    if (!this.queryForm.valid) return;

    const formValue = this.queryForm.value;
    this.currentQuery = {
      ...this.currentQuery,
      name: formValue.name,
      description: formValue.description,
      type: formValue.type,
      tables: this.selectedTables,
      fields: formValue.fields || [],
      joins: formValue.joins || [],
      conditions: formValue.conditions || [],
      orderBy: formValue.orderBy || [],
      limit: formValue.limit,
      offset: formValue.offset,
      metadata: {
        ...this.currentQuery.metadata,
        updatedAt: new Date(),
        isValid: this.queryForm.valid,
        validationErrors: this.getValidationErrors()
      }
    };

    this.queryChanged.emit(this.currentQuery);
  }

  private getValidationErrors(): string[] {
    const errors: string[] = [];
    
    if (this.selectedTables.length === 0) {
      errors.push('At least one table must be selected');
    }
    
    if (this.fieldsFormArray.length === 0 && this.currentQuery.type === QueryType.SELECT) {
      errors.push('At least one field must be selected for SELECT queries');
    }
    
    return errors;
  }

  private generateSql(): void {
    this.sqlBuilderService.generateSql(this.currentQuery).subscribe({
      next: (sql) => {
        this.generatedSql = sql;
      },
      error: (error) => {
        console.error('Error generating SQL:', error);
        this.generatedSql = '-- Error generating SQL';
      }
    });
  }

  // Public methods
  executeQuery(): void {
    if (!this.queryForm.valid) {
      this.snackBar.open('Please fix validation errors before executing', 'Close', { duration: 3000 });
      return;
    }

    this.isExecuting = true;
    this.sqlBuilderService.executeQuery(this.currentQuery).subscribe({
      next: (result) => {
        this.executionResult = result;
        this.isExecuting = false;
        this.queryExecuted.emit(result);
        
        if (result.success) {
          this.snackBar.open(`Query executed successfully. ${result.rowCount} rows returned.`, 'Close', { duration: 3000 });
        } else {
          this.snackBar.open(`Query failed: ${result.error}`, 'Close', { duration: 5000 });
        }
      },
      error: (error) => {
        this.isExecuting = false;
        this.snackBar.open('Failed to execute query', 'Close', { duration: 3000 });
        console.error('Query execution error:', error);
      }
    });
  }

  saveQuery(): void {
    if (!this.queryForm.valid) {
      this.snackBar.open('Please fix validation errors before saving', 'Close', { duration: 3000 });
      return;
    }

    this.sqlBuilderService.saveQuery(this.currentQuery).subscribe({
      next: (savedQuery) => {
        this.currentQuery = savedQuery;
        this.snackBar.open('Query saved successfully', 'Close', { duration: 2000 });
      },
      error: (error) => {
        this.snackBar.open('Failed to save query', 'Close', { duration: 3000 });
        console.error('Save error:', error);
      }
    });
  }

  loadQuery(queryId: string): void {
    this.sqlBuilderService.getQuery(queryId).subscribe({
      next: (query) => {
        this.currentQuery = query;
        this.selectedTables = query.tables;
        this.populateForm(query);
        this.generateSql();
      },
      error: (error) => {
        this.snackBar.open('Failed to load query', 'Close', { duration: 3000 });
        console.error('Load error:', error);
      }
    });
  }

  private populateForm(query: SqlQuery): void {
    this.queryForm.patchValue({
      name: query.name,
      description: query.description,
      type: query.type,
      limit: query.limit,
      offset: query.offset
    });

    // Clear existing form arrays
    while (this.tablesFormArray.length !== 0) {
      this.tablesFormArray.removeAt(0);
    }
    while (this.fieldsFormArray.length !== 0) {
      this.fieldsFormArray.removeAt(0);
    }
    while (this.joinsFormArray.length !== 0) {
      this.joinsFormArray.removeAt(0);
    }
    while (this.conditionsFormArray.length !== 0) {
      this.conditionsFormArray.removeAt(0);
    }
    while (this.orderByFormArray.length !== 0) {
      this.orderByFormArray.removeAt(0);
    }

    // Populate form arrays
    query.tables.forEach(table => {
      this.tablesFormArray.push(this.createTableFormGroup(table));
    });

    query.fields.forEach(field => {
      this.fieldsFormArray.push(this.createFieldFormGroup(field));
    });

    query.joins.forEach(join => {
      this.joinsFormArray.push(this.createJoinFormGroup(join));
    });

    query.conditions.forEach(condition => {
      this.conditionsFormArray.push(this.createConditionFormGroup(condition));
    });

    query.orderBy.forEach(orderBy => {
      this.orderByFormArray.push(this.createOrderByFormGroup(orderBy));
    });
  }

  newQuery(): void {
    this.currentQuery = this.createEmptyQuery();
    this.selectedTables = [];
    this.executionResult = null;
    this.generatedSql = '';
    this.queryForm.reset();
    this.initializeForm();
  }

  toggleSqlPreview(): void {
    this.showSqlPreview = !this.showSqlPreview;
  }

  showCommandHelp(command: SqlCommand): void {
    this.selectedCommand = command;
    this.showCommandHelp = true;
  }

  getCommandsByCategory(category: SqlCommandCategory): SqlCommand[] {
    return this.sqlCommands.filter(cmd => cmd.category === category);
  }

  getAvailableColumnsForTable(tableAlias: string): any[] {
    const table = this.selectedTables.find(t => t.alias === tableAlias);
    return table ? table.columns : [];
  }

  getAvailableTables(): TableReference[] {
    return this.selectedTables;
  }

  formatSql(sql: string): string {
    // Basic SQL formatting
    return sql
      .replace(/\bSELECT\b/gi, '\nSELECT')
      .replace(/\bFROM\b/gi, '\nFROM')
      .replace(/\bWHERE\b/gi, '\nWHERE')
      .replace(/\bJOIN\b/gi, '\nJOIN')
      .replace(/\bORDER BY\b/gi, '\nORDER BY')
      .replace(/\bGROUP BY\b/gi, '\nGROUP BY')
      .replace(/\bHAVING\b/gi, '\nHAVING')
      .trim();
  }
}

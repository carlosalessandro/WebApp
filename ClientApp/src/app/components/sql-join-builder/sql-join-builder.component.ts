import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

export interface DatabaseTable {
  name: string;
  alias?: string;
  columns: DatabaseColumn[];
}

export interface DatabaseColumn {
  name: string;
  type: string;
  table: string;
  isPrimaryKey?: boolean;
  isForeignKey?: boolean;
  referencedTable?: string;
  referencedColumn?: string;
}

export interface SqlJoin {
  id: string;
  type: 'INNER JOIN' | 'LEFT JOIN' | 'RIGHT JOIN' | 'FULL OUTER JOIN';
  leftTable: string;
  leftColumn: string;
  rightTable: string;
  rightColumn: string;
  operator: '=' | '!=' | '>' | '<' | '>=' | '<=';
}

export interface SqlField {
  id: string;
  table: string;
  column: string;
  alias?: string;
  aggregateFunction?: 'COUNT' | 'SUM' | 'AVG' | 'MIN' | 'MAX';
}

export interface SqlCondition {
  id: string;
  leftTable: string;
  leftColumn: string;
  operator: '=' | '!=' | '>' | '<' | '>=' | '<=' | 'LIKE' | 'IN';
  rightValue: string;
  logicalOperator?: 'AND' | 'OR';
}

export interface SqlQuery {
  id: string;
  name: string;
  tables: DatabaseTable[];
  fields: SqlField[];
  joins: SqlJoin[];
  conditions: SqlCondition[];
  groupBy: { table: string; column: string }[];
  orderBy: { table: string; column: string; direction: 'ASC' | 'DESC' }[];
  limit?: number;
  generatedSql: string;
}

@Component({
  selector: 'app-sql-join-builder',
  templateUrl: './sql-join-builder.component.html',
  styleUrls: ['./sql-join-builder.component.css']
})
export class SqlJoinBuilderComponent implements OnInit {
  @Output() queryGenerated = new EventEmitter<string>();
  @Output() queryExecuted = new EventEmitter<any>();

  queryForm: FormGroup;
  currentQuery: SqlQuery;
  generatedSql = '';

  // Mock database schema
  availableTables: DatabaseTable[] = [
    {
      name: 'users',
      columns: [
        { name: 'id', type: 'int', table: 'users', isPrimaryKey: true },
        { name: 'name', type: 'varchar', table: 'users' },
        { name: 'email', type: 'varchar', table: 'users' },
        { name: 'age', type: 'int', table: 'users' },
        { name: 'created_at', type: 'datetime', table: 'users' }
      ]
    },
    {
      name: 'orders',
      columns: [
        { name: 'id', type: 'int', table: 'orders', isPrimaryKey: true },
        { name: 'user_id', type: 'int', table: 'orders', isForeignKey: true, referencedTable: 'users', referencedColumn: 'id' },
        { name: 'total', type: 'decimal', table: 'orders' },
        { name: 'status', type: 'varchar', table: 'orders' },
        { name: 'created_at', type: 'datetime', table: 'orders' }
      ]
    },
    {
      name: 'order_items',
      columns: [
        { name: 'id', type: 'int', table: 'order_items', isPrimaryKey: true },
        { name: 'order_id', type: 'int', table: 'order_items', isForeignKey: true, referencedTable: 'orders', referencedColumn: 'id' },
        { name: 'product_name', type: 'varchar', table: 'order_items' },
        { name: 'quantity', type: 'int', table: 'order_items' },
        { name: 'price', type: 'decimal', table: 'order_items' }
      ]
    },
    {
      name: 'products',
      columns: [
        { name: 'id', type: 'int', table: 'products', isPrimaryKey: true },
        { name: 'name', type: 'varchar', table: 'products' },
        { name: 'price', type: 'decimal', table: 'products' },
        { name: 'category_id', type: 'int', table: 'products' }
      ]
    }
  ];

  joinTypes = [
    { value: 'INNER JOIN', label: 'INNER JOIN', description: 'Returns records that have matching values in both tables' },
    { value: 'LEFT JOIN', label: 'LEFT JOIN', description: 'Returns all records from the left table, and matched records from the right table' },
    { value: 'RIGHT JOIN', label: 'RIGHT JOIN', description: 'Returns all records from the right table, and matched records from the left table' },
    { value: 'FULL OUTER JOIN', label: 'FULL OUTER JOIN', description: 'Returns all records when there is a match in either left or right table' }
  ];

  aggregateFunctions = [
    { value: 'COUNT', label: 'COUNT', description: 'Count the number of rows' },
    { value: 'SUM', label: 'SUM', description: 'Calculate the sum of values' },
    { value: 'AVG', label: 'AVG', description: 'Calculate the average of values' },
    { value: 'MIN', label: 'MIN', description: 'Find the minimum value' },
    { value: 'MAX', label: 'MAX', description: 'Find the maximum value' }
  ];

  operators = [
    { value: '=', label: '=' },
    { value: '!=', label: '!=' },
    { value: '>', label: '>' },
    { value: '<', label: '<' },
    { value: '>=', label: '>=' },
    { value: '<=', label: '<=' }
  ];

  constructor(
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.initializeQuery();
    this.createForm();
  }

  ngOnInit(): void {
    this.generateSql();
  }

  private initializeQuery(): void {
    this.currentQuery = {
      id: this.generateId(),
      name: 'New Query',
      tables: [],
      fields: [],
      joins: [],
      conditions: [],
      groupBy: [],
      orderBy: [],
      generatedSql: ''
    };
  }

  private createForm(): void {
    this.queryForm = this.fb.group({
      name: [this.currentQuery.name, Validators.required],
      selectedTables: [[]],
      fields: this.fb.array([]),
      joins: this.fb.array([]),
      conditions: this.fb.array([]),
      groupBy: this.fb.array([]),
      orderBy: this.fb.array([]),
      limit: [null]
    });

    // Watch for form changes
    this.queryForm.valueChanges.subscribe(() => {
      this.updateQueryFromForm();
      this.generateSql();
    });
  }

  // Form Array Getters
  get fieldsArray(): FormArray {
    return this.queryForm.get('fields') as FormArray;
  }

  get joinsArray(): FormArray {
    return this.queryForm.get('joins') as FormArray;
  }

  get conditionsArray(): FormArray {
    return this.queryForm.get('conditions') as FormArray;
  }

  get groupByArray(): FormArray {
    return this.queryForm.get('groupBy') as FormArray;
  }

  get orderByArray(): FormArray {
    return this.queryForm.get('orderBy') as FormArray;
  }

  // Table Management
  onTablesChange(selectedTableNames: string[]): void {
    this.currentQuery.tables = selectedTableNames.map(name => 
      this.availableTables.find(t => t.name === name)!
    );
    this.generateSql();
  }

  getColumnsForTable(tableName: string): DatabaseColumn[] {
    const table = this.availableTables.find(t => t.name === tableName);
    return table ? table.columns : [];
  }

  // Field Management
  addField(): void {
    const fieldGroup = this.fb.group({
      table: ['', Validators.required],
      column: ['', Validators.required],
      alias: [''],
      aggregateFunction: ['']
    });
    this.fieldsArray.push(fieldGroup);
  }

  removeField(index: number): void {
    this.fieldsArray.removeAt(index);
  }

  // Join Management
  addJoin(): void {
    const joinGroup = this.fb.group({
      type: ['INNER JOIN', Validators.required],
      leftTable: ['', Validators.required],
      leftColumn: ['', Validators.required],
      rightTable: ['', Validators.required],
      rightColumn: ['', Validators.required],
      operator: ['=', Validators.required]
    });
    this.joinsArray.push(joinGroup);
  }

  removeJoin(index: number): void {
    this.joinsArray.removeAt(index);
  }

  // Condition Management
  addCondition(): void {
    const conditionGroup = this.fb.group({
      leftTable: ['', Validators.required],
      leftColumn: ['', Validators.required],
      operator: ['=', Validators.required],
      rightValue: ['', Validators.required],
      logicalOperator: ['AND']
    });
    this.conditionsArray.push(conditionGroup);
  }

  removeCondition(index: number): void {
    this.conditionsArray.removeAt(index);
  }

  // Group By Management
  addGroupBy(): void {
    const groupByGroup = this.fb.group({
      table: ['', Validators.required],
      column: ['', Validators.required]
    });
    this.groupByArray.push(groupByGroup);
  }

  removeGroupBy(index: number): void {
    this.groupByArray.removeAt(index);
  }

  // Order By Management
  addOrderBy(): void {
    const orderByGroup = this.fb.group({
      table: ['', Validators.required],
      column: ['', Validators.required],
      direction: ['ASC', Validators.required]
    });
    this.orderByArray.push(orderByGroup);
  }

  removeOrderBy(index: number): void {
    this.orderByArray.removeAt(index);
  }

  // SQL Generation
  private updateQueryFromForm(): void {
    const formValue = this.queryForm.value;
    
    this.currentQuery.name = formValue.name;
    this.currentQuery.fields = formValue.fields.map((field: any) => ({
      id: this.generateId(),
      table: field.table,
      column: field.column,
      alias: field.alias,
      aggregateFunction: field.aggregateFunction
    }));
    
    this.currentQuery.joins = formValue.joins.map((join: any) => ({
      id: this.generateId(),
      type: join.type,
      leftTable: join.leftTable,
      leftColumn: join.leftColumn,
      rightTable: join.rightTable,
      rightColumn: join.rightColumn,
      operator: join.operator
    }));
    
    this.currentQuery.conditions = formValue.conditions.map((condition: any) => ({
      id: this.generateId(),
      leftTable: condition.leftTable,
      leftColumn: condition.leftColumn,
      operator: condition.operator,
      rightValue: condition.rightValue,
      logicalOperator: condition.logicalOperator
    }));
    
    this.currentQuery.groupBy = formValue.groupBy;
    this.currentQuery.orderBy = formValue.orderBy;
    this.currentQuery.limit = formValue.limit;
  }

  generateSql(): void {
    let sql = 'SELECT ';

    // SELECT clause
    if (this.currentQuery.fields && this.currentQuery.fields.length > 0) {
      const fieldStrings = this.currentQuery.fields.map(field => {
        let fieldStr = '';
        
        if (field.aggregateFunction) {
          fieldStr = `${field.aggregateFunction}(${field.table}.${field.column})`;
        } else {
          fieldStr = `${field.table}.${field.column}`;
        }

        if (field.alias) {
          fieldStr += ` AS ${field.alias}`;
        }

        return fieldStr;
      });
      sql += fieldStrings.join(', ');
    } else {
      sql += '*';
    }

    // FROM clause
    if (this.currentQuery.tables && this.currentQuery.tables.length > 0) {
      sql += `\nFROM ${this.currentQuery.tables[0].name}`;
    }

    // JOIN clauses
    if (this.currentQuery.joins && this.currentQuery.joins.length > 0) {
      this.currentQuery.joins.forEach(join => {
        sql += `\n${join.type} ${join.rightTable}`;
        sql += ` ON ${join.leftTable}.${join.leftColumn} ${join.operator} ${join.rightTable}.${join.rightColumn}`;
      });
    }

    // WHERE clause
    if (this.currentQuery.conditions && this.currentQuery.conditions.length > 0) {
      sql += '\nWHERE ';
      const conditionStrings = this.currentQuery.conditions.map((condition, index) => {
        let condStr = '';
        
        if (index > 0 && condition.logicalOperator) {
          condStr += `${condition.logicalOperator} `;
        }

        condStr += `${condition.leftTable}.${condition.leftColumn} ${condition.operator} '${condition.rightValue}'`;
        return condStr;
      });
      sql += conditionStrings.join(' ');
    }

    // GROUP BY clause
    if (this.currentQuery.groupBy && this.currentQuery.groupBy.length > 0) {
      sql += '\nGROUP BY ';
      const groupByStrings = this.currentQuery.groupBy.map(gb => `${gb.table}.${gb.column}`);
      sql += groupByStrings.join(', ');
    }

    // ORDER BY clause
    if (this.currentQuery.orderBy && this.currentQuery.orderBy.length > 0) {
      sql += '\nORDER BY ';
      const orderByStrings = this.currentQuery.orderBy.map(ob => `${ob.table}.${ob.column} ${ob.direction}`);
      sql += orderByStrings.join(', ');
    }

    // LIMIT clause
    if (this.currentQuery.limit) {
      sql += `\nLIMIT ${this.currentQuery.limit}`;
    }

    this.generatedSql = sql;
    this.currentQuery.generatedSql = sql;
    this.queryGenerated.emit(sql);
  }

  // Quick Examples
  loadInnerJoinExample(): void {
    this.resetQuery();
    this.onTablesChange(['users', 'orders']);
    
    // Add fields
    this.addField();
    this.fieldsArray.at(0).patchValue({ table: 'users', column: 'name' });
    this.addField();
    this.fieldsArray.at(1).patchValue({ table: 'users', column: 'email' });
    this.addField();
    this.fieldsArray.at(2).patchValue({ table: 'orders', column: 'total', aggregateFunction: 'SUM', alias: 'total_orders' });
    
    // Add INNER JOIN
    this.addJoin();
    this.joinsArray.at(0).patchValue({
      type: 'INNER JOIN',
      leftTable: 'users',
      leftColumn: 'id',
      rightTable: 'orders',
      rightColumn: 'user_id',
      operator: '='
    });
    
    // Add GROUP BY
    this.addGroupBy();
    this.groupByArray.at(0).patchValue({ table: 'users', column: 'id' });
    this.addGroupBy();
    this.groupByArray.at(1).patchValue({ table: 'users', column: 'name' });
    
    this.snackBar.open('INNER JOIN example loaded', 'Close', { duration: 2000 });
  }

  loadLeftJoinExample(): void {
    this.resetQuery();
    this.onTablesChange(['users', 'orders']);
    
    // Add fields
    this.addField();
    this.fieldsArray.at(0).patchValue({ table: 'users', column: 'name' });
    this.addField();
    this.fieldsArray.at(1).patchValue({ table: 'users', column: 'email' });
    this.addField();
    this.fieldsArray.at(2).patchValue({ table: 'orders', column: 'total' });
    
    // Add LEFT JOIN
    this.addJoin();
    this.joinsArray.at(0).patchValue({
      type: 'LEFT JOIN',
      leftTable: 'users',
      leftColumn: 'id',
      rightTable: 'orders',
      rightColumn: 'user_id',
      operator: '='
    });
    
    this.snackBar.open('LEFT JOIN example loaded', 'Close', { duration: 2000 });
  }

  loadSumExample(): void {
    this.resetQuery();
    this.onTablesChange(['users', 'orders', 'order_items']);
    
    // Add fields with SUM
    this.addField();
    this.fieldsArray.at(0).patchValue({ table: 'users', column: 'name' });
    this.addField();
    this.fieldsArray.at(1).patchValue({ table: 'order_items', column: 'price', aggregateFunction: 'SUM', alias: 'total_spent' });
    this.addField();
    this.fieldsArray.at(2).patchValue({ table: 'order_items', column: 'quantity', aggregateFunction: 'SUM', alias: 'total_items' });
    
    // Add JOINs
    this.addJoin();
    this.joinsArray.at(0).patchValue({
      type: 'INNER JOIN',
      leftTable: 'users',
      leftColumn: 'id',
      rightTable: 'orders',
      rightColumn: 'user_id',
      operator: '='
    });
    
    this.addJoin();
    this.joinsArray.at(1).patchValue({
      type: 'INNER JOIN',
      leftTable: 'orders',
      leftColumn: 'id',
      rightTable: 'order_items',
      rightColumn: 'order_id',
      operator: '='
    });
    
    // Add GROUP BY
    this.addGroupBy();
    this.groupByArray.at(0).patchValue({ table: 'users', column: 'id' });
    this.addGroupBy();
    this.groupByArray.at(1).patchValue({ table: 'users', column: 'name' });
    
    this.snackBar.open('SUM with JOINs example loaded', 'Close', { duration: 2000 });
  }

  executeQuery(): void {
    if (!this.generatedSql.trim()) {
      this.snackBar.open('No SQL query to execute', 'Close', { duration: 2000 });
      return;
    }

    // Mock execution result
    const mockResult = {
      success: true,
      data: [
        { name: 'John Doe', email: 'john@example.com', total_orders: 350.00 },
        { name: 'Jane Smith', email: 'jane@example.com', total_orders: 275.50 },
        { name: 'Bob Johnson', email: 'bob@example.com', total_orders: 125.75 }
      ],
      rowCount: 3,
      executionTime: 45,
      sql: this.generatedSql
    };

    this.queryExecuted.emit(mockResult);
    this.snackBar.open(`Query executed successfully! ${mockResult.rowCount} rows returned`, 'Close', { duration: 3000 });
  }

  resetQuery(): void {
    this.initializeQuery();
    this.createForm();
    this.generateSql();
  }

  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }
}

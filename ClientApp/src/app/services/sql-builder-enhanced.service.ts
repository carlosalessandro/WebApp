import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { 
  SqlQuery, 
  QueryType,
  QueryExecutionResult,
  DatabaseSchema,
  TableSchema,
  JoinType,
  AggregateFunction,
  JoinClause,
  WhereCondition,
  ComparisonOperator,
  LogicalOperator
} from '../models/sql-builder.models';

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
  type: JoinType;
  leftTable: string;
  leftColumn: string;
  rightTable: string;
  rightColumn: string;
  operator: ComparisonOperator;
}

export interface SqlField {
  id: string;
  table: string;
  column: string;
  alias?: string;
  aggregateFunction?: AggregateFunction;
  expression?: string;
}

export interface SqlCondition {
  id: string;
  leftTable: string;
  leftColumn: string;
  operator: ComparisonOperator;
  rightValue: string;
  rightTable?: string;
  rightColumn?: string;
  logicalOperator?: LogicalOperator;
}

@Injectable({
  providedIn: 'root'
})
export class SqlBuilderEnhancedService {
  private baseUrl = '/api/SqlBuilder';
  private currentQuerySubject = new BehaviorSubject<SqlQuery>(this.createEmptyQuery());
  public currentQuery$ = this.currentQuerySubject.asObservable();

  // Mock database schema for demonstration
  private mockTables: DatabaseTable[] = [
    {
      name: 'users',
      columns: [
        { name: 'id', type: 'int', table: 'users', isPrimaryKey: true },
        { name: 'name', type: 'varchar', table: 'users' },
        { name: 'email', type: 'varchar', table: 'users' },
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

  constructor(private http: HttpClient) {}

  // Query Management
  getCurrentQuery(): SqlQuery {
    return this.currentQuerySubject.value;
  }

  updateQuery(query: SqlQuery): void {
    this.currentQuerySubject.next(query);
  }

  resetQuery(): void {
    this.currentQuerySubject.next(this.createEmptyQuery());
  }

  // Database Schema
  getDatabaseTables(): Observable<DatabaseTable[]> {
    // In production, this would call the API
    return of(this.mockTables);
  }

  getTableColumns(tableName: string): Observable<DatabaseColumn[]> {
    const table = this.mockTables.find(t => t.name === tableName);
    return of(table ? table.columns : []);
  }

  // SQL Generation with JOIN support
  generateSql(query: SqlQuery): string {
    let sql = '';

    switch (query.type) {
      case QueryType.SELECT:
        sql = this.generateSelectQuery(query);
        break;
      case QueryType.INSERT:
        sql = this.generateInsertQuery(query);
        break;
      case QueryType.UPDATE:
        sql = this.generateUpdateQuery(query);
        break;
      case QueryType.DELETE:
        sql = this.generateDeleteQuery(query);
        break;
      default:
        sql = 'SELECT * FROM table_name';
    }

    return sql.trim();
  }

  private generateSelectQuery(query: SqlQuery): string {
    let sql = 'SELECT ';

    // SELECT clause with aggregates
    if (query.fields && query.fields.length > 0) {
      const fieldStrings = query.fields.map(field => {
        let fieldStr = '';
        
        if (field.aggregateFunction) {
          fieldStr = `${field.aggregateFunction}(${field.table}.${field.column})`;
        } else if (field.expression) {
          fieldStr = field.expression;
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
    if (query.tables && query.tables.length > 0) {
      sql += `\nFROM ${query.tables[0].name}`;
      if (query.tables[0].alias) {
        sql += ` AS ${query.tables[0].alias}`;
      }
    }

    // JOIN clauses
    if (query.joins && query.joins.length > 0) {
      query.joins.forEach(join => {
        sql += `\n${join.type} ${join.rightTable}`;
        sql += ` ON ${join.leftTable}.${join.leftColumn} ${join.operator} ${join.rightTable}.${join.rightColumn}`;
      });
    }

    // WHERE clause
    if (query.conditions && query.conditions.length > 0) {
      sql += '\nWHERE ';
      const conditionStrings = query.conditions.map((condition, index) => {
        let condStr = '';
        
        if (index > 0 && condition.logicalOperator) {
          condStr += `${condition.logicalOperator} `;
        }

        if (condition.rightTable && condition.rightColumn) {
          condStr += `${condition.leftTable}.${condition.leftColumn} ${condition.operator} ${condition.rightTable}.${condition.rightColumn}`;
        } else {
          condStr += `${condition.leftTable}.${condition.leftColumn} ${condition.operator} '${condition.rightValue}'`;
        }

        return condStr;
      });
      sql += conditionStrings.join(' ');
    }

    // GROUP BY clause
    if (query.groupBy && query.groupBy.length > 0) {
      sql += '\nGROUP BY ';
      const groupByStrings = query.groupBy.map(gb => `${gb.table}.${gb.column}`);
      sql += groupByStrings.join(', ');
    }

    // HAVING clause
    if (query.having && query.having.length > 0) {
      sql += '\nHAVING ';
      const havingStrings = query.having.map(having => {
        if (having.aggregateFunction) {
          return `${having.aggregateFunction}(${having.table}.${having.column}) ${having.operator} ${having.value}`;
        }
        return `${having.table}.${having.column} ${having.operator} '${having.value}'`;
      });
      sql += havingStrings.join(' AND ');
    }

    // ORDER BY clause
    if (query.orderBy && query.orderBy.length > 0) {
      sql += '\nORDER BY ';
      const orderByStrings = query.orderBy.map(ob => `${ob.table}.${ob.column} ${ob.direction}`);
      sql += orderByStrings.join(', ');
    }

    // LIMIT clause
    if (query.limit) {
      sql += `\nLIMIT ${query.limit}`;
      if (query.offset) {
        sql += ` OFFSET ${query.offset}`;
      }
    }

    return sql;
  }

  private generateInsertQuery(query: SqlQuery): string {
    if (!query.tables || query.tables.length === 0) {
      return 'INSERT INTO table_name (column1, column2) VALUES (value1, value2)';
    }

    let sql = `INSERT INTO ${query.tables[0].name}`;
    
    if (query.fields && query.fields.length > 0) {
      const columns = query.fields.map(f => f.column).join(', ');
      const values = query.fields.map(f => `'${f.expression || 'value'}'`).join(', ');
      sql += ` (${columns}) VALUES (${values})`;
    }

    return sql;
  }

  private generateUpdateQuery(query: SqlQuery): string {
    if (!query.tables || query.tables.length === 0) {
      return 'UPDATE table_name SET column1 = value1 WHERE condition';
    }

    let sql = `UPDATE ${query.tables[0].name} SET `;
    
    if (query.fields && query.fields.length > 0) {
      const setClause = query.fields.map(f => `${f.column} = '${f.expression || 'value'}'`).join(', ');
      sql += setClause;
    }

    if (query.conditions && query.conditions.length > 0) {
      sql += ' WHERE ';
      const conditionStrings = query.conditions.map(c => 
        `${c.leftColumn} ${c.operator} '${c.rightValue}'`
      );
      sql += conditionStrings.join(' AND ');
    }

    return sql;
  }

  private generateDeleteQuery(query: SqlQuery): string {
    if (!query.tables || query.tables.length === 0) {
      return 'DELETE FROM table_name WHERE condition';
    }

    let sql = `DELETE FROM ${query.tables[0].name}`;
    
    if (query.conditions && query.conditions.length > 0) {
      sql += ' WHERE ';
      const conditionStrings = query.conditions.map(c => 
        `${c.leftColumn} ${c.operator} '${c.rightValue}'`
      );
      sql += conditionStrings.join(' AND ');
    }

    return sql;
  }

  // Helper methods for building queries
  addTable(query: SqlQuery, tableName: string, alias?: string): SqlQuery {
    const updatedQuery = { ...query };
    if (!updatedQuery.tables) {
      updatedQuery.tables = [];
    }
    updatedQuery.tables.push({ name: tableName, alias });
    return updatedQuery;
  }

  addField(query: SqlQuery, table: string, column: string, alias?: string, aggregateFunction?: AggregateFunction): SqlQuery {
    const updatedQuery = { ...query };
    if (!updatedQuery.fields) {
      updatedQuery.fields = [];
    }
    updatedQuery.fields.push({
      id: this.generateId(),
      table,
      column,
      alias,
      aggregateFunction
    });
    return updatedQuery;
  }

  addJoin(query: SqlQuery, type: JoinType, leftTable: string, leftColumn: string, rightTable: string, rightColumn: string, operator: ComparisonOperator = ComparisonOperator.EQUALS): SqlQuery {
    const updatedQuery = { ...query };
    if (!updatedQuery.joins) {
      updatedQuery.joins = [];
    }
    updatedQuery.joins.push({
      id: this.generateId(),
      type,
      leftTable,
      leftColumn,
      rightTable,
      rightColumn,
      operator
    });
    return updatedQuery;
  }

  addCondition(query: SqlQuery, leftTable: string, leftColumn: string, operator: ComparisonOperator, rightValue: string, logicalOperator?: LogicalOperator): SqlQuery {
    const updatedQuery = { ...query };
    if (!updatedQuery.conditions) {
      updatedQuery.conditions = [];
    }
    updatedQuery.conditions.push({
      id: this.generateId(),
      leftTable,
      leftColumn,
      operator,
      rightValue,
      logicalOperator
    });
    return updatedQuery;
  }

  // Execute query
  executeQuery(query: SqlQuery): Observable<QueryExecutionResult> {
    const sql = this.generateSql(query);
    
    // Mock execution for demonstration
    return of({
      success: true,
      data: [
        { id: 1, name: 'John Doe', email: 'john@example.com', total: 150.00 },
        { id: 2, name: 'Jane Smith', email: 'jane@example.com', total: 200.00 }
      ],
      rowCount: 2,
      executionTime: 45,
      sql: sql,
      error: null
    });
  }

  // Validation
  validateQuery(query: SqlQuery): { isValid: boolean; errors: string[] } {
    const errors: string[] = [];

    if (!query.tables || query.tables.length === 0) {
      errors.push('At least one table must be selected');
    }

    if (query.type === QueryType.SELECT && (!query.fields || query.fields.length === 0)) {
      // This is OK, will default to SELECT *
    }

    if (query.joins) {
      query.joins.forEach(join => {
        if (!join.leftTable || !join.leftColumn || !join.rightTable || !join.rightColumn) {
          errors.push(`Invalid join condition: ${join.leftTable}.${join.leftColumn} = ${join.rightTable}.${join.rightColumn}`);
        }
      });
    }

    return {
      isValid: errors.length === 0,
      errors
    };
  }

  private createEmptyQuery(): SqlQuery {
    return {
      id: this.generateId(),
      name: 'New Query',
      description: '',
      type: QueryType.SELECT,
      tables: [],
      fields: [],
      joins: [],
      conditions: [],
      groupBy: [],
      having: [],
      orderBy: [],
      limit: null,
      offset: null,
      createdAt: new Date(),
      updatedAt: new Date(),
      createdBy: 'user',
      executionTime: null,
      resultCount: null,
      isValid: false,
      validationErrors: []
    };
  }

  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  // Get available aggregate functions
  getAggregateFunctions(): AggregateFunction[] {
    return [
      AggregateFunction.COUNT,
      AggregateFunction.SUM,
      AggregateFunction.AVG,
      AggregateFunction.MIN,
      AggregateFunction.MAX
    ];
  }

  // Get available join types
  getJoinTypes(): JoinType[] {
    return [
      JoinType.INNER,
      JoinType.LEFT,
      JoinType.RIGHT,
      JoinType.FULL
    ];
  }

  // Get comparison operators
  getComparisonOperators(): ComparisonOperator[] {
    return [
      ComparisonOperator.EQUALS,
      ComparisonOperator.NOT_EQUALS,
      ComparisonOperator.GREATER_THAN,
      ComparisonOperator.GREATER_THAN_OR_EQUAL,
      ComparisonOperator.LESS_THAN,
      ComparisonOperator.LESS_THAN_OR_EQUAL,
      ComparisonOperator.LIKE,
      ComparisonOperator.IN,
      ComparisonOperator.NOT_IN,
      ComparisonOperator.IS_NULL,
      ComparisonOperator.IS_NOT_NULL
    ];
  }
}

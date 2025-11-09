import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { 
  DatabaseTable, 
  DatabaseColumn, 
  SqlQuery, 
  QueryExecutionResult,
  SelectClause,
  FromClause,
  WhereClause,
  JoinClause,
  GroupByClause,
  OrderByClause,
  DatabaseType
} from '../models/sql-builder.model';

@Injectable({
  providedIn: 'root'
})
export class SqlBuilderService {
  private baseUrl = '/api/SqlBuilder';
  private currentQuerySubject = new BehaviorSubject<SqlQuery>(this.createEmptyQuery());
  public currentQuery$ = this.currentQuerySubject.asObservable();

  constructor(private http: HttpClient) {}

  // Gerenciamento da query atual
  getCurrentQuery(): SqlQuery {
    return this.currentQuerySubject.value;
  }

  updateQuery(query: SqlQuery): void {
    this.currentQuerySubject.next(query);
  }

  resetQuery(): void {
    this.currentQuerySubject.next(this.createEmptyQuery());
  }

  // Obter metadados do banco
  getDatabaseTables(): Observable<DatabaseTable[]> {
    return this.http.get<DatabaseTable[]>(`${this.baseUrl}/tables`);
  }

  getTableColumns(tableName: string): Observable<DatabaseColumn[]> {
    return this.http.get<DatabaseColumn[]>(`${this.baseUrl}/tables/${tableName}/columns`);
  }

  // Geração de SQL
  generateSql(query: SqlQuery, databaseType: DatabaseType = DatabaseType.MYSQL): string {
    let sql = '';

    // SELECT clause
    sql += this.generateSelectClause(query.select);

    // FROM clause
    sql += this.generateFromClause(query.from);

    // JOIN clauses
    if (query.joins && query.joins.length > 0) {
      sql += this.generateJoinClauses(query.joins);
    }

    // WHERE clause
    if (query.where && query.where.conditions.length > 0) {
      sql += this.generateWhereClause(query.where);
    }

    // GROUP BY clause
    if (query.groupBy && query.groupBy.columns.length > 0) {
      sql += this.generateGroupByClause(query.groupBy);
    }

    // HAVING clause
    if (query.having && query.having.conditions.length > 0) {
      sql += this.generateHavingClause(query.having);
    }

    // ORDER BY clause
    if (query.orderBy && query.orderBy.columns.length > 0) {
      sql += this.generateOrderByClause(query.orderBy);
    }

    // LIMIT clause
    if (query.limit) {
      sql += this.generateLimitClause(query.limit, query.offset);
    }

    return sql.trim();
  }

  private generateSelectClause(select: SelectClause): string {
    if (!select || !select.columns || select.columns.length === 0) {
      return 'SELECT *\n';
    }

    let clause = 'SELECT ';
    if (select.distinct) {
      clause += 'DISTINCT ';
    }

    const columnStrings = select.columns.map(col => {
      let columnStr = '';
      
      if (col.aggregateFunction) {
        columnStr = `${col.aggregateFunction}(${col.column.table}.${col.column.name})`;
      } else if (col.expression) {
        columnStr = col.expression;
      } else {
        columnStr = `${col.column.table}.${col.column.name}`;
      }

      if (col.alias) {
        columnStr += ` AS ${col.alias}`;
      }

      return columnStr;
    });

    clause += columnStrings.join(',\n       ') + '\n';
    return clause;
  }

  private generateFromClause(from: FromClause): string {
    if (!from || !from.table) {
      return '';
    }

    let clause = `FROM ${from.table.name}`;
    if (from.table.alias) {
      clause += ` AS ${from.table.alias}`;
    }
    return clause + '\n';
  }

  private generateJoinClauses(joins: JoinClause[]): string {
    return joins.map(join => {
      let clause = `${join.type} ${join.table.name}`;
      if (join.table.alias) {
        clause += ` AS ${join.table.alias}`;
      }

      if (join.conditions && join.conditions.length > 0) {
        const conditionStrings = join.conditions.map(condition => 
          `${condition.leftColumn.table}.${condition.leftColumn.name} ${condition.operator} ${condition.rightColumn.table}.${condition.rightColumn.name}`
        );
        clause += ` ON ${conditionStrings.join(' AND ')}`;
      }

      return clause;
    }).join('\n') + '\n';
  }

  private generateWhereClause(where: WhereClause): string {
    if (!where.conditions || where.conditions.length === 0) {
      return '';
    }

    const conditionStrings = where.conditions.map(condition => {
      let condStr = `${condition.column.table}.${condition.column.name} ${condition.operator}`;
      
      if (condition.operator === 'IS NULL' || condition.operator === 'IS NOT NULL') {
        // Não adiciona valor para IS NULL/IS NOT NULL
      } else if (condition.valueType === 'COLUMN') {
        condStr += ` ${condition.value}`;
      } else {
        condStr += ` '${condition.value}'`;
      }

      return condStr;
    });

    return `WHERE ${conditionStrings.join(` ${where.logicalOperator} `)}\n`;
  }

  private generateGroupByClause(groupBy: GroupByClause): string {
    const columnStrings = groupBy.columns.map(col => `${col.table}.${col.name}`);
    return `GROUP BY ${columnStrings.join(', ')}\n`;
  }

  private generateHavingClause(having: WhereClause): string {
    if (!having.conditions || having.conditions.length === 0) {
      return '';
    }

    const conditionStrings = having.conditions.map(condition => {
      return `${condition.column.table}.${condition.column.name} ${condition.operator} '${condition.value}'`;
    });

    return `HAVING ${conditionStrings.join(` ${having.logicalOperator} `)}\n`;
  }

  private generateOrderByClause(orderBy: OrderByClause): string {
    const columnStrings = orderBy.columns.map(col => 
      `${col.column.table}.${col.column.name} ${col.direction}`
    );
    return `ORDER BY ${columnStrings.join(', ')}\n`;
  }

  private generateLimitClause(limit: number, offset?: number): string {
    let clause = `LIMIT ${limit}`;
    if (offset && offset > 0) {
      clause += ` OFFSET ${offset}`;
    }
    return clause + '\n';
  }

  // Execução de queries
  executeQuery(sql: string): Observable<QueryExecutionResult> {
    return this.http.post<QueryExecutionResult>(`${this.baseUrl}/execute`, { sql });
  }

  validateQuery(sql: string): Observable<{ isValid: boolean; errors: string[] }> {
    return this.http.post<{ isValid: boolean; errors: string[] }>(`${this.baseUrl}/validate`, { sql });
  }

  // Utilitários
  private createEmptyQuery(): SqlQuery {
    return {
      select: { distinct: false, columns: [] },
      from: { table: null as any },
      joins: [],
      where: { conditions: [], logicalOperator: 'AND' as any },
      groupBy: { columns: [] },
      having: { conditions: [], logicalOperator: 'AND' as any },
      orderBy: { columns: [] }
    };
  }

  // Métodos para manipular a query
  addSelectColumn(column: any): void {
    const query = this.getCurrentQuery();
    query.select.columns.push(column);
    this.updateQuery(query);
  }

  removeSelectColumn(index: number): void {
    const query = this.getCurrentQuery();
    query.select.columns.splice(index, 1);
    this.updateQuery(query);
  }

  setFromTable(table: DatabaseTable): void {
    const query = this.getCurrentQuery();
    query.from.table = table;
    this.updateQuery(query);
  }

  addJoin(join: JoinClause): void {
    const query = this.getCurrentQuery();
    query.joins.push(join);
    this.updateQuery(query);
  }

  removeJoin(index: number): void {
    const query = this.getCurrentQuery();
    query.joins.splice(index, 1);
    this.updateQuery(query);
  }

  addWhereCondition(condition: any): void {
    const query = this.getCurrentQuery();
    query.where.conditions.push(condition);
    this.updateQuery(query);
  }

  removeWhereCondition(index: number): void {
    const query = this.getCurrentQuery();
    query.where.conditions.splice(index, 1);
    this.updateQuery(query);
  }

  addOrderBy(orderBy: any): void {
    const query = this.getCurrentQuery();
    query.orderBy.columns.push(orderBy);
    this.updateQuery(query);
  }

  removeOrderBy(index: number): void {
    const query = this.getCurrentQuery();
    query.orderBy.columns.splice(index, 1);
    this.updateQuery(query);
  }
}

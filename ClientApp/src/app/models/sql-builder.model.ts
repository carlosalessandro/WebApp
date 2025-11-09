export interface DatabaseTable {
  name: string;
  alias?: string;
  columns: DatabaseColumn[];
  schema?: string;
}

export interface DatabaseColumn {
  name: string;
  type: string;
  nullable: boolean;
  isPrimaryKey: boolean;
  isForeignKey: boolean;
  table: string;
  alias?: string;
}

export interface SqlQuery {
  select: SelectClause;
  from: FromClause;
  joins: JoinClause[];
  where: WhereClause;
  groupBy: GroupByClause;
  having: WhereClause;
  orderBy: OrderByClause;
  limit?: number;
  offset?: number;
}

export interface SelectClause {
  distinct: boolean;
  columns: SelectedColumn[];
}

export interface SelectedColumn {
  column: DatabaseColumn;
  alias?: string;
  aggregateFunction?: AggregateFunction;
  expression?: string;
}

export interface FromClause {
  table: DatabaseTable;
}

export interface JoinClause {
  type: JoinType;
  table: DatabaseTable;
  conditions: JoinCondition[];
}

export interface JoinCondition {
  leftColumn: DatabaseColumn;
  operator: ComparisonOperator;
  rightColumn: DatabaseColumn;
}

export interface WhereClause {
  conditions: WhereCondition[];
  logicalOperator: LogicalOperator;
}

export interface WhereCondition {
  column: DatabaseColumn;
  operator: ComparisonOperator;
  value: any;
  valueType: ValueType;
  logicalOperator?: LogicalOperator;
  isGroup?: boolean;
  groupConditions?: WhereCondition[];
}

export interface GroupByClause {
  columns: DatabaseColumn[];
}

export interface OrderByClause {
  columns: OrderByColumn[];
}

export interface OrderByColumn {
  column: DatabaseColumn;
  direction: SortDirection;
}

export enum JoinType {
  INNER = 'INNER JOIN',
  LEFT = 'LEFT JOIN',
  RIGHT = 'RIGHT JOIN',
  FULL = 'FULL OUTER JOIN',
  CROSS = 'CROSS JOIN'
}

export enum ComparisonOperator {
  EQUALS = '=',
  NOT_EQUALS = '!=',
  GREATER_THAN = '>',
  GREATER_THAN_OR_EQUAL = '>=',
  LESS_THAN = '<',
  LESS_THAN_OR_EQUAL = '<=',
  LIKE = 'LIKE',
  NOT_LIKE = 'NOT LIKE',
  IN = 'IN',
  NOT_IN = 'NOT IN',
  IS_NULL = 'IS NULL',
  IS_NOT_NULL = 'IS NOT NULL',
  BETWEEN = 'BETWEEN'
}

export enum LogicalOperator {
  AND = 'AND',
  OR = 'OR'
}

export enum AggregateFunction {
  COUNT = 'COUNT',
  SUM = 'SUM',
  AVG = 'AVG',
  MIN = 'MIN',
  MAX = 'MAX',
  GROUP_CONCAT = 'GROUP_CONCAT'
}

export enum ValueType {
  LITERAL = 'LITERAL',
  COLUMN = 'COLUMN',
  PARAMETER = 'PARAMETER'
}

export enum SortDirection {
  ASC = 'ASC',
  DESC = 'DESC'
}

export interface SqlBuilderConfig {
  allowSubqueries: boolean;
  allowCustomFunctions: boolean;
  maxJoins: number;
  supportedDatabases: DatabaseType[];
}

export enum DatabaseType {
  MYSQL = 'MySQL',
  POSTGRESQL = 'PostgreSQL',
  SQLSERVER = 'SQL Server',
  SQLITE = 'SQLite'
}

export interface QueryExecutionResult {
  success: boolean;
  data: any[];
  columns: string[];
  rowCount: number;
  executionTime: number;
  error?: string;
}

export interface SqlQuery {
  id: string;
  name: string;
  description?: string;
  type: QueryType;
  tables: TableReference[];
  fields: FieldSelection[];
  joins: JoinClause[];
  conditions: WhereCondition[];
  groupBy: GroupByClause[];
  having: HavingCondition[];
  orderBy: OrderByClause[];
  limit?: number;
  offset?: number;
  rawSql?: string;
  metadata: QueryMetadata;
}

export enum QueryType {
  SELECT = 'SELECT',
  INSERT = 'INSERT',
  UPDATE = 'UPDATE',
  DELETE = 'DELETE',
  CREATE_TABLE = 'CREATE_TABLE',
  ALTER_TABLE = 'ALTER_TABLE',
  DROP_TABLE = 'DROP_TABLE'
}

export interface TableReference {
  id: string;
  name: string;
  alias?: string;
  schema?: string;
  columns: ColumnInfo[];
  position: { x: number; y: number };
}

export interface ColumnInfo {
  name: string;
  type: string;
  nullable: boolean;
  primaryKey: boolean;
  foreignKey?: ForeignKeyInfo;
  defaultValue?: any;
  description?: string;
}

export interface ForeignKeyInfo {
  referencedTable: string;
  referencedColumn: string;
  onDelete?: 'CASCADE' | 'SET NULL' | 'RESTRICT';
  onUpdate?: 'CASCADE' | 'SET NULL' | 'RESTRICT';
}

export interface FieldSelection {
  id: string;
  tableAlias: string;
  columnName: string;
  alias?: string;
  aggregateFunction?: AggregateFunction;
  expression?: string;
  isCalculated: boolean;
}

export enum AggregateFunction {
  COUNT = 'COUNT',
  SUM = 'SUM',
  AVG = 'AVG',
  MIN = 'MIN',
  MAX = 'MAX',
  GROUP_CONCAT = 'GROUP_CONCAT'
}

export interface JoinClause {
  id: string;
  type: JoinType;
  table: string;
  alias?: string;
  condition: string;
  onConditions: JoinCondition[];
  leftTable?: string;
  leftColumn?: string;
  rightTable?: string;
  rightColumn?: string;
}

export enum JoinType {
  INNER = 'INNER JOIN',
  LEFT = 'LEFT JOIN',
  RIGHT = 'RIGHT JOIN',
  FULL = 'FULL OUTER JOIN',
  CROSS = 'CROSS JOIN'
}

export interface JoinCondition {
  leftColumn: string;
  operator: ComparisonOperator;
  rightColumn: string;
}

export interface WhereCondition {
  id: string;
  field: string;
  operator: ComparisonOperator;
  value: any;
  logicalOperator?: LogicalOperator;
  groupId?: string;
  isNested: boolean;
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

export interface GroupByClause {
  field: string;
  tableAlias: string;
}

export interface HavingCondition {
  id: string;
  aggregateFunction: AggregateFunction;
  field: string;
  operator: ComparisonOperator;
  value: any;
  logicalOperator?: LogicalOperator;
}

export interface OrderByClause {
  field: string;
  direction: 'ASC' | 'DESC';
  tableAlias: string;
}

export interface QueryMetadata {
  createdAt: Date;
  updatedAt: Date;
  createdBy: string;
  executionTime?: number;
  resultCount?: number;
  isValid: boolean;
  validationErrors: string[];
}

export interface SqlCommand {
  name: string;
  description: string;
  syntax: string;
  examples: SqlExample[];
  category: SqlCommandCategory;
  difficulty: 'Beginner' | 'Intermediate' | 'Advanced';
  relatedCommands: string[];
}

export enum SqlCommandCategory {
  DATA_QUERY = 'Data Query',
  DATA_MANIPULATION = 'Data Manipulation',
  DATA_DEFINITION = 'Data Definition',
  DATA_CONTROL = 'Data Control',
  FUNCTIONS = 'Functions',
  OPERATORS = 'Operators',
  JOINS = 'Joins',
  SUBQUERIES = 'Subqueries'
}

export interface SqlExample {
  title: string;
  description: string;
  sql: string;
  expectedResult?: string;
  explanation: string;
}

export interface QueryExecutionResult {
  success: boolean;
  data?: any[];
  columns?: ColumnInfo[];
  rowCount: number;
  executionTime: number;
  error?: string;
  warnings?: string[];
}

export interface DatabaseSchema {
  name: string;
  tables: TableSchema[];
  views: ViewSchema[];
  procedures: ProcedureSchema[];
  functions: FunctionSchema[];
}

export interface TableSchema {
  name: string;
  schema: string;
  columns: ColumnInfo[];
  indexes: IndexInfo[];
  constraints: ConstraintInfo[];
  rowCount?: number;
}

export interface ViewSchema {
  name: string;
  schema: string;
  definition: string;
  columns: ColumnInfo[];
}

export interface ProcedureSchema {
  name: string;
  schema: string;
  parameters: ParameterInfo[];
  returnType?: string;
}

export interface FunctionSchema {
  name: string;
  schema: string;
  parameters: ParameterInfo[];
  returnType: string;
}

export interface IndexInfo {
  name: string;
  columns: string[];
  isUnique: boolean;
  isPrimary: boolean;
}

export interface ConstraintInfo {
  name: string;
  type: 'PRIMARY KEY' | 'FOREIGN KEY' | 'UNIQUE' | 'CHECK';
  columns: string[];
  referencedTable?: string;
  referencedColumns?: string[];
}

export interface ParameterInfo {
  name: string;
  type: string;
  direction: 'IN' | 'OUT' | 'INOUT';
  defaultValue?: any;
}

export interface QueryBuilderState {
  currentQuery: SqlQuery;
  selectedTables: TableReference[];
  availableTables: TableSchema[];
  queryHistory: SqlQuery[];
  executionResults: QueryExecutionResult[];
  isExecuting: boolean;
  showSqlPreview: boolean;
  showTutorial: boolean;
}

// SQL Builder JavaScript
class SqlBuilder {
    constructor() {
        this.tables = [];
        this.query = {
            select: [],
            from: null,
            where: [],
            orderBy: []
        };
        this.draggedItem = null;
        this.draggedType = null;
        
        this.init();
    }

    init() {
        this.loadTables();
        this.setupEventListeners();
        this.setupDragAndDrop();
    }

    async loadTables() {
        try {
            const response = await fetch('/api/SqlBuilder/tables');
            if (response.ok) {
                this.tables = await response.json();
                this.renderTables();
            } else {
                console.error('Erro ao carregar tabelas:', response.statusText);
                this.showError('Erro ao carregar tabelas do banco de dados');
            }
        } catch (error) {
            console.error('Erro ao carregar tabelas:', error);
            this.showError('Erro de conexão com o servidor');
        }
    }

    renderTables() {
        const tablesList = document.getElementById('tablesList');
        if (!tablesList) return;

        tablesList.innerHTML = '';

        this.tables.forEach(table => {
            const tableElement = this.createTableElement(table);
            tablesList.appendChild(tableElement);
        });
    }

    createTableElement(table) {
        const tableDiv = document.createElement('div');
        tableDiv.className = 'table-item';
        tableDiv.innerHTML = `
            <div class="table-header" onclick="sqlBuilder.toggleTable('${table.name}')">
                <div class="table-info">
                    <i class="bi bi-chevron-right" id="chevron-${table.name}"></i>
                    <i class="bi bi-table text-primary ms-1"></i>
                    <span class="table-name">${table.name}</span>
                </div>
                <div class="table-actions">
                    <span class="badge bg-light text-dark">${table.columns?.length || 0}</span>
                </div>
            </div>
            <div class="columns-list" id="columns-${table.name}">
                <!-- Colunas serão carregadas aqui -->
            </div>
        `;

        // Tornar a tabela arrastável
        tableDiv.draggable = true;
        tableDiv.addEventListener('dragstart', (e) => {
            this.onDragStart(e, table, 'table');
        });

        return tableDiv;
    }

    async toggleTable(tableName) {
        const chevron = document.getElementById(`chevron-${tableName}`);
        const columnsList = document.getElementById(`columns-${tableName}`);
        
        if (!chevron || !columnsList) return;

        if (columnsList.classList.contains('expanded')) {
            // Fechar
            columnsList.classList.remove('expanded');
            chevron.className = 'bi bi-chevron-right';
        } else {
            // Abrir
            columnsList.classList.add('expanded');
            chevron.className = 'bi bi-chevron-down';
            
            // Carregar colunas se ainda não foram carregadas
            if (columnsList.children.length === 0) {
                await this.loadTableColumns(tableName);
            }
        }
    }

    async loadTableColumns(tableName) {
        try {
            const response = await fetch(`/api/SqlBuilder/tables/${tableName}/columns`);
            if (response.ok) {
                const columns = await response.json();
                this.renderColumns(tableName, columns);
            } else {
                console.error('Erro ao carregar colunas:', response.statusText);
            }
        } catch (error) {
            console.error('Erro ao carregar colunas:', error);
        }
    }

    renderColumns(tableName, columns) {
        const columnsList = document.getElementById(`columns-${tableName}`);
        if (!columnsList) return;

        columnsList.innerHTML = '';

        columns.forEach(column => {
            const columnElement = this.createColumnElement(column);
            columnsList.appendChild(columnElement);
        });
    }

    createColumnElement(column) {
        const columnDiv = document.createElement('div');
        columnDiv.className = 'column-item draggable-item';
        columnDiv.draggable = true;
        
        const icon = this.getColumnIcon(column);
        const badges = this.getColumnBadges(column);
        
        columnDiv.innerHTML = `
            <div class="column-info">
                <i class="bi ${icon}"></i>
                <span class="column-name">${column.name}</span>
                <span class="column-type">${column.type}</span>
            </div>
            <div class="column-badges">
                ${badges}
            </div>
        `;

        columnDiv.addEventListener('dragstart', (e) => {
            this.onDragStart(e, column, 'column');
        });

        return columnDiv;
    }

    getColumnIcon(column) {
        if (column.isPrimaryKey) return 'bi-key-fill text-warning';
        if (column.isForeignKey) return 'bi-link-45deg text-info';
        
        const type = column.type.toLowerCase();
        if (type.includes('int')) return 'bi-123 text-primary';
        if (type.includes('varchar') || type.includes('text')) return 'bi-fonts text-success';
        if (type.includes('date') || type.includes('time')) return 'bi-calendar-date text-secondary';
        if (type.includes('decimal') || type.includes('float')) return 'bi-calculator text-info';
        if (type.includes('bit') || type.includes('boolean')) return 'bi-toggle-on text-warning';
        
        return 'bi-circle text-muted';
    }

    getColumnBadges(column) {
        let badges = '';
        if (column.isPrimaryKey) badges += '<span class="badge-pk">PK</span>';
        if (column.isForeignKey) badges += '<span class="badge-fk">FK</span>';
        return badges;
    }

    setupEventListeners() {
        // Busca de tabelas
        const tableSearch = document.getElementById('tableSearch');
        if (tableSearch) {
            tableSearch.addEventListener('input', (e) => {
                this.filterTables(e.target.value);
            });
        }
    }

    setupDragAndDrop() {
        // Configurar drop zones
        const dropZones = document.querySelectorAll('.drop-zone');
        dropZones.forEach(zone => {
            zone.addEventListener('dragover', this.onDragOver.bind(this));
            zone.addEventListener('dragenter', this.onDragEnter.bind(this));
            zone.addEventListener('dragleave', this.onDragLeave.bind(this));
            zone.addEventListener('drop', this.onDrop.bind(this));
        });
    }

    onDragStart(event, item, type) {
        this.draggedItem = item;
        this.draggedType = type;
        
        if (event.dataTransfer) {
            event.dataTransfer.effectAllowed = 'copy';
            event.dataTransfer.setData('text/plain', JSON.stringify({ item, type }));
        }

        // Adicionar classe visual
        event.target.classList.add('dragging');
    }

    onDragOver(event) {
        event.preventDefault();
        if (event.dataTransfer) {
            event.dataTransfer.dropEffect = 'copy';
        }
    }

    onDragEnter(event) {
        event.preventDefault();
        event.currentTarget.classList.add('drag-over');
    }

    onDragLeave(event) {
        event.currentTarget.classList.remove('drag-over');
    }

    onDrop(event) {
        event.preventDefault();
        event.currentTarget.classList.remove('drag-over');

        if (!this.draggedItem || !this.draggedType) return;

        const dropType = event.currentTarget.getAttribute('data-type');
        this.handleDrop(this.draggedItem, this.draggedType, dropType);

        // Limpar estado do drag
        this.draggedItem = null;
        this.draggedType = null;
        
        // Remover classes visuais
        document.querySelectorAll('.dragging').forEach(el => {
            el.classList.remove('dragging');
        });
    }

    handleDrop(item, itemType, dropType) {
        switch (dropType) {
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
            case 'order':
                if (itemType === 'column') {
                    this.addOrderByColumn(item);
                }
                break;
        }
    }

    addSelectColumn(column) {
        // Verificar se já existe
        const exists = this.query.select.some(item => 
            item.table === column.table && item.name === column.name
        );
        
        if (!exists) {
            this.query.select.push({
                table: column.table,
                name: column.name,
                type: column.type,
                alias: ''
            });
            this.renderSelectItems();
            this.generateSQL();
        }
    }

    setFromTable(table) {
        this.query.from = {
            name: table.name,
            alias: ''
        };
        this.renderFromItems();
        this.generateSQL();
    }

    addWhereCondition(column) {
        this.query.where.push({
            table: column.table,
            column: column.name,
            operator: '=',
            value: '',
            logicalOperator: 'AND'
        });
        this.renderWhereItems();
        this.generateSQL();
    }

    addOrderByColumn(column) {
        // Verificar se já existe
        const exists = this.query.orderBy.some(item => 
            item.table === column.table && item.column === column.name
        );
        
        if (!exists) {
            this.query.orderBy.push({
                table: column.table,
                column: column.name,
                direction: 'ASC'
            });
            this.renderOrderByItems();
            this.generateSQL();
        }
    }

    renderSelectItems() {
        const container = document.getElementById('selectItems');
        if (!container) return;

        container.innerHTML = '';
        
        this.query.select.forEach((item, index) => {
            const itemElement = this.createSelectItemElement(item, index);
            container.appendChild(itemElement);
        });
    }

    createSelectItemElement(item, index) {
        const div = document.createElement('div');
        div.className = 'selected-item';
        div.innerHTML = `
            <div class="selected-item-content">
                <span class="selected-item-text">${item.table}.${item.name}</span>
                <input type="text" class="form-control form-control-sm ms-2" 
                       placeholder="Alias" value="${item.alias}" 
                       onchange="sqlBuilder.updateSelectAlias(${index}, this.value)"
                       style="width: 100px;">
            </div>
            <div class="selected-item-actions">
                <button class="btn btn-sm btn-outline-danger" onclick="sqlBuilder.removeSelectItem(${index})">
                    <i class="bi bi-x"></i>
                </button>
            </div>
        `;
        return div;
    }

    renderFromItems() {
        const container = document.getElementById('fromItems');
        if (!container) return;

        container.innerHTML = '';
        
        if (this.query.from) {
            const itemElement = this.createFromItemElement(this.query.from);
            container.appendChild(itemElement);
        }
    }

    createFromItemElement(item) {
        const div = document.createElement('div');
        div.className = 'selected-item';
        div.innerHTML = `
            <div class="selected-item-content">
                <span class="selected-item-text">${item.name}</span>
                <input type="text" class="form-control form-control-sm ms-2" 
                       placeholder="Alias" value="${item.alias}" 
                       onchange="sqlBuilder.updateFromAlias(this.value)"
                       style="width: 100px;">
            </div>
            <div class="selected-item-actions">
                <button class="btn btn-sm btn-outline-danger" onclick="sqlBuilder.removeFromItem()">
                    <i class="bi bi-x"></i>
                </button>
            </div>
        `;
        return div;
    }

    renderWhereItems() {
        const container = document.getElementById('whereItems');
        if (!container) return;

        container.innerHTML = '';
        
        this.query.where.forEach((item, index) => {
            const itemElement = this.createWhereItemElement(item, index);
            container.appendChild(itemElement);
        });
    }

    createWhereItemElement(item, index) {
        const div = document.createElement('div');
        div.className = 'selected-item';
        div.innerHTML = `
            <div class="selected-item-content">
                <span class="selected-item-text">${item.table}.${item.column}</span>
                <select class="form-select form-select-sm ms-2" 
                        onchange="sqlBuilder.updateWhereOperator(${index}, this.value)"
                        style="width: 80px;">
                    <option value="=" ${item.operator === '=' ? 'selected' : ''}>=</option>
                    <option value="!=" ${item.operator === '!=' ? 'selected' : ''}>!=</option>
                    <option value=">" ${item.operator === '>' ? 'selected' : ''}>&gt;</option>
                    <option value="<" ${item.operator === '<' ? 'selected' : ''}>&lt;</option>
                    <option value="LIKE" ${item.operator === 'LIKE' ? 'selected' : ''}>LIKE</option>
                </select>
                <input type="text" class="form-control form-control-sm ms-2" 
                       placeholder="Valor" value="${item.value}" 
                       onchange="sqlBuilder.updateWhereValue(${index}, this.value)"
                       style="width: 100px;">
            </div>
            <div class="selected-item-actions">
                <button class="btn btn-sm btn-outline-danger" onclick="sqlBuilder.removeWhereItem(${index})">
                    <i class="bi bi-x"></i>
                </button>
            </div>
        `;
        return div;
    }

    renderOrderByItems() {
        const container = document.getElementById('orderItems');
        if (!container) return;

        container.innerHTML = '';
        
        this.query.orderBy.forEach((item, index) => {
            const itemElement = this.createOrderByItemElement(item, index);
            container.appendChild(itemElement);
        });
    }

    createOrderByItemElement(item, index) {
        const div = document.createElement('div');
        div.className = 'selected-item';
        div.innerHTML = `
            <div class="selected-item-content">
                <span class="selected-item-text">${item.table}.${item.column}</span>
                <select class="form-select form-select-sm ms-2" 
                        onchange="sqlBuilder.updateOrderDirection(${index}, this.value)"
                        style="width: 80px;">
                    <option value="ASC" ${item.direction === 'ASC' ? 'selected' : ''}>ASC</option>
                    <option value="DESC" ${item.direction === 'DESC' ? 'selected' : ''}>DESC</option>
                </select>
            </div>
            <div class="selected-item-actions">
                <button class="btn btn-sm btn-outline-danger" onclick="sqlBuilder.removeOrderByItem(${index})">
                    <i class="bi bi-x"></i>
                </button>
            </div>
        `;
        return div;
    }

    // Métodos de atualização
    updateSelectAlias(index, alias) {
        if (this.query.select[index]) {
            this.query.select[index].alias = alias;
            this.generateSQL();
        }
    }

    updateFromAlias(alias) {
        if (this.query.from) {
            this.query.from.alias = alias;
            this.generateSQL();
        }
    }

    updateWhereOperator(index, operator) {
        if (this.query.where[index]) {
            this.query.where[index].operator = operator;
            this.generateSQL();
        }
    }

    updateWhereValue(index, value) {
        if (this.query.where[index]) {
            this.query.where[index].value = value;
            this.generateSQL();
        }
    }

    updateOrderDirection(index, direction) {
        if (this.query.orderBy[index]) {
            this.query.orderBy[index].direction = direction;
            this.generateSQL();
        }
    }

    // Métodos de remoção
    removeSelectItem(index) {
        this.query.select.splice(index, 1);
        this.renderSelectItems();
        this.generateSQL();
    }

    removeFromItem() {
        this.query.from = null;
        this.renderFromItems();
        this.generateSQL();
    }

    removeWhereItem(index) {
        this.query.where.splice(index, 1);
        this.renderWhereItems();
        this.generateSQL();
    }

    removeOrderByItem(index) {
        this.query.orderBy.splice(index, 1);
        this.renderOrderByItems();
        this.generateSQL();
    }

    generateSQL() {
        let sql = '';

        // SELECT
        if (this.query.select.length > 0) {
            const selectColumns = this.query.select.map(col => {
                let columnStr = `${col.table}.${col.name}`;
                if (col.alias) {
                    columnStr += ` AS ${col.alias}`;
                }
                return columnStr;
            });
            sql += `SELECT ${selectColumns.join(',\n       ')}\n`;
        } else {
            sql += 'SELECT *\n';
        }

        // FROM
        if (this.query.from) {
            sql += `FROM ${this.query.from.name}`;
            if (this.query.from.alias) {
                sql += ` AS ${this.query.from.alias}`;
            }
            sql += '\n';
        }

        // WHERE
        if (this.query.where.length > 0) {
            const whereConditions = this.query.where.map(condition => {
                let condStr = `${condition.table}.${condition.column} ${condition.operator}`;
                if (condition.value) {
                    condStr += ` '${condition.value}'`;
                }
                return condStr;
            });
            sql += `WHERE ${whereConditions.join(' AND ')}\n`;
        }

        // ORDER BY
        if (this.query.orderBy.length > 0) {
            const orderColumns = this.query.orderBy.map(col => 
                `${col.table}.${col.column} ${col.direction}`
            );
            sql += `ORDER BY ${orderColumns.join(', ')}\n`;
        }

        // Atualizar preview
        const sqlPreview = document.getElementById('sqlPreview');
        if (sqlPreview) {
            sqlPreview.textContent = sql.trim() || '-- Sua query aparecerá aqui';
        }

        return sql.trim();
    }

    filterTables(searchTerm) {
        const tableItems = document.querySelectorAll('.table-item');
        const term = searchTerm.toLowerCase();

        tableItems.forEach(item => {
            const tableName = item.querySelector('.table-name').textContent.toLowerCase();
            const shouldShow = tableName.includes(term);
            item.style.display = shouldShow ? 'block' : 'none';
        });
    }

    showError(message) {
        // Implementar notificação de erro
        console.error(message);
    }
}

// Funções globais
function copySQL() {
    const sqlPreview = document.getElementById('sqlPreview');
    if (sqlPreview && sqlPreview.textContent.trim()) {
        navigator.clipboard.writeText(sqlPreview.textContent).then(() => {
            // Mostrar feedback visual
            const btn = event.target.closest('button');
            const originalHTML = btn.innerHTML;
            btn.innerHTML = '<i class="bi bi-check"></i>';
            setTimeout(() => {
                btn.innerHTML = originalHTML;
            }, 1000);
        });
    }
}

async function executeQuery() {
    const sqlPreview = document.getElementById('sqlPreview');
    const sql = sqlPreview?.textContent?.trim();
    
    if (!sql || sql === '-- Sua query aparecerá aqui') {
        alert('Construa uma query primeiro');
        return;
    }

    try {
        const response = await fetch('/api/SqlBuilder/execute', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ sql })
        });

        const result = await response.json();
        displayQueryResults(result);
    } catch (error) {
        console.error('Erro ao executar query:', error);
        alert('Erro ao executar query');
    }
}

function displayQueryResults(result) {
    const resultsContainer = document.getElementById('queryResults');
    const resultsContent = resultsContainer?.querySelector('.results-content');
    
    if (!resultsContainer || !resultsContent) return;

    resultsContainer.style.display = 'block';

    if (!result.success) {
        resultsContent.innerHTML = `
            <div class="error-message">
                <strong>Erro:</strong> ${result.error || 'Erro desconhecido'}
            </div>
        `;
        return;
    }

    if (result.data.length === 0) {
        resultsContent.innerHTML = `
            <div class="text-center text-muted p-3">
                <i class="bi bi-info-circle"></i>
                Nenhum resultado encontrado
            </div>
        `;
        return;
    }

    // Criar tabela de resultados
    let tableHTML = '<table class="results-table table table-sm table-hover">';
    
    // Cabeçalho
    tableHTML += '<thead><tr>';
    result.columns.forEach(column => {
        tableHTML += `<th>${column}</th>`;
    });
    tableHTML += '</tr></thead>';
    
    // Dados
    tableHTML += '<tbody>';
    result.data.forEach(row => {
        tableHTML += '<tr>';
        result.columns.forEach(column => {
            const value = row[column];
            tableHTML += `<td>${value !== null && value !== undefined ? value : ''}</td>`;
        });
        tableHTML += '</tr>';
    });
    tableHTML += '</tbody></table>';
    
    tableHTML += `
        <div class="results-info p-2 text-muted">
            <small>
                ${result.rowCount} registro(s) • 
                Executado em ${result.executionTime.toFixed(2)}ms
            </small>
        </div>
    `;

    resultsContent.innerHTML = tableHTML;
}

// Inicializar quando o DOM estiver pronto
document.addEventListener('DOMContentLoaded', function() {
    window.sqlBuilder = new SqlBuilder();
});

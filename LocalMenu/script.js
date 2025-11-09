// Script para o menu local sem banco de dados

// Estado da aplicação
let appState = {
    currentSection: 'dashboard',
    cart: [],
    orders: [
        { id: '001', product: 'Produto A', quantity: 100, status: 'Em Andamento', priority: 'Alta', startDate: '07/11/2025' },
        { id: '002', product: 'Produto B', quantity: 50, status: 'Planejada', priority: 'Média', startDate: '08/11/2025' },
        { id: '003', product: 'Produto C', quantity: 75, status: 'Concluída', priority: 'Baixa', startDate: '05/11/2025' }
    ],
    products: [
        { id: 1, name: 'Produto 1', price: 25.90, stock: 100 },
        { id: 2, name: 'Produto 2', price: 15.50, stock: 50 },
        { id: 3, name: 'Produto 3', price: 35.00, stock: 25 },
        { id: 4, name: 'Produto 4', price: 45.00, stock: 80 },
        { id: 5, name: 'Produto 5', price: 12.90, stock: 200 }
    ]
};

// Função para mostrar seções
function showSection(sectionId) {
    // Esconder todas as seções
    const sections = document.querySelectorAll('.content-section');
    sections.forEach(section => {
        section.style.display = 'none';
    });

    // Mostrar a seção selecionada
    const targetSection = document.getElementById(sectionId);
    if (targetSection) {
        targetSection.style.display = 'block';
        appState.currentSection = sectionId;
        
        // Atualizar navbar ativa
        updateActiveNavItem(sectionId);
        
        // Carregar dados específicos da seção
        loadSectionData(sectionId);
    }
}

// Função para atualizar item ativo na navbar
function updateActiveNavItem(sectionId) {
    const navLinks = document.querySelectorAll('.nav-link');
    navLinks.forEach(link => {
        link.classList.remove('active');
    });
    
    // Lógica para marcar o item correto como ativo baseado na seção
    if (sectionId === 'dashboard') {
        document.querySelector('a[onclick="showSection(\'dashboard\')"]').classList.add('active');
    }
}

// Função para carregar dados específicos da seção
function loadSectionData(sectionId) {
    switch (sectionId) {
        case 'pcp-ordens':
            loadOrdersTable();
            break;
        case 'pdv-vendas':
            loadProductsGrid();
            break;
        case 'dashboard':
            updateDashboardStats();
            break;
    }
}

// Função para carregar tabela de ordens
function loadOrdersTable() {
    const tbody = document.querySelector('#pcp-ordens tbody');
    if (!tbody) return;

    tbody.innerHTML = '';
    appState.orders.forEach(order => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${order.id}</td>
            <td>${order.product}</td>
            <td>${order.quantity}</td>
            <td><span class="badge ${getStatusBadgeClass(order.status)}">${order.status}</span></td>
            <td><span class="badge ${getPriorityBadgeClass(order.priority)}">${order.priority}</span></td>
            <td>${order.startDate}</td>
            <td>
                <button class="btn btn-sm btn-outline-primary" onclick="editOrder('${order.id}')">Editar</button>
                <button class="btn btn-sm btn-outline-success" onclick="startOrder('${order.id}')">
                    ${order.status === 'Planejada' ? 'Iniciar' : 'Apontar'}
                </button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Função para obter classe CSS do badge de status
function getStatusBadgeClass(status) {
    const statusClasses = {
        'Planejada': 'bg-primary',
        'Em Andamento': 'bg-warning',
        'Concluída': 'bg-success',
        'Pausada': 'bg-danger'
    };
    return statusClasses[status] || 'bg-secondary';
}

// Função para obter classe CSS do badge de prioridade
function getPriorityBadgeClass(priority) {
    const priorityClasses = {
        'Alta': 'bg-danger',
        'Média': 'bg-warning',
        'Baixa': 'bg-success'
    };
    return priorityClasses[priority] || 'bg-secondary';
}

// Função para carregar grid de produtos
function loadProductsGrid() {
    // Esta função pode ser expandida para carregar produtos dinamicamente
    console.log('Carregando produtos...');
}

// Função para atualizar estatísticas do dashboard
function updateDashboardStats() {
    // Simular dados em tempo real
    const stats = {
        vendasHoje: Math.floor(Math.random() * 200) + 100,
        ordensAtivas: appState.orders.filter(o => o.status === 'Em Andamento').length,
        faturamento: (Math.random() * 20000 + 10000).toFixed(2),
        produtos: appState.products.length
    };

    // Atualizar elementos do dashboard se existirem
    const elements = {
        vendas: document.querySelector('.card.bg-primary h4'),
        ordens: document.querySelector('.card.bg-success h4'),
        faturamento: document.querySelector('.card.bg-warning h4'),
        produtos: document.querySelector('.card.bg-info h4')
    };

    if (elements.vendas) elements.vendas.textContent = stats.vendasHoje;
    if (elements.ordens) elements.ordens.textContent = stats.ordensAtivas;
    if (elements.faturamento) elements.faturamento.textContent = `R$ ${stats.faturamento}`;
    if (elements.produtos) elements.produtos.textContent = stats.produtos;
}

// Funções do PDV - Carrinho de Compras
function addToCart(productName, price) {
    const existingItem = appState.cart.find(item => item.name === productName);
    
    if (existingItem) {
        existingItem.quantity += 1;
        existingItem.total = existingItem.quantity * existingItem.price;
    } else {
        appState.cart.push({
            name: productName,
            price: price,
            quantity: 1,
            total: price
        });
    }
    
    updateCartDisplay();
    showNotification(`${productName} adicionado ao carrinho!`, 'success');
}

function removeFromCart(productName) {
    appState.cart = appState.cart.filter(item => item.name !== productName);
    updateCartDisplay();
    showNotification(`${productName} removido do carrinho!`, 'warning');
}

function updateCartDisplay() {
    const cartItems = document.getElementById('cart-items');
    const cartTotal = document.getElementById('cart-total');
    
    if (!cartItems || !cartTotal) return;

    if (appState.cart.length === 0) {
        cartItems.innerHTML = '<p class="text-muted">Carrinho vazio</p>';
        cartTotal.textContent = 'R$ 0,00';
        return;
    }

    let html = '';
    let total = 0;

    appState.cart.forEach(item => {
        total += item.total;
        html += `
            <div class="cart-item d-flex justify-content-between align-items-center">
                <div>
                    <strong>${item.name}</strong><br>
                    <small>Qtd: ${item.quantity} x R$ ${item.price.toFixed(2)}</small>
                </div>
                <div>
                    <span class="text-success">R$ ${item.total.toFixed(2)}</span>
                    <button class="btn btn-sm btn-outline-danger ms-2" onclick="removeFromCart('${item.name}')">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        `;
    });

    cartItems.innerHTML = html;
    cartTotal.textContent = `R$ ${total.toFixed(2)}`;
}

function finalizeSale() {
    if (appState.cart.length === 0) {
        showNotification('Carrinho vazio!', 'warning');
        return;
    }

    const total = appState.cart.reduce((sum, item) => sum + item.total, 0);
    
    // Simular finalização da venda
    showNotification(`Venda finalizada! Total: R$ ${total.toFixed(2)}`, 'success');
    
    // Limpar carrinho
    appState.cart = [];
    updateCartDisplay();
    
    // Atualizar estatísticas
    updateDashboardStats();
}

// Funções do PCP
function editOrder(orderId) {
    showNotification(`Editando ordem ${orderId}`, 'info');
}

function startOrder(orderId) {
    const order = appState.orders.find(o => o.id === orderId);
    if (order) {
        if (order.status === 'Planejada') {
            order.status = 'Em Andamento';
            showNotification(`Ordem ${orderId} iniciada!`, 'success');
        } else {
            showNotification(`Apontamento registrado para ordem ${orderId}`, 'success');
        }
        loadOrdersTable();
    }
}

// Função para mostrar modal
function showModal(modalId) {
    const modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
}

// Função para mostrar notificações
function showNotification(message, type = 'info') {
    // Criar elemento de notificação
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    document.body.appendChild(notification);

    // Remover automaticamente após 3 segundos
    setTimeout(() => {
        if (notification.parentNode) {
            notification.parentNode.removeChild(notification);
        }
    }, 3000);
}

// Função para salvar dados no localStorage
function saveData() {
    localStorage.setItem('localMenuData', JSON.stringify(appState));
}

// Função para carregar dados do localStorage
function loadData() {
    const savedData = localStorage.getItem('localMenuData');
    if (savedData) {
        appState = { ...appState, ...JSON.parse(savedData) };
    }
}

// Função para exportar dados
function exportData() {
    const dataStr = JSON.stringify(appState, null, 2);
    const dataBlob = new Blob([dataStr], { type: 'application/json' });
    const url = URL.createObjectURL(dataBlob);
    
    const link = document.createElement('a');
    link.href = url;
    link.download = 'menu-local-data.json';
    link.click();
    
    URL.revokeObjectURL(url);
    showNotification('Dados exportados com sucesso!', 'success');
}

// Função para importar dados
function importData(event) {
    const file = event.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = function(e) {
        try {
            const importedData = JSON.parse(e.target.result);
            appState = { ...appState, ...importedData };
            saveData();
            showNotification('Dados importados com sucesso!', 'success');
            loadSectionData(appState.currentSection);
        } catch (error) {
            showNotification('Erro ao importar dados!', 'danger');
        }
    };
    reader.readAsText(file);
}

// Inicialização da aplicação
document.addEventListener('DOMContentLoaded', function() {
    // Carregar dados salvos
    loadData();
    
    // Mostrar seção inicial
    showSection('dashboard');
    
    // Salvar dados periodicamente
    setInterval(saveData, 30000); // A cada 30 segundos
    
    // Atualizar dashboard periodicamente
    setInterval(() => {
        if (appState.currentSection === 'dashboard') {
            updateDashboardStats();
        }
    }, 60000); // A cada minuto
    
    console.log('Menu Local inicializado com sucesso!');
});

// Salvar dados antes de fechar a página
window.addEventListener('beforeunload', saveData);

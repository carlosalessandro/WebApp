# Menu Local - Sistema de Gestão

## Descrição

Esta é uma versão **local e simplificada** do sistema de gestão, criada para funcionar **sem dependência de banco de dados**. O sistema oferece uma interface completa para gerenciar PCP (Planejamento e Controle de Produção) e PDV (Ponto de Venda) através de um menu intuitivo.

## Características

### ✅ **Sem Banco de Dados**
- Todos os dados são armazenados localmente no navegador (localStorage)
- Não requer instalação de servidor ou banco de dados
- Funciona completamente offline

### 🎯 **Módulos Incluídos**
- **Dashboard Principal** - Visão geral do sistema
- **PCP (Planejamento e Controle de Produção)**
  - Dashboard PCP
  - Ordens de Produção
  - Recursos
  - Apontamentos
- **PDV (Ponto de Venda)**
  - Interface de vendas
  - Gerenciamento de produtos
  - NFC-e
- **Relatórios**
  - Vendas
  - Produção
  - Estoque
- **Configurações**
  - Usuários
  - Permissões
  - Sistema

### 🚀 **Funcionalidades**

#### **PCP**
- ✅ Visualização de ordens de produção
- ✅ Criação de novas ordens
- ✅ Controle de status (Planejada, Em Andamento, Concluída, Pausada)
- ✅ Gerenciamento de prioridades
- ✅ Dashboard com estatísticas

#### **PDV**
- ✅ Interface de vendas intuitiva
- ✅ Carrinho de compras funcional
- ✅ Adição/remoção de produtos
- ✅ Cálculo automático de totais
- ✅ Finalização de vendas

#### **Geral**
- ✅ Interface responsiva (Bootstrap 5)
- ✅ Ícones modernos (Bootstrap Icons)
- ✅ Notificações em tempo real
- ✅ Salvamento automático de dados
- ✅ Exportação/importação de dados
- ✅ Animações suaves

## Como Usar

### **Instalação**
1. Copie todos os arquivos para uma pasta local
2. Abra o arquivo `index.html` em qualquer navegador moderno
3. Pronto! O sistema está funcionando

### **Navegação**
- Use o menu superior para navegar entre os módulos
- Clique nos itens do menu dropdown para acessar funcionalidades específicas
- Todas as alterações são salvas automaticamente

### **Dados**
- Os dados são salvos automaticamente no navegador
- Use as funções de exportar/importar para backup
- Os dados persistem entre sessões

## Estrutura de Arquivos

```
LocalMenu/
├── index.html          # Página principal
├── styles.css          # Estilos personalizados
├── script.js           # Lógica da aplicação
└── README.md          # Este arquivo
```

## Tecnologias Utilizadas

- **HTML5** - Estrutura da aplicação
- **CSS3** - Estilos e animações
- **JavaScript (ES6+)** - Lógica da aplicação
- **Bootstrap 5** - Framework CSS responsivo
- **Bootstrap Icons** - Ícones modernos
- **LocalStorage** - Armazenamento local de dados

## Vantagens desta Solução

### ✅ **Simplicidade**
- Não requer instalação de servidor
- Não precisa de banco de dados
- Funciona em qualquer computador com navegador

### ✅ **Portabilidade**
- Pode ser executado de um pen drive
- Funciona offline
- Fácil de distribuir

### ✅ **Manutenção**
- Sem dependências externas
- Código simples e limpo
- Fácil de modificar e expandir

### ✅ **Performance**
- Carregamento instantâneo
- Sem latência de rede
- Interface fluida

## Limitações

- Dados limitados ao navegador local
- Não suporta múltiplos usuários simultâneos
- Sem sincronização entre dispositivos
- Capacidade de armazenamento limitada pelo navegador

## Expansões Futuras

- Integração com APIs externas
- Sincronização em nuvem
- Relatórios mais avançados
- Módulos adicionais
- Temas personalizáveis

## Suporte

Este sistema foi criado como uma alternativa local ao sistema principal com banco de dados. É ideal para:

- Demonstrações
- Testes
- Uso offline
- Ambientes sem infraestrutura de servidor
- Prototipagem rápida

---

**Desenvolvido como solução local para o sistema de gestão WebApp**

# Sistema de Temas Personalizados

## 📋 Descrição

Sistema completo de personalização de temas com cores customizáveis para o sistema. Permite criar, editar, ativar e gerenciar múltiplos temas visuais.

## 🎨 Funcionalidades

- ✅ Criar temas personalizados com cores customizadas
- ✅ Editar temas existentes
- ✅ Ativar/desativar temas
- ✅ Preview em tempo real das cores
- ✅ Temas predefinidos prontos para uso
- ✅ Persistência do tema ativo no localStorage
- ✅ 6 temas padrão incluídos

## 🚀 Como Usar

### Acessar a Tela de Configuração

1. No menu lateral, clique em **"Temas"** (ícone de paleta)
2. Ou acesse diretamente: `/configuracoes/temas`

### Criar um Novo Tema

1. Clique no botão **"Novo Tema"**
2. Escolha um tema predefinido ou personalize as cores:
   - **Cor Principal**: Cor primária do sistema
   - **Cor Secundária**: Cor de destaque
   - **Cor Escura**: Variação escura
   - **Cor Clara**: Variação clara
   - **Cor Hover**: Cor ao passar o mouse
   - **Texto Escuro**: Cor do texto principal
   - **Texto Médio**: Cor do texto secundário
   - **Cor de Fundo**: Cor de fundo da aplicação
3. Use o botão **"Visualizar"** para ver o preview
4. Clique em **"Salvar"** para criar o tema

### Ativar um Tema

1. Na lista de temas, clique no botão verde (✓) ao lado do tema desejado
2. O tema será aplicado imediatamente em todo o sistema

### Editar um Tema

1. Clique no botão azul (lápis) ao lado do tema
2. Modifique as cores desejadas
3. Clique em **"Atualizar"**

### Excluir um Tema

1. Clique no botão vermelho (lixeira) ao lado do tema
2. Confirme a exclusão
3. **Nota**: Não é possível excluir o tema ativo

## 🎨 Temas Padrão Incluídos

1. **Verde Louro (Padrão)** - Tema original do sistema
2. **Azul Oceano** - Tons de azul profissional
3. **Roxo Moderno** - Tons de roxo vibrante
4. **Laranja Vibrante** - Tons de laranja energético
5. **Escuro Profissional** - Tema escuro para trabalho noturno
6. **Verde Esmeralda** - Tons de verde natural

## 🔧 Estrutura Técnica

### Backend (C#)

- **Model**: `Models/ThemeConfig.cs`
- **Controller**: `Controllers/ThemeController.cs`
- **DbContext**: Adicionado `DbSet<ThemeConfig>` em `ApplicationDbContext.cs`
- **Migration**: `AddThemeConfig`
- **Seed**: `Scripts/SeedDefaultThemes.cs`

### Frontend (Angular)

- **Component**: `ClientApp/src/app/components/theme-config/`
- **Service**: `ClientApp/src/app/services/theme.service.ts`
- **Route**: `/configuracoes/temas`

### API Endpoints

```
GET    /api/theme              - Lista todos os temas
GET    /api/theme/{id}         - Busca tema por ID
GET    /api/theme/active       - Busca tema ativo
POST   /api/theme              - Cria novo tema
PUT    /api/theme/{id}         - Atualiza tema
POST   /api/theme/{id}/activate - Ativa tema
DELETE /api/theme/{id}         - Exclui tema
```

## 💾 Banco de Dados

### Tabela: ThemeConfigs

| Campo | Tipo | Descrição |
|-------|------|-----------|
| Id | int | Chave primária |
| Name | string(100) | Nome do tema |
| PrimaryColor | string(7) | Cor principal (hex) |
| SecondaryColor | string(7) | Cor secundária (hex) |
| DarkColor | string(7) | Cor escura (hex) |
| LightColor | string(7) | Cor clara (hex) |
| HoverColor | string(7) | Cor hover (hex) |
| TextDark | string(7) | Cor texto escuro (hex) |
| TextMedium | string(7) | Cor texto médio (hex) |
| BackgroundColor | string(7) | Cor de fundo (hex) |
| IsActive | bool | Se o tema está ativo |
| UserId | int? | ID do usuário (opcional) |
| CreatedAt | DateTime | Data de criação |
| UpdatedAt | DateTime? | Data de atualização |

## 🎯 Variáveis CSS Aplicadas

O sistema aplica as seguintes variáveis CSS dinamicamente:

```css
--primary-green: [PrimaryColor]
--primary-lime: [SecondaryColor]
--dark-green: [DarkColor]
--light-green: [LightColor]
--hover-green: [HoverColor]
--text-dark: [TextDark]
--text-medium: [TextMedium]
```

## 📱 Persistência

O tema ativo é salvo no `localStorage` do navegador, garantindo que a preferência do usuário seja mantida entre sessões.

## 🔄 Aplicação Automática

O tema ativo é carregado automaticamente ao iniciar a aplicação através do `AppComponent`.

## 🎨 Dicas de Personalização

1. Use cores contrastantes para melhor legibilidade
2. Teste o tema em diferentes telas antes de ativar
3. Mantenha consistência entre cores relacionadas
4. Use o preview para validar antes de salvar
5. Considere acessibilidade ao escolher cores de texto

## 🐛 Troubleshooting

### Tema não está sendo aplicado
- Verifique se o tema está marcado como ativo
- Limpe o cache do navegador
- Verifique o console do navegador por erros

### Cores não estão mudando
- Certifique-se de que as variáveis CSS estão sendo usadas nos componentes
- Verifique se o ThemeService está sendo injetado no AppComponent

### Erro ao salvar tema
- Verifique se todos os campos obrigatórios estão preenchidos
- Confirme que as cores estão no formato hexadecimal (#RRGGBB)

## 📝 Notas

- Apenas um tema pode estar ativo por vez
- Temas inativos podem ser editados ou excluídos
- O tema ativo não pode ser excluído
- Cores devem estar no formato hexadecimal (#RRGGBB)

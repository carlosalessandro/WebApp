# Guia de Teste - Sistema de Temas

## 🚀 Como Testar

### 1. Iniciar o Sistema

```bash
# Backend (C#)
dotnet run

# Frontend (Angular) - em outro terminal
cd ClientApp
npm start
```

### 2. Acessar a Aplicação

Abra o navegador em: `http://localhost:4200`

### 3. Testar Funcionalidades

#### ✅ Teste 1: Visualizar Temas Padrão
1. Clique em "Temas" no menu lateral
2. Verifique se os 6 temas padrão estão listados
3. Confirme que "Verde Louro (Padrão)" está marcado como ativo

#### ✅ Teste 2: Ativar Tema Diferente
1. Clique no botão verde (✓) ao lado de "Azul Oceano"
2. Observe a mudança imediata das cores no sistema
3. Verifique se o tema foi marcado como ativo na lista

#### ✅ Teste 3: Criar Tema Personalizado
1. Clique em "Novo Tema"
2. Digite um nome: "Meu Tema Teste"
3. Escolha um tema predefinido ou personalize as cores
4. Clique em "Visualizar" para ver o preview
5. Clique em "Salvar"
6. Verifique se o tema aparece na lista

#### ✅ Teste 4: Editar Tema
1. Clique no botão azul (lápis) ao lado do tema criado
2. Modifique algumas cores
3. Clique em "Visualizar" para ver as mudanças
4. Clique em "Atualizar"
5. Verifique se as alterações foram salvas

#### ✅ Teste 5: Preview em Tempo Real
1. Ao criar/editar um tema, altere as cores
2. Clique em "Visualizar"
3. Observe o preview no card à direita
4. Teste diferentes combinações de cores

#### ✅ Teste 6: Persistência do Tema
1. Ative um tema específico
2. Feche o navegador
3. Abra novamente a aplicação
4. Verifique se o tema ativo foi mantido

#### ✅ Teste 7: Excluir Tema
1. Tente excluir o tema ativo (deve falhar)
2. Ative outro tema
3. Exclua o tema anterior
4. Confirme que foi removido da lista

#### ✅ Teste 8: Temas Predefinidos
1. Ao criar novo tema, teste cada tema predefinido:
   - Verde Louro (Padrão)
   - Azul Oceano
   - Roxo Moderno
   - Laranja Vibrante
2. Observe como as cores são aplicadas automaticamente

### 4. Testar API Diretamente

#### Listar Temas
```bash
curl http://localhost:5000/api/theme
```

#### Buscar Tema Ativo
```bash
curl http://localhost:5000/api/theme/active
```

#### Criar Tema
```bash
curl -X POST http://localhost:5000/api/theme \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Tema API",
    "primaryColor": "#ff0000",
    "secondaryColor": "#00ff00",
    "darkColor": "#0000ff",
    "lightColor": "#ffff00",
    "hoverColor": "#ff00ff",
    "textDark": "#000000",
    "textMedium": "#666666",
    "backgroundColor": "#ffffff",
    "isActive": false
  }'
```

#### Ativar Tema
```bash
curl -X POST http://localhost:5000/api/theme/1/activate
```

### 5. Verificar Banco de Dados

```bash
# Abrir banco SQLite
sqlite3 WebApp.db

# Listar temas
SELECT * FROM ThemeConfigs;

# Ver tema ativo
SELECT * FROM ThemeConfigs WHERE IsActive = 1;
```

## 🐛 Problemas Comuns

### Tema não aplica ao iniciar
- Limpe o localStorage: `localStorage.clear()`
- Recarregue a página

### Erro ao criar tema
- Verifique se todos os campos estão preenchidos
- Confirme formato hexadecimal das cores (#RRGGBB)

### API não responde
- Verifique se o backend está rodando
- Confirme a porta (padrão: 5000)
- Verifique CORS no Program.cs

### Cores não mudam
- Abra o DevTools (F12)
- Verifique o console por erros
- Confirme que as variáveis CSS estão sendo aplicadas

## ✅ Checklist de Validação

- [ ] Backend compila sem erros
- [ ] Frontend compila sem erros
- [ ] Temas padrão são inseridos automaticamente
- [ ] Menu "Temas" aparece no sidebar
- [ ] Lista de temas carrega corretamente
- [ ] Criar tema funciona
- [ ] Editar tema funciona
- [ ] Ativar tema funciona e aplica cores
- [ ] Excluir tema funciona (exceto ativo)
- [ ] Preview mostra cores corretamente
- [ ] Tema persiste após reload
- [ ] API endpoints respondem corretamente

## 📊 Resultados Esperados

### Visual
- Cores mudam imediatamente ao ativar tema
- Sidebar reflete as novas cores
- Botões e cards usam as cores do tema
- Preview mostra representação fiel

### Funcional
- CRUD completo de temas
- Apenas um tema ativo por vez
- Tema ativo não pode ser excluído
- Persistência entre sessões

### Performance
- Mudança de tema é instantânea
- Sem flickering ao carregar
- Preview atualiza em tempo real

import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'kanban',
    pathMatch: 'full'
  },
  {
    path: 'kanban',
    loadComponent: () => import('./components/kanban/kanban.component').then(m => m.KanbanComponent)
  },
  {
    path: 'tarefas',
    loadComponent: () => import('./components/tarefa-list/tarefa-list.component').then(m => m.TarefaListComponent)
  },
  {
    path: 'crm',
    loadComponent: () => import('./components/crm/crm-dashboard/crm-dashboard.component').then(m => m.CrmDashboardComponent)
  },
  {
    path: 'erp',
    loadComponent: () => import('./components/erp/erp-dashboard/erp-dashboard.component').then(m => m.ErpDashboardComponent)
  },
  {
    path: 'privacidade',
    loadComponent: () => import('./components/privacy/privacy.component').then(m => m.PrivacyComponent)
  },
  {
    path: 'configuracoes/temas',
    loadComponent: () => import('./components/theme-config/theme-config.component').then(m => m.ThemeConfigComponent)
  },
  {
    path: '**',
    redirectTo: 'kanban'
  }
];

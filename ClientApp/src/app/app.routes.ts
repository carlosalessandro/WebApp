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
    path: '**',
    redirectTo: 'kanban'
  }
];

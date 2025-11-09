import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="sidebar d-flex flex-column">
      <div class="navbar-brand text-center">
        <i class="bi bi-kanban"></i> Sistema Kanban
      </div>
      
      <ul class="nav flex-column px-2">
        <li class="nav-item">
          <a class="nav-link" routerLink="/kanban" routerLinkActive="active">
            <i class="bi bi-kanban"></i> Kanban
          </a>
        </li>
        <li class="nav-item">
          <a class="nav-link" routerLink="/tarefas" routerLinkActive="active">
            <i class="bi bi-list-check"></i> Lista de Tarefas
          </a>
        </li>
      </ul>
      
      <div class="mt-auto p-3">
        <button class="btn btn-outline-danger btn-sm w-100">
          <i class="bi bi-box-arrow-right"></i> Sair
        </button>
      </div>
    </nav>
  `,
  styles: [`
    .sidebar {
      height: 100vh;
      position: sticky;
      top: 0;
    }
    
    .nav-item {
      margin: 3px 0;
    }
    
    .nav-link i {
      margin-right: 8px;
    }
  `]
})
export class SidebarComponent {}

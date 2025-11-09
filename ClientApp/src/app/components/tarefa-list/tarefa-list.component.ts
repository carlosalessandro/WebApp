import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { TarefaService } from '../../services/tarefa.service';
import { Tarefa, StatusTarefa, PrioridadeTarefa } from '../../models/tarefa.model';

@Component({
  selector: 'app-tarefa-list',
  standalone: true,
  imports: [CommonModule, RouterModule, HttpClientModule],
  template: `
    <div class="container-fluid">
      <div class="row mb-4">
        <div class="col-12">
          <div class="d-flex justify-content-between align-items-center">
            <h2><i class="bi bi-list-check"></i> Lista de Tarefas</h2>
            <div>
              <button class="btn btn-primary me-2">
                <i class="bi bi-plus-circle"></i> Nova Tarefa
              </button>
              <a routerLink="/kanban" class="btn btn-outline-secondary">
                <i class="bi bi-kanban"></i> Kanban
              </a>
            </div>
          </div>
        </div>
      </div>

      <div class="row">
        <div class="col-12">
          <div class="card">
            <div class="card-body">
              <div class="table-responsive">
                <table class="table table-hover">
                  <thead>
                    <tr>
                      <th>Título</th>
                      <th>Status</th>
                      <th>Prioridade</th>
                      <th>Responsável</th>
                      <th>Vencimento</th>
                      <th>Ações</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr *ngFor="let tarefa of tarefas">
                      <td>
                        <strong>{{tarefa.titulo}}</strong>
                        <br>
                        <small class="text-muted" *ngIf="tarefa.descricao">
                          {{tarefa.descricao.length > 50 ? (tarefa.descricao.substring(0, 50) + '...') : tarefa.descricao}}
                        </small>
                      </td>
                      <td>
                        <span class="badge" [ngClass]="getStatusBadgeClass(tarefa.status)">
                          {{getStatusName(tarefa.status)}}
                        </span>
                      </td>
                      <td>
                        <span class="badge priority-badge {{getPrioridadeClass(tarefa.prioridade)}}">
                          {{getPrioridadeNome(tarefa.prioridade)}}
                        </span>
                      </td>
                      <td>
                        <span *ngIf="tarefa.responsavel">
                          <i class="bi bi-person"></i> {{tarefa.responsavel}}
                        </span>
                        <span *ngIf="!tarefa.responsavel" class="text-muted">-</span>
                      </td>
                      <td>
                        <span *ngIf="tarefa.dataVencimento"
                              [class.text-danger]="isAtrasada(tarefa)"
                              [class.text-muted]="!isAtrasada(tarefa)">
                          {{formatDate(tarefa.dataVencimento)}}
                        </span>
                        <span *ngIf="!tarefa.dataVencimento" class="text-muted">-</span>
                      </td>
                      <td>
                        <div class="btn-group btn-group-sm">
                          <button class="btn btn-outline-primary" title="Ver">
                            <i class="bi bi-eye"></i>
                          </button>
                          <button class="btn btn-outline-secondary" title="Editar">
                            <i class="bi bi-pencil"></i>
                          </button>
                          <button class="btn btn-outline-danger" title="Excluir">
                            <i class="bi bi-trash"></i>
                          </button>
                        </div>
                      </td>
                    </tr>
                    <tr *ngIf="tarefas.length === 0">
                      <td colspan="6" class="text-center text-muted">
                        Nenhuma tarefa encontrada
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .table th {
      background-color: var(--primary-lime);
      color: var(--text-dark);
      font-weight: 600;
    }

    .table-hover tbody tr:hover {
      background-color: rgba(204, 255, 0, 0.1);
    }
  `]
})
export class TarefaListComponent implements OnInit {
  tarefas: Tarefa[] = [];

  constructor(private tarefaService: TarefaService) {}

  ngOnInit(): void {
    this.loadTarefas();
  }

  loadTarefas(): void {
    this.tarefaService.getTarefas().subscribe({
      next: (data) => {
        this.tarefas = data;
      },
      error: (error) => {
        console.error('Erro ao carregar tarefas:', error);
      }
    });
  }

  getStatusName(status: StatusTarefa): string {
    switch (status) {
      case StatusTarefa.ToDo: return 'A Fazer';
      case StatusTarefa.InProgress: return 'Em Progresso';
      case StatusTarefa.InReview: return 'Em Revisão';
      case StatusTarefa.Done: return 'Concluída';
      default: return '';
    }
  }

  getStatusBadgeClass(status: StatusTarefa): string {
    switch (status) {
      case StatusTarefa.ToDo: return 'bg-primary';
      case StatusTarefa.InProgress: return 'bg-warning text-dark';
      case StatusTarefa.InReview: return 'bg-info';
      case StatusTarefa.Done: return 'bg-success-custom';
      default: return 'bg-secondary';
    }
  }

  getPrioridadeClass(prioridade: PrioridadeTarefa): string {
    switch (prioridade) {
      case PrioridadeTarefa.Baixa: return 'priority-baixa';
      case PrioridadeTarefa.Media: return 'priority-media';
      case PrioridadeTarefa.Alta: return 'priority-alta';
      case PrioridadeTarefa.Critica: return 'priority-critica';
      default: return '';
    }
  }

  getPrioridadeNome(prioridade: PrioridadeTarefa): string {
    switch (prioridade) {
      case PrioridadeTarefa.Baixa: return 'Baixa';
      case PrioridadeTarefa.Media: return 'Média';
      case PrioridadeTarefa.Alta: return 'Alta';
      case PrioridadeTarefa.Critica: return 'Crítica';
      default: return '';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '';
    const d = new Date(date);
    return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`;
  }

  isAtrasada(tarefa: Tarefa): boolean {
    if (!tarefa.dataVencimento || tarefa.status === StatusTarefa.Done) {
      return false;
    }
    return new Date(tarefa.dataVencimento) < new Date();
  }
}

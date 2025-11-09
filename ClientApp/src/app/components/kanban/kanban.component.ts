import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { TarefaService } from '../../services/tarefa.service';
import { Tarefa, StatusTarefa, PrioridadeTarefa } from '../../models/tarefa.model';

@Component({
  selector: 'app-kanban',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './kanban.component.html',
  styleUrls: ['./kanban.component.css']
})
export class KanbanComponent implements OnInit {
  tarefas: Tarefa[] = [];
  StatusTarefa = StatusTarefa;
  PrioridadeTarefa = PrioridadeTarefa;
  
  draggedTarefa: Tarefa | null = null;
  
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
  
  getTarefasByStatus(status: StatusTarefa): Tarefa[] {
    return this.tarefas
      .filter(t => t.status === status)
      .sort((a, b) => a.ordem - b.ordem);
  }
  
  onDragStart(event: DragEvent, tarefa: Tarefa): void {
    this.draggedTarefa = tarefa;
    if (event.dataTransfer) {
      event.dataTransfer.effectAllowed = 'move';
      event.dataTransfer.setData('text/html', '');
    }
  }
  
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    if (event.dataTransfer) {
      event.dataTransfer.dropEffect = 'move';
    }
  }
  
  onDrop(event: DragEvent, novoStatus: StatusTarefa): void {
    event.preventDefault();
    
    if (this.draggedTarefa && this.draggedTarefa.status !== novoStatus) {
      console.log('Movendo tarefa:', {
        id: this.draggedTarefa.id,
        titulo: this.draggedTarefa.titulo,
        statusAtual: this.draggedTarefa.status,
        novoStatus: novoStatus
      });

      const request = {
        tarefaId: this.draggedTarefa.id,
        novoStatus: novoStatus,
        novaOrdem: 0
      };
      
      console.log('Enviando request:', request);
      
      this.tarefaService.updateStatus(request).subscribe({
        next: (response) => {
          console.log('Resposta do servidor:', response);
          if (response.success) {
            console.log('Status atualizado com sucesso, recarregando tarefas...');
            this.loadTarefas();
          } else {
            console.error('Erro ao atualizar status:', response.message);
            alert('Erro ao mover tarefa: ' + response.message);
          }
        },
        error: (error) => {
          console.error('Erro na requisição:', error);
          alert('Erro de comunicação com o servidor. Verifique o console para mais detalhes.');
        }
      });
    } else if (this.draggedTarefa && this.draggedTarefa.status === novoStatus) {
      console.log('Tarefa já está no status correto, não fazendo nada');
    } else {
      console.log('Nenhuma tarefa sendo arrastada');
    }
    
    this.draggedTarefa = null;
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
  
  getStatusIcon(status: StatusTarefa): string {
    switch (status) {
      case StatusTarefa.ToDo: return 'bi-list-ul';
      case StatusTarefa.InProgress: return 'bi-play-circle';
      case StatusTarefa.InReview: return 'bi-eye';
      case StatusTarefa.Done: return 'bi-check-circle';
      default: return '';
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
    return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}`;
  }
  
  isAtrasada(tarefa: Tarefa): boolean {
    if (!tarefa.dataVencimento || tarefa.status === StatusTarefa.Done) {
      return false;
    }
    return new Date(tarefa.dataVencimento) < new Date();
  }
}

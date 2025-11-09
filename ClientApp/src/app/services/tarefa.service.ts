import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tarefa, UpdateStatusRequest } from '../models/tarefa.model';

@Injectable({
  providedIn: 'root'
})
export class TarefaService {
  private apiUrl = '/Tarefa';

  constructor(private http: HttpClient) {}

  getTarefas(): Observable<Tarefa[]> {
    return this.http.get<Tarefa[]>(`${this.apiUrl}/GetAll`);
  }

  getTarefaById(id: number): Observable<Tarefa> {
    return this.http.get<Tarefa>(`${this.apiUrl}/Details/${id}`);
  }

  createTarefa(tarefa: Tarefa): Observable<any> {
    return this.http.post(`${this.apiUrl}/Create`, tarefa);
  }

  updateTarefa(tarefa: Tarefa): Observable<any> {
    return this.http.post(`${this.apiUrl}/Edit/${tarefa.id}`, tarefa);
  }

  deleteTarefa(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/Delete/${id}`, {});
  }

  updateStatus(request: UpdateStatusRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/UpdateStatus`, request);
  }

  testStatus(): Observable<any> {
    return this.http.get(`${this.apiUrl}/TestStatus`);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface ThemeConfig {
  id?: number;
  name: string;
  primaryColor: string;
  secondaryColor: string;
  darkColor: string;
  lightColor: string;
  hoverColor: string;
  textDark: string;
  textMedium: string;
  backgroundColor: string;
  isActive: boolean;
  userId?: number;
  createdAt?: Date;
  updatedAt?: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private apiUrl = '/api/theme';
  private currentTheme$ = new BehaviorSubject<ThemeConfig | null>(null);

  constructor(private http: HttpClient) {
    this.loadActiveTheme();
  }

  getThemes(): Observable<ThemeConfig[]> {
    return this.http.get<ThemeConfig[]>(this.apiUrl);
  }

  getTheme(id: number): Observable<ThemeConfig> {
    return this.http.get<ThemeConfig>(`${this.apiUrl}/${id}`);
  }

  getActiveTheme(): Observable<ThemeConfig> {
    return this.http.get<ThemeConfig>(`${this.apiUrl}/active`).pipe(
      tap(theme => this.applyTheme(theme))
    );
  }

  createTheme(theme: ThemeConfig): Observable<ThemeConfig> {
    return this.http.post<ThemeConfig>(this.apiUrl, theme);
  }

  updateTheme(id: number, theme: ThemeConfig): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, theme);
  }

  activateTheme(id: number): Observable<ThemeConfig> {
    return this.http.post<ThemeConfig>(`${this.apiUrl}/${id}/activate`, {}).pipe(
      tap(theme => this.applyTheme(theme))
    );
  }

  deleteTheme(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  applyTheme(theme: ThemeConfig): void {
    const root = document.documentElement;
    root.style.setProperty('--primary-green', theme.primaryColor);
    root.style.setProperty('--primary-lime', theme.secondaryColor);
    root.style.setProperty('--dark-green', theme.darkColor);
    root.style.setProperty('--light-green', theme.lightColor);
    root.style.setProperty('--hover-green', theme.hoverColor);
    root.style.setProperty('--text-dark', theme.textDark);
    root.style.setProperty('--text-medium', theme.textMedium);
    document.body.style.backgroundColor = theme.backgroundColor;
    
    this.currentTheme$.next(theme);
    localStorage.setItem('activeTheme', JSON.stringify(theme));
  }

  loadActiveTheme(): void {
    const savedTheme = localStorage.getItem('activeTheme');
    if (savedTheme) {
      this.applyTheme(JSON.parse(savedTheme));
    } else {
      this.getActiveTheme().subscribe({
        error: () => {
          // Se não houver tema ativo, usar o padrão
          this.applyDefaultTheme();
        }
      });
    }
  }

  private applyDefaultTheme(): void {
    const defaultTheme: ThemeConfig = {
      name: 'Padrão',
      primaryColor: '#9acd32',
      secondaryColor: '#ccff00',
      darkColor: '#6b8e23',
      lightColor: '#e6ff99',
      hoverColor: '#b3e600',
      textDark: '#1a3309',
      textMedium: '#2d5016',
      backgroundColor: '#f8f9fa',
      isActive: true
    };
    this.applyTheme(defaultTheme);
  }

  getCurrentTheme(): Observable<ThemeConfig | null> {
    return this.currentTheme$.asObservable();
  }
}

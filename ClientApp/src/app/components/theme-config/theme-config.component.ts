import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ThemeService, ThemeConfig } from '../../services/theme.service';

@Component({
  selector: 'app-theme-config',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './theme-config.component.html',
  styleUrls: ['./theme-config.component.css']
})
export class ThemeConfigComponent implements OnInit {
  themes: ThemeConfig[] = [];
  selectedTheme: ThemeConfig | null = null;
  isEditing = false;
  isCreating = false;
  
  newTheme: ThemeConfig = {
    name: '',
    primaryColor: '#9acd32',
    secondaryColor: '#ccff00',
    darkColor: '#6b8e23',
    lightColor: '#e6ff99',
    hoverColor: '#b3e600',
    textDark: '#1a3309',
    textMedium: '#2d5016',
    backgroundColor: '#f8f9fa',
    isActive: false
  };

  presetThemes = [
    {
      name: 'Verde Louro (Padrão)',
      primaryColor: '#9acd32',
      secondaryColor: '#ccff00',
      darkColor: '#6b8e23',
      lightColor: '#e6ff99',
      hoverColor: '#b3e600',
      textDark: '#1a3309',
      textMedium: '#2d5016',
      backgroundColor: '#f8f9fa'
    },
    {
      name: 'Azul Oceano',
      primaryColor: '#0077be',
      secondaryColor: '#00a8e8',
      darkColor: '#003f5c',
      lightColor: '#b3e5fc',
      hoverColor: '#0096d6',
      textDark: '#001f3f',
      textMedium: '#003d5c',
      backgroundColor: '#f0f8ff'
    },
    {
      name: 'Roxo Moderno',
      primaryColor: '#7b2cbf',
      secondaryColor: '#9d4edd',
      darkColor: '#5a189a',
      lightColor: '#e0aaff',
      hoverColor: '#8b3dcc',
      textDark: '#240046',
      textMedium: '#3c096c',
      backgroundColor: '#f8f4ff'
    },
    {
      name: 'Laranja Vibrante',
      primaryColor: '#ff6b35',
      secondaryColor: '#ff9f1c',
      darkColor: '#d84315',
      lightColor: '#ffccbc',
      hoverColor: '#ff7b45',
      textDark: '#4a1504',
      textMedium: '#7a2504',
      backgroundColor: '#fff8f5'
    }
  ];

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    this.loadThemes();
  }

  loadThemes(): void {
    this.themeService.getThemes().subscribe({
      next: (themes) => {
        this.themes = themes;
      },
      error: (error) => {
        console.error('Erro ao carregar temas:', error);
      }
    });
  }

  startCreate(): void {
    this.isCreating = true;
    this.isEditing = false;
    this.newTheme = {
      name: '',
      primaryColor: '#9acd32',
      secondaryColor: '#ccff00',
      darkColor: '#6b8e23',
      lightColor: '#e6ff99',
      hoverColor: '#b3e600',
      textDark: '#1a3309',
      textMedium: '#2d5016',
      backgroundColor: '#f8f9fa',
      isActive: false
    };
  }

  applyPreset(preset: any): void {
    this.newTheme = {
      ...this.newTheme,
      name: preset.name,
      primaryColor: preset.primaryColor,
      secondaryColor: preset.secondaryColor,
      darkColor: preset.darkColor,
      lightColor: preset.lightColor,
      hoverColor: preset.hoverColor,
      textDark: preset.textDark,
      textMedium: preset.textMedium,
      backgroundColor: preset.backgroundColor
    };
  }

  previewTheme(): void {
    this.themeService.applyTheme(this.newTheme);
  }

  saveTheme(): void {
    if (this.isEditing && this.selectedTheme?.id) {
      this.themeService.updateTheme(this.selectedTheme.id, this.newTheme).subscribe({
        next: () => {
          this.loadThemes();
          this.cancelEdit();
          alert('Tema atualizado com sucesso!');
        },
        error: (error) => {
          console.error('Erro ao atualizar tema:', error);
          alert('Erro ao atualizar tema');
        }
      });
    } else {
      this.themeService.createTheme(this.newTheme).subscribe({
        next: () => {
          this.loadThemes();
          this.cancelEdit();
          alert('Tema criado com sucesso!');
        },
        error: (error) => {
          console.error('Erro ao criar tema:', error);
          alert('Erro ao criar tema');
        }
      });
    }
  }

  editTheme(theme: ThemeConfig): void {
    this.isEditing = true;
    this.isCreating = true;
    this.selectedTheme = theme;
    this.newTheme = { ...theme };
  }

  activateTheme(theme: ThemeConfig): void {
    if (theme.id) {
      this.themeService.activateTheme(theme.id).subscribe({
        next: () => {
          this.loadThemes();
          alert('Tema ativado com sucesso!');
        },
        error: (error) => {
          console.error('Erro ao ativar tema:', error);
          alert('Erro ao ativar tema');
        }
      });
    }
  }

  deleteTheme(theme: ThemeConfig): void {
    if (theme.id && confirm(`Deseja realmente excluir o tema "${theme.name}"?`)) {
      this.themeService.deleteTheme(theme.id).subscribe({
        next: () => {
          this.loadThemes();
          alert('Tema excluído com sucesso!');
        },
        error: (error) => {
          console.error('Erro ao excluir tema:', error);
          alert(error.error?.message || 'Erro ao excluir tema');
        }
      });
    }
  }

  cancelEdit(): void {
    this.isCreating = false;
    this.isEditing = false;
    this.selectedTheme = null;
    this.themeService.loadActiveTheme();
  }
}

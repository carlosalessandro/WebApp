import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { ThemeService } from './services/theme.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SidebarComponent],
  template: `
    <div class="container-fluid">
      <div class="row">
        <app-sidebar class="col-md-3 col-lg-2 p-0"></app-sidebar>
        <main class="col-md-9 col-lg-10 p-4">
          <router-outlet></router-outlet>
        </main>
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      min-height: 100vh;
    }
  `]
})
export class AppComponent implements OnInit {
  title = 'Sistema Kanban';

  constructor(private themeService: ThemeService) {}

  ngOnInit(): void {
    // Carrega o tema ativo ao iniciar a aplicação
    this.themeService.loadActiveTheme();
  }
}

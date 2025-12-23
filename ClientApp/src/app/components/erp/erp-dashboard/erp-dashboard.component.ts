import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

interface ERPStats {
  totalContasPagar: number;
  totalContasReceber: number;
  fluxoCaixaMes: number;
  opsEmAndamento: number;
  opsAtrasadas: number;
  produtividadeMedia: number;
  recursosDisponiveis: number;
  recursosManutencao: number;
  taxaAprovacaoQualidade: number;
}

@Component({
  selector: 'app-erp-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './erp-dashboard.component.html',
  styleUrls: ['./erp-dashboard.component.css']
})
export class ErpDashboardComponent implements OnInit {
  stats: ERPStats = {
    totalContasPagar: 0,
    totalContasReceber: 0,
    fluxoCaixaMes: 0,
    opsEmAndamento: 0,
    opsAtrasadas: 0,
    produtividadeMedia: 0,
    recursosDisponiveis: 0,
    recursosManutencao: 0,
    taxaAprovacaoQualidade: 0
  };

  loading = true;
  useMvcView = true; // Flag para usar view MVC

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    // Redirecionar para a view MVC que já existe
    if (this.useMvcView) {
      window.location.href = '/ERP/Index';
    } else {
      this.loadStats();
    }
  }

  loadStats(): void {
    // Simulando dados - você pode substituir por chamada real à API
    setTimeout(() => {
      this.stats = {
        totalContasPagar: 125000,
        totalContasReceber: 285000,
        fluxoCaixaMes: 160000,
        opsEmAndamento: 24,
        opsAtrasadas: 3,
        produtividadeMedia: 87.5,
        recursosDisponiveis: 18,
        recursosManutencao: 2,
        taxaAprovacaoQualidade: 96.8
      };
      this.loading = false;
    }, 500);
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  }
}

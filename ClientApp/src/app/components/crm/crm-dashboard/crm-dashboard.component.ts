import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

interface CRMStats {
  totalLeads: number;
  leadsNovos: number;
  leadsQualificados: number;
  leadsConvertidos: number;
  totalOportunidades: number;
  oportunidadesAbertas: number;
  valorPipeline: number;
  campanhasAtivas: number;
  leadsEsteMes: number;
  taxaConversao: number;
}

@Component({
  selector: 'app-crm-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './crm-dashboard.component.html',
  styleUrls: ['./crm-dashboard.component.css']
})
export class CrmDashboardComponent implements OnInit {
  stats: CRMStats = {
    totalLeads: 0,
    leadsNovos: 0,
    leadsQualificados: 0,
    leadsConvertidos: 0,
    totalOportunidades: 0,
    oportunidadesAbertas: 0,
    valorPipeline: 0,
    campanhasAtivas: 0,
    leadsEsteMes: 0,
    taxaConversao: 0
  };

  loading = true;
  useMvcView = true; // Flag para usar view MVC

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    // Redirecionar para a view MVC que já existe
    if (this.useMvcView) {
      window.location.href = '/CRM/Index';
    } else {
      this.loadStats();
    }
  }

  loadStats(): void {
    // Simulando dados - você pode substituir por chamada real à API
    setTimeout(() => {
      this.stats = {
        totalLeads: 245,
        leadsNovos: 42,
        leadsQualificados: 87,
        leadsConvertidos: 56,
        totalOportunidades: 128,
        oportunidadesAbertas: 73,
        valorPipeline: 1250000,
        campanhasAtivas: 8,
        leadsEsteMes: 89,
        taxaConversao: 22.86
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

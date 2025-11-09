export interface Tarefa {
  id: number;
  titulo: string;
  descricao?: string;
  status: StatusTarefa;
  prioridade: PrioridadeTarefa;
  dataVencimento?: Date;
  responsavel?: string;
  estimativaHoras?: number;
  tempoGasto?: number;
  tags?: string;
  cor: string;
  ordem: number;
  dataCriacao: Date;
  dataAtualizacao?: Date;
}

export enum StatusTarefa {
  ToDo = 1,
  InProgress = 2,
  InReview = 3,
  Done = 4
}

export enum PrioridadeTarefa {
  Baixa = 1,
  Media = 2,
  Alta = 3,
  Critica = 4
}

export interface UpdateStatusRequest {
  tarefaId: number;
  novoStatus: StatusTarefa;
  novaOrdem: number;
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { 
  Diagram, 
  DiagramComponent, 
  ComponentPalette, 
  ComponentTemplate, 
  ComponentType,
  DiagramEvent,
  CanvasSettings
} from '../models/diagram.models';
import { v4 as uuidv4 } from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class DiagramService {
  private readonly baseUrl = '/api/diagrams';
  private currentDiagram$ = new BehaviorSubject<Diagram | null>(null);
  private componentPalettes$ = new BehaviorSubject<ComponentPalette[]>([]);
  private diagramHistory: Diagram[] = [];
  private historyIndex = -1;

  constructor(private http: HttpClient) {
    this.initializeDefaultPalettes();
  }

  // Diagram CRUD operations
  createDiagram(name: string, description?: string): Observable<Diagram> {
    const diagram: Diagram = {
      id: uuidv4(),
      name,
      description,
      components: [],
      connections: [],
      canvas: this.getDefaultCanvasSettings(),
      metadata: {
        createdAt: new Date(),
        updatedAt: new Date(),
        createdBy: 'current-user', // This should come from auth service
        version: '1.0.0',
        tags: [],
        isPublic: false
      }
    };

    return this.http.post<Diagram>(this.baseUrl, diagram).pipe(
      map(savedDiagram => {
        this.currentDiagram$.next(savedDiagram);
        this.addToHistory(savedDiagram);
        return savedDiagram;
      }),
      catchError(error => {
        console.error('Error creating diagram:', error);
        // Fallback to local storage for offline mode
        this.saveDiagramLocally(diagram);
        this.currentDiagram$.next(diagram);
        this.addToHistory(diagram);
        return of(diagram);
      })
    );
  }

  getDiagram(id: string): Observable<Diagram> {
    return this.http.get<Diagram>(`${this.baseUrl}/${id}`).pipe(
      map(diagram => {
        this.currentDiagram$.next(diagram);
        this.addToHistory(diagram);
        return diagram;
      }),
      catchError(error => {
        console.error('Error loading diagram:', error);
        // Try to load from local storage
        const localDiagram = this.loadDiagramLocally(id);
        if (localDiagram) {
          this.currentDiagram$.next(localDiagram);
          return of(localDiagram);
        }
        throw error;
      })
    );
  }

  saveDiagram(diagram: Diagram): Observable<Diagram> {
    diagram.metadata.updatedAt = new Date();
    
    return this.http.put<Diagram>(`${this.baseUrl}/${diagram.id}`, diagram).pipe(
      map(savedDiagram => {
        this.currentDiagram$.next(savedDiagram);
        this.addToHistory(savedDiagram);
        return savedDiagram;
      }),
      catchError(error => {
        console.error('Error saving diagram:', error);
        // Fallback to local storage
        this.saveDiagramLocally(diagram);
        this.currentDiagram$.next(diagram);
        this.addToHistory(diagram);
        return of(diagram);
      })
    );
  }

  deleteDiagram(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}/${id}`).pipe(
      map(() => {
        if (this.currentDiagram$.value?.id === id) {
          this.currentDiagram$.next(null);
        }
        this.removeDiagramLocally(id);
        return true;
      }),
      catchError(error => {
        console.error('Error deleting diagram:', error);
        return of(false);
      })
    );
  }

  getDiagrams(): Observable<Diagram[]> {
    return this.http.get<Diagram[]>(this.baseUrl).pipe(
      catchError(error => {
        console.error('Error loading diagrams:', error);
        // Return local diagrams as fallback
        return of(this.getLocalDiagrams());
      })
    );
  }

  // Component operations
  addComponent(component: DiagramComponent): void {
    const currentDiagram = this.currentDiagram$.value;
    if (currentDiagram) {
      currentDiagram.components.push(component);
      this.currentDiagram$.next(currentDiagram);
      this.addToHistory(currentDiagram);
    }
  }

  updateComponent(component: DiagramComponent): void {
    const currentDiagram = this.currentDiagram$.value;
    if (currentDiagram) {
      const index = currentDiagram.components.findIndex(c => c.id === component.id);
      if (index !== -1) {
        currentDiagram.components[index] = component;
        this.currentDiagram$.next(currentDiagram);
        this.addToHistory(currentDiagram);
      }
    }
  }

  removeComponent(componentId: string): void {
    const currentDiagram = this.currentDiagram$.value;
    if (currentDiagram) {
      currentDiagram.components = currentDiagram.components.filter(c => c.id !== componentId);
      // Also remove connections involving this component
      currentDiagram.connections = currentDiagram.connections.filter(
        conn => conn.fromComponentId !== componentId && conn.toComponentId !== componentId
      );
      this.currentDiagram$.next(currentDiagram);
      this.addToHistory(currentDiagram);
    }
  }

  // Palette operations
  getComponentPalettes(): Observable<ComponentPalette[]> {
    return this.componentPalettes$.asObservable();
  }

  createCustomPalette(name: string, description: string): ComponentPalette {
    const palette: ComponentPalette = {
      id: uuidv4(),
      name,
      description,
      category: 'Custom',
      components: [],
      isCustom: true,
      createdBy: 'current-user',
      createdAt: new Date()
    };

    const currentPalettes = this.componentPalettes$.value;
    this.componentPalettes$.next([...currentPalettes, palette]);
    this.savePalettesLocally();
    
    return palette;
  }

  addComponentToPalette(paletteId: string, template: ComponentTemplate): void {
    const palettes = this.componentPalettes$.value;
    const palette = palettes.find(p => p.id === paletteId);
    if (palette) {
      palette.components.push(template);
      this.componentPalettes$.next([...palettes]);
      this.savePalettesLocally();
    }
  }

  // History operations
  undo(): Diagram | null {
    if (this.historyIndex > 0) {
      this.historyIndex--;
      const diagram = this.diagramHistory[this.historyIndex];
      this.currentDiagram$.next(diagram);
      return diagram;
    }
    return null;
  }

  redo(): Diagram | null {
    if (this.historyIndex < this.diagramHistory.length - 1) {
      this.historyIndex++;
      const diagram = this.diagramHistory[this.historyIndex];
      this.currentDiagram$.next(diagram);
      return diagram;
    }
    return null;
  }

  canUndo(): boolean {
    return this.historyIndex > 0;
  }

  canRedo(): boolean {
    return this.historyIndex < this.diagramHistory.length - 1;
  }

  // Export/Import operations
  exportDiagram(diagram: Diagram, format: 'json' | 'svg' | 'png'): Observable<Blob> {
    switch (format) {
      case 'json':
        return of(new Blob([JSON.stringify(diagram, null, 2)], { type: 'application/json' }));
      case 'svg':
        return this.exportToSvg(diagram);
      case 'png':
        return this.exportToPng(diagram);
      default:
        throw new Error(`Unsupported export format: ${format}`);
    }
  }

  importDiagram(file: File): Observable<Diagram> {
    return new Observable(observer => {
      const reader = new FileReader();
      reader.onload = (e) => {
        try {
          const content = e.target?.result as string;
          const diagram = JSON.parse(content) as Diagram;
          
          // Validate diagram structure
          if (this.validateDiagram(diagram)) {
            diagram.id = uuidv4(); // Generate new ID for imported diagram
            diagram.metadata.createdAt = new Date();
            diagram.metadata.updatedAt = new Date();
            
            observer.next(diagram);
            observer.complete();
          } else {
            observer.error(new Error('Invalid diagram format'));
          }
        } catch (error) {
          observer.error(error);
        }
      };
      reader.readAsText(file);
    });
  }

  // Observables
  getCurrentDiagram(): Observable<Diagram | null> {
    return this.currentDiagram$.asObservable();
  }

  // Private methods
  private initializeDefaultPalettes(): void {
    const basicShapes = this.createBasicShapesPalette();
    const flowchartElements = this.createFlowchartPalette();
    const umlElements = this.createUmlPalette();
    const networkElements = this.createNetworkPalette();
    
    this.componentPalettes$.next([basicShapes, flowchartElements, umlElements, networkElements]);
  }

  private createBasicShapesPalette(): ComponentPalette {
    return {
      id: 'basic-shapes',
      name: 'Basic Shapes',
      description: 'Fundamental geometric shapes',
      category: 'Basic',
      isCustom: false,
      components: [
        {
          id: 'rectangle-template',
          name: 'Rectangle',
          type: ComponentType.RECTANGLE,
          icon: 'crop_din',
          defaultProperties: { text: 'Rectangle' },
          defaultStyle: { fill: '#ffffff', stroke: '#000000', strokeWidth: 2 },
          defaultSize: { width: 120, height: 80 },
          propertySchema: [
            { key: 'text', label: 'Text', type: 'text', defaultValue: 'Rectangle', required: false, description: 'Text to display inside the rectangle' },
            { key: 'fill', label: 'Fill Color', type: 'color', defaultValue: '#ffffff', required: false, description: 'Background color' },
            { key: 'stroke', label: 'Border Color', type: 'color', defaultValue: '#000000', required: false, description: 'Border color' },
            { key: 'strokeWidth', label: 'Border Width', type: 'number', defaultValue: 2, min: 0, max: 10, required: false, description: 'Border thickness' }
          ]
        },
        {
          id: 'circle-template',
          name: 'Circle',
          type: ComponentType.CIRCLE,
          icon: 'radio_button_unchecked',
          defaultProperties: { text: 'Circle' },
          defaultStyle: { fill: '#ffffff', stroke: '#000000', strokeWidth: 2 },
          defaultSize: { width: 100, height: 100 },
          propertySchema: [
            { key: 'text', label: 'Text', type: 'text', defaultValue: 'Circle', required: false, description: 'Text to display inside the circle' },
            { key: 'fill', label: 'Fill Color', type: 'color', defaultValue: '#ffffff', required: false, description: 'Background color' },
            { key: 'stroke', label: 'Border Color', type: 'color', defaultValue: '#000000', required: false, description: 'Border color' }
          ]
        },
        {
          id: 'triangle-template',
          name: 'Triangle',
          type: ComponentType.TRIANGLE,
          icon: 'change_history',
          defaultProperties: { text: 'Triangle' },
          defaultStyle: { fill: '#ffffff', stroke: '#000000', strokeWidth: 2 },
          defaultSize: { width: 100, height: 100 },
          propertySchema: [
            { key: 'text', label: 'Text', type: 'text', defaultValue: 'Triangle', required: false, description: 'Text to display inside the triangle' }
          ]
        },
        {
          id: 'diamond-template',
          name: 'Diamond',
          type: ComponentType.DIAMOND,
          icon: 'crop_rotate',
          defaultProperties: { text: 'Diamond' },
          defaultStyle: { fill: '#ffffff', stroke: '#000000', strokeWidth: 2 },
          defaultSize: { width: 120, height: 80 },
          propertySchema: [
            { key: 'text', label: 'Text', type: 'text', defaultValue: 'Diamond', required: false, description: 'Text to display inside the diamond' }
          ]
        }
      ]
    };
  }

  private createFlowchartPalette(): ComponentPalette {
    return {
      id: 'flowchart',
      name: 'Flowchart',
      description: 'Flowchart and process diagram elements',
      category: 'Diagrams',
      isCustom: false,
      components: [
        {
          id: 'process-template',
          name: 'Process',
          type: ComponentType.PROCESS,
          icon: 'crop_din',
          defaultProperties: { text: 'Process' },
          defaultStyle: { fill: '#e3f2fd', stroke: '#1976d2', strokeWidth: 2 },
          defaultSize: { width: 120, height: 60 },
          propertySchema: [
            { key: 'text', label: 'Process Name', type: 'text', defaultValue: 'Process', required: true, description: 'Name of the process step' }
          ]
        },
        {
          id: 'decision-template',
          name: 'Decision',
          type: ComponentType.DECISION,
          icon: 'crop_rotate',
          defaultProperties: { text: 'Decision?' },
          defaultStyle: { fill: '#fff3e0', stroke: '#f57c00', strokeWidth: 2 },
          defaultSize: { width: 120, height: 80 },
          propertySchema: [
            { key: 'text', label: 'Decision Question', type: 'text', defaultValue: 'Decision?', required: true, description: 'The decision question' }
          ]
        },
        {
          id: 'start-end-template',
          name: 'Start/End',
          type: ComponentType.START_END,
          icon: 'radio_button_unchecked',
          defaultProperties: { text: 'Start' },
          defaultStyle: { fill: '#e8f5e8', stroke: '#4caf50', strokeWidth: 2 },
          defaultSize: { width: 100, height: 50 },
          propertySchema: [
            { key: 'text', label: 'Label', type: 'text', defaultValue: 'Start', required: true, description: 'Start or End label' }
          ]
        }
      ]
    };
  }

  private createUmlPalette(): ComponentPalette {
    return {
      id: 'uml',
      name: 'UML',
      description: 'Unified Modeling Language elements',
      category: 'Diagrams',
      isCustom: false,
      components: [
        {
          id: 'class-template',
          name: 'Class',
          type: ComponentType.CLASS,
          icon: 'view_module',
          defaultProperties: { text: 'ClassName', attributes: [], methods: [] },
          defaultStyle: { fill: '#ffffff', stroke: '#000000', strokeWidth: 1 },
          defaultSize: { width: 150, height: 100 },
          propertySchema: [
            { key: 'text', label: 'Class Name', type: 'text', defaultValue: 'ClassName', required: true, description: 'Name of the class' }
          ]
        },
        {
          id: 'actor-template',
          name: 'Actor',
          type: ComponentType.ACTOR,
          icon: 'person',
          defaultProperties: { text: 'Actor' },
          defaultStyle: { fill: '#ffffff', stroke: '#000000', strokeWidth: 2 },
          defaultSize: { width: 60, height: 80 },
          propertySchema: [
            { key: 'text', label: 'Actor Name', type: 'text', defaultValue: 'Actor', required: true, description: 'Name of the actor' }
          ]
        }
      ]
    };
  }

  private createNetworkPalette(): ComponentPalette {
    return {
      id: 'network',
      name: 'Network',
      description: 'Network and infrastructure elements',
      category: 'Technical',
      isCustom: false,
      components: [
        {
          id: 'server-template',
          name: 'Server',
          type: ComponentType.SERVER,
          icon: 'dns',
          defaultProperties: { text: 'Server' },
          defaultStyle: { fill: '#f3e5f5', stroke: '#7b1fa2', strokeWidth: 2 },
          defaultSize: { width: 80, height: 100 },
          propertySchema: [
            { key: 'text', label: 'Server Name', type: 'text', defaultValue: 'Server', required: false, description: 'Name or label for the server' }
          ]
        },
        {
          id: 'database-template',
          name: 'Database',
          type: ComponentType.DATABASE,
          icon: 'storage',
          defaultProperties: { text: 'Database' },
          defaultStyle: { fill: '#e0f2f1', stroke: '#00695c', strokeWidth: 2 },
          defaultSize: { width: 80, height: 80 },
          propertySchema: [
            { key: 'text', label: 'Database Name', type: 'text', defaultValue: 'Database', required: false, description: 'Name of the database' }
          ]
        },
        {
          id: 'cloud-template',
          name: 'Cloud',
          type: ComponentType.CLOUD,
          icon: 'cloud',
          defaultProperties: { text: 'Cloud' },
          defaultStyle: { fill: '#e3f2fd', stroke: '#1565c0', strokeWidth: 2 },
          defaultSize: { width: 120, height: 80 },
          propertySchema: [
            { key: 'text', label: 'Cloud Service', type: 'text', defaultValue: 'Cloud', required: false, description: 'Name of the cloud service' }
          ]
        }
      ]
    };
  }

  private getDefaultCanvasSettings(): CanvasSettings {
    return {
      width: 1200,
      height: 800,
      backgroundColor: '#ffffff',
      gridEnabled: true,
      gridSize: 20,
      gridColor: '#e0e0e0',
      snapToGrid: true,
      zoom: 1,
      panX: 0,
      panY: 0
    };
  }

  private addToHistory(diagram: Diagram): void {
    // Remove any history after current index
    this.diagramHistory = this.diagramHistory.slice(0, this.historyIndex + 1);
    
    // Add new state
    this.diagramHistory.push(JSON.parse(JSON.stringify(diagram))); // Deep copy
    this.historyIndex = this.diagramHistory.length - 1;
    
    // Limit history size
    if (this.diagramHistory.length > 50) {
      this.diagramHistory.shift();
      this.historyIndex--;
    }
  }

  private saveDiagramLocally(diagram: Diagram): void {
    const diagrams = this.getLocalDiagrams();
    const existingIndex = diagrams.findIndex(d => d.id === diagram.id);
    
    if (existingIndex !== -1) {
      diagrams[existingIndex] = diagram;
    } else {
      diagrams.push(diagram);
    }
    
    localStorage.setItem('diagrams', JSON.stringify(diagrams));
  }

  private loadDiagramLocally(id: string): Diagram | null {
    const diagrams = this.getLocalDiagrams();
    return diagrams.find(d => d.id === id) || null;
  }

  private removeDiagramLocally(id: string): void {
    const diagrams = this.getLocalDiagrams();
    const filtered = diagrams.filter(d => d.id !== id);
    localStorage.setItem('diagrams', JSON.stringify(filtered));
  }

  private getLocalDiagrams(): Diagram[] {
    const stored = localStorage.getItem('diagrams');
    return stored ? JSON.parse(stored) : [];
  }

  private savePalettesLocally(): void {
    const customPalettes = this.componentPalettes$.value.filter(p => p.isCustom);
    localStorage.setItem('customPalettes', JSON.stringify(customPalettes));
  }

  private validateDiagram(diagram: any): diagram is Diagram {
    return diagram &&
           typeof diagram.id === 'string' &&
           typeof diagram.name === 'string' &&
           Array.isArray(diagram.components) &&
           Array.isArray(diagram.connections) &&
           diagram.canvas &&
           diagram.metadata;
  }

  private exportToSvg(diagram: Diagram): Observable<Blob> {
    // Implementation for SVG export
    // This would render the diagram to SVG format
    return of(new Blob(['<svg></svg>'], { type: 'image/svg+xml' }));
  }

  private exportToPng(diagram: Diagram): Observable<Blob> {
    // Implementation for PNG export
    // This would render the diagram to PNG format
    return of(new Blob([], { type: 'image/png' }));
  }
}

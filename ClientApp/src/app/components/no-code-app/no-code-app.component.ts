import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { 
  Diagram, 
  DiagramComponent, 
  ComponentTemplate,
  DiagramEvent 
} from '../../models/diagram.models';
import { 
  SqlQuery, 
  QueryExecutionResult 
} from '../../models/sql-builder.models';
import { DiagramService } from '../../services/diagram.service';
import { DiagramCanvasComponent } from '../diagram-canvas/diagram-canvas.component';
import { ComponentPaletteComponent } from '../component-palette/component-palette.component';
import { SqlQueryBuilderComponent } from '../sql-query-builder/sql-query-builder.component';

@Component({
  selector: 'app-no-code-app',
  templateUrl: './no-code-app.component.html',
  styleUrls: ['./no-code-app.component.css']
})
export class NoCodeAppComponent implements OnInit {
  @ViewChild('diagramCanvas') diagramCanvas!: DiagramCanvasComponent;
  @ViewChild('componentPalette') componentPalette!: ComponentPaletteComponent;
  @ViewChild('sqlQueryBuilder') sqlQueryBuilder!: SqlQueryBuilderComponent;

  // Application state
  currentTab = 0;
  isLoading = false;
  
  // Diagram state
  currentDiagram: Diagram | null = null;
  selectedComponentId: string | null = null;
  selectedTemplate: ComponentTemplate | null = null;
  
  // SQL Builder state
  currentQuery: SqlQuery | null = null;
  queryResult: QueryExecutionResult | null = null;
  
  // UI state
  sidebarOpen = true;
  propertiesPanelOpen = true;
  tutorialMode = false;
  
  // Tutorial and help
  showWelcomeDialog = true;
  currentTutorialStep = 0;
  tutorialSteps = [
    {
      title: 'Welcome to No-Code Builder',
      description: 'Create diagrams and SQL queries visually without writing code.',
      target: '.main-toolbar'
    },
    {
      title: 'Diagram Builder',
      description: 'Drag components from the palette to create flowcharts, UML diagrams, and more.',
      target: '.diagram-tab'
    },
    {
      title: 'SQL Query Builder',
      description: 'Build complex SQL queries using a visual interface with educational tooltips.',
      target: '.sql-tab'
    },
    {
      title: 'Component Palette',
      description: 'Browse and customize components. Create your own custom palettes.',
      target: '.component-palette'
    }
  ];

  constructor(
    private diagramService: DiagramService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.initializeApplication();
    this.setupEventListeners();
  }

  private initializeApplication(): void {
    // Create a new diagram by default
    this.createNewDiagram();
    
    // Check if user wants tutorial
    const skipTutorial = localStorage.getItem('skipTutorial');
    if (!skipTutorial) {
      this.tutorialMode = true;
    }
  }

  private setupEventListeners(): void {
    // Listen to diagram changes
    this.diagramService.getCurrentDiagram().subscribe(diagram => {
      this.currentDiagram = diagram;
    });
  }

  // Tab management
  onTabChange(event: MatTabChangeEvent): void {
    this.currentTab = event.index;
    
    // Reset selections when switching tabs
    this.selectedComponentId = null;
    this.selectedTemplate = null;
    
    // Show relevant help based on tab
    if (this.tutorialMode) {
      this.showTabHelp(event.index);
    }
  }

  private showTabHelp(tabIndex: number): void {
    const helpMessages = [
      'Diagram Builder: Create visual diagrams by dragging components from the palette.',
      'SQL Query Builder: Build database queries visually with educational guidance.',
      'Settings: Configure your workspace and preferences.'
    ];
    
    if (helpMessages[tabIndex]) {
      this.snackBar.open(helpMessages[tabIndex], 'Got it', { duration: 5000 });
    }
  }

  // Diagram operations
  createNewDiagram(): void {
    this.isLoading = true;
    this.diagramService.createDiagram('New Diagram', 'Created with No-Code Builder').subscribe({
      next: (diagram) => {
        this.currentDiagram = diagram;
        this.isLoading = false;
        this.snackBar.open('New diagram created', 'Close', { duration: 2000 });
      },
      error: (error) => {
        this.isLoading = false;
        this.snackBar.open('Failed to create diagram', 'Close', { duration: 3000 });
        console.error('Error creating diagram:', error);
      }
    });
  }

  saveDiagram(): void {
    if (!this.currentDiagram) return;
    
    this.isLoading = true;
    this.diagramService.saveDiagram(this.currentDiagram).subscribe({
      next: (savedDiagram) => {
        this.currentDiagram = savedDiagram;
        this.isLoading = false;
        this.snackBar.open('Diagram saved successfully', 'Close', { duration: 2000 });
      },
      error: (error) => {
        this.isLoading = false;
        this.snackBar.open('Failed to save diagram', 'Close', { duration: 3000 });
        console.error('Error saving diagram:', error);
      }
    });
  }

  exportDiagram(format: 'json' | 'svg' | 'png'): void {
    if (!this.currentDiagram) return;
    
    this.diagramService.exportDiagram(this.currentDiagram, format).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `${this.currentDiagram!.name}.${format}`;
        link.click();
        window.URL.revokeObjectURL(url);
        
        this.snackBar.open(`Diagram exported as ${format.toUpperCase()}`, 'Close', { duration: 2000 });
      },
      error: (error) => {
        this.snackBar.open('Failed to export diagram', 'Close', { duration: 3000 });
        console.error('Error exporting diagram:', error);
      }
    });
  }

  // Component operations
  onComponentSelected(template: ComponentTemplate): void {
    this.selectedTemplate = template;
  }

  onComponentDragStart(template: ComponentTemplate): void {
    this.selectedTemplate = template;
  }

  onComponentDragEnd(): void {
    // Handle drag end if needed
  }

  onCanvasComponentSelected(componentId: string | null): void {
    this.selectedComponentId = componentId;
  }

  onComponentAdded(component: DiagramComponent): void {
    this.snackBar.open(`${component.type} component added`, 'Close', { duration: 1500 });
  }

  onComponentUpdated(component: DiagramComponent): void {
    // Handle component updates
  }

  onComponentDeleted(componentId: string): void {
    this.selectedComponentId = null;
    this.snackBar.open('Component deleted', 'Close', { duration: 1500 });
  }

  onDiagramChanged(event: DiagramEvent): void {
    // Handle diagram changes for undo/redo functionality
    console.log('Diagram changed:', event);
  }

  // SQL Builder operations
  onQueryExecuted(result: QueryExecutionResult): void {
    this.queryResult = result;
    
    if (result.success) {
      this.snackBar.open(`Query executed: ${result.rowCount} rows returned`, 'Close', { duration: 3000 });
    } else {
      this.snackBar.open(`Query failed: ${result.error}`, 'Close', { duration: 5000 });
    }
  }

  onQueryChanged(query: SqlQuery): void {
    this.currentQuery = query;
  }

  // UI operations
  toggleSidebar(): void {
    this.sidebarOpen = !this.sidebarOpen;
  }

  togglePropertiesPanel(): void {
    this.propertiesPanelOpen = !this.propertiesPanelOpen;
  }

  toggleTutorialMode(): void {
    this.tutorialMode = !this.tutorialMode;
    
    if (!this.tutorialMode) {
      localStorage.setItem('skipTutorial', 'true');
      this.snackBar.open('Tutorial mode disabled', 'Close', { duration: 2000 });
    } else {
      localStorage.removeItem('skipTutorial');
      this.currentTutorialStep = 0;
      this.snackBar.open('Tutorial mode enabled', 'Close', { duration: 2000 });
    }
  }

  nextTutorialStep(): void {
    if (this.currentTutorialStep < this.tutorialSteps.length - 1) {
      this.currentTutorialStep++;
    } else {
      this.tutorialMode = false;
      localStorage.setItem('skipTutorial', 'true');
      this.snackBar.open('Tutorial completed!', 'Close', { duration: 3000 });
    }
  }

  previousTutorialStep(): void {
    if (this.currentTutorialStep > 0) {
      this.currentTutorialStep--;
    }
  }

  skipTutorial(): void {
    this.tutorialMode = false;
    localStorage.setItem('skipTutorial', 'true');
    this.snackBar.open('Tutorial skipped', 'Close', { duration: 2000 });
  }

  // Utility methods
  getCurrentTutorialStep(): any {
    return this.tutorialSteps[this.currentTutorialStep];
  }

  canUndo(): boolean {
    return this.diagramService.canUndo();
  }

  canRedo(): boolean {
    return this.diagramService.canRedo();
  }

  undo(): void {
    const diagram = this.diagramService.undo();
    if (diagram) {
      this.currentDiagram = diagram;
      this.snackBar.open('Undone', 'Close', { duration: 1000 });
    }
  }

  redo(): void {
    const diagram = this.diagramService.redo();
    if (diagram) {
      this.currentDiagram = diagram;
      this.snackBar.open('Redone', 'Close', { duration: 1000 });
    }
  }

  // Keyboard shortcuts
  onKeyDown(event: KeyboardEvent): void {
    if (event.ctrlKey || event.metaKey) {
      switch (event.key) {
        case 'z':
          event.preventDefault();
          if (event.shiftKey) {
            this.redo();
          } else {
            this.undo();
          }
          break;
        case 's':
          event.preventDefault();
          this.saveDiagram();
          break;
        case 'n':
          event.preventDefault();
          this.createNewDiagram();
          break;
      }
    }
    
    if (event.key === 'F1') {
      event.preventDefault();
      this.toggleTutorialMode();
    }
  }

  // Component lifecycle
  ngOnDestroy(): void {
    // Clean up subscriptions if needed
  }
}

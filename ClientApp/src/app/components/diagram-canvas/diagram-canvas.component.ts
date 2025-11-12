import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { CdkDragDrop, CdkDragEnd, CdkDragStart } from '@angular/cdk/drag-drop';
import { Subject, takeUntil } from 'rxjs';
import { 
  Diagram, 
  DiagramComponent, 
  ComponentType, 
  DiagramEvent, 
  DiagramEventType,
  CanvasSettings,
  Connection,
  ComponentTemplate
} from '../../models/diagram.models';
import { DiagramService } from '../../services/diagram.service';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-diagram-canvas',
  templateUrl: './diagram-canvas.component.html',
  styleUrls: ['./diagram-canvas.component.css']
})
export class DiagramCanvasComponent implements OnInit, OnDestroy {
  @ViewChild('canvas', { static: true }) canvasRef!: ElementRef<HTMLCanvasElement>;
  @ViewChild('canvasContainer', { static: true }) containerRef!: ElementRef<HTMLDivElement>;
  
  @Input() diagram!: Diagram;
  @Input() selectedComponentId: string | null = null;
  @Input() isReadOnly: boolean = false;
  
  @Output() componentSelected = new EventEmitter<string | null>();
  @Output() componentAdded = new EventEmitter<DiagramComponent>();
  @Output() componentUpdated = new EventEmitter<DiagramComponent>();
  @Output() componentDeleted = new EventEmitter<string>();
  @Output() diagramChanged = new EventEmitter<DiagramEvent>();

  private destroy$ = new Subject<void>();
  private canvas!: HTMLCanvasElement;
  private ctx!: CanvasRenderingContext2D;
  private isDragging = false;
  private dragStartPos = { x: 0, y: 0 };
  private selectedComponent: DiagramComponent | null = null;
  private resizeHandles: ResizeHandle[] = [];
  private connectionMode = false;
  private connectionStart: DiagramComponent | null = null;

  // Canvas state
  canvasSettings: CanvasSettings = {
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

  constructor(private diagramService: DiagramService) {}

  ngOnInit(): void {
    this.initializeCanvas();
    this.setupEventListeners();
    this.render();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeCanvas(): void {
    this.canvas = this.canvasRef.nativeElement;
    this.ctx = this.canvas.getContext('2d')!;
    
    // Set canvas size
    this.canvas.width = this.canvasSettings.width;
    this.canvas.height = this.canvasSettings.height;
    
    // Apply canvas settings from diagram if available
    if (this.diagram?.canvas) {
      this.canvasSettings = { ...this.canvasSettings, ...this.diagram.canvas };
    }
  }

  private setupEventListeners(): void {
    // Mouse events
    this.canvas.addEventListener('mousedown', this.onMouseDown.bind(this));
    this.canvas.addEventListener('mousemove', this.onMouseMove.bind(this));
    this.canvas.addEventListener('mouseup', this.onMouseUp.bind(this));
    this.canvas.addEventListener('click', this.onClick.bind(this));
    this.canvas.addEventListener('dblclick', this.onDoubleClick.bind(this));
    
    // Keyboard events
    document.addEventListener('keydown', this.onKeyDown.bind(this));
    
    // Wheel event for zooming
    this.canvas.addEventListener('wheel', this.onWheel.bind(this));
  }

  private render(): void {
    this.clearCanvas();
    this.drawGrid();
    this.drawComponents();
    this.drawConnections();
    this.drawSelectionHandles();
  }

  private clearCanvas(): void {
    this.ctx.fillStyle = this.canvasSettings.backgroundColor;
    this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);
  }

  private drawGrid(): void {
    if (!this.canvasSettings.gridEnabled) return;

    this.ctx.strokeStyle = this.canvasSettings.gridColor;
    this.ctx.lineWidth = 1;
    this.ctx.setLineDash([]);

    const gridSize = this.canvasSettings.gridSize * this.canvasSettings.zoom;
    
    // Vertical lines
    for (let x = 0; x <= this.canvas.width; x += gridSize) {
      this.ctx.beginPath();
      this.ctx.moveTo(x, 0);
      this.ctx.lineTo(x, this.canvas.height);
      this.ctx.stroke();
    }
    
    // Horizontal lines
    for (let y = 0; y <= this.canvas.height; y += gridSize) {
      this.ctx.beginPath();
      this.ctx.moveTo(0, y);
      this.ctx.lineTo(this.canvas.width, y);
      this.ctx.stroke();
    }
  }

  private drawComponents(): void {
    if (!this.diagram?.components) return;

    this.diagram.components.forEach(component => {
      this.drawComponent(component);
    });
  }

  private drawComponent(component: DiagramComponent): void {
    this.ctx.save();
    
    // Apply transformations
    this.ctx.translate(component.x + component.width / 2, component.y + component.height / 2);
    this.ctx.rotate(component.rotation * Math.PI / 180);
    this.ctx.translate(-component.width / 2, -component.height / 2);
    
    // Apply styles
    this.ctx.fillStyle = component.style.fill || '#ffffff';
    this.ctx.strokeStyle = component.style.stroke || '#000000';
    this.ctx.lineWidth = component.style.strokeWidth || 1;
    this.ctx.globalAlpha = component.style.opacity || 1;

    // Draw based on component type
    switch (component.type) {
      case ComponentType.RECTANGLE:
        this.drawRectangle(component);
        break;
      case ComponentType.CIRCLE:
        this.drawCircle(component);
        break;
      case ComponentType.ELLIPSE:
        this.drawEllipse(component);
        break;
      case ComponentType.TRIANGLE:
        this.drawTriangle(component);
        break;
      case ComponentType.DIAMOND:
        this.drawDiamond(component);
        break;
      case ComponentType.TEXT:
        this.drawText(component);
        break;
      default:
        this.drawRectangle(component); // Default fallback
    }

    this.ctx.restore();
  }

  private drawRectangle(component: DiagramComponent): void {
    const borderRadius = component.style.borderRadius || 0;
    
    if (borderRadius > 0) {
      this.drawRoundedRect(0, 0, component.width, component.height, borderRadius);
    } else {
      this.ctx.fillRect(0, 0, component.width, component.height);
      this.ctx.strokeRect(0, 0, component.width, component.height);
    }
    
    this.drawComponentText(component);
  }

  private drawCircle(component: DiagramComponent): void {
    const radius = Math.min(component.width, component.height) / 2;
    const centerX = component.width / 2;
    const centerY = component.height / 2;
    
    this.ctx.beginPath();
    this.ctx.arc(centerX, centerY, radius, 0, 2 * Math.PI);
    this.ctx.fill();
    this.ctx.stroke();
    
    this.drawComponentText(component);
  }

  private drawEllipse(component: DiagramComponent): void {
    const centerX = component.width / 2;
    const centerY = component.height / 2;
    const radiusX = component.width / 2;
    const radiusY = component.height / 2;
    
    this.ctx.beginPath();
    this.ctx.ellipse(centerX, centerY, radiusX, radiusY, 0, 0, 2 * Math.PI);
    this.ctx.fill();
    this.ctx.stroke();
    
    this.drawComponentText(component);
  }

  private drawTriangle(component: DiagramComponent): void {
    this.ctx.beginPath();
    this.ctx.moveTo(component.width / 2, 0);
    this.ctx.lineTo(component.width, component.height);
    this.ctx.lineTo(0, component.height);
    this.ctx.closePath();
    this.ctx.fill();
    this.ctx.stroke();
    
    this.drawComponentText(component);
  }

  private drawDiamond(component: DiagramComponent): void {
    this.ctx.beginPath();
    this.ctx.moveTo(component.width / 2, 0);
    this.ctx.lineTo(component.width, component.height / 2);
    this.ctx.lineTo(component.width / 2, component.height);
    this.ctx.lineTo(0, component.height / 2);
    this.ctx.closePath();
    this.ctx.fill();
    this.ctx.stroke();
    
    this.drawComponentText(component);
  }

  private drawText(component: DiagramComponent): void {
    const text = component.properties.text || '';
    this.ctx.fillStyle = component.style.stroke || '#000000';
    this.ctx.font = `${component.style.fontSize || 14}px ${component.style.fontFamily || 'Arial'}`;
    this.ctx.textAlign = component.style.textAlign || 'center';
    this.ctx.textBaseline = 'middle';
    
    this.ctx.fillText(text, component.width / 2, component.height / 2);
  }

  private drawComponentText(component: DiagramComponent): void {
    const text = component.properties.text || component.properties.label || '';
    if (!text) return;

    this.ctx.fillStyle = component.style.stroke || '#000000';
    this.ctx.font = `${component.style.fontSize || 12}px ${component.style.fontFamily || 'Arial'}`;
    this.ctx.textAlign = 'center';
    this.ctx.textBaseline = 'middle';
    
    this.ctx.fillText(text, component.width / 2, component.height / 2);
  }

  private drawRoundedRect(x: number, y: number, width: number, height: number, radius: number): void {
    this.ctx.beginPath();
    this.ctx.moveTo(x + radius, y);
    this.ctx.lineTo(x + width - radius, y);
    this.ctx.quadraticCurveTo(x + width, y, x + width, y + radius);
    this.ctx.lineTo(x + width, y + height - radius);
    this.ctx.quadraticCurveTo(x + width, y + height, x + width - radius, y + height);
    this.ctx.lineTo(x + radius, y + height);
    this.ctx.quadraticCurveTo(x, y + height, x, y + height - radius);
    this.ctx.lineTo(x, y + radius);
    this.ctx.quadraticCurveTo(x, y, x + radius, y);
    this.ctx.closePath();
    this.ctx.fill();
    this.ctx.stroke();
  }

  private drawConnections(): void {
    if (!this.diagram?.connections) return;

    this.diagram.connections.forEach(connection => {
      this.drawConnection(connection);
    });
  }

  private drawConnection(connection: Connection): void {
    this.ctx.strokeStyle = connection.style.stroke;
    this.ctx.lineWidth = connection.style.strokeWidth;
    
    if (connection.style.strokeDashArray) {
      this.ctx.setLineDash(connection.style.strokeDashArray);
    } else {
      this.ctx.setLineDash([]);
    }

    this.ctx.beginPath();
    this.ctx.moveTo(connection.fromPoint.x, connection.fromPoint.y);
    this.ctx.lineTo(connection.toPoint.x, connection.toPoint.y);
    this.ctx.stroke();

    // Draw arrow if specified
    if (connection.style.arrowType === 'arrow') {
      this.drawArrow(connection.toPoint.x, connection.toPoint.y, connection.fromPoint.x, connection.fromPoint.y);
    }
  }

  private drawArrow(toX: number, toY: number, fromX: number, fromY: number): void {
    const angle = Math.atan2(toY - fromY, toX - fromX);
    const arrowLength = 10;
    const arrowAngle = Math.PI / 6;

    this.ctx.beginPath();
    this.ctx.moveTo(toX, toY);
    this.ctx.lineTo(
      toX - arrowLength * Math.cos(angle - arrowAngle),
      toY - arrowLength * Math.sin(angle - arrowAngle)
    );
    this.ctx.moveTo(toX, toY);
    this.ctx.lineTo(
      toX - arrowLength * Math.cos(angle + arrowAngle),
      toY - arrowLength * Math.sin(angle + arrowAngle)
    );
    this.ctx.stroke();
  }

  private drawSelectionHandles(): void {
    if (!this.selectedComponent) return;

    const component = this.selectedComponent;
    const handleSize = 8;
    
    this.ctx.fillStyle = '#007bff';
    this.ctx.strokeStyle = '#ffffff';
    this.ctx.lineWidth = 2;

    // Corner handles
    const handles = [
      { x: component.x - handleSize / 2, y: component.y - handleSize / 2 },
      { x: component.x + component.width - handleSize / 2, y: component.y - handleSize / 2 },
      { x: component.x + component.width - handleSize / 2, y: component.y + component.height - handleSize / 2 },
      { x: component.x - handleSize / 2, y: component.y + component.height - handleSize / 2 }
    ];

    handles.forEach(handle => {
      this.ctx.fillRect(handle.x, handle.y, handleSize, handleSize);
      this.ctx.strokeRect(handle.x, handle.y, handleSize, handleSize);
    });
  }

  // Event handlers
  private onMouseDown(event: MouseEvent): void {
    if (this.isReadOnly) return;

    const rect = this.canvas.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;

    const clickedComponent = this.getComponentAtPosition(x, y);
    
    if (clickedComponent) {
      this.selectComponent(clickedComponent);
      this.isDragging = true;
      this.dragStartPos = { x, y };
    } else {
      this.selectComponent(null);
    }
  }

  private onMouseMove(event: MouseEvent): void {
    if (!this.isDragging || !this.selectedComponent) return;

    const rect = this.canvas.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;

    const deltaX = x - this.dragStartPos.x;
    const deltaY = y - this.dragStartPos.y;

    this.selectedComponent.x += deltaX;
    this.selectedComponent.y += deltaY;

    if (this.canvasSettings.snapToGrid) {
      this.selectedComponent.x = Math.round(this.selectedComponent.x / this.canvasSettings.gridSize) * this.canvasSettings.gridSize;
      this.selectedComponent.y = Math.round(this.selectedComponent.y / this.canvasSettings.gridSize) * this.canvasSettings.gridSize;
    }

    this.dragStartPos = { x, y };
    this.render();
    
    this.componentUpdated.emit(this.selectedComponent);
    this.emitDiagramEvent(DiagramEventType.COMPONENT_MOVED, this.selectedComponent.id);
  }

  private onMouseUp(event: MouseEvent): void {
    this.isDragging = false;
  }

  private onClick(event: MouseEvent): void {
    // Handle single click events
  }

  private onDoubleClick(event: MouseEvent): void {
    const rect = this.canvas.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;

    const clickedComponent = this.getComponentAtPosition(x, y);
    
    if (clickedComponent && clickedComponent.type === ComponentType.TEXT) {
      // Enable text editing
      this.enableTextEditing(clickedComponent);
    }
  }

  private onKeyDown(event: KeyboardEvent): void {
    if (this.isReadOnly) return;

    switch (event.key) {
      case 'Delete':
        if (this.selectedComponent) {
          this.deleteComponent(this.selectedComponent.id);
        }
        break;
      case 'Escape':
        this.selectComponent(null);
        break;
    }
  }

  private onWheel(event: WheelEvent): void {
    event.preventDefault();
    
    const zoomFactor = event.deltaY > 0 ? 0.9 : 1.1;
    this.canvasSettings.zoom = Math.max(0.1, Math.min(3, this.canvasSettings.zoom * zoomFactor));
    
    this.render();
    this.emitDiagramEvent(DiagramEventType.CANVAS_ZOOMED);
  }

  // Public methods
  addComponent(template: ComponentTemplate, x: number, y: number): void {
    const component: DiagramComponent = {
      id: uuidv4(),
      type: template.type,
      x,
      y,
      width: template.defaultSize.width,
      height: template.defaultSize.height,
      rotation: 0,
      properties: { ...template.defaultProperties },
      style: { ...template.defaultStyle },
      connections: []
    };

    if (this.canvasSettings.snapToGrid) {
      component.x = Math.round(component.x / this.canvasSettings.gridSize) * this.canvasSettings.gridSize;
      component.y = Math.round(component.y / this.canvasSettings.gridSize) * this.canvasSettings.gridSize;
    }

    this.diagram.components.push(component);
    this.render();
    
    this.componentAdded.emit(component);
    this.emitDiagramEvent(DiagramEventType.COMPONENT_ADDED, component.id);
  }

  deleteComponent(componentId: string): void {
    const index = this.diagram.components.findIndex(c => c.id === componentId);
    if (index !== -1) {
      this.diagram.components.splice(index, 1);
      
      // Remove connections involving this component
      this.diagram.connections = this.diagram.connections.filter(
        conn => conn.fromComponentId !== componentId && conn.toComponentId !== componentId
      );
      
      if (this.selectedComponent?.id === componentId) {
        this.selectComponent(null);
      }
      
      this.render();
      this.componentDeleted.emit(componentId);
      this.emitDiagramEvent(DiagramEventType.COMPONENT_REMOVED, componentId);
    }
  }

  selectComponent(component: DiagramComponent | null): void {
    this.selectedComponent = component;
    this.selectedComponentId = component?.id || null;
    this.componentSelected.emit(this.selectedComponentId);
    this.render();
  }

  private getComponentAtPosition(x: number, y: number): DiagramComponent | null {
    // Check components in reverse order (top to bottom)
    for (let i = this.diagram.components.length - 1; i >= 0; i--) {
      const component = this.diagram.components[i];
      if (x >= component.x && x <= component.x + component.width &&
          y >= component.y && y <= component.y + component.height) {
        return component;
      }
    }
    return null;
  }

  private enableTextEditing(component: DiagramComponent): void {
    // Create temporary input element for text editing
    const input = document.createElement('input');
    input.type = 'text';
    input.value = component.properties.text || '';
    input.style.position = 'absolute';
    input.style.left = `${component.x}px`;
    input.style.top = `${component.y}px`;
    input.style.width = `${component.width}px`;
    input.style.height = `${component.height}px`;
    input.style.fontSize = `${component.style.fontSize || 12}px`;
    input.style.textAlign = component.style.textAlign || 'center';
    input.style.border = '2px solid #007bff';
    input.style.background = 'transparent';
    input.style.zIndex = '1000';

    this.containerRef.nativeElement.appendChild(input);
    input.focus();
    input.select();

    const finishEditing = () => {
      component.properties.text = input.value;
      this.containerRef.nativeElement.removeChild(input);
      this.render();
      this.componentUpdated.emit(component);
    };

    input.addEventListener('blur', finishEditing);
    input.addEventListener('keydown', (e) => {
      if (e.key === 'Enter') {
        finishEditing();
      }
    });
  }

  private emitDiagramEvent(type: DiagramEventType, componentId?: string): void {
    const event: DiagramEvent = {
      type,
      componentId,
      timestamp: new Date()
    };
    this.diagramChanged.emit(event);
  }

  // Canvas manipulation methods
  zoomIn(): void {
    this.canvasSettings.zoom = Math.min(3, this.canvasSettings.zoom * 1.2);
    this.render();
  }

  zoomOut(): void {
    this.canvasSettings.zoom = Math.max(0.1, this.canvasSettings.zoom * 0.8);
    this.render();
  }

  resetZoom(): void {
    this.canvasSettings.zoom = 1;
    this.canvasSettings.panX = 0;
    this.canvasSettings.panY = 0;
    this.render();
  }

  toggleGrid(): void {
    this.canvasSettings.gridEnabled = !this.canvasSettings.gridEnabled;
    this.render();
  }

  toggleSnapToGrid(): void {
    this.canvasSettings.snapToGrid = !this.canvasSettings.snapToGrid;
  }
}

interface ResizeHandle {
  x: number;
  y: number;
  cursor: string;
}

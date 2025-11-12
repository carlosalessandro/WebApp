export interface DiagramComponent {
  id: string;
  type: ComponentType;
  x: number;
  y: number;
  width: number;
  height: number;
  rotation: number;
  properties: ComponentProperties;
  style: ComponentStyle;
  connections: Connection[];
}

export interface ComponentProperties {
  [key: string]: any;
  text?: string;
  label?: string;
  value?: any;
  placeholder?: string;
  required?: boolean;
  disabled?: boolean;
}

export interface ComponentStyle {
  fill?: string;
  stroke?: string;
  strokeWidth?: number;
  opacity?: number;
  fontSize?: number;
  fontFamily?: string;
  fontWeight?: string;
  textAlign?: 'left' | 'center' | 'right';
  borderRadius?: number;
  shadow?: boolean;
  shadowColor?: string;
  shadowBlur?: number;
  shadowOffset?: { x: number; y: number };
}

export interface Connection {
  id: string;
  fromComponentId: string;
  toComponentId: string;
  fromPoint: ConnectionPoint;
  toPoint: ConnectionPoint;
  style: ConnectionStyle;
  label?: string;
}

export interface ConnectionPoint {
  x: number;
  y: number;
  side: 'top' | 'right' | 'bottom' | 'left';
}

export interface ConnectionStyle {
  stroke: string;
  strokeWidth: number;
  strokeDashArray?: number[];
  arrowType: 'none' | 'arrow' | 'diamond' | 'circle';
}

export enum ComponentType {
  // Basic Shapes
  RECTANGLE = 'rectangle',
  CIRCLE = 'circle',
  ELLIPSE = 'ellipse',
  TRIANGLE = 'triangle',
  DIAMOND = 'diamond',
  POLYGON = 'polygon',
  
  // Text Elements
  TEXT = 'text',
  LABEL = 'label',
  TITLE = 'title',
  
  // Flowchart Elements
  PROCESS = 'process',
  DECISION = 'decision',
  START_END = 'start_end',
  CONNECTOR = 'connector',
  
  // UML Elements
  CLASS = 'class',
  INTERFACE = 'interface',
  ACTOR = 'actor',
  USE_CASE = 'use_case',
  
  // Network Elements
  SERVER = 'server',
  DATABASE = 'database',
  CLOUD = 'cloud',
  ROUTER = 'router',
  
  // UI Elements
  BUTTON = 'button',
  INPUT = 'input',
  CHECKBOX = 'checkbox',
  RADIO = 'radio',
  DROPDOWN = 'dropdown',
  
  // Custom
  CUSTOM = 'custom'
}

export interface ComponentPalette {
  id: string;
  name: string;
  description: string;
  category: string;
  components: ComponentTemplate[];
  isCustom: boolean;
  createdBy?: string;
  createdAt?: Date;
}

export interface ComponentTemplate {
  id: string;
  name: string;
  type: ComponentType;
  icon: string;
  defaultProperties: ComponentProperties;
  defaultStyle: ComponentStyle;
  defaultSize: { width: number; height: number };
  propertySchema: PropertySchema[];
}

export interface PropertySchema {
  key: string;
  label: string;
  type: 'text' | 'number' | 'color' | 'boolean' | 'select' | 'range';
  defaultValue: any;
  options?: { label: string; value: any }[];
  min?: number;
  max?: number;
  step?: number;
  required?: boolean;
  description?: string;
}

export interface Diagram {
  id: string;
  name: string;
  description?: string;
  components: DiagramComponent[];
  connections: Connection[];
  canvas: CanvasSettings;
  metadata: DiagramMetadata;
}

export interface CanvasSettings {
  width: number;
  height: number;
  backgroundColor: string;
  gridEnabled: boolean;
  gridSize: number;
  gridColor: string;
  snapToGrid: boolean;
  zoom: number;
  panX: number;
  panY: number;
}

export interface DiagramMetadata {
  createdAt: Date;
  updatedAt: Date;
  createdBy: string;
  version: string;
  tags: string[];
  isPublic: boolean;
}

export interface DiagramEvent {
  type: DiagramEventType;
  componentId?: string;
  data?: any;
  timestamp: Date;
}

export enum DiagramEventType {
  COMPONENT_ADDED = 'component_added',
  COMPONENT_REMOVED = 'component_removed',
  COMPONENT_MOVED = 'component_moved',
  COMPONENT_RESIZED = 'component_resized',
  COMPONENT_ROTATED = 'component_rotated',
  COMPONENT_STYLED = 'component_styled',
  COMPONENT_CONNECTED = 'component_connected',
  CONNECTION_REMOVED = 'connection_removed',
  CANVAS_ZOOMED = 'canvas_zoomed',
  CANVAS_PANNED = 'canvas_panned'
}

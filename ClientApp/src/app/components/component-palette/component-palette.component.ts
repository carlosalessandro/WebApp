import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { CdkDragStart, CdkDragEnd } from '@angular/cdk/drag-drop';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { 
  ComponentPalette, 
  ComponentTemplate, 
  ComponentType 
} from '../../models/diagram.models';
import { DiagramService } from '../../services/diagram.service';
import { CreatePaletteDialogComponent } from '../create-palette-dialog/create-palette-dialog.component';

@Component({
  selector: 'app-component-palette',
  templateUrl: './component-palette.component.html',
  styleUrls: ['./component-palette.component.css']
})
export class ComponentPaletteComponent implements OnInit {
  @Input() isReadOnly: boolean = false;
  @Output() componentSelected = new EventEmitter<ComponentTemplate>();
  @Output() componentDragStart = new EventEmitter<ComponentTemplate>();
  @Output() componentDragEnd = new EventEmitter<void>();

  palettes$: Observable<ComponentPalette[]>;
  selectedPaletteId: string | null = null;
  selectedTemplate: ComponentTemplate | null = null;
  searchTerm: string = '';
  filteredPalettes: ComponentPalette[] = [];
  expandedPalettes: Set<string> = new Set();

  constructor(
    private diagramService: DiagramService,
    private dialog: MatDialog
  ) {
    this.palettes$ = this.diagramService.getComponentPalettes();
  }

  ngOnInit(): void {
    this.palettes$.subscribe(palettes => {
      this.filteredPalettes = this.filterPalettes(palettes);
      // Expand first palette by default
      if (palettes.length > 0 && this.expandedPalettes.size === 0) {
        this.expandedPalettes.add(palettes[0].id);
      }
    });
  }

  onSearchChange(): void {
    this.palettes$.subscribe(palettes => {
      this.filteredPalettes = this.filterPalettes(palettes);
    });
  }

  private filterPalettes(palettes: ComponentPalette[]): ComponentPalette[] {
    if (!this.searchTerm.trim()) {
      return palettes;
    }

    const searchLower = this.searchTerm.toLowerCase();
    return palettes.map(palette => ({
      ...palette,
      components: palette.components.filter(component =>
        component.name.toLowerCase().includes(searchLower) ||
        component.type.toLowerCase().includes(searchLower)
      )
    })).filter(palette => palette.components.length > 0);
  }

  togglePalette(paletteId: string): void {
    if (this.expandedPalettes.has(paletteId)) {
      this.expandedPalettes.delete(paletteId);
    } else {
      this.expandedPalettes.add(paletteId);
    }
  }

  isPaletteExpanded(paletteId: string): boolean {
    return this.expandedPalettes.has(paletteId);
  }

  selectTemplate(template: ComponentTemplate): void {
    this.selectedTemplate = template;
    this.componentSelected.emit(template);
  }

  onDragStart(event: CdkDragStart, template: ComponentTemplate): void {
    this.selectedTemplate = template;
    this.componentDragStart.emit(template);
  }

  onDragEnd(event: CdkDragEnd): void {
    this.componentDragEnd.emit();
  }

  createCustomPalette(): void {
    const dialogRef = this.dialog.open(CreatePaletteDialogComponent, {
      width: '400px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const palette = this.diagramService.createCustomPalette(result.name, result.description);
        this.expandedPalettes.add(palette.id);
      }
    });
  }

  editPalette(palette: ComponentPalette): void {
    if (!palette.isCustom) return;

    const dialogRef = this.dialog.open(CreatePaletteDialogComponent, {
      width: '400px',
      data: { palette }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Update palette logic would go here
        console.log('Update palette:', result);
      }
    });
  }

  deletePalette(palette: ComponentPalette): void {
    if (!palette.isCustom) return;
    
    if (confirm(`Are you sure you want to delete the palette "${palette.name}"?`)) {
      // Delete palette logic would go here
      console.log('Delete palette:', palette.id);
    }
  }

  addComponentToPalette(paletteId: string): void {
    // This would open a dialog to create a custom component
    console.log('Add component to palette:', paletteId);
  }

  getComponentIcon(type: ComponentType): string {
    const iconMap: { [key in ComponentType]: string } = {
      [ComponentType.RECTANGLE]: 'crop_din',
      [ComponentType.CIRCLE]: 'radio_button_unchecked',
      [ComponentType.ELLIPSE]: 'panorama_fish_eye',
      [ComponentType.TRIANGLE]: 'change_history',
      [ComponentType.DIAMOND]: 'crop_rotate',
      [ComponentType.POLYGON]: 'hexagon',
      [ComponentType.TEXT]: 'text_fields',
      [ComponentType.LABEL]: 'label',
      [ComponentType.TITLE]: 'title',
      [ComponentType.PROCESS]: 'crop_din',
      [ComponentType.DECISION]: 'crop_rotate',
      [ComponentType.START_END]: 'radio_button_unchecked',
      [ComponentType.CONNECTOR]: 'timeline',
      [ComponentType.CLASS]: 'view_module',
      [ComponentType.INTERFACE]: 'view_stream',
      [ComponentType.ACTOR]: 'person',
      [ComponentType.USE_CASE]: 'radio_button_unchecked',
      [ComponentType.SERVER]: 'dns',
      [ComponentType.DATABASE]: 'storage',
      [ComponentType.CLOUD]: 'cloud',
      [ComponentType.ROUTER]: 'router',
      [ComponentType.BUTTON]: 'smart_button',
      [ComponentType.INPUT]: 'input',
      [ComponentType.CHECKBOX]: 'check_box',
      [ComponentType.RADIO]: 'radio_button_checked',
      [ComponentType.DROPDOWN]: 'arrow_drop_down',
      [ComponentType.CUSTOM]: 'extension'
    };

    return iconMap[type] || 'help';
  }

  getComponentPreviewStyle(template: ComponentTemplate): any {
    return {
      'background-color': template.defaultStyle.fill || '#ffffff',
      'border': `${template.defaultStyle.strokeWidth || 1}px solid ${template.defaultStyle.stroke || '#000000'}`,
      'border-radius': template.defaultStyle.borderRadius ? `${template.defaultStyle.borderRadius}px` : '0',
      'opacity': template.defaultStyle.opacity || 1
    };
  }

  trackByPaletteId(index: number, palette: ComponentPalette): string {
    return palette.id;
  }

  trackByTemplateId(index: number, template: ComponentTemplate): string {
    return template.id;
  }
}

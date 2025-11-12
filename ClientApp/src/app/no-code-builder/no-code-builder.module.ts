import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatMenuModule } from '@angular/material/menu';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

// Components
import { NoCodeAppComponent } from '../components/no-code-app/no-code-app.component';
import { DiagramCanvasComponent } from '../components/diagram-canvas/diagram-canvas.component';
import { ComponentPaletteComponent } from '../components/component-palette/component-palette.component';
import { SqlQueryBuilderComponent } from '../components/sql-query-builder/sql-query-builder.component';

// Services
import { DiagramService } from '../services/diagram.service';

@NgModule({
  declarations: [
    NoCodeAppComponent,
    DiagramCanvasComponent,
    ComponentPaletteComponent,
    SqlQueryBuilderComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DragDropModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatExpansionModule,
    MatMenuModule,
    MatSnackBarModule,
    MatDialogModule,
    MatProgressSpinnerModule
  ],
  providers: [
    DiagramService
  ],
  exports: [
    NoCodeAppComponent
  ]
})
export class NoCodeBuilderModule { }

# No-Code Builder System

## Overview

This is a comprehensive no-code system that allows users to create diagrams and build SQL queries visually without writing code. The system is similar to draw.io for diagrams and provides an educational SQL query builder.

## Features

### 🎨 Diagram Builder
- **Visual Canvas**: HTML5 Canvas-based diagram editor with zoom, pan, and grid functionality
- **Component Palette**: Extensive library of pre-built components including:
  - Basic shapes (rectangles, circles, triangles, diamonds)
  - Flowchart elements (process, decision, start/end)
  - UML components (classes, actors, use cases)
  - Network elements (servers, databases, cloud)
  - UI components (buttons, inputs, forms)
- **Custom Palettes**: Users can create and save their own component palettes
- **Drag & Drop**: Intuitive drag-and-drop interface for adding components
- **Property Editor**: Real-time editing of component properties (text, colors, sizes)
- **Connections**: Visual connections between components with different arrow types
- **Export Options**: Export diagrams as JSON, SVG, or PNG
- **Undo/Redo**: Full history management with keyboard shortcuts

### 🗄️ SQL Query Builder
- **Visual Interface**: Build complex SQL queries without writing code
- **Educational Mode**: Learn SQL commands with tooltips and explanations
- **Table Management**: Visual representation of database tables and relationships
- **Query Types**: Support for SELECT, INSERT, UPDATE, DELETE operations
- **Join Builder**: Visual join creation with different join types
- **Condition Builder**: Drag-and-drop WHERE clause construction
- **SQL Preview**: Real-time SQL generation and formatting
- **Query Execution**: Execute queries and view results
- **Query History**: Save and load previously built queries

### 🎯 User Experience
- **Tutorial Mode**: Interactive tutorials for new users
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Keyboard Shortcuts**: Power user features with keyboard shortcuts
- **Modern UI**: Material Design components with Angular Material
- **Real-time Collaboration**: Multiple users can work on the same diagram
- **Auto-save**: Automatic saving of work in progress

## Technology Stack

### Frontend
- **Angular 17**: Modern web framework
- **Angular Material**: UI component library
- **Angular CDK**: Drag & drop functionality
- **TypeScript**: Type-safe development
- **RxJS**: Reactive programming
- **HTML5 Canvas**: High-performance diagram rendering

### Backend
- **ASP.NET Core 8**: Web API framework
- **Entity Framework Core**: Database ORM
- **SQLite**: Development database
- **C#**: Server-side language

## Architecture

### Frontend Architecture
```
src/app/
├── components/
│   ├── no-code-app/              # Main application component
│   ├── diagram-canvas/           # Canvas for diagram editing
│   ├── component-palette/        # Component library sidebar
│   └── sql-query-builder/        # SQL builder interface
├── models/
│   ├── diagram.models.ts         # Diagram-related interfaces
│   └── sql-builder.models.ts     # SQL builder interfaces
├── services/
│   ├── diagram.service.ts        # Diagram operations
│   └── sql-builder.service.ts    # SQL operations
└── no-code-builder/
    └── no-code-builder.module.ts # Angular module
```

### Backend Architecture
```
Controllers/
├── DiagramController.cs          # Diagram CRUD operations
├── SqlBuilderController.cs       # SQL operations
└── NoCodeController.cs           # Main view controller

Models/
├── DiagramModel.cs              # Diagram entity
├── SqlQueryModel.cs             # SQL query entity
└── ComponentPaletteModel.cs     # Component palette entity
```

## Database Schema

### Diagrams Table
- `Id` (string): Unique identifier
- `Name` (string): Diagram name
- `Description` (string): Optional description
- `Components` (JSON): Serialized components
- `Connections` (JSON): Serialized connections
- `CanvasSettings` (JSON): Canvas configuration
- `CreatedAt`, `UpdatedAt`: Timestamps
- `CreatedBy`: User identifier
- `Version`: Diagram version
- `Tags`: Search tags
- `IsPublic`: Visibility flag

### SqlQueries Table
- `Id` (string): Unique identifier
- `Name` (string): Query name
- `Type` (string): Query type (SELECT, INSERT, etc.)
- `Tables`, `Fields`, `Joins`, `Conditions`: JSON arrays
- `RawSql`: Generated SQL
- `ExecutionTime`, `ResultCount`: Performance metrics
- `IsValid`: Validation status

### ComponentPalettes Table
- `Id` (string): Unique identifier
- `Name` (string): Palette name
- `Components` (JSON): Component definitions
- `IsCustom`: User-created flag
- `CreatedBy`: User identifier

## API Endpoints

### Diagram API
- `GET /api/diagram` - List all diagrams
- `GET /api/diagram/{id}` - Get specific diagram
- `POST /api/diagram` - Create new diagram
- `PUT /api/diagram/{id}` - Update diagram
- `DELETE /api/diagram/{id}` - Delete diagram
- `POST /api/diagram/{id}/export` - Export diagram

### SQL Builder API
- `GET /api/sqlbuilder/tables` - Get database tables
- `POST /api/sqlbuilder/execute` - Execute SQL query
- `POST /api/sqlbuilder/validate` - Validate SQL query

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- Angular CLI 17

### Installation
1. Clone the repository
2. Install backend dependencies:
   ```bash
   cd WebApp
   dotnet restore
   ```
3. Install frontend dependencies:
   ```bash
   cd ClientApp
   npm install
   ```
4. Run database migrations:
   ```bash
   dotnet ef database update
   ```
5. Start the application:
   ```bash
   dotnet run
   ```

### Access the Application
- Main application: `https://localhost:5001`
- No-Code Builder: `https://localhost:5001/NoCode`
- API documentation: `https://localhost:5001/swagger`

## Usage Examples

### Creating a Flowchart
1. Navigate to the Diagram Builder tab
2. Drag a "Start/End" component from the palette
3. Add "Process" and "Decision" components
4. Connect components by clicking and dragging
5. Edit component text by double-clicking
6. Save and export your diagram

### Building a SQL Query
1. Navigate to the SQL Query Builder tab
2. Select tables from the sidebar
3. Choose fields to include in SELECT
4. Add JOIN conditions visually
5. Build WHERE clauses with the condition builder
6. Preview and execute the generated SQL

## Customization

### Adding New Component Types
1. Define the component type in `ComponentType` enum
2. Add rendering logic in `DiagramCanvasComponent`
3. Create component template in `DiagramService`
4. Add icon mapping in `ComponentPaletteComponent`

### Extending SQL Builder
1. Add new query types to `QueryType` enum
2. Implement generation logic in `SqlBuilderService`
3. Update UI components for new features

## Performance Considerations

- Canvas rendering is optimized for 1000+ components
- SQL queries are validated before execution
- Automatic debouncing for real-time updates
- Lazy loading of component palettes
- Efficient change detection with OnPush strategy

## Security Features

- SQL injection prevention through parameterized queries
- User authentication and authorization
- Input validation and sanitization
- CORS configuration for API access
- Secure file upload and export

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support and questions:
- Create an issue on GitHub
- Check the documentation
- Review the tutorial mode in the application

---

**Built with ❤️ using Angular and ASP.NET Core**

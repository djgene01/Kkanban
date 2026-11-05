# Kkanban

A personal Kanban board built with **Blazor Server** (.NET 10) and **SQLite**.

Manage your tasks with drag-and-drop columns and cards, labels, assignees, search, filtering, archiving, and a dark/light theme toggle.

## Features

- **Kanban Board** — default columns: To Do, In Progress, Done
- **Columns** — add, rename, reorder (drag-and-drop), and delete columns
- **Cards** — create, edit, delete with title, description, colour-coded labels, and assignee
- **Drag & Drop** — move cards between columns and reorder within columns
- **Archive** — soft-delete cards and columns with restore capability
- **Search & Filter** — search by title/description, filter by label and assignee
- **Theme** — dark/light mode toggle with persistence across reloads
- **Single-User** — no auth, no backend, just your data in a SQLite file

## Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | Blazor Server (.NET 10) |
| Database | SQLite via Entity Framework Core 10 |
| Drag-and-Drop | BlazorSortable 8.x |
| Styling | Plain CSS variables (custom, no framework) |

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Getting Started

### Clone and restore

```bash
git clone https://github.com/djgene01/Kkanban.git
cd Kkanban
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

The app starts at `http://localhost:5000` by default.

### Database

The SQLite database file (`Data/kkanban.db`) is created automatically on first run. Default columns ("To Do", "In Progress", "Done") and colour labels are seeded.

To reset the database, delete `Data/kkanban.db` and restart the app.

## Project Structure

```
Kkanban/
├── Program.cs                   # App entry point, DI setup
├── appsettings.json             # Configuration (connection string)
├── GlobalUsings.cs              # Shared global using directives
├── Data/
│   ├── Models/                  # Entity classes
│   │   ├── BoardColumn.cs       # Column with title, order, soft-delete
│   │   ├── KanbanCard.cs        # Card with title, description, labels, assignee
│   │   ├── Label.cs             # Shared label entity (name + colour)
│   │   └── CardLabel.cs         # Junction table for card-label M2M
│   ├── KkanbanContext.cs        # EF DbContext with relationships + seed data
│   └── Migrations/              # EF Core migrations
├── Services/                    # Application services (IBoardService, ICardService, etc.)
├── Components/
│   ├── Layout/                  # MainLayout.razor, _Imports.razor
│   ├── Pages/
│   │   ├── Index.razor          # Main Kanban board page
│   │   └── Archive.razor        # Archive management page
│   └── Shared/
│       ├── BoardColumn.razor    # Column component (header + card list)
│       ├── KanbanCard.razor     # Card display component
│       ├── CardEditor.razor     # Modal for create/edit card
│       ├── ColumnEditor.razor   # Modal for create/rename column
│       ├── SearchBar.razor      # Search + filter controls
│       └── ThemeToggle.razor    # Dark/light theme switch
├── wwwroot/
│   ├── css/
│   │   ├── app.css              # Light theme (CSS variables)
│   │   └── dark.css             # Dark theme overrides
│   └── app.css                  # Shared layout styles
├── docs/                        # Full project documentation
│   ├── 01-requirements.md
│   ├── 02-architecture.md
│   ├── 03-research-and-reuse.md
│   ├── 04-phased-implementation.md
│   ├── 05-testing-guidance.md
│   ├── 06-deployment-guidance.md
│   ├── 07-risks-and-acceptance-checks.md
│   └── phase-plan.json
├── Kkanban.csproj
├── LICENSE
└── README.md
```

## Development

### Adding a new column

Use the "Add Column" button on the board. A modal prompts for the title.

### Adding a card

Click "Add card" on any column. The editor dialog lets you set:
- **Title** (required)
- **Description** (plain text textarea)
- **Labels** (click colour chips to toggle)
- **Assignee** (free text)

### Drag and Drop

- **Cards**: drag from one column to another, or reorder within a column
- **Columns**: drag a column header left or right to reorder columns

### Archiving

Cards and columns can be archived (soft-delete). They disappear from the main board and can be restored from the Archive page (`/Archive`).

### Theme

Toggle dark/light mode with the theme switch in the top-right corner. The preference persists across page reloads via localStorage.

### Search and Filters

Use the search bar at the top of the board:
- Type to search by card title or description
- Select label(s) to filter by colour/category
- Select an assignee to filter by person
- All filters combine with AND logic

## Deployment

### Local

```bash
dotnet publish -c Release -o out
cd out
dotnet Kkanban.dll
```

### Docker

```bash
docker compose up --build
```

See `docs/06-deployment-guidance.md` for Windows Service and Azure App Service options.

## Documentation

Full documentation is in the `docs/` folder:

| File | Contents |
|------|----------|
| `01-requirements.md` | Complete requirements spec with mapped IDs |
| `02-architecture.md` | Three-tier architecture, data model, service design |
| `03-research-and-reuse.md` | NuGet evaluations, reference projects, code patterns |
| `04-phased-implementation.md` | Six-phase implementation plan with task lists |
| `05-testing-guidance.md` | Manual test procedures and acceptance criteria |
| `06-deployment-guidance.md` | Local, Docker, Azure deployment options |
| `07-risks-and-acceptance-checks.md` | Risk register with mitigations and sign-off table |
| `phase-plan.json` | Machine-readable phase plan with gates and decision points |

## License

MIT License — see [LICENSE](LICENSE)

# Agent Instructions

Instructions for AI coding agents working on this project.

## Critical Rules

### NEVER Push Without Explicit Confirmation

**Do NOT run `git push`, `git push origin`, or any push command unless the user EXPLICITLY asks for it.**  
This is non-negotiable. Commits are fine. Pushes require explicit human approval.

Acceptable:
- `git add`, `git commit` — always fine
- `git branch`, `git checkout`, `git switch` — always fine
- `git push` — **ONLY when the user says "push", "push it", "push to remote", etc.**

### Ask Before Destructive Operations

Do not run destructive git commands without confirmation:
- `git reset --hard`
- `git force-push`
- `git rebase` on shared branches
- `git clean -fd`
- Deleting branches that have been pushed

## Project Context

This is **Terminal.Forms**, a C# WinForms-like UI framework for console/terminal applications originally written in 2009 targeting .NET Framework 2.0. It is being modernized to .NET 10 with AI assistance.

### Repository Layout

```
console-forms/                  # Repository root
├── Terminal.Forms.slnx          # Solution file (.NET 10 XML format)
├── global.json                 # SDK version pin
├── Directory.Build.props        # Shared project properties (net10.0-windows, nullable, etc.)
├── Directory.Packages.props     # Central package management
├── .editorconfig               # Code style rules
├── src/
│   └── Terminal.Forms/          # Core library
│       ├── Terminal.Forms.csproj
│       ├── Drawing/            # ConsoleCanvas, ConsoleGraphics, ConsoleElement
│       ├── Events/             # Custom event args and handlers
│       └── *.cs                # Controls: Form, Button, Label, TextBox, etc.
├── samples/
│   ├── NanoSharp/              # Demo app — nano-like text editor
│   └── SharpEdit/              # Demo app — simple text editor
├── tests/
│   └── Terminal.Forms.Tests/    # xUnit test project
└── docs/                       # Documentation (architecture, plans, issues, guides)
```

### Current State

- **Phase 0 (Documentation)** is complete
- **Phase 1 (Port to .NET 10)** is complete — SDK-style projects, modern layout
- See `docs/plans/00-roadmap.md` for the full modernization plan

### Key Technical Details

- **Target Framework**: `net10.0-windows` (temporary — will become `net10.0` after Phase 2)
- **Namespace**: `Terminal.Forms`
- **Dependencies to remove**: `System.Windows.Forms`, `System.Drawing`, `System.ComponentModel`
- **Rendering**: Double-buffered via `ConsoleCanvas` singleton (hardcoded 80×25)
- **Known bug**: `Control.ForeColor` getter returns `parent.BackColor` instead of `parent.ForeColor`
- **Branches**: `main` (active), `legacy/original` (preserved 2009 code)

## Git Conventions

### Commit Messages

Use [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: add CheckBox control
fix: correct ForeColor inheritance from parent
refactor: convert Control.cs to file-scoped namespace
docs: update Phase 1 plan with progress
chore: remove .suo and .user files from tracking
test: add unit tests for ControlCollection
```

### Branching

- `main` — main development branch
- `legacy/original` — untouched 2009 code (never modify)
- Feature work: `phase-2/remove-winforms`, `phase-3/modern-csharp`, etc.

## Code Style

- Follow `.editorconfig` in the repo root
- Private fields: `m_` prefix (existing convention from 2009 code, keep for now)
- After Phase 3: migrate to `_camelCase` per modern C# conventions
- Use XML doc comments on all public APIs
- Prefer `string.Empty` over `""`

## Build & Test

```powershell
# Git is not on PATH — use the SmartGit-bundled git:
& "C:\Program Files\SmartGit\git\cmd\git.exe" <command>

# Build the entire solution:
dotnet build Terminal.Forms.slnx

# Run tests:
dotnet test Terminal.Forms.slnx

# Run a specific sample:
dotnet run --project samples/NanoSharp
dotnet run --project samples/SharpEdit
```

## Documentation

- All plans are in `docs/plans/`
- All known issues are in `docs/issues/known-issues.md`
- Update `CHANGELOG.md` when making notable changes
- Update `docs/plans/00-roadmap.md` checklist as phases progress

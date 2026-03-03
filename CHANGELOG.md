# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Project documentation (README, architecture docs, modernization roadmap)
- `.gitignore`, `.editorconfig`, `LICENSE`, `CONTRIBUTING.md`
- `docs/` folder structure with plans, architecture, issues, and guides
- Detailed phase plans for all 6 modernization phases
- AI revival narrative in README ("Why This Project Exists (Again)")
- `legacy/original` branch preserving the original 2009 codebase

### Removed
- Tracked IDE artifacts (`.suo`, `.csproj.user`) from git index

## [0.1.0] - 2026-03-03 (Phase 1: Port to .NET 10)

### Added
- `global.json` — pins .NET SDK 10.0
- `Directory.Build.props` — shared project properties (net10.0-windows, nullable, ImplicitUsings, etc.)
- `Directory.Packages.props` — central NuGet package management
- `Terminal.Forms.slnx` — .NET 10 XML solution format at repo root
- `tests/Terminal.Forms.Tests/` — xUnit test project with sanity tests
- SDK-style `.csproj` files for all projects

### Changed
- Reorganized project to modern `src/` / `samples/` / `tests/` layout
- Ported all projects from .NET Framework 2.0 to .NET 10 (`net10.0-windows`)
- Library project renamed from `Crsouza.Console.Forms.csproj` to `Terminal.Forms.csproj`
- Solution file moved from `Console.Forms/Console.Forms.sln` to repo root `Terminal.Forms.slnx`
- Namespace renamed from `Crsouza.Console.Forms` to `Terminal.Forms`

### Removed
- Old `Console.Forms/` solution wrapper directory (double-nested structure)
- `Properties/AssemblyInfo.cs` files (SDK auto-generates these)
- `app.config` files (empty/unused)
- `ClassDiagram.cd` (Visual Studio 2008 specific)
- Old `.sln` file (replaced by `.slnx`)

### Fixed
- Missing `#endif` in `Drawing/Size.cs`
- Ambiguous type references between `Terminal.Forms` and `System.Windows.Forms` in sample projects

---

## [0.0.0.1] - 2009-xx-xx (Original Release)

### Added
- Core framework: `Application`, `ApplicationContext`, `Control`, `ContainerControl`, `Form`
- Controls: `Button`, `Label`, `TextBox`, `TextBoxBase`, `MaskedTextBox`, `Line`, `Scrollbar`
- Container controls: `ScrollableControl`, `UserControl`, `Panel`, `GroupBox` (stub)
- Layout stubs: `FlowLayoutPanel`, `TableLayoutPanel`
- Drawing engine: `ConsoleCanvas` (double-buffered), `ConsoleGraphics`, `ConsoleElement`
- Events: `ConsoleKeyPressEventHandler`, `ConsolePaintEventHandler`, `ConsolePreviewKeyPressEventHandler`
- Dialogs: `MessageBox` with `ShowDialog()` and `DialogResult`
- Demo apps: **NanoSharp** (nano-like editor), **SharpEdit** (simple editor)
- WinForms-compatible API surface: focus management, tab ordering, `ActiveControl`, `ControlCollection`

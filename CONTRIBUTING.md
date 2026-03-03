# Contributing to Terminal.Forms

Thank you for your interest in contributing to Terminal.Forms! This project is being revived after a 16-year hiatus — not to compete with mature frameworks like Terminal.Gui or Spectre.Console, but as a **fun experiment** in using AI coding agents to bring dead code back to life. There's plenty of work to do, and all contributions are welcome.

## Getting Started

1. Fork the repository
2. Clone your fork
3. Create a feature branch from `main`
4. Make your changes
5. Submit a pull request

## Areas Where Help Is Needed

### High Priority
- **Porting to .NET 10**: Converting old-style .csproj files to SDK-style, removing WinForms dependencies
- **Replacing WinForms types**: Implementing our own `Size`, `Point`, `Padding`, `Rectangle` (stubs already exist in `Drawing/`)
- **Unit tests**: There are currently zero tests — any coverage is welcome

### Medium Priority
- **Implementing stub controls**: `GroupBox`, `Panel`, `TableLayoutPanel`, `FlowLayoutPanel`, `TextRenderer` are empty shells
- **ANSI escape sequence support**: Modern terminals support rich formatting beyond `ConsoleColor`
- **Cross-platform testing**: Verify behavior on Linux and macOS terminals

### Lower Priority
- **New controls**: `CheckBox`, `RadioButton`, `ListBox`, `ComboBox`, `ProgressBar`, `MenuStrip`
- **Mouse support**: Terminal mouse events via ANSI escape sequences
- **Documentation**: Tutorials, API docs, examples

## Code Style

- Follow the `.editorconfig` in the repository root
- Use C# language features appropriate to the target framework version
- Prefer descriptive names over abbreviations
- Add XML documentation comments to all public APIs

## Commit Messages

Use clear, descriptive commit messages:
- `feat: add CheckBox control with toggle support`
- `fix: correct tab ordering when controls are disabled`
- `refactor: port Control.cs to SDK-style project`
- `docs: add rendering pipeline documentation`

## Questions?

Open an issue for discussion before starting large changes.

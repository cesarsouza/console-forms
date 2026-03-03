# Terminal.Forms

A **Windows Forms-like UI framework for console applications**, written in C#.

Terminal.Forms provides a familiar programming model — controls, events, layout, focus management — but renders everything to a terminal using `System.Console`. If you've ever built a WinForms app, you already know how to use this.

> **Note:** This is not meant to compete with [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) or [Spectre.Console](https://github.com/spectreconsole/spectre.console). This is a **fun experiment** in using AI coding agents to revive a dead project from 2009 — one that predates every other .NET TUI framework by nearly a decade. See [Comparison with Alternatives](docs/references/comparison-with-alternatives.md) for details.

![Language: C#](https://img.shields.io/badge/language-C%23-blue)
![Status: Reviving](https://img.shields.io/badge/status-reviving-yellow)
![License: MIT](https://img.shields.io/badge/license-MIT-green)

## Why This Project Exists (Again)

This project was originally created in **2009** using Visual Studio 2008 targeting .NET Framework 2.0. It was hosted on Google Code, auto-exported to GitHub in 2015, and then sat untouched for over **16 years**.

Like many side projects from that era, it was abandoned — not because the idea was bad, but because life happened, and the amount of work left to do was more than one person had time for.

**Then AI happened.**

In 2026, with tools like Claude available, something remarkable became possible: **going back in time to revisit abandoned projects and seeing what they could have been.** Not because the world needs another TUI framework — but because there's a unique joy in picking up where you left off, with a collaborator that never gets tired and can help you move at a pace that would have been unimaginable in 2009.

This revival is a mix of:
- **Nostalgia** — reconnecting with code written 17 years ago, in a different era of .NET
- **Empowerment** — AI doesn't just help write code faster, it lets you *dream bigger* about what's possible in a weekend
- **Proof of concept** — demonstrating that AI-assisted development isn't just about productivity; it's about unlocking a completely new kind of creative fun
- **Going back in time** — what if you could send a collaborator back to 2009 to help your past self finish what they started?

The sky really is the limit now. Old dreams don't have to stay abandoned. If you have a dusty repo from 2008 or 2012 or 2015, maybe it's time to blow off the cobwebs too.

## What It Does

Terminal.Forms lets you build text-based UIs with the same patterns as Windows Forms:

```csharp
using Terminal.Forms;

class Program
{
    static void Main()
    {
        Application.Run(new MainForm());
    }
}

class MainForm : Form
{
    public MainForm()
    {
        Text = "My Console App";
        Size = new Size(80, 25);

        var label = new Label { Text = "Hello, World!", Location = new Point(2, 1) };
        var button = new Button { Text = "OK", Location = new Point(2, 3) };
        button.Click += (s, e) => Close();

        Controls.Add(label);
        Controls.Add(button);
    }
}
```

### Key Features

- **WinForms-compatible API**: `Control`, `Form`, `Button`, `Label`, `TextBox`, `MessageBox`, etc.
- **Event-driven**: `Click`, `KeyPress`, `GotFocus`, `LostFocus`, `TextChanged`, and more
- **Double-buffered rendering**: Only changed characters are written to the console
- **Focus & tab ordering**: Tab between controls, focus management, `ActiveControl`
- **Dialogs**: Modal `ShowDialog()` support with `DialogResult`
- **Color support**: Per-control foreground/background using `ConsoleColor`

### Demo Applications

- **NanoSharp** — a nano-like console text editor with help dialogs and status bar
- **SharpEdit** — a simpler text editor variant

## Project Structure

```
console-forms/
├── README.md                          # This file
├── CHANGELOG.md                       # Version history
├── CONTRIBUTING.md                    # How to contribute
├── LICENSE                            # MIT License
├── Terminal.Forms.slnx                # .NET 10 XML solution file
├── Directory.Build.props              # Shared build properties
├── global.json                        # SDK version pin
│
├── src/
│   └── Terminal.Forms/                # Core library
│       ├── Terminal.Forms.csproj      # Library project
│       ├── Application.cs            # Message loop
│       ├── Control.cs                # Base control class
│       ├── Form.cs                   # Top-level window
│       ├── Button.cs, Label.cs, ...  # Controls
│       ├── Drawing/                  # Rendering engine
│       └── Events/                   # Event types
│
├── samples/
│   ├── NanoSharp/                     # Demo: nano-like editor
│   └── SharpEdit/                     # Demo: simple editor
│
├── tests/
│   └── Terminal.Forms.Tests/          # xUnit test project
│
└── docs/
    ├── architecture/                  # System design & class diagrams
    ├── plans/                         # Modernization roadmap & milestones
    ├── issues/                        # Known bugs & technical debt
    ├── guides/                        # How-to guides & tutorials
    └── references/                    # .NET evolution, API comparisons
```

## Current Status

The project is in the **early revival phase**. See the [modernization roadmap](docs/plans/00-roadmap.md) for the full plan.

| Phase | Description | Status |
|-------|-------------|--------|
| 0 | Document & understand existing code | **Complete** |
| 1 | Port to .NET 10 / SDK-style projects | **Complete** |
| 2 | Remove WinForms dependency | Planned |
| 3 | Modern C# language features | Planned |
| 4 | Dynamic terminal & ANSI support | Planned |
| 5 | Cross-platform testing | Planned |
| 6 | New features & NuGet packaging | Planned |

## Documentation

- [Architecture Overview](docs/architecture/overview.md)
- [Control Hierarchy](docs/architecture/control-hierarchy.md)
- [Rendering Pipeline](docs/architecture/rendering-pipeline.md)
- [Modernization Roadmap](docs/plans/00-roadmap.md)
- [.NET Evolution Guide](docs/references/dotnet-evolution.md) — What changed from .NET Framework 2.0 to .NET 10
- [Known Issues](docs/issues/known-issues.md)

## Building

> **Note**: The project targets .NET 10 (`net10.0-windows`). Build with `dotnet build Terminal.Forms.slnx`.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

# Comparison with Other TUI Frameworks

Terminal.Forms is not the only terminal UI framework for .NET. This document compares it with alternatives.

## Overview

| Framework | First Release | .NET Support | Approach | Active? |
|-----------|--------------|--------------|----------|---------|
| **Terminal.Forms** | 2009 | .NET Framework 2.0 (porting to .NET 10) | WinForms-compatible API | Reviving |
| **Terminal.Gui** (gui.cs) | 2017 | .NET 6+ | Custom toolkit (ncurses-inspired) | Yes |
| **Spectre.Console** | 2020 | .NET 6+ | Rich console output (not interactive controls) | Yes |
| **Gui.cs (Miguel's original)** | 2017 | .NET Core 2.0+ | Merged into Terminal.Gui | Merged |

## Terminal.Gui

[Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) (formerly gui.cs) is the most feature-complete .NET TUI framework. Created by Miguel de Icaza (Mono, Xamarin, GNOME).

**Similarities to Terminal.Forms:**
- Control hierarchy with events
- Focus management, tab ordering
- Modal dialogs
- Double-buffered rendering

**Differences:**
- Much more mature and feature-complete  
- Custom API (not WinForms-compatible)
- Uses `Application.Init()` / `Application.Run()` / `Application.Shutdown()` lifecycle
- Has mouse support, menus, file dialogs, tree views, tables
- Uses ncurses-style drivers (ConsoleDriver abstraction)
- Uses its own coordinate system and layout engine

**Terminal.Forms' unique angle**: WinForms API compatibility. A developer who knows WinForms can use Terminal.Forms without learning a new API. Terminal.Gui has its own conventions.

## Spectre.Console

[Spectre.Console](https://github.com/spectreconsole/spectre.console) is focused on **rich console output** rather than interactive UI:

- Beautiful tables, trees, progress bars, charts
- Markup language for styled text
- Interactive prompts (selection lists, text input, confirmation)
- NOT a full UI framework — no persistent controls, no forms, no focus management

**Complementary to Terminal.Forms**: Spectre.Console's rendering techniques could inspire Terminal.Forms' ANSI backend.

## What Makes Terminal.Forms Different

1. **WinForms API compatibility** — same class names, property names, event patterns
2. **InitializeComponent pattern** — partial classes with designer-style layout code  
3. **Form-based architecture** — true modal/modeless forms with `ShowDialog()`
4. **Lightweight** — no external dependencies (once WinForms types are replaced)
5. **Educational value** — small codebase that demonstrates TUI framework design

## Positioning

Terminal.Forms is **not intended to replace any of the alternatives listed above**. Terminal.Gui and Spectre.Console are mature, actively developed projects that should be your first choice for production terminal UIs in .NET.

This project exists primarily for the **fun of it**. It's a testbed for using AI coding agents to revive a dead codebase — one that, notably, predates every other .NET TUI framework by almost a decade (2009 vs. 2017+). The core question driving this project is: *what happens when you point modern AI tools at abandoned code and try to bring it back to life?*

Beyond the AI experiment, Terminal.Forms occupies a small niche:
- **Simpler** than Terminal.Gui for basic interactive console apps
- **More structured** than raw `Console.Write` / `Console.ReadKey` coding
- **Familiar** to .NET developers who know WinForms
- **Educational** as a study in how UI frameworks work internally

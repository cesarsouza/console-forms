# Documentation Index

## Architecture
- [Architecture Overview](architecture/overview.md) — High-level design, subsystems, implementation status
- [Control Hierarchy](architecture/control-hierarchy.md) — Class inheritance tree, properties, events, methods
- [Rendering Pipeline](architecture/rendering-pipeline.md) — Double-buffered rendering, ConsoleCanvas, coordinate system

## Plans
- [Modernization Roadmap](plans/00-roadmap.md) — 7-phase plan from .NET Framework 2.0 to .NET 10
- [Phase 1: Port to .NET 10](plans/01-port-to-dotnet10.md) — SDK-style projects, step-by-step
- [Phase 2: Remove WinForms Dependency](plans/02-remove-winforms-dependency.md) — Type replacements, cross-platform
- [Phase 3: Modern C# Features](plans/03-modern-csharp.md) — File-scoped namespaces, nullable refs, pattern matching
- [Phase 4: Dynamic Terminal & ANSI](plans/04-dynamic-terminal-ansi.md) — Adaptive sizing, ANSI rendering backend
- [Phase 5: Cross-Platform & Testing](plans/05-crossplatform-testing.md) — Unit tests, CI/CD, platform matrix
- [Phase 6: New Features & Packaging](plans/06-new-features-packaging.md) — New controls, mouse support, NuGet

## Issues
- [Known Issues & Technical Debt](issues/known-issues.md) — 22 cataloged issues with severity and fix strategies

## Guides
- [Getting Started](guides/getting-started.md) — Hello World, InitializeComponent, dialogs, keyboard shortcuts

## References
- [.NET Evolution Guide](references/dotnet-evolution.md) — What changed from .NET 2.0 to .NET 10 (2005–2026)
- [WinForms API Comparison](references/winforms-api-comparison.md) — What's implemented vs. WinForms
- [Comparison with Alternatives](references/comparison-with-alternatives.md) — Terminal.Gui, Spectre.Console

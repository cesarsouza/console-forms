# Modernization Roadmap

This document outlines the plan to bring Terminal.Forms from .NET Framework 2.0 (2009) to modern .NET 9+ (2026).

> This project is being revived as a fun, AI-assisted experiment in picking up where we left off 17 years ago. The roadmap is ambitious on purpose — the whole point is to see how far we can go.

## Guiding Principles

1. **Incremental**: Each phase produces a working, buildable project
2. **Non-destructive**: Keep the original source on a `legacy/original` branch for reference
3. **API-compatible first**: Modernize the internals before changing the public API
4. **Test as we go**: Add tests for each component as it's ported
5. **Have fun**: This is a passion project, not a production deadline

---

## Phase 0: Document & Understand (Current Phase)

**Goal**: Fully understand the existing codebase before changing anything.

- [x] Clone repository
- [x] Read and catalog all source files
- [x] Document architecture (overview, control hierarchy, rendering pipeline)
- [x] Document .NET evolution since 2009
- [x] Create this roadmap
- [x] Catalog all known issues and technical debt
- [x] Create `legacy/original` branch to preserve the original code
- [x] Remove tracked IDE artifacts (`.suo`, `.csproj.user`) from git index

**Deliverable**: Complete documentation in `docs/`

---

## Phase 1: Port to .NET 10 SDK-Style Projects

**Goal**: Get the project building on modern .NET with minimal code changes.

### Tasks

1. **Convert `.csproj` files to SDK-style format**
   - [x] `Terminal.Forms.csproj` → library targeting `net10.0-windows`
   - [x] `NanoSharp.csproj` → console app targeting `net10.0-windows`
   - [x] `SharpEdit.csproj` → console app targeting `net10.0-windows`
   - [x] Create `Terminal.Forms.slnx` at repo root (.NET 10 XML solution format)
   - [x] Remove `.suo`, `.user` files

2. **Reorganize repo to modern layout**
   - [x] Move core library to `src/Terminal.Forms/`
   - [x] Move demo apps to `samples/NanoSharp/` and `samples/SharpEdit/`
   - [x] Create `tests/Terminal.Forms.Tests/` with xUnit
   - [x] Remove old `Terminal.Forms/` solution wrapper directory

3. **Add modern .NET infrastructure**
   - [x] `global.json` — pin SDK version
   - [x] `Directory.Build.props` — shared properties (TargetFramework, Nullable, etc.)
   - [x] `Directory.Packages.props` — central package management

4. **Delete files that are no longer needed**
   - [x] `app.config` files (empty/default)
   - [x] `Properties/AssemblyInfo.cs` (SDK projects generate this)
   - [x] `ClassDiagram.cd` (Visual Studio specific)
   - [x] `*.csproj.user` files
   - [x] Old `Terminal.Forms.sln` (replaced by `Terminal.Forms.slnx`)

5. **Temporarily keep WinForms dependency**
   - [x] Add `<UseWindowsForms>true</UseWindowsForms>` to get `System.Windows.Forms` and `System.Drawing`
   - This is a stepping stone — we'll remove it in Phase 2
   - Marked the project as Windows-only for now: `net10.0-windows`

6. **Fix compilation errors**
   - [x] Fix missing `#endif` in `Drawing/Size.cs`
   - [x] Resolve ambiguous type references in samples

7. **Verify demos still work**
   - `dotnet run --project samples/NanoSharp` should launch the editor
   - `dotnet run --project samples/SharpEdit` should launch the editor

**Deliverable**: All four projects build and tests pass on `net10.0-windows` ✅

---

## Phase 2: Remove WinForms / System.Drawing Dependency

**Goal**: Make Terminal.Forms a pure console library with zero desktop framework dependencies.

### Types to Replace

| WinForms Type | Used In | Replacement Strategy |
|---------------|---------|---------------------|
| `System.Drawing.Size` | Control, Form, TextBox, ConsoleCanvas, etc. | Define `Terminal.Forms.Size` |
| `System.Drawing.Point` | Control, Form, TextBox, ConsoleGraphics, etc. | Define `Terminal.Forms.Point` |
| `System.Drawing.Rectangle` | ConsoleGraphics, ConsoleCanvas | Define `Terminal.Forms.Rectangle` |
| `System.Windows.Forms.Padding` | Control, Label, NanoSharp Designer | Define `Terminal.Forms.Padding` |
| `System.Windows.Forms.DialogResult` | Form, Button, MessageBox | Define `Terminal.Forms.DialogResult` (enum) |
| `System.Windows.Forms.HorizontalAlignment` | Label, Button | Define our own enum |
| `System.Windows.Forms.ScrollBars` | TextBox | Define our own enum |
| `System.Windows.Forms.Orientation` | Line | Define our own enum |
| `System.Windows.Forms.FormStartPosition` | Form | Define our own enum |
| `System.Windows.Forms.MessageBoxButtons` | MessageBox | Define our own enum |
| `System.Windows.Forms.MessageBoxIcon` | MessageBox | Define our own enum |
| `System.Windows.Forms.MessageBoxDefaultButton` | MessageBox | Define our own enum |
| `System.Windows.Forms.MessageBoxOptions` | MessageBox | Define our own enum |
| `System.Windows.Forms.ScrollEventType` | Scrollbar | Define our own enum |
| `System.Windows.Forms.ScrollEventArgs` | Scrollbar | Define our own class |
| `System.Windows.Forms.ScrollOrientation` | Scrollbar | Define our own enum |
| `System.ComponentModel.CancelEventArgs` | Form.Closing | Define our own class |
| `System.Windows.Forms.Control.IsMnemonic()` | Control (static method) | The `#if NO_WINFORMS_DEPENDENCY` code path already exists! |

### Tasks

1. **Create `Terminal.Forms.Drawing` replacement types**
   - `Point`, `Size`, `Rectangle` as `record struct` (stubs already exist in `Drawing/`)
   - `Padding` struct

2. **Create enum replacements** in a new `Enums.cs` or individual files
   - `DialogResult`, `HorizontalAlignment`, `FormStartPosition`, `ScrollBars`, `Orientation`
   - `MessageBoxButtons`, `MessageBoxIcon`, `MessageBoxDefaultButton`, `MessageBoxOptions`
   - `ScrollEventType`, `ScrollOrientation`

3. **Create `CancelEventArgs`** class
4. **Create `ScrollEventArgs`** class

5. **Update all `using` statements** — remove `System.Drawing`, `System.Windows.Forms`

6. **Remove `<UseWindowsForms>` from `.csproj`**

7. **Change target to `net10.0`** (cross-platform!)

8. **Define `NO_WINFORMS_DEPENDENCY`** to activate the existing `IsMnemonic` code path

**Deliverable**: Project targets `net10.0` (not `net10.0-windows`), no WinForms/Drawing dependency

---

## Phase 3: Modern C# Language Features

**Goal**: Modernize the code style while preserving behavior.

### Tasks

1. **File-scoped namespaces** — remove one level of indentation everywhere
2. **Null-conditional operators** — `TextChanged?.Invoke(this, e)` instead of null checks
3. **Pattern matching** — `if (parent is ContainerControl container)` instead of `as` + null check
4. **String interpolation** — replace `String.Format` calls
5. **Expression-bodied members** — for simple getters/methods
6. **Nullable reference types** — enable `#nullable enable` and annotate
7. **`record struct`** for value types like `Point`, `Size`, `ConsoleElement`
8. **Primary constructors** where appropriate
9. **Collection expressions** — `[]` syntax for initializers
10. **Auto-properties** — replace field-backed properties where trivial

**Deliverable**: Clean, idiomatic modern C# code

---

## Phase 4: Dynamic Terminal Support & ANSI Rendering

**Goal**: Support any terminal size, support modern terminal features.

### Tasks

1. **Dynamic buffer sizing** — `ConsoleCanvas` should use `Console.WindowWidth × Console.WindowHeight` instead of hardcoded 80×25
2. **Handle terminal resize** — detect `Console.WindowWidth`/`Height` changes
3. **ANSI escape sequence rendering** — alternative to `Console.SetCursorPosition` + `Console.Write` per cell
   - Cursor movement: `\x1b[{row};{col}H`
   - Colors: `\x1b[38;5;{n}m` (256-color) or `\x1b[38;2;{r};{g};{b}m` (24-bit)
   - Bold, underline, etc.
4. **Extended color support** — beyond the 16 `ConsoleColor` values
5. **Virtual Terminal processing** — enable VT mode on Windows (`ENABLE_VIRTUAL_TERMINAL_PROCESSING`)

**Deliverable**: Framework works in any terminal size, optional ANSI rendering backend

---

## Phase 5: Cross-Platform Testing & Polish

**Goal**: Verify and fix behavior on Linux and macOS terminals.

### Tasks

1. **Test on Linux** — WSL, native Linux
2. **Test on macOS** — Terminal.app, iTerm2
3. **Fix platform-specific issues** — key mappings, color rendering, cursor behavior
4. **GitHub Actions CI** — build + test on Windows, Linux, macOS
5. **Add unit tests** — xUnit project
   - Control tree manipulation
   - Tab ordering
   - Rendering pipeline (ConsoleCanvas, ConsoleGraphics)
   - Event dispatching

**Deliverable**: CI pipeline, passing tests on all platforms

---

## Phase 6: New Features & Packaging

**Goal**: Complete the control library and publish to NuGet.

### Tasks

1. **Implement stub controls**: `Panel`, `GroupBox`, `FlowLayoutPanel`, `TableLayoutPanel`
2. **New controls**: `CheckBox`, `RadioButton`, `ListBox`, `ComboBox`, `ProgressBar`, `MenuStrip`
3. **Border drawing** — the `Border` class exists but is unused
4. **Clipping** — prevent controls from drawing outside their parent's bounds
5. **Z-ordering** — proper layering of overlapping controls
6. **Mouse support** — terminal mouse events (SGR, X10, etc.)
7. **NuGet packaging** — `dotnet pack`, publish to nuget.org
8. **API documentation** — DocFX or similar
9. **Sample application gallery**

**Deliverable**: Published NuGet package, rich control library

---

## Version Mapping

| Phase | Version | .NET Target |
|-------|---------|-------------|
| 0 | — | .NET Framework 2.0 (original) |
| 1 | 0.1.0 | net10.0-windows |
| 2 | 0.2.0 | net10.0 (cross-platform) |
| 3 | 0.3.0 | net10.0 |
| 4 | 0.4.0 | net10.0 |
| 5 | 0.5.0 | net10.0 |
| 6 | 1.0.0 | net10.0 |

# Phase 1 Plan: Port to .NET 10 SDK-Style Projects

**Status**: ✅ **Complete** (2026-03-03)

**Objective**: Get the entire solution building and running on .NET 10 with modern SDK-style project files, reorganized to follow modern .NET repository conventions.

## What Was Done

### 1. Reorganized Repository Layout

Moved from the old Visual Studio 2008 nested structure to the modern `src/`/`samples/`/`tests/` convention:

```
# Before (2009 era)                    # After (modern .NET)
Console.Forms/                           src/
  Console.Forms.sln                        Terminal.Forms/
  Console.Forms/                             Terminal.Forms.csproj
    Crsouza.Console.Forms.csproj       Drawing/
    Drawing/                                Events/
    Events/                                 *.cs
    Properties/AssemblyInfo.cs          samples/
  NanoSharp/                              NanoSharp/
    NanoSharp.csproj                        NanoSharp.csproj
    Properties/AssemblyInfo.cs            SharpEdit/
  SharpEdit/                                SharpEdit.csproj
    SharpEdit.csproj                    tests/
    Properties/AssemblyInfo.cs            Terminal.Forms.Tests/
                                            Terminal.Forms.Tests.csproj
```

### 2. Created Modern .NET Infrastructure

- **`Terminal.Forms.slnx`** — .NET 10 XML solution format at repo root
- **`global.json`** — pins SDK version 10.0 with `latestMinor` rollForward
- **`Directory.Build.props`** — shared properties: `net10.0-windows`, `nullable`, `ImplicitUsings`, `AnalysisLevel`
- **`Directory.Packages.props`** — central package management for test dependencies

### 3. Converted Project Files to SDK-Style

All three old VS2008 `.csproj` files (80-100 lines each) replaced with minimal SDK-style files (~15 lines each).

The library project was renamed from `Crsouza.Console.Forms.csproj` to `Terminal.Forms.csproj`.

### 4. Deleted Obsolete Files

- `Properties/AssemblyInfo.cs` (3 files) — SDK auto-generates
- `app.config` (3 files) — empty/unused
- `ClassDiagram.cd` — Visual Studio 2008 specific
- `Console.Forms.sln` — replaced by `.slnx`
- `Console.Forms/` wrapper directory — eliminated double-nesting

### 5. Fixed Compilation Issues

- Missing `#endif` in `Drawing/Size.cs`
- Ambiguous type references in samples: `ImplicitUsings` + `UseWindowsForms` added a global `using System.Windows.Forms;` that conflicted with `using Terminal.Forms;`. Fixed by removing the implicit WinForms using in sample `.csproj` files.

### 6. Added Test Project

Created `tests/Terminal.Forms.Tests/` with xUnit and basic sanity tests.

## Build & Test Commands

```powershell
dotnet build Terminal.Forms.slnx    # Build all projects
dotnet test Terminal.Forms.slnx     # Run tests
dotnet run --project samples/NanoSharp   # Run NanoSharp demo
dotnet run --project samples/SharpEdit   # Run SharpEdit demo
```

## Result

- All 4 projects build on .NET 10 (`net10.0-windows`)
- 2 sanity tests pass
- 104 warnings in the core library (mostly nullable reference types — will be addressed in Phase 3)
- Git history preserved for all moved files (using `git mv`)

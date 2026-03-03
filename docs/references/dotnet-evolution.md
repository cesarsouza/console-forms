# .NET Evolution: From .NET Framework 2.0 to .NET 10 (2005–2026)

> This guide is written for someone who last worked with C# around 2009–2013 and needs to understand what happened to the .NET ecosystem since then. It covers everything relevant to modernizing Terminal.Forms.

## TL;DR

- **.NET Framework** is now in **maintenance mode** — no new features, only security patches
- Microsoft created **.NET Core** (2016) as a cross-platform, open-source rewrite
- In 2020, they dropped "Core" and unified everything as just **.NET 5**, then **.NET 6, 7, 8, 9...**
- The current version is **.NET 10** (November 2025).
- C# has gone from **version 3.0** to **version 13**, adding tons of features
- Project files (`.csproj`) are now simple and clean ("SDK-style")
- NuGet is the universal package manager, built into everything
- The build system is the `dotnet` CLI — no Visual Studio required

---

## Timeline

| Year | .NET | C# | Key Changes |
|------|------|----|-------------|
| 2005 | Framework 2.0 | 2.0 | Generics, nullable types |
| 2007 | Framework 3.5 | 3.0 | LINQ, lambda expressions, var |
| 2010 | Framework 4.0 | 4.0 | Dynamic, optional params, covariance |
| 2012 | Framework 4.5 | 5.0 | **async/await**, caller info attributes |
| 2015 | Framework 4.6 | 6.0 | String interpolation, null-conditional `?.`, `nameof` |
| 2016 | **.NET Core 1.0** | — | Cross-platform, open-source reboot |
| 2017 | .NET Core 2.0 | 7.0 | Tuples, pattern matching, `out var` |
| 2018 | .NET Core 2.1 | 7.1-7.3 | `Span<T>`, performance focus |
| 2019 | .NET Core 3.0/3.1 | 8.0 | Nullable reference types, switch expressions, ranges |
| 2020 | **.NET 5** | 9.0 | Unified platform (no more "Core"), records, top-level statements |
| 2021 | **.NET 6 LTS** | 10 | File-scoped namespaces, global usings, minimal APIs |
| 2022 | .NET 7 | 11 | Raw string literals, required members, generic math |
| 2023 | **.NET 8 LTS** | 12 | Primary constructors, collection expressions |
| 2024 | **.NET 9** | 13 | `params` collections, `Lock` type, semi-auto properties |

---

## What Actually Changed (the important bits for this project)

### 1. Project Files Are Simple Now

**Before (old-style .csproj — what Terminal.Forms has today):**
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="3.5" DefaultTargets="Build"
         xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{E3F3492C-1AD9-472A-B51B-7508E2213BF2}</ProjectGuid>
    <OutputType>Library</OutputType>
    <RootNamespace>Terminal.Forms</RootNamespace>
    <AssemblyName>Terminal.Forms</AssemblyName>
    <TargetFrameworkVersion>v2.0</TargetFrameworkVersion>
    <!-- ... 70+ more lines ... -->
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Windows.Forms" />
    <!-- Every .cs file must be listed individually -->
    <Compile Include="Application.cs" />
    <Compile Include="Control.cs" />
    <!-- ... -->
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
```

**After (SDK-style .csproj):**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <RootNamespace>Terminal.Forms</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

That's it. All `.cs` files are included automatically. No GUIDs, no boilerplate.

### 2. .NET Framework vs .NET (Core) — What's Available

**Things that exist in both:**
- `System.Console` (ReadKey, Write, SetCursorPosition, colors — all still there)
- `System.Collections.Generic`
- `System.Text`
- `System.Threading`
- `System.ComponentModel`

**Things that DON'T exist in modern .NET (or shouldn't be used):**
- `System.Windows.Forms` — **WinForms still exists** in modern .NET but only for Windows desktop apps; it doesn't make sense as a dependency for a console UI library
- `System.Drawing` — partially available via `System.Drawing.Common` NuGet package, but deprecated on non-Windows platforms

**What this means for Terminal.Forms:** We need to replace the WinForms types we're using (`Size`, `Point`, `Rectangle`, `Padding`, `DialogResult`, `HorizontalAlignment`, `ScrollBars`, `Orientation`, `FormStartPosition`, `MessageBoxButtons`, `MessageBoxIcon`, `ScrollEventType`, `ScrollEventArgs`, `CancelEventArgs`). Most of these are trivial structs/enums we can define ourselves.

### 3. C# Language Features Worth Using

**async/await (C# 5, 2012)** — For any future I/O operations:
```csharp
// Old way
string text = File.ReadAllText("file.txt");

// New way
string text = await File.ReadAllTextAsync("file.txt");
```

**String interpolation (C# 6, 2015):**
```csharp
// Old way
string msg = String.Format("Position: {0}, {1}", x, y);

// New way  
string msg = $"Position: {x}, {y}";
```

**Null-conditional operators (C# 6, 2015):**
```csharp
// Old way
if (TextChanged != null)
    TextChanged(this, e);

// New way
TextChanged?.Invoke(this, e);
```

**Pattern matching (C# 7+):**
```csharp
// Old way
if (m_parent != null && m_parent is ContainerControl)
{
    (m_parent as ContainerControl).ActiveControl = this;
}

// New way
if (m_parent is ContainerControl container)
{
    container.ActiveControl = this;
}
```

**Nullable reference types (C# 8, 2019)** — The compiler warns you about potential nulls:
```csharp
#nullable enable
Control? parent = control.Parent;  // might be null
Control parent2 = control.Parent;  // compiler warns if Parent could be null
```

**Records (C# 9):**
```csharp
// Great for our Drawing types
public record struct Point(int X, int Y);
public record struct Size(int Width, int Height);
```

**File-scoped namespaces (C# 10):**
```csharp
// Old way (extra indentation for everything)
namespace Terminal.Forms
{
    public class Control { ... }
}

// New way
namespace Terminal.Forms;

public class Control { ... }
```

**Primary constructors (C# 12):**
```csharp
// Old way
public class ConsoleElement
{
    private char _character;
    public ConsoleElement(char ch) { _character = ch; }
}

// New way
public class ConsoleElement(char character)
{
    public char Character => character;
}
```

### 4. The `dotnet` CLI

You no longer need Visual Studio to build, run, or test:

```bash
# Create a new project
dotnet new classlib -n Terminal.Forms

# Build
dotnet build

# Run
dotnet run --project NanoSharp

# Add a NuGet package
dotnet add package SomePackage

# Run tests
dotnet test

# Create a NuGet package
dotnet pack
```

### 5. Terminal / Console APIs in Modern .NET

`System.Console` still works exactly the same way, but there are modern alternatives:

- **ANSI/VT100 escape sequences**: Windows Terminal (default since Windows 11) and all Linux/macOS terminals support them. They allow 24-bit color, cursor movement, text styling, and more — without P/Invoke or WinAPI calls.

```csharp
// 24-bit color using ANSI escape codes
Console.Write("\x1b[38;2;255;100;0mOrange text\x1b[0m");

// Move cursor
Console.Write($"\x1b[{row};{col}H");
```

- **Spectre.Console**, **Terminal.Gui** (formerly gui.cs by Miguel de Icaza): popular third-party libraries. Terminal.Gui is actually the closest equivalent to what Terminal.Forms does — it's a full TUI framework. Worth studying as a reference, but Terminal.Forms has its own identity as a WinForms-compatible API.

### 6. Other Ecosystem Changes

| Concept | 2009 | 2025 |
|---------|------|------|
| **Package manager** | Manual DLL references | NuGet (built into everything) |
| **CI/CD** | TeamCity / Jenkins | GitHub Actions |
| **Source control** | SVN / Google Code | Git / GitHub |
| **IDE** | Visual Studio only | VS Code, Rider, VS, CLI |
| **Testing** | NUnit, MSTest | xUnit (default), NUnit, MSTest |
| **Docs** | Sandcastle/SHFB | DocFX, GitHub Pages |
| **API docs** | MSDN | https://learn.microsoft.com/dotnet/api |

---

## Recommended Reading

- [What's new in C# (Microsoft Docs)](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/)
- [Porting from .NET Framework to .NET](https://learn.microsoft.com/en-us/dotnet/core/porting/)
- [.NET API Browser](https://learn.microsoft.com/en-us/dotnet/api/)
- [Breaking changes in .NET](https://learn.microsoft.com/en-us/dotnet/core/compatibility/)
- [ANSI Escape Codes reference](https://en.wikipedia.org/wiki/ANSI_escape_code)

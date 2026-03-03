# Phase 5 Plan: Cross-Platform Testing, CI/CD & Unit Tests

**Objective**: Ensure Terminal.Forms works reliably on Windows, Linux, and macOS, with automated testing and continuous integration.

## Tasks

### 5.1: Unit Test Project

Create `Terminal.Forms.Tests` using xUnit:

```bash
dotnet new xunit -n Terminal.Forms.Tests
dotnet sln add Terminal.Forms.Tests/Terminal.Forms.Tests.csproj
```

#### Test Categories

**Control Tree Tests**:
- Adding/removing controls from `ControlCollection`
- Parent assignment on add
- `Contains()` walking the tree
- Tab ordering with various `TabIndex` values

**Focus Management Tests**:
- `SelectNextControl()` forward and backward
- `GetNextControl()` wrap-around behavior
- Disabled controls are skipped
- Hidden controls are skipped

**Event Tests**:
- `TextChanged` fires when `Text` is set
- `VisibleChanged` fires on `Show()`/`Hide()`
- `EnabledChanged` fires when `Enabled` is set
- `OnKeyPressed` dispatches tab correctly

**Rendering Tests** (require abstracting `System.Console`):
- `ConsoleElement` equality and hashing
- `ConsoleCanvas` buffer diff logic
- `ConsoleGraphics.DrawText` writes correct elements
- `ConsoleGraphics.DrawRectangle` fills correct region
- Coordinate translation (`PointToScreen`, `getAbsolutePosition`)

**Form Tests**:
- `DialogResult` is set when button with `DialogResult` is clicked
- `Close()` fires `Closing` then `Closed`
- `Closing` with `Cancel = true` prevents close
- `KeyPreview` intercepts keys before active control

#### Testing Strategy for Console Output

The `ConsoleCanvas` is a singleton that writes to `System.Console`. To test rendering without a real terminal:

1. **Extract an interface** from `ConsoleCanvas` or make the buffer accessible
2. **Inspect `drawingBuffer` directly** after paint operations
3. **Mock `IConsoleBackend`** (from Phase 4) for integration tests

### 5.2: Cross-Platform Testing Matrix

| Platform | Terminal | Priority |
|----------|----------|----------|
| Windows 11 | Windows Terminal | High — primary dev platform |
| Windows 11 | PowerShell (conhost) | High — legacy fallback |
| Windows 11 | VS Code integrated terminal | High — dev experience |
| Ubuntu (WSL2) | Windows Terminal (WSL pane) | Medium |
| Ubuntu (native) | gnome-terminal | Medium |
| macOS | Terminal.app | Medium |
| macOS | iTerm2 | Low |

#### Known Platform Differences

| Issue | Windows | Linux | macOS |
|-------|---------|-------|-------|
| `Console.WindowWidth` | Works | Works | Works (may throw in pipe) |
| `Console.SetCursorPosition` | Works | Works | Works |
| `Console.CursorVisible` | Works | Works | May not work in all terminals |
| `Console.ReadKey(true)` | Works | Works | Works |
| ANSI escape sequences | Windows Terminal: yes, conhost: needs VT mode | Yes | Yes |
| `ConsoleColor` mapping | Exact | Theme-dependent | Theme-dependent |

### 5.3: GitHub Actions CI

```yaml
name: CI
on: [push, pull_request]

jobs:
  build:
    strategy:
      matrix:
        os: [windows-latest, ubuntu-latest, macos-latest]
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - run: dotnet build --configuration Release
      - run: dotnet test --configuration Release --no-build
```

### 5.4: Code Coverage

Add Coverlet for code coverage:

```bash
dotnet add Terminal.Forms.Tests package coverlet.collector
```

Generate coverage reports in CI and set a minimum threshold (start with 30%, increase over time).

### 5.5: Platform-Specific Fix Patterns

For platform-specific behavior, use runtime checks:

```csharp
if (OperatingSystem.IsWindows())
{
    // Enable VT processing
}
else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
{
    // Handle SIGWINCH for resize
}
```

## Deliverables

- `Terminal.Forms.Tests` project with 50+ unit tests
- GitHub Actions workflow building on 3 platforms
- Code coverage reporting
- All tests passing on Windows, Linux, macOS

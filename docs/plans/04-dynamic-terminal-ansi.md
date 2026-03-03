# Phase 4 Plan: Dynamic Terminal Support & ANSI Rendering

**Objective**: Make the rendering engine adaptive to any terminal size, and add an ANSI escape sequence backend for modern terminals.

## Problem Statement

The current `ConsoleCanvas` is hardcoded to 80×25:

```csharp
drawingBuffer = new ConsoleElement[80, 25];
consoleBuffer = new ConsoleElement[80, 25];
```

Modern terminals are typically 120×30 or larger. The framework also renders character-by-character using `Console.SetCursorPosition` + `Console.Write` + `Console.BackgroundColor`/`ForegroundColor`, which is slow compared to ANSI escape sequences.

## Tasks

### 4.1: Dynamic Buffer Sizing

```csharp
// Replace hardcoded constructor
private ConsoleCanvas()
{
    int width = System.Console.WindowWidth;
    int height = System.Console.WindowHeight;
    drawingBuffer = new ConsoleElement[width, height];
    consoleBuffer = new ConsoleElement[width, height];
}
```

All controls that reference `Console.WindowWidth` / `Console.WindowHeight` in their `InitializeComponent()` already work dynamically — they read the values at runtime. The canvas just needs to match.

### 4.2: Terminal Resize Handling

Options:
1. **Poll-based**: Check `Console.WindowWidth`/`Height` each frame (simple, works everywhere)
2. **Signal-based**: Handle `SIGWINCH` on Unix (more responsive, platform-specific)
3. **Hybrid**: Poll on Windows, signal on Unix

On resize:
1. Reallocate `drawingBuffer` and `consoleBuffer`
2. Clear `consoleBuffer` (force full repaint)
3. Call `PerformLayout()` on the root form
4. Fire a `Resize` event on the `Application` or `Form`

### 4.3: Rendering Backend Abstraction

Introduce a rendering backend interface:

```csharp
public interface IConsoleBackend
{
    int Width { get; }
    int Height { get; }
    void SetCell(int x, int y, char ch, ConsoleColor fg, ConsoleColor bg);
    void Flush();
    void Clear();
    ConsoleKeyInfo ReadKey(bool intercept);
}
```

Implementations:
- `SystemConsoleBackend` — current approach using `System.Console` API (compatible everywhere)
- `AnsiBackend` — uses ANSI escape sequences (faster, supports more colors)

### 4.4: ANSI Escape Sequence Renderer

Instead of calling `Console.SetCursorPosition` + `Console.Write` for each changed cell, batch changes into ANSI escape sequences:

```csharp
// Move cursor + set colors + write character in one escape sequence
var sb = new StringBuilder();
sb.Append($"\x1b[{row + 1};{col + 1}H");     // Move cursor (1-indexed)
sb.Append($"\x1b[38;5;{fgIndex}m");            // Set foreground (256-color)
sb.Append($"\x1b[48;5;{bgIndex}m");            // Set background (256-color)
sb.Append(character);
Console.Write(sb.ToString());
```

Better yet, batch consecutive cells on the same row into a single write.

### 4.5: Extended Color Support

Map `ConsoleColor` (16 values) to ANSI 256-color palette, and optionally support 24-bit true color:

```csharp
public readonly struct Color
{
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }

    // Convert to nearest ConsoleColor for legacy backends
    public ConsoleColor ToConsoleColor() { ... }

    // Predefined console colors
    public static Color Black => new(0, 0, 0);
    public static Color Red => new(170, 0, 0);
    // ...
}
```

### 4.6: Enable Virtual Terminal Processing on Windows

Windows requires opting in to VT processing:

```csharp
[DllImport("kernel32.dll", SetLastError = true)]
static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
```

Or use the .NET 6+ approach: just write escape sequences — Windows Terminal supports them natively.

## Dependencies

- Phase 2 (remove WinForms) should be done first so we own the `Size`/`Point`/`Rectangle` types
- Phase 3 (modern C#) is nice-to-have but not required

## Verification

- Canvas dynamically sizes to terminal dimensions
- Resizing terminal triggers a clean relayout
- ANSI backend produces identical visual output to System.Console backend
- Test in: Windows Terminal, PowerShell, CMD, WSL, VS Code terminal

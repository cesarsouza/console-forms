# Rendering Pipeline

## Overview

Terminal.Forms uses a **double-buffered rendering** approach. All drawing operations write to an off-screen buffer (`drawingBuffer`), which is then compared against the last-written state (`consoleBuffer`). Only cells that have changed are written to `System.Console`, minimizing flickering and I/O.

## The Rendering Stack

```
Control.PerformLayout() / Control.Invalidate()
       │
       ▼
Control.OnPaintBackground(ConsolePaintEventArgs)
       │  └── Fills the control area with BackColor
       ▼
Control.OnPaint(ConsolePaintEventArgs)
       │  └── Draws control-specific content
       ▼
ConsoleGraphics
       │  └── DrawText(), DrawRectangle(), DrawLine()
       │      Translates control-local coordinates to absolute positions
       ▼
ConsoleCanvas.Instance[x, y] = ConsoleElement
       │  └── Writes to drawingBuffer[x, y]
       ▼
ConsoleCanvas.Update()
       │  └── Compares drawingBuffer vs consoleBuffer
       │      For each changed cell:
       │        Console.SetCursorPosition(x, y)
       │        Console.BackgroundColor = element.Background
       │        Console.ForegroundColor = element.Foreground
       │        Console.Write(element.Character)
       │      Then restores cursor position
       ▼
Terminal (visible output)
```

## Key Classes

### ConsoleElement

The atomic unit of display — a single character cell:

```csharp
public struct ConsoleElement
{
    public char Character;
    public ConsoleColor Background;
    public ConsoleColor Foreground;
}
```

### ConsoleCanvas (Singleton)

The double-buffer manager:

```csharp
public class ConsoleCanvas
{
    public static readonly ConsoleCanvas Instance = new ConsoleCanvas();

    private ConsoleElement[,] drawingBuffer;   // 80×25 — what we want to show
    private ConsoleElement[,] consoleBuffer;    // 80×25 — what's currently on screen

    public void Update()  // Diff & flush to System.Console
    public void Invalidate()  // Clear consoleBuffer to force full repaint
}
```

**Current limitation**: Buffer is hardcoded to 80×25. This needs to be dynamic based on `Console.WindowWidth` × `Console.WindowHeight`.

### ConsoleGraphics

Provides drawing primitives, scoped to a rectangular area of the canvas:

```csharp
public sealed class ConsoleGraphics
{
    private Rectangle m_area;  // Absolute position + size of the control

    public void DrawText(string text, Point location, ConsoleColor fg, ConsoleColor bg, bool multiline)
    public void DrawRectangle(Rectangle rect, char ch, ConsoleColor fg, ConsoleColor bg)
    public void DrawLine(char ch, Point p1, Point p2, ConsoleColor fg, ConsoleColor bg)
}
```

Coordinates passed to `ConsoleGraphics` are **relative to the control** — it adds `m_area.X/Y` to convert to absolute canvas positions.

## Paint Cycle

### Full Layout (PerformLayout)

Called when a control becomes visible, or explicitly:

1. Check if layout is suspended or control isn't shown → skip
2. Create `ConsoleGraphics` scoped to this control's absolute bounds
3. Call `OnPaintBackground()` → fills with `BackColor`
4. Call `OnPaint()` → control draws its content
5. Recursively call `PerformLayout()` on all child controls
6. If this is the root control (no parent), call `ConsoleCanvas.Instance.Update()`

### Invalidation (Invalidate)

Called when a single control needs repainting:

1. Check if layout is suspended → skip
2. Repaint self (background + foreground)
3. Call `ConsoleCanvas.Instance.Update()` immediately

**Note**: `Invalidate()` does NOT repaint children — only `PerformLayout()` does. This is a difference from WinForms where `Invalidate()` also invalidates child controls.

## Coordinate System

```
(0,0) ────────────────────────── (79,0)
│                                     │
│   Form (0,0)                        │
│   ├── Label at (2,1)                │
│   ├── TextBox at (0,2)              │
│   │   └── Text starts at (0,0)     │
│   │       → absolute: (0+0, 2+0)   │
│   └── Button at (5,20)              │
│                                     │
(0,24) ──────────────────────── (79,24)
```

- Each control stores `Location` relative to its parent
- `getAbsolutePosition()` walks up the parent chain to compute screen coordinates
- `CreateGraphics()` returns a `ConsoleGraphics` bound to the absolute rectangle

## Known Issues

1. **Hardcoded 80×25 buffer** — doesn't adapt to terminal size
2. **No clipping** — a child control can draw outside its parent's bounds
3. **No z-ordering** — controls are painted in `ControlCollection` order; overlapping is undefined
4. **Full repaint on text change** — TextBox repaints all visible lines when caret moves
5. **No dirty region tracking** — `Invalidate()` repaints the whole control even for small changes
6. **Cursor visibility** — `Console.CursorVisible` is saved/restored during Update, but modal dialogs can leave it in a bad state

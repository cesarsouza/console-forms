# Phase 2 Plan: Remove WinForms Dependency

**Objective**: Eliminate all dependencies on `System.Windows.Forms` and `System.Drawing`, making Terminal.Forms a pure console library that runs cross-platform.

## Dependency Audit

### From `System.Drawing`

| Type | Where Used | Occurrences |
|------|------------|-------------|
| `Size` | Control, Form, ConsoleCanvas, NanoSharp Designer, SharpEdit Designer | ~20 |
| `Point` | Control, Form, ConsoleGraphics, TextBoxBase, NanoSharp Designer, SharpEdit Designer | ~30 |
| `Rectangle` | ConsoleGraphics, ConsoleCanvas, Control | ~10 |

### From `System.Windows.Forms`

| Type | Where Used |
|------|------------|
| `Padding` | Control, NanoSharp Designer |
| `DialogResult` | Form, Button, MessageBox |
| `HorizontalAlignment` | Label, Button |
| `ScrollBars` | TextBox |
| `Orientation` | Line |
| `FormStartPosition` | Form |
| `MessageBoxButtons` | MessageBox |
| `MessageBoxIcon` | MessageBox |
| `MessageBoxDefaultButton` | MessageBox |
| `MessageBoxOptions` | MessageBox |
| `ScrollEventType` | Scrollbar |
| `ScrollEventArgs` | Scrollbar |
| `ScrollOrientation` | Scrollbar |
| `Control.IsMnemonic()` (static) | Control |

### From `System.ComponentModel`

| Type | Where Used |
|------|------------|
| `CancelEventArgs` | Form.OnClosing |

## Implementation Plan

### Step 2.1: Create replacement types in `Drawing/`

The `Drawing/` folder already has stub files wrapped in `#if NO_WINFORMS_DEPENDENCY`. We'll replace them with real implementations:

**`Drawing/Point.cs`**:
```csharp
namespace Terminal.Forms;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y) { X = x; Y = y; }

    public static readonly Point Empty = new(0, 0);
}
```

**`Drawing/Size.cs`**:
```csharp
namespace Terminal.Forms;

public struct Size
{
    public int Width { get; set; }
    public int Height { get; set; }

    public Size(int width, int height) { Width = width; Height = height; }
}
```

**`Drawing/Rectangle.cs`**:
```csharp
namespace Terminal.Forms;

public struct Rectangle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle(int x, int y, int width, int height)
    {
        X = x; Y = y; Width = width; Height = height;
    }

    public Rectangle(Point location, Size size)
    {
        X = location.X; Y = location.Y;
        Width = size.Width; Height = size.Height;
    }
}
```

**`Drawing/Padding.cs`**:
```csharp
namespace Terminal.Forms;

public struct Padding
{
    public int Left { get; set; }
    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }

    public Padding(int all) { Left = Top = Right = Bottom = all; }
    public Padding(int left, int top, int right, int bottom)
    {
        Left = left; Top = top; Right = right; Bottom = bottom;
    }
}
```

### Step 2.2: Create enum replacements

Create `Enums/` directory or a single `Enums.cs`:

```csharp
namespace Terminal.Forms;

public enum DialogResult
{
    None, OK, Cancel, Abort, Retry, Ignore, Yes, No
}

public enum HorizontalAlignment { Left, Right, Center }
public enum ScrollBars { None, Horizontal, Vertical, Both }
public enum Orientation { Horizontal, Vertical }
public enum FormStartPosition { Manual, CenterScreen, CenterParent }
public enum MessageBoxButtons { OK, OKCancel, AbortRetryIgnore, YesNoCancel, YesNo, RetryCancel }
public enum MessageBoxIcon { None, Error, Warning, Information, Question }
public enum MessageBoxDefaultButton { Button1, Button2, Button3 }
public enum MessageBoxOptions { DefaultDesktopOnly = 0 }
public enum ScrollEventType { SmallDecrement, SmallIncrement, LargeDecrement, LargeIncrement, ThumbPosition, ThumbTrack, First, Last, EndScroll }
public enum ScrollOrientation { HorizontalScroll, VerticalScroll }
```

### Step 2.3: Create event arg replacements

```csharp
namespace Terminal.Forms;

public class CancelEventArgs : EventArgs
{
    public bool Cancel { get; set; }
}

public class ScrollEventArgs : EventArgs
{
    public ScrollEventType Type { get; }
    public int OldValue { get; }
    public int NewValue { get; set; }
    public ScrollOrientation ScrollOrientation { get; }

    public ScrollEventArgs(ScrollEventType type, int oldValue, int newValue, ScrollOrientation orientation)
    {
        Type = type; OldValue = oldValue; NewValue = newValue; ScrollOrientation = orientation;
    }
}
```

### Step 2.4: Update `using` directives

In every `.cs` file, remove:
```csharp
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
```

The types will now resolve from `Terminal.Forms` namespace directly.

### Step 2.5: Define `NO_WINFORMS_DEPENDENCY` and clean up

- Add `<DefineConstants>NO_WINFORMS_DEPENDENCY</DefineConstants>` to the `.csproj`
- Or better: remove the `#if` blocks entirely and keep only the self-contained code path

### Step 2.6: Update `.csproj` files

Remove `<UseWindowsForms>true</UseWindowsForms>` and change target:

```xml
<TargetFramework>net10.0</TargetFramework>
```

No more `-windows` suffix = cross-platform!

### Step 2.7: Update demo projects

NanoSharp and SharpEdit use `System.Drawing.Size`, `System.Drawing.Point`, `System.Windows.Forms.Padding`, etc. in their Designer files. Update these to use the new framework types.

### Step 2.8: Build, test, commit

```bash
dotnet build
dotnet run --project NanoSharp
dotnet run --project SharpEdit
git commit -m "feat: remove WinForms dependency, target net10.0 cross-platform"
```

## Expected Challenges

1. **Type ambiguity**: `System.Drawing.Point` vs `Terminal.Forms.Point` — careful with `using` directives
2. **`Padding` constructor differences**: WinForms `Padding` has different constructors — check all call sites
3. **`Rectangle` member names**: Make sure our `Rectangle` has the same property names as `System.Drawing.Rectangle`
4. **`CancelEventHandler` delegate**: WinForms defines `CancelEventHandler` — we'll need our own

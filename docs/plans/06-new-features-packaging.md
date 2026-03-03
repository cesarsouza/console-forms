# Phase 6 Plan: New Features & NuGet Packaging

**Objective**: Flesh out the control library, fix remaining issues, and publish to NuGet.

## New Controls

### 6.1: CheckBox

```
[x] Enable logging
[ ] Verbose mode
```

- Toggle with `Space` or `Enter`
- `Checked` property, `CheckedChanged` event
- `ThreeState` support: Checked, Unchecked, Indeterminate

### 6.2: RadioButton

```
( ) Option A
(*) Option B
( ) Option C
```

- Grouped by parent container — only one selected per group
- `Checked` property, `CheckedChanged` event

### 6.3: ListBox

```
┌──────────────┐
│ Item 1       │
│ Item 2     ▓ │
│> Item 3      │
│ Item 4       │
└──────────────┘
```

- `Items` collection
- `SelectedIndex`, `SelectedItem`
- Keyboard navigation (Up/Down, Home/End, PageUp/PageDown)
- `SelectionChanged` event
- Optional multi-select

### 6.4: ComboBox

```
[Item 3        ▼]
```

Collapsed: shows selected item. Expanded: shows a dropdown ListBox overlay.

### 6.5: ProgressBar

```
[████████░░░░░░░░░░] 45%
```

- `Value`, `Minimum`, `Maximum`
- `Style`: Blocks, Continuous, Marquee
- Use Unicode block characters: `█`, `▓`, `▒`, `░`

### 6.6: MenuStrip

```
┌─File──Edit──View──Help──────────────────┐
```

- `MenuStrip` at form top
- `ToolStripMenuItem` with text, hotkey, submenu
- Alt+letter mnemonic activation
- Dropdown submenu overlay

### 6.7: StatusStrip

```
└─Ready────────────────Line 42, Col 7─────┘
```

- `StatusStrip` at form bottom
- `ToolStripStatusLabel` items with alignment

### 6.8: TabControl

```
┌─Tab1─┬─Tab2─┬─Tab3─┐
│                      │
│  (Tab 1 content)     │
│                      │
└──────────────────────┘
```

- `TabPages` collection
- `SelectedTab`, `SelectedIndex`
- Keyboard: Ctrl+Tab to switch

### 6.9: NumericUpDown

```
[  42  ][▲][▼]
```

- `Value`, `Minimum`, `Maximum`, `Increment`
- Up/Down arrow keys to change value

## Infrastructure Improvements

### 6.10: Border Drawing

Integrate the existing `Border` class into `Control`:

```
┌─── Title ───┐         ╔═══ Title ═══╗
│             │         ║             ║
│  Content    │         ║  Content    ║
│             │         ║             ║
└─────────────┘         ╚═════════════╝
```

Use Unicode box-drawing characters:
- Single: `┌ ┐ └ ┘ ─ │ ├ ┤ ┬ ┴ ┼`
- Double: `╔ ╗ ╚ ╝ ═ ║ ╠ ╣ ╦ ╩ ╬`
- Rounded: `╭ ╮ ╰ ╯`

### 6.11: Clipping

`ConsoleGraphics` should prevent drawing outside the control's bounds:

```csharp
public void SetCell(int x, int y, ConsoleElement element)
{
    int absX = m_area.X + x;
    int absY = m_area.Y + y;

    // Clip to control bounds
    if (absX < m_area.X || absX >= m_area.X + m_area.Width) return;
    if (absY < m_area.Y || absY >= m_area.Y + m_area.Height) return;

    // Clip to parent bounds (if any)
    // ...

    ConsoleCanvas.Instance[absX, absY] = element;
}
```

### 6.12: Z-Ordering

Controls should paint in z-order, with forms/dialogs always on top:

- Maintain a z-order list in `ControlCollection`
- `BringToFront()` / `SendToBack()` methods
- Modal dialogs always paint last (highest z-order)

### 6.13: Mouse Support

Modern terminals support mouse events via escape sequences:

- **SGR (1006) mode**: `\x1b[?1006h` to enable, reports `\x1b[<button;col;row;M/m`
- Events: MouseDown, MouseUp, MouseMove, MouseWheel
- Add `MouseEventArgs` and mouse events to `Control`
- Click = MouseDown + MouseUp on same control
- Focus follows click

## NuGet Packaging

### 6.14: Package Metadata

```xml
<PropertyGroup>
    <PackageId>Terminal.Forms</PackageId>
    <Version>1.0.0</Version>
    <Authors>César De Souza</Authors>
    <Description>A Windows Forms-like UI framework for console applications</Description>
    <PackageTags>console;tui;terminal;ui;forms;winforms</PackageTags>
    <PackageProjectUrl>https://github.com/cesarsouza/console-forms</PackageProjectUrl>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageReadmeFile>README.md</PackageReadmeFile>
</PropertyGroup>
```

### 6.15: Publish Workflow

```bash
dotnet pack --configuration Release
dotnet nuget push bin/Release/Terminal.Forms.1.0.0.nupkg --api-key $NUGET_KEY --source https://api.nuget.org/v3/index.json
```

Automate via GitHub Actions on tag push.

### 6.16: Sample Application Gallery

Create a `samples/` directory:

```
samples/
├── HelloWorld/          # Minimal example
├── Calculator/          # Simple calculator with buttons
├── TodoList/            # Interactive todo list
├── FileExplorer/        # Directory browser
└── TextEditor/          # NanoSharp refined
```

## Priority Order

1. Border drawing (visual polish, unblocks many controls)
2. Clipping (correctness, needed for overlapping controls)
3. CheckBox, RadioButton (simple, high value)
4. ListBox (needed for many scenarios)
5. ProgressBar (simple, fun)
6. MenuStrip + StatusStrip (makes apps feel complete)
7. Mouse support (transforms the experience)
8. NuGet packaging
9. Sample gallery
10. ComboBox, TabControl, NumericUpDown (nice to have)

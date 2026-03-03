# Architecture Overview

Terminal.Forms is a text-based UI framework that mirrors the Windows Forms programming model, but renders to a terminal using `System.Console`.

## Design Philosophy

The original design goal was **API compatibility with Windows Forms**. A developer familiar with WinForms should be able to build console UIs using the same patterns: create controls, set properties, wire up events, add to `Controls` collection, call `Application.Run()`.

This is not just inspiration — the class names, property names, event names, and inheritance hierarchy are intentionally the same as `System.Windows.Forms`.

## High-Level Architecture

```
┌─────────────────────────────────────────────┐
│                Application                   │
│  (message loop: ReadKey → ProcessKey → ...)  │
├─────────────────────────────────────────────┤
│            ApplicationContext                 │
│  (holds MainForm, raises ThreadExit)         │
├─────────────────────────────────────────────┤
│                 Form                         │
│  (top-level window, KeyPreview, dialogs)     │
├─────────────────────────────────────────────┤
│           ContainerControl                   │
│  (ActiveControl, focus routing)              │
├─────────────────────────────────────────────┤
│           ScrollableControl                  │
│  (scroll properties — mostly stub)           │
├─────────────────────────────────────────────┤
│               Control                        │
│  (base class: layout, events, painting,      │
│   tab ordering, ControlCollection)           │
├─────────────────────────────────────────────┤
│          Drawing Layer                       │
│  ConsoleCanvas → ConsoleElement[80,25]       │
│  ConsoleGraphics (DrawText, DrawRectangle)   │
└──────────────┬──────────────────────────────┘
               │
               ▼
         System.Console
    (SetCursorPosition, Write,
     BackgroundColor, ForegroundColor)
```

## Key Subsystems

### 1. Application Lifecycle

- `Application.Run(Form)` creates an `ApplicationContext` and enters a blocking `while` loop
- The loop calls `System.Console.ReadKey(true)` and dispatches each key through `ApplicationContext.ProcessKey()`
- Keys flow: `ApplicationContext` → `Form.ProcessKey()` → `ContainerControl.ProcessKey()` → `ActiveControl.ProcessKey()` → `Control.OnKeyPressed()`
- `Application.Exit()` sets a flag that breaks the loop

### 2. Control Tree

Controls form a tree via `Control.Controls` (a `ControlCollection`, which is a `List<Control>`):

```
Form
├── Label (title)
├── TextBox (document)
├── Label (status bar)
└── UserControl (CaptionLabel)
    ├── Label (hotkey)
    └── Label (description)
```

Each control knows its `Parent`. Adding a control to a collection sets `Parent` and marks it as shown.

### 3. Focus Management

- `ContainerControl.ActiveControl` tracks which child has focus
- `Control.SelectNextControl()` walks the tab order to find the next focusable control
- Tab ordering uses `TabIndex` property with `ControlTabOrderHolder` / `ControlTabOrderComparer`
- `Tab` key is intercepted in `Control.OnKeyPressed()` and triggers `SelectNextControl()`

### 4. The Rendering Pipeline

See [Rendering Pipeline](rendering-pipeline.md) for details.

In short:
1. `Control.PerformLayout()` or `Control.Invalidate()` triggers painting
2. `OnPaintBackground()` fills the control area with background color
3. `OnPaint()` draws the control content (text, borders, etc.)
4. All drawing goes to `ConsoleCanvas.Instance` (a singleton double-buffer)
5. `ConsoleCanvas.Update()` compares drawing buffer vs. console buffer and only writes changed cells

### 5. Event System

Events follow the WinForms pattern:
- Public events: `Click`, `KeyPress`, `GotFocus`, `TextChanged`, etc.
- Protected virtual `On*` methods that raise them
- Subclasses override `On*` methods and call `base.On*()` (or not, to suppress)

### 6. Modal Dialogs

`Form.ShowDialog(Form owner)`:
1. Shows the form and focuses it
2. Enters its own `ReadKey` loop (blocking the parent's loop)
3. Returns when the form is disposed (e.g., `Close()` is called)
4. Returns a `DialogResult`

## What's Implemented vs. Stubbed

### Fully Implemented
| Class | Description |
|-------|-------------|
| `Application` | Message loop, exit |
| `ApplicationContext` | Form lifecycle, key dispatch |
| `Control` | Full base class w/ layout, focus, painting, tab ordering |
| `ContainerControl` | Active control, focus routing |
| `Form` | Dialogs, key preview, title, show/close |
| `Button` | Click, enter/space activation, DialogResult |
| `Label` | Text alignment (left/center/right), multiline |
| `TextBoxBase` | Full text editing: insert, delete, backspace, enter, arrow keys, selection |
| `TextBox` | Extends TextBoxBase with scrolling, caret management |
| `Line` | Horizontal line drawing |
| `Scrollbar` | Value/min/max logic, scroll events (painting incomplete) |
| `ConsoleCanvas` | Double-buffered 80x25 rendering |
| `ConsoleGraphics` | DrawText, DrawRectangle, DrawLine |
| `ConsoleElement` | Character + foreground + background color |
| `MessageBox` | All Show() overloads (core dialog rendering incomplete) |

### Stubs (Empty Classes)
| Class | Description |
|-------|-------------|
| `Panel` | Empty |
| `GroupBox` | Empty |
| `FlowLayoutPanel` | Empty |
| `TableLayoutPanel` | Empty |
| `TextRenderer` | Empty |
| `MaskedTextBox` | Empty (inherits TextBoxBase) |
| `ScrollableControl` | Properties only, no scrolling logic |

### Not Implemented
- `CheckBox`, `RadioButton`, `ListBox`, `ComboBox`
- `MenuStrip`, `ToolStrip`, `StatusStrip`
- `ProgressBar`, `TabControl`
- Mouse input
- Resize handling
- Border drawing (class exists but unused)
- `ConsoleExtensions.ConsoleColorFromRgb()` — returns black always

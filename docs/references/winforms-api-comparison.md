# WinForms API Comparison

This document maps the WinForms API surface to what Terminal.Forms implements, what makes sense for a console environment, and what doesn't.

## Controls

| WinForms Control | Terminal.Forms | Status | Notes |
|------------------|---------------|--------|-------|
| `Control` | `Control` | **Implemented** | Abstract base class, events, layout |
| `ContainerControl` | `ContainerControl` | **Implemented** | ActiveControl, focus routing |
| `ScrollableControl` | `ScrollableControl` | **Partial** | Properties only, no scroll logic |
| `Form` | `Form` | **Implemented** | Dialogs, key preview, title |
| `UserControl` | `UserControl` | **Implemented** | Empty but functional (inherits ContainerControl) |
| `Button` | `Button` | **Implemented** | Click, Enter/Space, DialogResult |
| `Label` | `Label` | **Implemented** | Alignment, multiline |
| `TextBox` | `TextBox` | **Implemented** | Full editing, scrolling |
| `MaskedTextBox` | `MaskedTextBox` | **Stub** | Empty class |
| `Panel` | `Panel` | **Stub** | Empty class |
| `GroupBox` | `GroupBox` | **Stub** | Empty class |
| `FlowLayoutPanel` | `FlowLayoutPanel` | **Stub** | Empty class |
| `TableLayoutPanel` | `TableLayoutPanel` | **Stub** | Empty class |
| `MessageBox` | `MessageBox` | **Partial** | All overloads exist, ShowCore creates empty dialog |
| `CheckBox` | — | Not implemented | Good candidate |
| `RadioButton` | — | Not implemented | Good candidate |
| `ListBox` | — | Not implemented | Good candidate |
| `ComboBox` | — | Not implemented | Possible |
| `ProgressBar` | — | Not implemented | Good candidate |
| `MenuStrip` | — | Not implemented | Useful for console apps |
| `StatusStrip` | — | Not implemented | Useful for console apps |
| `TabControl` | — | Not implemented | Possible |
| `TreeView` | — | Not implemented | Complex but useful |
| `DataGridView` | — | Not implemented | Very complex |
| `RichTextBox` | — | Not implemented | Complex |
| `NumericUpDown` | — | Not implemented | Good candidate |
| `DateTimePicker` | — | Not implemented | Possible |

## Custom Controls (Console-specific)

| Control | Status | Notes |
|---------|--------|-------|
| `Line` | **Implemented** | Horizontal/vertical line separator |
| `Scrollbar` | **Partial** | Value logic works, painting incomplete |
| `CaptionLabel` | **Implemented** | NanoSharp-specific UserControl (hotkey + description) |

## Patterns

| WinForms Pattern | Terminal.Forms | Status |
|------------------|---------------|--------|
| `Application.Run()` | `Application.Run()` | **Implemented** |
| `InitializeComponent()` | Hand-written | **Implemented** (no designer, code-only) |
| Event model (`OnXxx` / `EventHandler`) | Same pattern | **Implemented** |
| `SuspendLayout/ResumeLayout` | Same | **Implemented** |
| `ShowDialog()` with `DialogResult` | Same | **Implemented** |
| `KeyPreview` on Form | Same | **Implemented** |
| `ControlCollection.Add()` | Same | **Implemented** |
| Tab ordering (`TabIndex`, `TabStop`) | Same | **Implemented** |
| `Show()` / `Hide()` | Same | **Implemented** |

## What Doesn't Apply to Console

| Feature | Why |
|---------|-----|
| `Font`, `FontFamily`, `FontSize` | Terminal uses monospace font controlled by terminal emulator |
| `Cursor` (mouse cursor) | No mouse cursor in typical console |
| `Opacity` / `TransparencyKey` | Not possible in console |
| `Icon` | No window icons |
| Images / `PictureBox` | No bitmap rendering (could do ASCII art) |
| `Anchor` / `Dock` | Could be implemented for console but wasn't |
| Drag and drop | No mouse |
| `DoubleBuffered` property | Terminal.Forms is always double-buffered via ConsoleCanvas |
| GDI+ / Graphics | Replaced by ConsoleGraphics |
| `BackgroundImage` | N/A |

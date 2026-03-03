# Known Issues & Technical Debt

This document catalogs all known bugs, limitations, and technical debt in the current Terminal.Forms codebase.

## Critical Issues

### ISSUE-001: Hardcoded 80×25 console buffer
- **File**: `Drawing/ConsoleCanvas.cs`
- **Problem**: `drawingBuffer` and `consoleBuffer` are hardcoded to `new ConsoleElement[80, 25]`. Any terminal larger or smaller than 80×25 will either clip or crash with `IndexOutOfRangeException`.
- **Fix**: Initialize buffers based on `Console.WindowWidth` × `Console.WindowHeight`, handle resize.

### ISSUE-002: No clipping for child controls
- **File**: `ConsoleGraphics.cs`, `Control.cs`
- **Problem**: A child control can draw outside its parent's bounding rectangle. `ConsoleGraphics` writes directly to the global `ConsoleCanvas` with only basic bounds checking.
- **Fix**: `ConsoleGraphics` should clip all drawing operations to the control's declared bounds.

### ISSUE-003: `ForeColor` getter returns parent's `BackColor` instead of `ForeColor`
- **File**: `Control.cs`, ForeColor property getter
- **Code**: `return parent.BackColor;` — should be `return parent.ForeColor;`
- **Severity**: Bug — children without explicit ForeColor inherit the wrong color.

---

## Moderate Issues

### ISSUE-004: `SelectNextControl` doesn't properly handle edge cases
- **File**: `Control.cs`
- **Problem**: `GetNextControl` throws `InvalidOperationException` if the control is not found in the tab order list. Should wrap around or return null gracefully.

### ISSUE-005: `ClientSize` throws `NotImplementedException`
- **File**: `Control.cs`
- **Problem**: `ClientSize` getter and setter both throw. Any code using `ClientSize` will crash.
- **Fix**: Implement as `Size` minus padding/borders, or return `Size` as a default.

### ISSUE-006: `PointToClient` throws `NotImplementedException`
- **File**: `Control.cs`
- **Problem**: Inverse of `PointToScreen` is not implemented.

### ISSUE-007: `Select(bool, bool)` throws `NotImplementedException`
- **File**: `Control.cs`
- **Problem**: The directed selection overload is not implemented.

### ISSUE-008: `GetFirstChildControlInTabOrder` throws `NotImplementedException`
- **File**: `Control.cs`
- **Problem**: Method always throws.

### ISSUE-009: `ConsoleExtensions.ConsoleColorFromRgb` always returns black
- **File**: `ConsoleExtensions.cs`
- **Problem**: Stub implementation. Should map RGB to nearest `ConsoleColor`, or use ANSI 256/24-bit colors.

### ISSUE-010: MessageBox.ShowCore creates empty dialog
- **File**: `MessageBox.cs`
- **Problem**: `ShowCore` creates a `MessageBoxDialog` (empty `Form`) and calls `ShowDialog`, but the form has no content — no text, no buttons. It's a blank box.
- **Fix**: Build out the dialog content: title, message text, and appropriate buttons.

### ISSUE-011: Modal dialog `ShowDialog` blocks with its own ReadKey loop
- **File**: `Form.cs`
- **Problem**: `ShowDialog` enters a new `while (!IsDisposed)` loop calling `Console.ReadKey`. This works but means we have nested blocking loops. If multiple dialogs open, we get deeply nested key loops.
- **Consideration**: This is actually how Terminal.Forms is designed to work (like modal WinForms dialogs), but it could cause stack depth issues with many nested dialogs.

### ISSUE-012: `TextBoxBase` caret position logic has edge cases
- **File**: `TextBoxBase.cs`, `setCaretPosition` method
- **Problem**: Complex boundary logic for scrolling and caret movement has several edge cases where the caret can go out of bounds or the scroll position is wrong. The col < 0 / col > line.Length wrapping is fragile.

---

## Low Priority / Cosmetic

### ISSUE-013: `NoBackground` property is a hack
- **File**: `Control.cs`
- **Problem**: Comment in code says "This is a hack and should be removed in future." Used by NanoSharp to overlay labels on the title bar.
- **Fix**: Implement proper transparency/overlay painting, or z-ordering with transparent backgrounds.

### ISSUE-014: Empty stub classes
- **Files**: `Panel.cs`, `GroupBox.cs`, `FlowLayoutPanel.cs`, `TableLayoutPanel.cs`, `TextRenderer.cs`, `MaskedTextBox.cs`
- **Problem**: Empty classes that don't do anything. Some don't even inherit from the correct base class.
- **Fix**: Either implement them or remove them to avoid confusion.

### ISSUE-015: `Border` class exists but is never used
- **File**: `Border.cs`
- **Problem**: Has `BorderStyle` enum and `Border` class with color property, but no control uses it.
- **Fix**: Integrate into `Control` base class for border drawing.

### ISSUE-016: Event handlers for `ControlAdded`/`ControlRemoved`/`ParentChanged` are never raised
- **File**: `Control.cs`
- **Problem**: Events are declared but `OnControlAdded`, `OnControlRemoved`, `OnParentChanged` methods don't exist. `ControlCollection.Add` doesn't raise `ControlAdded`.

### ISSUE-017: `Scrollbar.OnPaint` is incomplete
- **File**: `Scrollbar.cs`
- **Problem**: Paint method calculates `trackPosition` but doesn't actually draw anything. The `for` loop for vertical scrollbar is empty.

### ISSUE-018: `TextBox.Select()` and `SelectAll()` are empty
- **File**: `TextBox.cs`
- **Problem**: Selection methods do nothing.

### ISSUE-019: `TextBox.Copy/Paste/Cut` are empty
- **File**: `TextBox.cs`
- **Problem**: Clipboard operations not implemented.

### ISSUE-020: Thread safety concerns
- **File**: `Application.cs`
- **Problem**: `m_exit` flag is not `volatile` or using proper synchronization. Not a real issue in the current single-threaded design, but could be if async operations are added.

### ISSUE-021: `Dispose()` only hides and sets disposed flag
- **File**: `Control.cs`
- **Problem**: `Dispose()` sets `Visible = false` and `m_disposed = true`, but doesn't clean up event handlers, child controls, or other resources.

### ISSUE-022: `ControlCollection` doesn't call `OnControlRemoved` on `Remove`
- **File**: `Control.cs` (nested `ControlCollection` class)
- **Problem**: `Add` is overridden to set parent, but there's no `Remove` override to clear parent or raise events.

---

## WinForms API Compatibility Gaps

These are WinForms features that Terminal.Forms claims to support (through its API surface) but doesn't actually implement:

| Feature | Status |
|---------|--------|
| `Control.Anchor` / `Dock` | Not implemented |
| `Control.AutoSize` | Not implemented |
| `Control.Cursor` | N/A for console |
| `Control.Font` | N/A — terminals are monospace |
| `Control.Margin` | Not implemented |
| `Control.MinimumSize / MaximumSize` | Not implemented |
| `Form.AcceptButton / CancelButton` | Not implemented |
| `Form.FormBorderStyle` | Not implemented |
| `Form.Opacity` | N/A for console |
| `ToolTip` | Not implemented |
| Mouse events | Not implemented |
| Drag and drop | Not implemented |
| Data binding | Not implemented |

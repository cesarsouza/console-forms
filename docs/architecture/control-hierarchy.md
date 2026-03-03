# Control Hierarchy

## Class Inheritance Tree

```
IDisposable
└── Control                          (abstract base — layout, events, painting)
    ├── Button                       (click, DialogResult)
    ├── Label                        (text alignment, multiline)
    ├── Line                         (horizontal/vertical line)
    ├── Scrollbar                    (value tracking, scroll events)
    ├── TextBoxBase                  (abstract — text editing, caret, selection)
    │   ├── TextBox                  (scrolling text editor)
    │   └── MaskedTextBox            (stub)
    └── ScrollableControl            (scroll properties)
        └── ContainerControl         (abstract — ActiveControl, focus routing)
            │                        implements IContainerControl
            ├── UserControl          (composable container)
            └── Form                 (top-level window, dialogs, key preview)

IContainerControl                    (interface)
└── ContainerControl

(Stubs — empty classes)
├── Panel                            (inherits Control — should inherit ScrollableControl)
├── GroupBox                         (inherits nothing explicit — should inherit Control)
├── FlowLayoutPanel                  (inherits nothing explicit)
├── TableLayoutPanel                 (inherits nothing explicit)
└── TextRenderer                     (static utility — empty)
```

## Interface

```csharp
public interface IContainerControl
{
    bool ActivateControl(Control active);
    Control ActiveControl { get; set; }
}
```

## Control Base Class — Key Members

### Properties
| Property | Type | Description |
|----------|------|-------------|
| `Parent` | `Control` | Parent control in the tree |
| `Controls` | `ControlCollection` | Child controls |
| `Size` | `Size` | Width and height |
| `Location` | `Point` | Position relative to parent |
| `Text` | `string` | Display text |
| `Name` | `string` | Control name |
| `Visible` | `bool` | Whether control is shown |
| `Enabled` | `bool` | Whether control responds to input |
| `Focused` | `bool` | Whether control has focus |
| `TabIndex` | `int` | Tab ordering position |
| `TabStop` | `bool` | Whether tab can stop here |
| `BackColor` | `ConsoleColor` | Background color (nullable, inherits from parent) |
| `ForeColor` | `ConsoleColor` | Foreground color (nullable, inherits from parent) |
| `Padding` | `Padding` | Inner padding (from WinForms) |
| `NoBackground` | `bool` | Hack to skip background painting |

### Events
| Event | Description |
|-------|-------------|
| `KeyPress` | Key was pressed while focused |
| `Click` | Control was clicked (Enter/Space for buttons) |
| `Enter` | Control was entered (selected) |
| `GotFocus` | Control received focus |
| `LostFocus` | Control lost focus |
| `TextChanged` | Text property changed |
| `VisibleChanged` | Visibility changed |
| `EnabledChanged` | Enabled state changed |
| `TabIndexChanged` | Tab index changed |
| `BackColorChanged` | Background color changed |
| `ForeColorChanged` | Foreground color changed |
| `ParentChanged` | Parent changed |
| `ControlAdded` | Child control added |
| `ControlRemoved` | Child control removed |

### Key Methods
| Method | Description |
|--------|-------------|
| `Show()` / `Hide()` | Set visibility |
| `Focus()` | Request focus |
| `Select()` | Select this control |
| `Invalidate()` | Trigger repaint |
| `PerformLayout()` | Paint self + all children |
| `SuspendLayout()` / `ResumeLayout()` | Batch layout changes |
| `CreateGraphics()` | Get a `ConsoleGraphics` for this control |
| `SelectNextControl()` | Move focus to next control |
| `PointToScreen()` | Convert local coordinates to absolute |

## Form — Additional Members

### Properties
| Property | Type | Description |
|----------|------|-------------|
| `Owner` | `Form` | Parent form (for dialogs) |
| `KeyPreview` | `bool` | Intercept keys before they reach controls |
| `DialogResult` | `DialogResult` | Result for modal dialogs |
| `StartPosition` | `FormStartPosition` | Where dialog appears |
| `ShowTitleInConsole` | `bool` | Update `Console.Title` with form text |

### Events
| Event | Description |
|-------|-------------|
| `Load` | Form is about to be shown for the first time |
| `Shown` | Form has been shown |
| `Activated` | Form received focus |
| `Closing` | Form is about to close (cancelable) |
| `Closed` | Form has closed |
| `PreviewKeyPress` | Key press preview (when `KeyPreview = true`) |

### Methods
| Method | Description |
|--------|-------------|
| `Show(Form owner)` | Show as owned form |
| `ShowDialog(Form owner)` | Show as modal dialog, return DialogResult |
| `Close()` | Close and dispose |
| `Activate()` | Activate and focus |

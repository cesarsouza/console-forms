# Getting Started with Terminal.Forms

> **Note**: This guide assumes Modern .NET. If you're working with the original .NET Framework 2.0 codebase, the same concepts apply but the build process is different.

## Creating a Simple Application

### 1. Basic "Hello World"

```csharp
using Terminal.Forms;
using System.Drawing;  // For Size, Point (until Phase 2 removes this)

class Program
{
    static void Main()
    {
        Application.Run(new HelloForm());
    }
}

class HelloForm : Form
{
    public HelloForm()
    {
        // Set up the form
        Text = "Hello Terminal.Forms";
        Size = new Size(Console.WindowWidth, Console.WindowHeight);
        Location = new Point(0, 0);

        // Create a label
        var label = new Label();
        label.Text = "Hello, World!";
        label.Size = new Size(20, 1);
        label.Location = new Point(2, 2);
        label.ForeColor = ConsoleColor.Yellow;

        // Create an exit button
        var btnExit = new Button();
        btnExit.Text = "Exit";
        btnExit.Size = new Size(6, 1);
        btnExit.Location = new Point(2, 4);
        btnExit.BackColor = ConsoleColor.DarkRed;
        btnExit.ForeColor = ConsoleColor.White;
        btnExit.Click += (s, e) => Close();

        // Add controls and show
        Controls.Add(label);
        Controls.Add(btnExit);
    }
}
```

### 2. Using the InitializeComponent Pattern

Terminal.Forms follows the WinForms pattern of separating form logic from layout using `partial` classes:

**MyForm.cs**:
```csharp
partial class MyForm : Form
{
    public MyForm()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // Initialize data here
    }
}
```

**MyForm.Designer.cs**:
```csharp
partial class MyForm
{
    private Label lblTitle;
    private TextBox txtInput;
    private Button btnOk;

    public void InitializeComponent()
    {
        SuspendLayout();

        // Form setup
        Text = "My Form";
        Size = new Size(60, 20);
        KeyPreview = true;

        // Title label
        lblTitle = new Label();
        lblTitle.Text = "Enter your name:";
        lblTitle.Size = new Size(20, 1);
        lblTitle.Location = new Point(2, 1);

        // Text input
        txtInput = new TextBox();
        txtInput.Size = new Size(40, 1);
        txtInput.Location = new Point(2, 3);

        // OK button
        btnOk = new Button();
        btnOk.Text = "OK";
        btnOk.Size = new Size(4, 1);
        btnOk.Location = new Point(2, 5);
        btnOk.Click += BtnOk_Click;
        btnOk.TabIndex = 1;

        Controls.Add(lblTitle);
        Controls.Add(txtInput);
        Controls.Add(btnOk);
        ResumeLayout(true);
    }
}
```

### 3. Modal Dialogs

```csharp
class ConfirmDialog : Form
{
    public ConfirmDialog()
    {
        Text = "Confirm";
        Size = new Size(40, 8);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = ConsoleColor.DarkBlue;

        var label = new Label();
        label.Text = "Are you sure?";
        label.Size = new Size(20, 1);
        label.Location = new Point(2, 2);

        var btnYes = new Button();
        btnYes.Text = "Yes";
        btnYes.Size = new Size(5, 1);
        btnYes.Location = new Point(2, 5);
        btnYes.DialogResult = DialogResult.Yes;
        btnYes.Click += (s, e) => Close();

        var btnNo = new Button();
        btnNo.Text = "No";
        btnNo.Size = new Size(4, 1);
        btnNo.Location = new Point(10, 5);
        btnNo.DialogResult = DialogResult.No;
        btnNo.Click += (s, e) => Close();

        Controls.Add(label);
        Controls.Add(btnYes);
        Controls.Add(btnNo);
    }
}

// Usage:
var dialog = new ConfirmDialog();
DialogResult result = dialog.ShowDialog(this);
if (result == DialogResult.Yes)
{
    // User confirmed
}
```

### 4. Key Preview (Keyboard Shortcuts)

```csharp
class MyForm : Form
{
    public MyForm()
    {
        KeyPreview = true;  // Form sees keys before controls
    }

    protected override void OnPreviewKeypress(ConsolePreviewKeyPressEventArgs e)
    {
        if (e.Control && e.KeyInfo.Key == ConsoleKey.S)
        {
            SaveDocument();
            e.Handled = true;  // Don't pass to active control
        }
        else if (e.Control && e.KeyInfo.Key == ConsoleKey.Q)
        {
            Close();
            e.Handled = true;
        }
    }
}
```

## Key Concepts

### Control Lifecycle
1. Create control, set properties
2. Add to parent's `Controls` collection → sets `Parent`, marks as shown
3. `SuspendLayout` / `ResumeLayout` batches multiple adds
4. `PerformLayout()` or `ResumeLayout(true)` triggers the first paint
5. `Close()` / `Dispose()` hides and marks as disposed

### Focus Flow
- `Tab` key cycles through controls in `TabIndex` order
- `ActiveControl` on a `ContainerControl` determines which child handles input
- Keys flow: `Form` → (KeyPreview?) → `ActiveControl` → `Control.OnKeyPressed()`

### Painting
- All controls paint to `ConsoleCanvas.Instance` (a double-buffered grid)
- Override `OnPaint()` to customize drawing
- Use `e.Graphics.DrawText()`, `DrawRectangle()`, `DrawLine()`
- Call `Invalidate()` to trigger a repaint of the control

# Phase 3 Plan: Modern C# Language Features

**Objective**: Modernize the code to idiomatic C# 13 while preserving all existing behavior. This is a purely cosmetic/ergonomic refactor — no functional changes.

## Approach

Work file-by-file, applying all modernizations together. Run the demos after each file to verify nothing broke.

## Transformations

### 3.1: File-Scoped Namespaces

Every file currently uses block-scoped namespaces with an extra level of indentation:

```csharp
// Before
namespace Terminal.Forms
{
    public class Button : Control
    {
        // everything indented one extra level
    }
}

// After
namespace Terminal.Forms;

public class Button : Control
{
    // one less level of indentation
}
```

**Files affected**: All ~30 .cs files.

### 3.2: Null-Conditional Event Invocation

The codebase has ~20 instances of this pattern:

```csharp
// Before
if (TextChanged != null)
    TextChanged(this, e);

// After
TextChanged?.Invoke(this, e);
```

**Files affected**: `Control.cs`, `Form.cs`, `ApplicationContext.cs`

### 3.3: Pattern Matching

```csharp
// Before
if (m_parent != null && m_parent is ContainerControl)
{
    (m_parent as ContainerControl).ActiveControl = this;
}

// After
if (m_parent is ContainerControl container)
{
    container.ActiveControl = this;
}
```

**Files affected**: `Control.cs`, `ContainerControl.cs`, `Button.cs`, `Form.cs`

### 3.4: Expression-Bodied Members

```csharp
// Before
public int Top
{
    get { return m_location.Y; }
}

// After
public int Top => m_location.Y;
```

**Files affected**: Most files with simple getters.

### 3.5: String Interpolation

```csharp
// Before (if any String.Format calls exist)
String.Format("Position: {0}, {1}", x, y);

// After
$"Position: {x}, {y}";
```

### 3.6: Auto-Properties

Replace trivial field-backed properties:

```csharp
// Before
private bool m_modified;
public bool Modified
{
    get { return m_modified; }
    set { m_modified = value; }
}

// After
public bool Modified { get; set; }
```

**Caution**: Only for properties that don't have side effects in getter/setter. Properties like `Text`, `Visible`, `Enabled` that fire events must keep their backing fields.

### 3.7: Nullable Reference Types

Enable `<Nullable>enable</Nullable>` in `.csproj` and annotate all public APIs:

```csharp
public Control? Parent { get; set; }
public string Text { get; set; } = string.Empty;
```

This will surface real null-safety issues. Expected to generate many warnings initially — work through them methodically.

### 3.8: Record Structs for Value Types

```csharp
// Point, Size can become record structs if we defined our own in Phase 2
public record struct Point(int X, int Y);
public record struct Size(int Width, int Height);
public record struct ConsoleElement(char Character, ConsoleColor Background, ConsoleColor Foreground);
```

### 3.9: Primary Constructors (Selective)

Use for simple classes where constructor just assigns fields:

```csharp
// Before
public class ConsoleGraphics
{
    private Rectangle m_area;
    public ConsoleGraphics(Rectangle area) { m_area = area; }
}

// After
public class ConsoleGraphics(Rectangle area)
{
    // area is available as a parameter
}
```

## Order of Operations

1. Enable nullable reference types in `.csproj` (generates warnings, doesn't break build)
2. Transform `Drawing/` types to record structs (Phase 2 prerequisite)
3. Work through each source file alphabetically:
   - File-scoped namespace
   - Expression-bodied members
   - Null-conditional events
   - Pattern matching
   - Auto-properties where safe
4. Run demos after each file
5. Address nullable warnings last (most tedious, most valuable)

## Verification

- `dotnet build` succeeds with zero errors
- `dotnet build` warnings should decrease, not increase
- NanoSharp and SharpEdit function identically to before

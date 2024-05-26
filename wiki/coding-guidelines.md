Sure! Here’s an example of a GitHub Wiki page that describes the coding conventions for your C# project. This page includes do's and don'ts, along with examples, to help maintain consistent and high-quality code.

---

## Coding Conventions

Welcome to the coding conventions guide for our C# project. This document outlines the standards and best practices that we follow to ensure our code is clean, consistent, and maintainable.

### General Conventions

#### Do:

- Use **spaces** for indentation.
- Set **indent size** to 4 spaces.
- Use **UTF-8** encoding for all files.
- **Trim trailing whitespace** on all lines.
- **Insert a final newline** at the end of each file.

```csharp
// Good
public class Example
{
    public void Method()
    {
        Console.WriteLine("Hello, World!");
    }
}
```

#### Don't:

- Use **tabs** for indentation.
- Mix different **indent sizes**.
- Use **different encodings**.
- Leave **trailing whitespace**.
- Forget to **insert a final newline**.

```csharp
// Bad
public class Example
{
	public void Method()
	{
	    Console.WriteLine("Hello, World!");
	}
}
```

### C# Specific Conventions

#### Naming Conventions

- **PascalCase** for public members, types, and methods.
- **camelCase** for private and internal fields.
- Prefix interfaces with **'I'**.
- Prefix private fields with an **underscore ('_')**.

```csharp
// Good
public interface IExampleInterface
{
    void DoSomething();
}

public class ExampleClass : IExampleInterface
{
    private int _exampleField;

    public void DoSomething()
    {
        _exampleField = 42;
    }
}
```

```csharp
// Bad
public interface ExampleInterface
{
    void doSomething();
}

public class exampleClass : ExampleInterface
{
    private int exampleField;

    public void doSomething()
    {
        exampleField = 42;
    }
}
```

#### System Directives and Imports

- Place `using System;` directives first.
- Separate groups of `using` directives with a blank line.

```csharp
// Good
using System;

using MonoGame.Framework;

namespace ExampleNamespace
{
    // Code
}
```

```csharp
// Bad
using MonoGame.Framework;
using System;
namespace ExampleNamespace
{
    // Code
}
```

#### Qualification Rules

- Avoid using `this.` unless necessary for disambiguation.

```csharp
// Good
public class Example
{
    private int _value;

    public void SetValue(int value)
    {
        _value = value;
    }
}
```

```csharp
// Bad
public class Example
{
    private int _value;

    public void SetValue(int value)
    {
        this._value = value;  // Avoid unnecessary 'this.'
    }
}
```

#### Code Documentation

- Use XML documentation comments for public members.
- Include `<summary>`, `<param>`, and `<returns>` tags as appropriate.

```csharp
// Good
/// <summary>
/// This method does something.
/// </summary>
/// <param name="value">The value to set.</param>
public void SetValue(int value)
{
    _value = value;
}
```

```csharp
// Bad
public void SetValue(int value)
{
    _value = value;
}
```

### Example Code Style Rules

#### Avoid Unused Parameters

```csharp
// Good
public void Method(int value)
{
    Console.WriteLine(value);
}
```

```csharp
// Bad
public void Method(int unused)
{
    // Do nothing
}
```

### .editorconfig Settings

The following `.editorconfig` settings enforce the above conventions:

```ini
root = true

[*]
indent_style = space
indent_size = 4
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

[*.cs]
indent_style = space
indent_size = 4

# Naming conventions
dotnet_naming_rule.interface_prefix = true
dotnet_naming_rule.interface_prefix.symbols = interface
dotnet_naming_rule.interface_prefix.style = prefix
dotnet_naming_rule.interface_prefix.style.prefix = I

dotnet_naming_symbols.interface.applicable_kinds = interface
dotnet_naming_symbols.interface.applicable_accessibilities = public

dotnet_naming_rule.private_field_prefix = true
dotnet_naming_rule.private_field_prefix.symbols = private_field
dotnet_naming_rule.private_field_prefix.style = prefix
dotnet_naming_rule.private_field_prefix.style.prefix = _

dotnet_naming_symbols.private_field.applicable_kinds = field
dotnet_naming_symbols.private_field.applicable_accessibilities = private

# System directives should be first
dotnet_sort_system_directives_first = true

# Separate import directive groups
dotnet_separate_import_directive_groups = true

# Qualification rules
dotnet_style_qualification_for_event = false
dotnet_style_qualification_for_field = false
dotnet_style_qualification_for_method = false
dotnet_style_qualification_for_property = false

# Enforce code documentation
dotnet_diagnostic.CS1591.severity = warning

# Avoid unused parameters
dotnet_diagnostic.IDE0060.severity = warning
```

### Summary

Following these coding conventions ensures that our codebase remains clean, readable, and maintainable. Consistency is key to effective collaboration and reducing technical debt. Please adhere to these guidelines when contributing to the project.

If you have any questions or suggestions regarding these conventions, feel free to reach out to the project maintainers.

---

This wiki page provides clear and concise guidelines for coding conventions, helping team members maintain a consistent style throughout the project. You can further customize the content to match any additional standards specific to your project.

# CSharp2TS
[![Build](https://github.com/ormesam/CSharp2TS/actions/workflows/build.yml/badge.svg)](https://github.com/ormesam/CSharp2TS/actions/workflows/build.yml) [![Deploy](https://github.com/ormesam/CSharp2TS/actions/workflows/deploy-package.yml/badge.svg)](https://github.com/ormesam/CSharp2TS/actions/workflows/deploy-package.yml)

CSharp2TS is a powerful tool to generate TypeScript files for classes, enums and API endpoints from your C# codebase. It automatically creates type-safe TypeScript interfaces, enums, and API client services, keeping your frontend and backend in sync.

## Features

- 🎯 **Type-Safe**: Generate TypeScript interfaces from C# classes with full type safety
- 🔄 **API Services**: Automatically generate Axios-based API client services from ASP.NET Core controllers
- 📦 **Class Stubs**: Generate stub implementations of interfaces with default values
- 🏷️ **Enhanced Enums**: Generate enums with descriptions and item arrays for UI components
- 🎨 **Flexible Naming**: Support for both PascalCase and camelCase file naming conventions
- ♻️ **Comprehensive Type Support**: Handles generics, collections, dictionaries, nullable types, records, and more
- 🎯 **Selective Generation**: Use attributes to control exactly what gets generated
- 🚀 **Multi-Framework Support**: Targets .NET 10.0, 9.0, and 8.0

## Components

- **CSharp2TS.Core** - A very lightweight nuget package containing the attributes to mark classes, enums and controllers for generation.
- **CSharp2TS.CLI**    - A dotnet tool to convert the marked files to TypeScript interfaces, enums and api services

## Quick Start

1. **Install the Core package** in your .NET project:
   ```
   dotnet add package CSharp2TS.Core --prerelease
   ```

2. **Mark your classes, enums, and controllers** with attributes:
   ```c#
   [TSInterface]
   public class Product {
       public int Id { get; set; }
       public string Name { get; set; }
       public decimal Price { get; set; }
   }

   [TSEnum]
   public enum ProductCategory {
       Electronics = 1,
       Clothing = 2,
       Food = 3
   }
   ```

3. **Install the CLI tool**:
   ```
   dotnet tool install -g CSharp2TS.CLI --prerelease
   ```

4. **Generate TypeScript files**:
   ```
   csharp2ts --model-output-folder ./src/types --model-assembly-path ./bin/Debug/net8.0/YourProject.dll
   ```

5. **Use the generated TypeScript files** in your frontend application!

## CSharp2TS.Core

![NuGet Version](https://img.shields.io/nuget/v/csharp2ts.core)

CSharp2TS.Core is a very lightweight package containing the attributes to mark classes, enums and controllers for generation. For more information see the [CSharp2TS.Core Docs](CSharp2TS.Core/PACKAGE.md).

### Installation

CSharp2TS.Core is available on NuGet.

```
dotnet add package CSharp2TS.Core --prerelease
```

### Example Usage

#### Generate TypeScript Interfaces

```c#
[TSInterface]
public class User {
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
}
```

Generates:
```typescript
interface User {
  id: number;
  name: string;
  email: string | null;
}

export default User;
```

#### Generate Class Stubs

```c#
[TSInterface(GenerateClass = true)]
public class CreateUserRequest {
    public string Name { get; set; }
    public string Email { get; set; }
}
```

Generates an interface with a stub class that has default values, useful for form initialization:
```typescript
export class CreateUserRequestStub implements CreateUserRequest {
  name: string = '';
  email: string = '';

  constructor(data?: Partial<CreateUserRequest>) {
    if (data) {
      Object.assign(this, data);
    }
  }
}
```

#### Generate TypeScript Enums

```c#
[TSEnum]
public enum UserRole {
    Admin = 1,
    User = 2,
    Guest = 3
}
```

#### Generate Enums with Descriptions

```c#
using System.ComponentModel;

[TSEnum(GenerateDescriptions = true, GenerateItemsArray = true)]
public enum UserRole {
    [Description("Administrator with full access")]
    Admin = 1,
    [Description("Regular user")]
    User = 2,
    [Description("Guest with limited access")]
    Guest = 3
}
```

Generates enum with description mapping and items array for dropdowns:
```typescript
export const UserRoleDescriptions: Record<number, string> = {
  [UserRole.Admin]: 'Administrator with full access',
  [UserRole.User]: 'Regular user',
  [UserRole.Guest]: 'Guest with limited access',
};

export const UserRoleItems = Object.entries(UserRoleDescriptions).map(
  ([key, value]) => ({
    value: Number(key),
    title: value,
  })
);
```

#### Generate API Services

```c#
[TSService]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id) {
        // ...
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request) {
        // ...
    }
}
```

Generates an Axios-based API client:
```typescript
export default {
  async getUser(id: number): Promise<User> {
    const response = await apiClient.instance.get<User>(`api/Users/${id}`);
    return response.data;
  },

  async createUser(request: CreateUserRequest): Promise<User> {
    const response = await apiClient.instance.post<User>(`api/Users`, request);
    return response.data;
  },
};
```

For more examples check out the [docs](CSharp2TS.Core/PACKAGE.md).

## Supported Attributes

### TSInterface
Marks a class or record to generate a TypeScript interface.

- **GenerateClass**: When set to `true`, generates a stub class with default values

### TSEnum  
Marks an enum to generate a TypeScript enum.

- **GenerateDescriptions**: Generates a description map from `[Description]` attributes
- **GenerateItemsArray**: Generates an array of `{ value, title }` objects for UI dropdowns

### TSService
Marks a controller to generate an API client service (requires `[ApiController]` attribute).

### TSEndpoint
Specifies or overrides the return type of an API endpoint.

```c#
[HttpGet]
[TSEndpoint(typeof(UserDto))]
public IActionResult GetUser() {
    // Return type will be UserDto in TypeScript
}
```

### TSExclude
Excludes a property or endpoint from TypeScript generation.

```c#
[TSExclude]
public string InternalField { get; set; }

[HttpGet]
[TSExclude]
public IActionResult InternalEndpoint() { /* ... */ }
```

### TSNullable
Marks a property as nullable in TypeScript (adds `| null` to the type).

```c#
[TSNullable]
public string OptionalValue { get; set; } // Generates: optionalValue: string | null
```

## Supported Types

CSharp2TS supports a comprehensive set of C# types:

### Primitive Types
- Numeric: `int`, `long`, `float`, `double`, `decimal`, `byte`, `sbyte`, `short`, `ushort`, `uint`, `ulong`
- Boolean: `bool`
- String: `string`, `char`
- Date/Time: `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`
- Other: `Guid`, `object`

### Special Types
- `JsonElement` → `unknown`
- `IFormFile`, `FormFile` → `File`
- `IFormFileCollection` → `File[]`

### Collections
- Arrays: `T[]`
- Lists: `List<T>`, `IList<T>`, `IEnumerable<T>`, `ICollection<T>`, `IReadOnlyList<T>`, `IReadOnlyCollection<T>`
- Sets: `HashSet<T>`, `ISet<T>`, `SortedSet<T>`
- Queues & Stacks: `Queue<T>`, `Stack<T>`, `LinkedList<T>`, `ConcurrentBag<T>`
- Dictionaries: `Dictionary<K,V>`, `IDictionary<K,V>`, `SortedDictionary<K,V>`, `SortedList<K,V>`, `IReadOnlyDictionary<K,V>`

### Advanced Features
- **Generics**: Full support for generic classes and methods
- **Nullable Types**: Both nullable value types (`int?`) and nullable reference types
- **Records**: C# record types are converted to TypeScript interfaces
- **Inheritance**: Base class properties are included in generated interfaces
- **Multi-dimensional arrays**: Jagged arrays and multi-dimensional arrays



## CSharp2TS.CLI

![NuGet Version](https://img.shields.io/nuget/v/csharp2ts.cli)

CSharp2TS.CLI is a dotnet tool to generate TypeScript files from .NET assemblies which have classes, enums and controllers marked with the attributes in the Core package. It can be run via command line arguments or a config file. For more information see the [CSharp2TS.CLI Docs](CSharp2TS.CLI/PACKAGE.md).

### Installation

CSharp2TS.CLI is available as a dotnet tool. To install globally run:

```
dotnet tool install -g CSharp2TS.CLI --prerelease
```

Or install locally in your project:

```
dotnet tool install CSharp2TS.CLI --prerelease
```

### Example Usage

The tool can be run via a config file. The config file can be created from the command line:

```
csharp2ts create-config
```

Then run with the config:

```
csharp2ts -c C:\path_to_config.json
```

Or run directly via command line arguments:

```
csharp2ts --model-output-folder C:\models_output --model-assembly-path C:\models_assembly --file-casing camel
```

For multiple assemblies, use comma-separated paths:

```
csharp2ts --model-assembly-path C:\assembly1.dll,C:\assembly2.dll --model-output-folder C:\output
```

### Configuration Options

| Option                               | Description                                              |
| ------------------------------------ | -------------------------------------------------------- |
| --model-output-folder, -mo <path>    | The folder where the generated model files will be saved |
| --model-assembly-path, -ma <paths>   | The path to the model assembly (comma-separated for multiple paths) |
| --services-output-folder, -so <path> | The folder where the services will be saved              |
| --services-assembly-path, -sa <paths> | The path to the assembly with the controllers (comma-separated for multiple paths) |
| --service-generator, -sg             | Service generator type (currently only 'axios' is supported)  |
| --file-casing, -fc <style>           | The file name casing style (`camel` or `pascal`)         |
| --nullable-strings                   | Make all strings nullable in the generated code          |

For more detailed usage information and advanced options, check out the [docs](CSharp2TS.CLI/PACKAGE.md).

## Under The Hood

CSharp2TS uses the [Mono.Cecil](https://github.com/jbevain/cecil) library to interrogate .NET assemblies without loading them into the AppDomain. This allows it to safely analyze compiled assemblies and extract type information to generate accurate TypeScript definitions.

The tool processes marked types and generates TypeScript files using T4 templates, ensuring consistent and customizable output.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Links

- [GitHub Repository](https://github.com/ormesam/CSharp2TS)
- [CSharp2TS.Core on NuGet](https://www.nuget.org/packages/CSharp2TS.Core)
- [CSharp2TS.CLI on NuGet](https://www.nuget.org/packages/CSharp2TS.CLI)

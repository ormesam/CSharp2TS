# CSharp2TS
[![Build](https://github.com/ormesam/CSharp2TS/actions/workflows/build.yml/badge.svg)](https://github.com/ormesam/CSharp2TS/actions/workflows/build.yml) [![Deploy](https://github.com/ormesam/CSharp2TS/actions/workflows/deploy-package.yml/badge.svg)](https://github.com/ormesam/CSharp2TS/actions/workflows/deploy-package.yml)

CSharp2TS is a tool to generate TypeScript files for classes, enums, constants and API endpoints. It consists of 2 parts:

- **CSharp2TS.Core** - A very lightweight nuget package containing the attributes to mark classes, enums and controllers for generation.
- **CSharp2TS.CLI**    - A dotnet tool to convert the marked files to TypeScript interfaces, enums, constants and api services

## CSharp2TS.Core

![NuGet Version](https://img.shields.io/nuget/v/csharp2ts.core)

CSharp2TS.Core is a very lightweight package containing the attributes to mark classes, enums and controllers for generation. For more information see the [CSharp2TS.Core Docs](CSharp2TS.Core/PACKAGE.md).

### Installation

CSharp2TS.Core is available on NuGet.

```
dotnet add package CSharp2TS.Core --prerelease
```

### Example Usage

```c#
[TSInterface]
public class TestModel {
    ...
}
```

```c#
[TSEnum]
public enum TestEnum {
    ...
}
```

```c#
[TSConstants]
public static class AppConstants {
    public const int MaxPageSize = 100;
    ...
}
```

```c#
[TSService]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase {
    ...
}
```

Additional attributes include `TSEndpoint` (override return types), `TSExclude` (exclude properties/endpoints), `TSNullable` (mark types as nullable), and `TSImport` (add custom imports to generated services). For more examples check out the [docs](CSharp2TS.Core/PACKAGE.md).

## CSharp2TS.CLI

![NuGet Version](https://img.shields.io/nuget/v/csharp2ts.cli)

CSharp2TS.CLI is a dotnet tool to generate TypeScript files from .NET assemblies which have classes, enums and controllers marked with the attributes in the Core package. It can be run via command line arguments or a config file. For more information see the [CSharp2TS.CLI Docs](CSharp2TS.CLI/PACKAGE.md).

### Installation

CSharp2TS.CLI is available as a dotnet tool. To install globally run:

```
dotnet tool install -g CSharp2TS.CLI --prerelease
```

### Example Usage

The tool can be run via a config file. The config file can be created from the command line, check out the [docs](CSharp2TS.CLI/PACKAGE.md) for more information.

```
csharp2ts -c C:\path_to_config.json
```

Or via command line arguments

```
csharp2ts --model-output-folder C:\models_output --model-assembly-path C:\models_assembly --file-casing camel
```

For more possible arguments, and help commands, check out the [docs](CSharp2TS.CLI/PACKAGE.md).

## Under The Hood

CSharp2TS uses the [Mono.Cecil](https://github.com/jbevain/cecil) project to interrogate assemblies without loading them into the AppDomain

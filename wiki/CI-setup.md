
## Continuous Integration (CI) Setup

Welcome to the CI setup documentation for our C# project. This page provides detailed information about our CI configuration, including build, test, and code quality checks.

### Overview

Our CI pipeline is configured using GitHub Actions. It automates the process of building the project, running tests, formatting code, and ensuring code quality using Roslyn Analyzers.

### CI Workflow Configuration

The CI workflow is defined in the `.github/workflows/ci.yml` file. It is triggered on pull requests to the `develop` branch. The workflow consists of two main jobs: `build-and-test` and `format`.

#### Workflow File

```yaml
name: CI

on:
    pull_request:
        branches:
            - 'develop'
        types: [opened]
        paths:
            - '**'

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout repository
      uses: actions/checkout@v3

    - name: Set up .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 6.x

    - name: Restore dependencies
      run: dotnet restore

    - name: Install Roslyn Analyzers
      run: dotnet add package Microsoft.CodeAnalysis.FxCopAnalyzers

    - name: Build
      run: dotnet build --no-restore

    - name: Run tests
      run: dotnet test --no-restore --verbosity normal --collect:"XPlat Code Coverage"

    - name: Run Roslyn Analyzers
      run: dotnet build --no-restore /p:RunAnalyzersDuringBuild=true /p:TreatWarningsAsErrors=true

  format:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout repository
      uses: actions/checkout@v3

    - name: Set up .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 6.x

    - name: Install dotnet format tool
      run: dotnet tool install -g dotnet-format

    - name: Add dotnet tools to PATH
      run: echo "/home/runner/.dotnet/tools" >> $GITHUB_PATH

    - name: Run dotnet format
      run: dotnet format --verify-no-changes
```

### Build and Test Job

The `build-and-test` job performs the following steps:

1. **Checkout repository**: Retrieves the latest code from the repository.
2. **Set up .NET**: Sets up the .NET SDK version 6.x.
3. **Restore dependencies**: Restores the project's NuGet dependencies.
4. **Install Roslyn Analyzers**: Installs the Roslyn Analyzers package to ensure code quality.
5. **Build**: Builds the project without restoring dependencies again.
6. **Run tests**: Executes the unit tests and collects code coverage.
7. **Run Roslyn Analyzers**: Runs the analyzers during the build to catch any code issues.

### Format Job

The `format` job performs the following steps:

1. **Checkout repository**: Retrieves the latest code from the repository.
2. **Set up .NET**: Sets up the .NET SDK version 6.x.
3. **Install dotnet format tool**: Installs the `dotnet format` tool globally.
4. **Add dotnet tools to PATH**: Adds the installed tools to the system PATH.
5. **Run dotnet format**: Runs `dotnet format` to verify that the code adheres to the defined formatting rules.

### EditorConfig

We use an `.editorconfig` file to enforce consistent coding styles across the project. This file contains settings for general formatting and C#-specific rules.

#### Sample `.editorconfig`

```ini
root = true

# All files
[*]
indent_style = space
indent_size = 4
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

# C# files
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

# System directives should be first
dotnet_sort_system_directives_first = true

# Separate import directive groups
dotnet_separate_import_directive_groups = true

# Qualification rules
dotnet_style_qualification_for_event = false
dotnet_style_qualification_for_field = false
dotnet_style_qualification_for_method = false
dotnet_style_qualification_for_property = false

# Naming conventions for private fields
dotnet_naming_rule.private_field_prefix = true
dotnet_naming_rule.private_field_prefix.symbols = private_field
dotnet_naming_rule.private_field_prefix.style = prefix
dotnet_naming_rule.private_field_prefix.style.prefix = _

dotnet_naming_symbols.private_field.applicable_kinds = field
dotnet_naming_symbols.private_field.applicable_accessibilities = private

# Enforce code documentation
dotnet_diagnostic.CS1591.severity = warning

# Example rule: Avoid unused parameters
dotnet_diagnostic.IDE0060.severity = warning
```

### Project Configuration

Ensure that both your main project and test project are configured to use Roslyn Analyzers and adhere to the coding standards.

#### Main Project (`GameProject.csproj`)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <RollForward>Major</RollForward>
    <PublishReadyToRun>false</PublishReadyToRun>
    <TieredCompilation>false</TieredCompilation>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <RunAnalyzersDuringBuild>true</RunAnalyzersDuringBuild>
    <RunAnalyzersDuringLiveAnalysis>true</RunAnalyzersDuringLiveAnalysis>
    <AnalysisMode>AllEnabledByDefault</AnalysisMode>
  </PropertyGroup>
  <PropertyGroup>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <ApplicationIcon>Icon.ico</ApplicationIcon>
  </PropertyGroup>
  <ItemGroup>
    <None Remove="Icon.ico" />
    <None Remove="Icon.bmp" />
  </ItemGroup>
  <ItemGroup>
    <EmbeddedResource Include="Icon.ico" />
    <EmbeddedResource Include="Icon.bmp" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="MonoGame.Framework.DesktopGL" Version="3.8.1.303" />
    <PackageReference Include="MonoGame.Content.Builder.Task" Version="3.8.1.303" />
    <PackageReference Include="Microsoft.CodeAnalysis.FxCopAnalyzers" Version="3.3.2" />
    <PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="6.0.3" />
  </ItemGroup>
  <Target Name="RestoreDotnetTools" BeforeTargets="Restore">
    <Message Text="Restoring dotnet tools" Importance="High" />
    <Exec Command="dotnet tool restore" />
  </Target>
  <ItemGroup>
    <EditorConfig Condition="Exists('$(MSBuildThisFileDirectory).editorconfig')" Include="$(MSBuildThisFileDirectory).editorconfig" />
  </ItemGroup>
</Project>
```

#### Test Project (`GameProject.Tests.csproj`)

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <OutputType>Exe</OutputType>
    <GenerateProgramFile>false</GenerateProgramFile>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <RunAnalyzersDuringBuild>true</RunAnalyzersDuringBuild>
    <RunAnalyzersDuringLiveAnalysis>true</RunAnalyzersDuringLiveAnalysis>
    <AnalysisMode>AllEnabledByDefault</AnalysisMode>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.9.0" />
    <PackageReference Include="xunit" Version="2.8.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.CodeAnalysis.FxCopAnalyzers" Version="3.3.2" />
    <PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="6.0.3" />


 </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\GameProject\GameProject.csproj" />
  </ItemGroup>
  <ItemGroup>
    <EditorConfig Condition="Exists('$(MSBuildThisFileDirectory).editorconfig')" Include="$(MSBuildThisFileDirectory).editorconfig" />
  </ItemGroup>
</Project>
```

### Summary

Our CI pipeline ensures that every pull request is thoroughly checked for build integrity, code formatting, and code quality. By integrating Roslyn Analyzers and consistent coding standards, we maintain high code quality and prevent potential issues early in the development process.

If you have any questions or need further assistance with the CI setup, please refer to the project's documentation or contact the maintainers.

---

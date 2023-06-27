== reference ==
https://learn.microsoft.com/en-us/dotnet/standard/runtime-libraries-overview

## Runtime libraries overview
The .NET runtime, which is installed on a machine for use by framework-dependent apps, 
has an expansive standard set of class libraries, known as runtime libraries, framework libraries, or the base class library (BCL). 
In addition, there are extensions to the runtime libraries, provided in NuGet packages.

These libraries provide implementations for many general and app-specific types, algorithms, and utility functionality.

## Runtime libraries
These libraries provide the foundational types and utility functionality and are the base of all other .NET class libraries. 
An example is the System.String class, which provides APIs for working with strings. Another example is the serialization libraries.

## Extensions to the runtime libraries
Some libraries are provided in NuGet packages rather than included in the runtime's shared framework. For example:

Conceptual content		NuGet package
Configuration			Microsoft.Extensions.Configuration
Dependency injection	Microsoft.Extensions.DependencyInjection
File globbing			Microsoft.Extensions.FileSystemGlobbing
Generic Host			Microsoft.Extensions.Hosting
HTTP	†				Microsoft.Extensions.Http
Localization			Microsoft.Extensions.Localization
Logging					Microsoft.Extensions.Logging

† For some target frameworks, including net6.0, these libraries are part of the shared framework and don't need to be installed separately.
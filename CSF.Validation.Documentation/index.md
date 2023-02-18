# Validation framework

**CSF.Validation** is a framework [for validating domain objects].
Its goal is to provide an extensible architecture for writing, executing and consuming the results of validation logic.

The [documentation home page] links to tutorials, examples & guidance for writing validators.
The source code is hosted on a [GitHub project site].
Please visit to contribute issue reports, changes, discussion or to see the project status.

[for validating domain objects]:articles/WhatIsDomainObjectValidation.md
[documentation home page]:articles/index.md
[GitHub project site]:https://github.com/csf-dev/CSF.Validation

## Compatibility

The CSF.Validation NuGet packages are _multi-targeted_ for a wide-range of supported .NET versions.
This table shows the .NET support.

| Runtime           | Versions          |
| -------           | -----------       |
| .NET Framework    | **4.6.1** and up  |
| .NET Core         | **2.0** and up    |
| .NET              | **5.0** and up    |

## NuGet packages

The validation framework is distributed across a number of packages available via NuGet.
This table summarises each of them and their purpose.

| Package                           | Description   |
| -------                           | ----------    |
| [CSF.Validation.Abstractions]     | Models & interfaces required to consume validation from your application's logic using dependency injection.  Has minimal/no dependencies of its own (depending upon target framework). |
| [CSF.Validation]                  | The core logic of the validation framework. Only needs to be referenced by an application's startup project to configure dependency injection. |
| [CSF.Validation.StandardRules]    | Pre-written validation rules & messages. Also a service to validate a validation manifest. Has minimal dependencies of its own. |
| [CSF.Validation.Json]             | A serializer service for reading/writing [manifest models] to/from JSON, demonstrating _validation as data_. |
| [CSF.Validation.Selfhosting]      | A library enabling use of the validation framework by apps which do not use dependency injection. |


[CSF.Validation.Abstractions]: https://www.nuget.org/packages/CSF.Validation.Abstractions/
[CSF.Validation]: https://www.nuget.org/packages/CSF.Validation/
[CSF.Validation.StandardRules]: https://www.nuget.org/packages/CSF.Validation.StandardRules/
[CSF.Validation.Json]: https://www.nuget.org/packages/CSF.Validation.Json/
[CSF.Validation.Selfhosting]: https://www.nuget.org/packages/CSF.Validation.Selfhosting/
[manifest models]: xref:ManifestModelIndexPage
---
uid: ConfiguringTheFramework
---
# Configuring the Validation Framework

The recommended and easiest way to configure and consume the Validation Framework is to add it to your application's **dependency injection**.

## Adding CSF.Validation to your app

In the method where you configure your `IServiceCollection`, typically in your application's startup project, you would use something like the following.
You will also require a reference to [the **CSF.Validation** NuGet package].

```csharp
services
    .UseValidationFramework()
    .UseStandardValidationRules()
    .UseValidationRulesInAssemblies(assemblies);
```

The [`UseValidationFramework()`] method adds the mandatory services to enable the framework.
[`UseStandardValidationRules()`] is optional and is found in [the standard validation rules NuGet package].

The most convenient way to add/register your own validation rules is via a method such as [`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]. This method scans the specified assemblies for rule classes and registers all of them with the service collection.
See [`ServiceCollectionExtensions`] for other methods/overloads which add rules.

[the **CSF.Validation** NuGet package]:https://www.nuget.org/packages/CSF.Validation/
[the standard validation rules NuGet package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[`UseValidationFramework()`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationFramework(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`UseStandardValidationRules()`]:xref:CSF.Validation.StandardRulesServiceCollectionExtensions.UseStandardValidationRules(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`ServiceCollectionExtensions`]:xref:CSF.Validation.ServiceCollectionExtensions
[`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})

## Use the abstractions package in your app logic

Assuming you are writing a multi-project application, you will unlikely want to take a dependency upon the main CSF.Validation NuGet package throughout the app.
For projects which make use of the framework, except where dependency injection is configured (above), you need only reference [the CSF.Validation.Abstractions package].
You may optionally also reference [the standard rules package] if you are using them.

The abstractions package contains all of the interfaces required to write rule & message provider classes and to define and consume validators.
It does not contain the core logic of the framework and has no external dependencies of its own.

[the CSF.Validation.Abstractions package]:https://www.nuget.org/packages/CSF.Validation.Abstractions/
[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
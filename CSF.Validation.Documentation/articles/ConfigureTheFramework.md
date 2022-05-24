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
    .UseValidationRulesInAssemblies(assemblies)
    .UseValidatorBuildersInAssemblies(assemblies);
```

The [`UseValidationFramework()`] method adds the mandatory services to enable the framework.
[`UseStandardValidationRules()`] is optional and is found in [the standard validation rules NuGet package].

It is almost certain that developers will want to [write their own validation rule logic classes], which will also need to be added to DI.
The most convenient way to accomplish this is via a method such as [`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]. This method scans the specified assemblies for rule classes and registers all of them with the service collection.
See [`ServiceCollectionExtensions`] for other methods/overloads which add rules.

It is also recommended to use the [`UseValidatorBuildersInAssemblies(IEnumerable<Assembly>)`] method to register your [validator builder implementations] with dependency injection.
Validator builders often have no injected dependencies, so you may not require this (the validation framework will instantiate them without dependency injection).
Registering builders with dependency injection is recommended though, even if only for consistency.

[the **CSF.Validation** NuGet package]:https://www.nuget.org/packages/CSF.Validation/
[the standard validation rules NuGet package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[`UseValidationFramework()`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationFramework(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`UseStandardValidationRules()`]:xref:CSF.Validation.StandardRulesServiceCollectionExtensions.UseStandardValidationRules(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[write their own validation rule logic classes]:WritingValidators/WritingValidationRules/index.md
[`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})
[`ServiceCollectionExtensions`]:xref:CSF.Validation.ServiceCollectionExtensions
[`UseValidatorBuildersInAssemblies(IEnumerable<Assembly>)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidatorBuildersInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})
[validator builder implementations]:WritingValidators/WritingValidatorBuilders/index.md

## Use the abstractions package in your app logic

Assuming you are writing a multi-project application, you will unlikely want to take a dependency upon the main CSF.Validation NuGet package throughout the app.
Apart from your startup project(s) where DI is configured, other projects need only reference [the **CSF.Validation.Abstractions** package].
You may optionally also reference [the standard rules package] if you are using it.

The abstractions package contains just interfaces and models which make-up the validator's API.
With property-configured dependency injection, this is sufficient to create and consume validators without depending upon the main package.
The abstractions package does not contain the core logic of the framework and has no external dependencies of its own.

[the **CSF.Validation.Abstractions** package]:https://www.nuget.org/packages/CSF.Validation.Abstractions/
[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
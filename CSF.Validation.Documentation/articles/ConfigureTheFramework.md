---
uid: ConfiguringTheFramework
---
# Configuring the Validation Framework

_The recommended and easiest_ way to configure and consume the Validation Framework is to add it to your application's **dependency injection**.

For applications which do not use dependency injection, [the CSF.Validation.Selfhosting NuGet package] provides [a self-contained way to use validators] which does not depend upon an application's dependency injection.

[the CSF.Validation.Selfhosting NuGet package]: https://www.nuget.org/packages/CSF.Validation.Selfhosting
[a self-contained way to use validators]: SelfHosting.md

## Adding CSF.Validation to your app

In the method where you configure your `IServiceCollection`, typically in your application's startup project, you would use something like the following.
You will also require a reference to [the **CSF.Validation** NuGet package].

```csharp
services
    // You will almost-certainly need to use these
    .UseValidationFramework()
    .UseValidationRulesInAssemblies(assemblies)

    // The following are optional
    .UseStandardValidationRules()
    .UseValidatorBuildersInAssemblies(assemblies)
    .UseMessageProviders(x => x.AddMessageProvidersInAssemblies(assemblies))
    ;
```

The [`UseValidationFramework()`] method adds the mandatory services to enable the framework.

It is almost certain that developers will want to [write their own validation rule logic classes], which will also need to be added to DI.
The most convenient way to accomplish this is via a method such as [`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]. This method scans the specified assemblies for rule classes and registers all of them with the service collection.
See [`ServiceCollectionExtensions`] for other methods/overloads which add rules.

### Optional: Add the Standard Rules

If you wish to use [the standard validation rules NuGet package] then you should additionally use the [`UseStandardValidationRules()`] method to register & enable them.

### Optional: Register Validator Builders

It is recommended to use the [`UseValidatorBuildersInAssemblies(IEnumerable<Assembly>)`] method to register your [validator builder implementations] with dependency injection.
Validator builders often have no injected dependencies; as long as they do not the validation framework is actually able to instantiate them without using dependency injection.
Registering builders with dependency injection is recommended though, even if only for consistency.

### Optional: Register Message Providers

If you would like the framework [to generate human-readable validation feedback messages] then you should use [`UseMessageProviders`] to register your message-provider classes with dependency injection.

### Optional: Configure default options

The [`UseValidationFramework()`] takes an optional parameter of `Action<ValidationOptions>`.
If specified then this configures a [`ValidationOptions`] instance which is used by default for any validator created by that dependency injection container.
Options specified in the configured default instance will be used unless overridden by explicitly-specified options provided directly to the validator.

[the **CSF.Validation** NuGet package]:https://www.nuget.org/packages/CSF.Validation/
[the standard validation rules NuGet package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[`UseValidationFramework()`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationFramework(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{CSF.Validation.ValidationOptions})
[`UseStandardValidationRules()`]:xref:CSF.Validation.StandardRulesServiceCollectionExtensions.UseStandardValidationRules(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[write their own validation rule logic classes]:WritingValidators/WritingValidationRules/index.md
[`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})
[`ServiceCollectionExtensions`]:xref:CSF.Validation.ServiceCollectionExtensions
[`UseValidatorBuildersInAssemblies(IEnumerable<Assembly>)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidatorBuildersInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})
[validator builder implementations]:WritingValidators/WritingValidatorBuilders/index.md
[to generate human-readable validation feedback messages]:GeneratingFeedbackMessages.md
[`UseMessageProviders`]:xref:CSF.Validation.ServiceCollectionExtensions.UseMessageProviders(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{CSF.Validation.Bootstrap.IRegistersMessageProviders})
[`ValidationOptions`]:xref:CSF.Validation.ValidationOptions

## Use the abstractions package in your app logic

Assuming you are writing a multi-project application, you will unlikely want to take a dependency upon the main CSF.Validation NuGet package throughout the app.
Apart from your startup project(s) where DI is configured, other projects need only reference [the **CSF.Validation.Abstractions** package].
You may optionally also reference [the standard rules package] if you are using it.

The abstractions package contains just interfaces and models which make-up the validator's API.
With property-configured dependency injection, this is sufficient to create and consume validators without depending upon the main package.
The abstractions package does not contain the core logic of the framework and has no external dependencies of its own.

[the **CSF.Validation.Abstractions** package]:https://www.nuget.org/packages/CSF.Validation.Abstractions/
[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
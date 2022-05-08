---
uid: ConfiguringTheFramework
---
# Configuring the Validation Framework

The recommended way to configure and consume the Validation Framework is to add it to your application's **dependency injection**.

## Configuring using DI

To add the framework to dependency injection, add a reference to [the **CSF.Validation** NuGet package] into the project where you configure DI, usually your application startup project.
Add a usage of [the `UseValidationFramework()` extension method] to the logic where you configure the `IServiceCollection`.
This sets up the framework _but it might not be very useful yet_.

Before executing rules from rule logic classes, the validation framework will try to create these rules from dependency injection.
Thus, any rule classes you wish to use should also be registered with DI.
The most convenient way to add custom rule classes to DI is to use one of the following convenience extension methods for `IServiceCollection`.

* [`UseValidationRulesInAssembly(Assembly)`]
* [`UseValidationRulesInAssemblies(params Assembly[])`][1]
* [`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]

Note that if a rule class has a public parameterless constructor you will still be able to use it without registering it with dependency injection. This is not the recommended approach though; the framework will fall-back upon `Activator.CreateInstance(Type)` in order to do this.

Optionally, if you plan to use [the standard rules package], add a reference to that package also and use [the `UseStandardValidationRules()` extension method].


[the **CSF.Validation** NuGet package]:https://www.nuget.org/packages/CSF.Validation/
[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[the `UseValidationFramework()` extension method]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationFramework(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[the `UseStandardValidationRules()` extension method]:xref:CSF.Validation.StandardRulesServiceCollectionExtensions.UseStandardValidationRules(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[`UseValidationRulesInAssembly(Assembly)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssembly(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Reflection.Assembly)
[1]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Reflection.Assembly[])
[`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})

## Example

Here is a brief example of how to configure the validation framework within dependency injection.

```csharp
using Microsoft.Extensions.DependencyInjection;
using CSF.Validation;

public void ConfigureServices(IServiceCollection serviceCollection)
{
    // Amongst other services you are adding to the the DI service collection

    serviceCollection
        .UseValidationFramework()
        .UseStandardValidationRules()
        .UseValidationRulesInAssemblies(GetAssembliesWhichContainCustomRules());
}
```

The last line of the example uses [`UseValidationRulesInAssemblies`].
This extension method is a convenience to bulk-register your own custom validation rules.

[`UseValidationRulesInAssemblies`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})
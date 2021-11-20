---
uid: ConfiguringTheFramework
---
# Configuring the Validation Framework

The Validation Framework is made available for consumption via **dependency injection** and this is the recommended way to use it.

To add the validation framework to DI, add a reference to [the **CSF.Validation** NuGet package] into the project where you configure dependency injection. Use [the `UseValidationFramework()` extension method] with an `IServiceCollection` to add the framework to DI.

Optionally, if you plan to use [the standard rules package], add a reference to that also and use [the `UseStandardValidationRules()` extension method].

Finally, the most convenient way to add your own custom rule classes to dependency injection is to use one of:

* [`UseValidationRulesInAssembly(Assembly)`]
* [`UseValidationRulesInAssemblies(params Assembly[])`][1]
* [`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]

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

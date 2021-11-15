# Configuring the Validation Framework

The Validation Framework is made available for consumption via **dependency injection** and this is the recommended way to use it.

To add the validation framework to DI, add a reference to [the **CSF.Validation** NuGet package] into the project where you configure dependency injection. Use [the `UseValidationFramework()` extension method] with an `IServiceCollection` to add the framework to DI.

Optionally, if you plan to use [the standard rules package], add a reference to that also and use [the `UseStandardValidationRules()` extension method].

[the **CSF.Validation** NuGet package]:https://www.nuget.org/packages/CSF.Validation/
[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[the `UseValidationFramework()` extension method]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationFramework(Microsoft.Extensions.DependencyInjection.IServiceCollection)
[the `UseStandardValidationRules()` extension method]:xref:CSF.Validation.StandardRulesServiceCollectionExtensions.UseStandardValidationRules(Microsoft.Extensions.DependencyInjection.IServiceCollection)
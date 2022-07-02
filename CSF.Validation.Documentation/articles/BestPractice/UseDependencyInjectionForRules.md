# Inject dependencies into rules

CSF.Validation is fully dependency-injection capable.
It is OK for validation rules to rely upon complex logic using services and dependencies from your application.

## Add your custom rules to dependency injection

To take advantage of dependency injection, when [configuring the framework], ensure that your own validation rule classes are added to the container.
The most convenient way to do this, and to ensure consistency, is to use one of the following methods.

* [`UseValidationRulesInAssembly(Assembly)`]
* [`UseValidationRulesInAssemblies(params Assembly[])`][1]
* [`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]
* [`UseValidationRule(Type)`]

Prefer the methods which perform assembly-scanning over `UseValidationRule`, as this leads to more concise configuration.
What all of these methods have in common is that they add the rules to the container with a **transient** lifetime (AKA instance-per-dependency).

_It is crucial that rule classes are registered with DI with a transient lifetime_, because it is permitted for rules to have mutable properties & state.
This state contains configuration for one specific usage of the rule object.
Because of this state, _rule objects are not suitable for re-use_, so they must be created anew upon each usage.

[configuring the framework]:../ConfigureTheFramework.md
[`UseValidationRulesInAssembly(Assembly)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssembly(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Reflection.Assembly)
[1]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Reflection.Assembly[])
[`UseValidationRulesInAssemblies(IEnumerable<Assembly>)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRulesInAssemblies(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Collections.Generic.IEnumerable{System.Reflection.Assembly})
[`UseValidationRule(Type)`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationRule(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Type)

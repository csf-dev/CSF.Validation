# Inject dependencies into rules

CSF.Validation is fully dependency-injection capable.
It is OK for validation rules to rely upon complex logic using services and dependencies from your application.
Register your custom rules with dependency injection using [`IServiceCollection.AddTransient`] and they will be resolved from DI along with their dependencies before they are executed.

[`IServiceCollection.AddTransient`]:https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addtransient

## Always register rules as transient instances

Ensure that you use `AddTransient` and not any methods that imply sharing single rule instances.
Because validation rule classes may have mutable properties for the purposes of rule-configuration, _it is not suitable to reuse instances of rule objects_.

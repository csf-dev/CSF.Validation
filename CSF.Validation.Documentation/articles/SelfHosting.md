# Self-hosting the validation framework

It is **strongly recommended** that consumers of this validation framework integrate it into their application's dependency injection via the steps shown on [the Configuration page].

For applications which do not use DI, the NuGet package [CSF.Validation.Selfhosting] provides a mechanism by which the validation framework may used without DI.
You must install this package to any .NET project where you wish to use the validator.

To create a self-hosted/self-contained instance of the framework, use the [`ValidatorHost`] class as follows:

```csharp
using CSF.Validation;

var host = ValidatorHost.Build(services => {
    // Register rules and/or message providers here
}, options => {
    // Configure default options here
});
```

The first parameter accepts all of the same syntax [as described in the Configuration guide].
Note that the standard validation rules are automatically integrated when using the self-hosting package in this way.
The second parameter provides for configuration of default options, in the same way as would have been provided as a parameter to `UseValidationFramework()`, as described at the docs linked above.

Once a self-hosting builder has been created, properties accessible from that object - the `host` variable in the example above - may be used to work with validators.

[the Configuration page]: ConfigureTheFramework.md
[CSF.Validation.Selfhosting]: https://www.nuget.org/packages/CSF.Validation.Selfhosting
[`ValidatorHost`]: xref:CSF.Validation.ValidatorHost
[as described in the Configuration guide]: ConfigureTheFramework.md#adding-csfvalidation-to-your-app
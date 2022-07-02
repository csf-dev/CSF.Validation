# Validation framework

This is the core logic package of the CSF.Validation framework.
It is an architecture for writing domain/business logic validation rules for potentially complex object models.

This package should only need to be consumed by your application startup project, where dependency injection is configured.
Projects which need to define or use validators need only reference [CSF.Validation.Abstractions], which has a lower dependency footprint.
You are also encouraged to use [CSF.Validation.StandardRules], as it contains a number of pre-written validation rules for common scenarios.

For more information, please see [the documentation website].

[CSF.Validation.Abstractions]:https://www.nuget.org/packages/CSF.Validation.Abstractions/
[CSF.Validation.StandardRules]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[the documentation website]:https://csf-dev.github.io/CSF.Validation/

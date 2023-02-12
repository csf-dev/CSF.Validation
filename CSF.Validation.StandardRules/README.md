# Standard validation rules

This package is an optional (but recommended) add-on to [CSF.Validation].
The standard rules package provides a number of pre-written rules for common validation scenarios.

Reference this package from both your application startup and your project(s) which perform validation.
In application startup, whilst configuring dependency injection, use the [`UseStandardValidationRules`] method to register them with DI.
In your validators you may now use these rules like any others.

This package also provides capabilities for _validating a validator_ - a sanity-check to verify that the composition of a validator does not contain obvious errors.
See the type [`IValidatesValidationManifest`] for this capability.

For more information, please see [the documentation website].

[CSF.Validation]:https://www.nuget.org/packages/CSF.Validation/
[`UseStandardValidationRules`]:https://csf-dev.github.io/CSF.Validation/_vnext/api/CSF.Validation.StandardRulesServiceCollectionExtensions.html#CSF_Validation_StandardRulesServiceCollectionExtensions_UseStandardValidationRules_Microsoft_Extensions_DependencyInjection_IServiceCollection_
[`IValidatesValidationManifest`]:https://csf-dev.github.io/CSF.Validation/_vnext/api/CSF.Validation.ValidatorValidation.IValidatesValidationManifest.html
[the documentation website]:https://csf-dev.github.io/CSF.Validation/

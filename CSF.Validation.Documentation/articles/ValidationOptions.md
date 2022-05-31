---
uid: ValidationOptions
---
# Validation options

The second parameter to the `ValidateAsync` method (of any validator) is an optional instance of [`ValidationOptions`].
The validation options class allows you to customise the behaviour of validation.
It is also _possible to specify default options for use by all validators_ by using the optional `Action<ValidationOptions>` parameter to [`UseValidationFramework()`] when the validator is added to dependency injection.

[`ValidationOptions`]:xref:CSF.Validation.ValidationOptions
[`UseValidationFramework()`]:xref:CSF.Validation.ServiceCollectionExtensions.UseValidationFramework(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{CSF.Validation.ValidationOptions})

## Should the validator throw if validation fails?

When a validator object completes validation, it may throw an exception.  This is governed by the [`RuleThrowingBehaviour` property] of the validation options passed to the `ValidateAsync` method.
By default, if no options are specified or if this property is left unset upon the options, the validator will throw if any of the validation results indicates [an outcome of `Errored`].
Other behaviours are available, documented with the [`RuleThrowingBehaviour` enum].

[`RuleThrowingBehaviour` property]:xref:CSF.Validation.ValidationOptions.RuleThrowingBehaviour
[an outcome of `Errored`]:xref:CSF.Validation.Rules.RuleOutcome
[`RuleThrowingBehaviour` enum]:xref:CSF.Validation.RuleThrowingBehaviour

## How should exceptions be treated when accessing values?

Exceptions which occur within validation rules are automatically caught and converted into `Errored` outcomes within the validation results.
Whilst _this happens to be the default behaviour_, it does not have to be the behaviour for accessors which gets values from the object model to be validated.
This behaviour is controlled from two places:

* Upon the validation options, [the property `AccessorExceptionBehaviour`] decides the default behaviour
* Upon each validation manifest value, an optional [`AccessorExceptionBehaviour`] value may override the default behaviour for individual values

If no behaviour is set anywhere (for example no options object is passed to the validator) then the default behaviour is [`TreatAsError`].

[the property `AccessorExceptionBehaviour`]:xref:CSF.Validation.ValidationOptions.AccessorExceptionBehaviour
[`AccessorExceptionBehaviour`]:xref:HandlingAccessorExceptions
[`TreatAsError`]:xref:CSF.Validation.Manifest.ValueAccessExceptionBehaviour

## Should validation feedback messages be generated?

The property [`EnableMessageGeneration`] controls whether or not validation feedback messages are to be generated or not.
This set to `false` by default.
See [the documentation for generating messages] for more information.

[`EnableMessageGeneration`]:xref:CSF.Validation.ValidationOptions.EnableMessageGeneration
[the documentation for generating messages]:GeneratingFeedbackMessages.md

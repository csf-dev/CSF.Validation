# Validation options

The second parameter to the `ValidateAsync` method (of any validator) is an optional instance of [`ValidationOptions`].
The validation options class allows you to customise the behaviour of validation.

[`ValidationOptions`]:xref:CSF.Validation.ValidationOptions

## Should the validator throw if validation fails?

When a validator object completes validation, it may throw an exception.  This is governed by the [`RuleThrowingBehaviour`] property of the validation options passed to the `ValidateAsync` method.
By default, if no options are specified or if this property is left unset upon the options, the validator will throw if any of the validation results indicates [an outcome of `Errored`].
Other behaviours are available, documented with the enum [`RuleThrowingBehaviour`].

[`RuleThrowingBehaviour`]:xref:CSF.Validation.ValidationOptions.RuleThrowingBehaviour
[an outcome of `Errored`]:xref:CSF.Validation.Rules.RuleOutcome
[`RuleThrowingBehaviour`]:xref:CSF.Validation.RuleThrowingBehaviour

## How should exceptions be treated when accessing values?

Exceptions which occur within validation rules are automatically caught and converted into `Errored` outcomes within the validation results.
The same is not automatically true for the accessors which gets values from the object model to be validated.
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

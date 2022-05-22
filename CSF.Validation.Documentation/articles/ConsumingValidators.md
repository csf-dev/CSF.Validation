# Consuming validators

Once you have one or more validators defined, using [any of the three available techniques], you may create and use a validator instance to validate objects using those rules.
To accomplish this you should inject an instance of the validator factory: [`IGetsValidator`] into your logic which needs validation.
You will require a reference to the [the CSF.Validation.Abstractions package].

The validator factory has a number of methods & extension methods which cater for all of the possible scenarios, regardless of which technique was used to define the validator.
The methods of the validator factory will allow you to get either [a `IValidator<T>`] or perhaps [a non-generic `IValidator`].
Which you use depends upon whether or not the type of the validated object is known at design-time.

Both validator interfaces provide the method `ValidateAsync`, which is used to validate the object and get the results.

[any of the three available techniques]:WritingValidators/index.md#creating-a-validator-from-some-rules
[`IGetsValidator`]:xref:CSF.Validation.IGetsValidator
[the CSF.Validation.Abstractions package]:https://www.nuget.org/packages/CSF.Validation.Abstractions
[a `IValidator<T>`]:xref:CSF.Validation.IValidator`1
[a non-generic `IValidator`]:xref:CSF.Validation.IValidator

## Specify options to customise validation

The second parameter to the `ValidateAsync` method (of any validator) is an optional instance of [`ValidationOptions`].
The validation options class allows you to customise the behaviour of validation.

[`ValidationOptions`]:xref:CSF.Validation.ValidationOptions

### Should the validator throw if validation fails?

When a validator object completes validation, it may throw an exception.  This is governed by the [`RuleThrowingBehaviour`] property of the validation options passed to the `ValidateAsync` method.
By default, if no options are specified or if this property is left unset upon the options, the validator will throw if any of the validation results indicates [an outcome of `Errored`].
Other behaviours are available, documented with the enum [`RuleThrowingBehaviour`].

[`RuleThrowingBehaviour`]:xref:CSF.Validation.ValidationOptions.RuleThrowingBehaviour
[an outcome of `Errored`]:xref:CSF.Validation.Rules.RuleOutcome
[`RuleThrowingBehaviour`]:xref:CSF.Validation.RuleThrowingBehaviour

### How should exceptions be treated when accessing values

Exceptions which occur within validation rules are automatically caught and converted into `Errored` outcomes within the validation results.
The same is not automatically true for the accessors which gets values from the object model to be validated.
This behaviour is controlled from two places:

* Upon the validation options, [the property `AccessorExceptionBehaviour`] decides the default behaviour
* Upon each validation manifest value, an optional [`AccessorExceptionBehaviour`] value may override the default behaviour for individual values

If no behaviour is set anywhere (for example no options object is passed to the validator) then the default behaviour is [`TreatAsError`].

[the property `AccessorExceptionBehaviour`]:xref:CSF.Validation.ValidationOptions.AccessorExceptionBehaviour
[`AccessorExceptionBehaviour`]:xref:HandlingAccessorExceptions
[`TreatAsError`]:xref:CSF.Validation.Manifest.ValueAccessExceptionBehaviour

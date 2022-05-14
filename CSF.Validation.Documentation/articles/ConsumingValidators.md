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
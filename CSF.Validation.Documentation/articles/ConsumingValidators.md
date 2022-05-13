# Consuming validators

To perform validation, you first require an instance of the validator factory: [`IGetsValidator`]. Assuming you are using dependency injection, this is available for injection into any project which references [the CSF
Validation.Abstractions package].

The validator factory has various methods (including strongly-typed extension methods) to get a validator.
These methods include overloads to get a validator from each of the mechanisms of specifying the validator's design:

* A validator builder type
* A manifest model
* A full validation manifest

Once you have [the `IValidator<T>` instance] (or perhaps [a non-generic `IValidator`] if the type is not known at design-time), use the `ValidateAsync` method to get the results.



[`IGetsValidator`]:xref:CSF.Validation.IGetsValidator
[the CSF
Validation.Abstractions package]: https://www.nuget.org/packages/CSF.Validation.Abstractions
[the `IValidator<T>` instance]:xref:CSF.Validation.IValidator`1
[a non-generic `IValidator`]:xref:CSF.Validation.IValidator
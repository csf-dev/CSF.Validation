# Writing validators

Fundamentally, a validator is a service which executes a collection of independent validation rules upon an object (or object graph) and returns the result.
In order to create a validator rules are assigned to objects and values, such as properties.

## Writing rules

Rules are individual, self-contained pieces of logic which fundamentally test for a pass or a fail.
Each validation rule in the validation manifest is a .NET class.

Some pre-written rules are available in [the standard rules package] but it's expected that developers will want to write their own.
You are advised to read [the documentation for writing rule classes] & the [best practices and guidance] applicable to rules.

[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[the documentation for writing rule classes]:WritingValidationRules/index.md
[best practices and guidance]:../BestPractice/index.md#writing-validation-rule-classes
[`IRule<in TValidated>`]:xref:CSF.Validation.Rules.IRule`1
[`IRule<in TValue, in TParent>`]:xref:CSF.Validation.Rules.IRule`2

## Assigning rules

To create a usable validator, the application must specify which rule(s) should be used to validate each object or value.
They could optionally specify how those rules should be configured.
We call this information _the validation manifest_ and CSF.Validation provides three mechanisms by which to create it.

* [A **validator builder**] is a declarative/fluent API to specify how an object should be validated by writing a .NET class.
* The validator [**manifest model**] is a way to describe how an object should be validated using very simple objects which are suitable for serialisation.
* You may [create a validation manifest directly], using the API/object model for doing so.

[A **validator builder**]:WritingValidatorBuilders/index.md
[**manifest model**]:UsingTheManifestModel/index.md
[create a validation manifest directly]:TheValidationManifest/index.md

### Which approach should I use?

All three mechanisms can produce almost the same results, so it is generally best to choose the simplest which meets your needs.
_The recommended approach_ is to use a validator builder wherever possible.
Validator builders only limitation is that they are written in code, but they are very intuitive to read and write.

The manifest model comes with some limitations compared to builders or editing a manifest directly but are suitable for serialization to persistent formats such as JSON, XML or a database.
Thus, the manifest model permits writing _"validation as data"_ architectures.

Finally, creating a manifest directly allows for advanced scenarios, which would not be possible using the mechanisms above on their own.

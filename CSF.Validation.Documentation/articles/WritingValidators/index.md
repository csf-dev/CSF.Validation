# Writing validators

Fundamentally, a validator is a service which executes a collection of independent validation rules upon an object (or object graph) and returns the result.
In order to create a validator rules are assigned to objects and values, such as properties.

## Writing rules

Rules are individual, self-contained pieces of logic which fundamentally test for a pass or a fail.
Each validation rule is written as a self-contained .NET class.

Some pre-written rules are available in [the standard rules package] but it's expected that developers will want to write their own.
You are advised to read [the documentation for writing rule classes] & the [best practices and guidance] applicable to rules.

[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[the documentation for writing rule classes]:WritingValidationRules/index.md
[best practices and guidance]:../BestPractice/index.md#writing-validation-rule-classes
[`IRule<in TValidated>`]:xref:CSF.Validation.Rules.IRule`1
[`IRule<in TValue, in TParent>`]:xref:CSF.Validation.Rules.IRule`2

## Creating a validator from some rules

To create a usable validator, the application must specify which rule(s) should be used to validate each object or value.
They could optionally specify how those rules should be configured.

This information about which rules are to be used, and how, is called _the validation manifest_.
The CSF.Validation framework provides _three mechanisms_ by which to define it, although **typical applications will need use only one** of them.

1. [A **validator builder**] is most suitable if your validators are to be defined using .NET code
2. If the composition/configuration of a validator is to be _defined using data_ then consider using the [**manifest model**]
3. Developers wishing to use advanced techniques may [create or manipulate a validation manifest directly]

Developers are advised to use the simplest mechanism which meets their requirements.
In most scenarios the validator builder API will be sufficient; _this is the recommended approach_.
The manifest model is a little more limited than working with the manifest directly but the API is created from [POCOs] which are easily serialized to/from other formats, such as JSON or databases.

[A **validator builder**]:WritingValidatorBuilders/index.md
[**manifest model**]:UsingTheManifestModel/index.md
[create or manipulate a validation manifest directly]:TheValidationManifest/index.md
[POCOs]:https://en.wikipedia.org/wiki/Plain_old_CLR_object

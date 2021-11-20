# Defining validators

Fundamentally, a validator is a service which executes a collection of independent validation rules upon an object and returns the result.
In this validation framework a validator is created from information about those rules and any configuration related to those rules. This information is called _a validation manifest_.

## Rules

Rules are individual, self-contained pieces of logic which fundamentally test for a pass or a fail.
Each validation rule in the validation manifest is a .NET class.
Developers are advised to read [the documentation for writing rule classes] & the [best practices and guidance] applicable to rules.

[the documentation for writing rule classes]:WritingValidationRules/index.md
[best practices and guidance]:../BestPractice/index.md#writing-validation-rule-classes
[`IRule<in TValidated>`]:xref:CSF.Validation.Rules.IRule`1
[`IRule<in TValue, in TParent>`]:xref:CSF.Validation.Rules.IRule`2

## Defining a validation manifest

The simplest and most clear way to define a validation manifest is via the declarative/fluent API provided by [a **validator builder**]. This is _the recommended approach_ if the validator manifest model is not required.

For occasions where the rules to execute or their configuration must be dynamic, [the validator **manifest model**] is available to define a validation manifest from a data-oriented approach.

It is also possible to create [a **validation manifest**] by-hand or programatically.  This might be suitable for advanced scenarios where neither a builder or model-based approach is viable.

[a **validator builder**]:WritingValidatorBuilders/index.md
[the validator **manifest model**]:UsingTheManifestModel/index.md
[a **validation manifest**]:TheValidationManifest/index.md
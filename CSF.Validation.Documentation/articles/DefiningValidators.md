# Defining validators

Fundamentally, a validator is a service which executes a collection of independent validation rules upon an object and returns the result.
In this validation framework a validator is created from information about those rules and any configuration related to those rules. This information is called _a validation manifest_.

## Rules

Each _[validation rule]_ in the validation manifest is a .NET class. These rule classes must implement at least one of two generic interfaces:

* [`IRule<TValidated>`]
* [`IRule<TValidated, TParent>`]

[validation rule]:WritingValidationRules.md
[`IRule<TValidated>`]:xref
[`IRule<TValidated, TParent>`]:xref

## Defining a validation manifest

The simplest and most clear way to define a validation manifest is via the declarative/fluent API provided by [a validator builder]. This is _the recommended approach_ if the validator manifest model is not required.

For occasions where the rules to execute or their configuration must be dynamic, [the validator manifest model] is available to define a validation manifest from a data-oriented approach.

[a validator builder]:WritingValidationBuilders.md
[the validator manifest model]:UsingTheValidationModel.md
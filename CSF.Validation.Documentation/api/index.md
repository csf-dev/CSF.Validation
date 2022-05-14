# CSF.Validation API documentation

## Getting started

If you wish to make use of the framework to create some validators and use them within your own application, including writing your own rule classes, then the regular documentation pages are a good place to start.
All of the documentation pages linked below contain links back to the relevant API types.

1. All apps must first [configure the framework] with some short one-time setup.  This makes the validation functionality available to your app's dependency injection.
2. Developers may then [define validators] within their business logic, likely [writing validation rule classes] as well.
3. Finally the application may [consume validators] and get validation results for validated objects.

[configure the framework]:../articles/ConfigureTheFramework.md
[define validators]:../articles/WritingValidators/index.md
[writing validation rule classes]:../articles/WritingValidators/WritingValidationRules/index.md
[consume validators]:../articles/ConsumingValidators.md

## Frequently-used types

This is a brief list of types which are expected to be frequently-used by developers making normal use of the framework.

* [`IGetsValidator`] is the entry-point for creating & consuming validators
* [`IValidator<TValidated>`] is the generic interface for performing validation on a specific object type
* [`IValidator`] is a non-generic equivalent of `IValidator<TValidated>` if the validated type is not known at design-time
* [`IBuildsValidator<TValidated>`] is the interface to implement when creating [a validator builder]
* [`IRule<TValidated>`] & [`IRule<TValidated,TParent>`] are [the two rule interfaces] which your own validation rule classes should implement

[`IGetsValidator`]:xref:CSF.Validation.IGetsValidator
[`IValidator<TValidated>`]:xref:CSF.Validation.IValidator`1
[`IValidator`]:xref:CSF.Validation.IValidator
[`IBuildsValidator<TValidated>`]:xref:CSF.Validation.IBuildsValidator`1
[a validator builder]:../articles/WritingValidators/WritingValidatorBuilders/index.md
[`IRule<TValidated>`]:xref:CSF.Validation.Rules.IRule`1
[`IRule<TValidated,TParent>`]:xref:CSF.Validation.Rules.IRule`2
[the two rule interfaces]:../articles/WritingValidators/WritingValidationRules/TheRuleInterfaces.md
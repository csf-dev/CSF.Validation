# Validation Manifests

The validation manifest is an object model which describes the composition of a validator.

_In almost all cases, developers do not need to work with a validation manifest directly._
If you wish to create a validator using .NET code then consider using a [Validator Builder].
If you would like to define a validator using data then consider the [Manifest Model] instead.
Working with a validation manifest directly is an advanced technique which is definitely not required in the most common scenarios.

[Validator Builder]: ../WritingValidatorBuilders/index.md
[Manifest Model]: ../UsingTheManifestModel/index.md

## Validation manifest classes

The validation manifest is created from the following model classes. These are all found in the [CSF.Validation.Abstractions] NuGet package and are generally in the [`CSF.Validation.Manifest`] namespace.

* [`VariationManifest`]
* [`ManifestValue`]
* [`ManifestRule`]
* [`ManifestRuleIdentifier`]
  * This type is a subclass of [`RuleIdentifierBase`]

[`CSF.Validation.Manifest`]:xref:CSF.Validation.Manifest
[CSF.Validation.Abstractions]:https://www.nuget.org/packages/CSF.Validation.Abstractions/
[`VariationManifest`]:xref:CSF.Validation.Manifest.ValidationManifest
[`ManifestValue`]:xref:CSF.Validation.Manifest.ManifestValue
[`ManifestRule`]:xref:CSF.Validation.Manifest.ManifestRule
[`ManifestRuleIdentifier`]:xref:CSF.Validation.Manifest.ManifestRuleIdentifier
[`RuleIdentifierBase`]:xref:CSF.Validation.Rules.RuleIdentifierBase

## Values & rules

A validation manifest describes values (including model objects) to be validated and the rules which should be applied to those values. A `ManifestValue` represents an object to be validated; this includes your own model objects but also their property values, such as strings and numbers.
Any non-primitive values typically have [a collection of child values] to be validated.

_A validation manifest (its manifest values) is contained within a `ValidationManifest` instance._
Unlike [The Manifest Model], the validation manifest class serves as a wrapper/root object for a manifest object graph.

Rules define the validation rules to be applied to values.
The `ManifestRule` class has properties that allows you to specify the validation rule type to be applied to the value, and optionally how that rule is configured.

[a collection of child values]::xref:CSF.Validation.Manifest.ManifestValue.Children
[The Manifest Model]:../UsingTheManifestModel/index.md
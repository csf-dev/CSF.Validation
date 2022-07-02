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
* [`ManifestCollectionItem`]
* [`ManifestPolymorphicType`]
* [`RecursiveManifestValue`]
* [`ManifestRule`]
* [`ManifestRuleIdentifier`]

[`CSF.Validation.Manifest`]:xref:CSF.Validation.Manifest
[CSF.Validation.Abstractions]:https://www.nuget.org/packages/CSF.Validation.Abstractions/
[`VariationManifest`]:xref:CSF.Validation.Manifest.ValidationManifest
[`ManifestValue`]:xref:CSF.Validation.Manifest.ManifestValue
[`ManifestCollectionItem`]:xref:CSF.Validation.Manifest.ManifestCollectionItem
[`ManifestPolymorphicType`]:xref:CSF.Validation.Manifest.ManifestPolymorphicType
[`RecursiveManifestValue`]:xref:CSF.Validation.Manifest.RecursiveManifestValue
[`ManifestRule`]:xref:CSF.Validation.Manifest.ManifestRule
[`ManifestRuleIdentifier`]:xref:CSF.Validation.Manifest.ManifestRuleIdentifier
[`RuleIdentifierBase`]:xref:CSF.Validation.Rules.RuleIdentifierBase

## Values & rules

### Values describe the objects to be validated

A validation manifest describes values (including model objects) to be validated and the rules which should be applied to those values. A `ManifestValue` represents an object to be validated.
This value may be of any type at all.
This includes classes/structs of your own design, types from other libraries or primitives such as `string`.

Manifest values create a hierarchical structure which corresponds to the design of the object graph you wish to validate.
Each manifest value has [a collection of child values]; each value in this collection corresponds to a value which is derived/accessed-from its parent value.
Usually 'accessed from' is simply a property-getter or other member access.
It could be derived via any arbitrary logic which could be described by a `Func<object, object>` though.
This makes it possible to validate complex object graphs, including many layers of traversal between objects.

_A validation manifest (its manifest values) is contained within a `ValidationManifest` instance._
Unlike [The Manifest Model], the validation manifest class serves as a wrapper/root object for a manifest object graph.

### Validating collection items

If the object represented by a particular manifest value implements `IEnumerable<T>` then the items within that collection may be validated individually.
In this case, set the `CollectionItemValue` property of the manifest value.
The manifest value corresponds to the collection as a whole, the collection item value corresponds to reach individual item.

### Validation rules

Rules define the validation rules to be applied to values.
The `ManifestRule` class has properties that allows you to specify the validation rule type to be applied to the value, and optionally how that rule is configured.

[a collection of child values]::xref:CSF.Validation.Manifest.ManifestValue.Children
[The Manifest Model]:../UsingTheManifestModel/index.md

## Why might you use the validation manifest

A developer might wish to use the validation manifest directly in order to create advanced validation scenarios.

Imagine a convention across a validated object graph whereby all `string` properties must not be `null`.
A developer could use reflection across their validated object graph in order to detect all properties of type `string` and to add [`NotNull`] rules for all of them automatically.

[`NotNull`]:xref:CSF.Validation.Rules.NotNull

## Converting a builder or manifest model to a validation manifest

Typically when using [a validator builder] or [the Manifest Model], the developer will want to create a validator from that builder or model via a method (or extension method) of [`IGetsValidator`].
It is possible, however, to convert a builder or manifest model into a validation manifest.
This is accomplished via these two interfaces:

* [`IGetsManifestFromBuilder`]
* [`IGetsValidationManifestFromModel`]

Once a builder or manifest model has been converted to a validation manifest, it may be supplemented with further
values and/or rules.
Developers may write logic which combines validation definitions created via builders/models with advanced techniques achieved by manipulating the validation manifest directly.

[a validator builder]: ../WritingValidatorBuilders/index.md
[`IGetsValidator`]:xref:CSF.Validation.IGetsValidator
[`IGetsManifestFromBuilder`]:xref:CSF.Validation.Manifest.IGetsManifestFromBuilder
[`IGetsValidationManifestFromModel`]:xref:CSF.Validation.ManifestModel.IGetsValidationManifestFromModel
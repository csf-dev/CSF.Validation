---
uid: ManifestModelIndexPage
---

# Using the Manifest Model

The manifest model is an object model which describes the composition of a validator.
Use the manifest model when you wish to create a validator **based upon data**.
For creating a validator with .NET code, a [Validation Builder] is a superior mechanism.

[Validation Builder]: ../WritingValidatorBuilders/index.md

## The Manifest Model classes

The manifest model is primarily three classes found in the [`CSF.Validation.ManifestModel`] namespace of the [CSF.Validation.Abstractions] NuGet package.
These three classes are:

* [`Value`]
* [`Rule`]
* [`RelativeIdentifier`]

These three classes have no dependencies upon other complex types or parts of the framework and are thus **suitable for serialization** to and from other data-types.
This allows validators to be defined using technologies such as JSON, XML, a relational database or any other format which may be converted to/from the three classes above.

[`CSF.Validation.ManifestModel`]:xref:CSF.Validation.ManifestModel
[CSF.Validation.Abstractions]:https://www.nuget.org/packages/CSF.Validation.Abstractions/
[`Value`]:xref:CSF.Validation.ManifestModel.Value
[`Rule`]:xref:CSF.Validation.ManifestModel.Rule
[`RelativeIdentifier`]:xref:CSF.Validation.ManifestModel.RelativeIdentifier

## Values & rules

Using the manifest model is primarily about declaring the appropriate values and rules.
A `Value` represents an object to be validated; this includes your own model objects but also their property values, such as strings and numbers.
Any non-primitive values typically have [a collection of child values] to be validated.

_All manifest models use a single `Value` instance as the root of the model._
Unlike the [`ValidationManifest`] class, there is no particular type which serves as the root of the object model.

Rules define the validation rules to be applied to values.
The `Rule` class has properties that allows you to specify the validation rule type to be applied to the value, and optionally how that rule is configured.

[a collection of child values]:xref:CSF.Validation.ManifestModel.ValueBase.Children
[`ValidationManifest`]:xref:CSF.Validation.Manifest.ValidationManifest

### Specifying rule-dependencies

Dependencies between validation rules may be specified using the manifest model.
To do so, use the [`Rule.Dependencies`] property using relative identifiers.

[`Rule.Dependencies`]:xref:CSF.Validation.ManifestModel.Rule.Dependencies

## Example

For an example of how this might look when serialized to JSON, please read through the [example of using the Manifest Model].

[example of using the Manifest Model]:ExampleOfTheManifestModel.md

## Converting a Manifest Model into a Validation Manifest

If you wish to directly convert a Manifest Model (IE: an instance of the `Value` class, possibly with many descendent values & rules) into an instance of [`ValidationManifest`] then you may do so by injecting an using an instance of [`IGetsValidationManifestFromModel`].

If all you want is a validator, though, then [`IGetsValidator`] includes [an overload which works from a Manifest Model `Value` and a `System.Type`].

[`IGetsValidationManifestFromModel`]:xref:CSF.Validation.ManifestModel.IGetsValidationManifestFromModel
[`IGetsValidator`]:xref:CSF.Validation.IGetsValidator
[an overload which works from a Manifest Model `Value` and a `System.Type`]:xref:CSF.Validation.IGetsValidator.GetValidator(CSF.Validation.ManifestModel.Value,System.Type)

## Manifest Model limitations

The Manifest Model sacrifices some functionality in order to provide an easily-serializable model.
For more information please read the article about [the Manifest Model's limitations].

[the Manifest Model's limitations]:Limitations.md
# Limitations of the Manifest Model

The manifest model provides a simplified API which is well-suited to serialization. This introduces a number of limitations into the validators that a manifest model may describe.

## Limitations upon child values

When using [a validator builder] or when using [the validation manifest] directly, it is possible to use arbitrary accessor logic to derive child values.

* In a builder you would use [`ForValue`] or [`ForValues`]
* In the manifest you would set [`ManifestValue.AccessorFromParent`] to an arbitrary function

Using the manifest model _you may only specify the `string` name of a member_ by which to access the child value.
This could be public property, field or parameterless method.
You may not use arbitrary logic to access child values.

[a validator builder]:../WritingValidatorBuilders/index.md
[the validation manifest]:../TheValidationManifest/index.md
[`ForValue`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValue``1(System.Func{`0,``0},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ForValues`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValues``1(System.Func{`0,System.Collections.Generic.IEnumerable{``0}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ManifestValue.AccessorFromParent`]:xref:CSF.Validation.Manifest.ManifestValue.AccessorFromParent

## You may not validate both a collection value and its items

When using [a validator builder] or [the validation manifest] and dealing with a value that is a collection type (an  `IEnumerable<T>`), it is possible to add both rules which validate the collection value as a whole and also which validate items within that collection.

* Using a builder, you would use both of [`ForMemberItems`]/[`ForValues`] as well as [`ForMember`]/[`ForValue`]
* Using the manifest you would add two [`ManifestValue`] instances to the [`Children`] of the parent value, one where [`EnumerateItems`] is `true` and one where it were `false`

This technique is unavailable and has no equivalent when using the manifest model.
You must choose either to value the enumerated items of the collection or to validate the entire collection on aggregate.

[`ForMemberItems`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForMemberItems``1(System.Linq.Expressions.Expression{System.Func{`0,System.Collections.Generic.IEnumerable{``0}}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ForMember`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForMember``1(System.Linq.Expressions.Expression{System.Func{`0,``0}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ManifestValue`]:xref:CSF.Validation.Manifest.ManifestValue
[`Children`]:xref:CSF.Validation.Manifest.ManifestValue.Children
[`EnumerateItems`]:xref:CSF.Validation.Manifest.ManifestValue.EnumerateItems

## No type safety

It will be apparent when using [a validator builder] that the API presents type safety, which allows IDE auto-completion (aka "Intellisense") of validated properties.
It also provides compile-time checking of types, accessors and rule selections.
The Manifest Model does not provide this type safety.
This means that it is easier to accidentally create a faulty validation manifest, which will raise exceptions when used at runtime.

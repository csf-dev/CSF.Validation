# Limitations of the Manifest Model

The manifest model provides a simplified API which is well-suited to serialization. This introduces a number of limitations into the validators that a manifest model may describe.

## Limitations upon child values

When using [a validator builder] or when using [the validation manifest] directly, it is possible to use arbitrary accessor logic to derive child values.

* In a builder you would use [`ForValue`] or [`ForValues`]
* In the manifest you would set [`ManifestItem.AccessorFromParent`] to an arbitrary function

Using the manifest model _you may only specify the `string` name of a member_ by which to access the child value.
This could be public property, field or parameterless method.
You may not use arbitrary logic to access child values.

[a validator builder]:../WritingValidatorBuilders/index.md
[the validation manifest]:../TheValidationManifest/index.md
[`ForValue`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValue``1(System.Func{`0,``0},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ForValues`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValues``1(System.Func{`0,System.Collections.Generic.IEnumerable{``0}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ManifestItem.AccessorFromParent`]:xref:CSF.Validation.Manifest.ManifestItem.AccessorFromParent

## No type safety

It will be apparent when using [a validator builder] that it is a type-safe API.
This allows IDE auto-completion (aka "Intellisense") of validated properties.
It also provides compile-time checking of types, accessors and rule selections.

The Manifest Model does not provide this type safety.
This means that it is easier to accidentally create a faulty validation manifest, which will raise exceptions when used at runtime.
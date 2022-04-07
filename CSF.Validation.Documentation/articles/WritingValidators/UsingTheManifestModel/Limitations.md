# Limitations of the Manifest Model

The manifest model provides a simplified API which is well-suited to serialization. This introduces a number of limitations into the validators that a manifest model may describe.

## Limitations upon child values

When using [a validator builder] or when [using the validation manifest directly], it is possible to use arbitrary accessor logic to derive child values.

* In a builder you would use [`ForValue`] or [`ForValues`]
* In the manifest you would set [`ManifestValue.AccessorFromParent`] to an arbitrary function

Using the manifest model you may only provide the `string` name of a member by which to access the child value. This could be public property, field or parameterless method.

[a validator builder]:../WritingValidatorBuilders/index.md
[using the validation manifest directly]:../TheValidationManifest/index.md
[`ForValue`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValue``1(System.Func{`0,``0},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ForValues`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValues``1(System.Func{`0,System.Collections.Generic.IEnumerable{``0}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ManifestValue.AccessorFromParent`]:xref:CSF.Validation.Manifest.ManifestValue.AccessorFromParent
# Limitations of the Manifest Model

The manifest model provides a simplified API which is well-suited to serialization. This introduces a number of limitations into the validators that a manifest model may describe.

## Child values may only be derived from member accessors

When using [a validator builder] or when [using the validation manifest directly], it is possible to use arbitrary accessor logic to derive child values.

* In a builder you would use [`ForValue`] or [`ForValues`]
* In the manifest you would set [`ManifestValue.AccessorFromParent`] to an arbitrary function

Using the manifest model you may only provide the `string` name of a member by which to access the child value. This could be public property, field or parameterless method.

[a validator builder]:../WritingValidationBuilders/index.md
[using the validation manifest directly]:../The ValidationManifestindex.md
[`ForValue`]:TODO
[`ForValues`]:TODO
[`ManifestValue.AccessorFromParent`]:TODO
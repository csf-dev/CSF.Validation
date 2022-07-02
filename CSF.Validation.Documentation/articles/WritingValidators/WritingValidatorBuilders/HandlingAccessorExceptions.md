---
uid: HandlingAccessorExceptions
---
# Handling accessor exceptions

When validating derived values or member values, there are occasionally instances when the accessor (executing a method, or a property getter, or arbitrary accessor logic) could raise an exception.

The CSF.Validation framework has three available behaviours to deal with this, represented in [the `ValueAccessExceptionBehaviour` enum].
Theese three behaviours are explained in the documentaion for that enumeration.

[the `ValueAccessExceptionBehaviour` enum]:xref:CSF.Validation.Manifest.ValueAccessExceptionBehaviour
[`Errored` outcome]:xref:CSF.Validation.Rules.RuleOutcome

## How to use these behaviours

The behaviour may be specified either or both per-manifest value & upon the [`ValidationOptions`] used when validation is performed.
To specify the behaviour using a validator builder, use the [`AccessorExceptionBehaviour`] method upon the builder, specifying the behaviour you wish to use.

When behaviour is not specified (it is `null`) upon a manifest value, then the behaviour specified in the validation options will be used.
When the behaviour is specified in both places, the behaviour upon the manifest value will be used instead.

[`ValidationOptions`]:xref:CSF.Validation.ValidationOptions
[`AccessorExceptionBehaviour`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor`2.AccessorExceptionBehaviour(System.Nullable{CSF.Validation.Manifest.ValueAccessExceptionBehaviour})

# Validating items within a collection

Sometimes validation calls for verifying that each item within a collection of items is valid.
Where a member or validated value is a type that implements `IEnumerable<T>`, it is possible to have a validator enumerate that collection and validate each item found.
This technique is accomplished via [the `ForMemberItems()`] and [the `ForValues()`] methods.

[the `ForMemberItems()`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForMemberItems``1(System.Linq.Expressions.Expression{System.Func{`0,System.Collections.Generic.IEnumerable{``0}}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[the `ForValues()`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValues``1(System.Func{`0,System.Collections.Generic.IEnumerable{``0}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})

## Prefer `ForMemberItems` when possible

In the majority of cases it should be possible to use `ForMemberItems`.
Use this where the collection is exposed by a single member, like a property.
This is _the recommended approach_ where it can be used.

The `ForValues` method allows you to write arbitrary getter-logic to retrieve the collection.
However, this approach will not associate any rules for the value with any particular member of the validated object.
This could make the results to those rules harder to consume.

## Validating the collection as a whole

Whilst the techniques described above allows for validation within a collection, [the `ForMember`] and [`ForValue`] techniques _remain available_.
Indeed, _you may use both techniques_ with a single collection.

Use `ForMember` or `ForValue` with a collection when you wish to validate the collection in aggregate, for example the count of items or the sum of contained values.

[the `ForMember`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForMember``1(System.Linq.Expressions.Expression{System.Func{`0,``0}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`ForValue`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValue``1(System.Func{`0,``0},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})

## Import another builder to validate collections of models

If the collection items are models of any non-trivial complexity then consider defining the rules for those model items within their own validator builder. This builder [would then be imported] for validating items within the collection.

This might look like the following:

```csharp
builder.ForMemberItems(x => x.MyCollectionProperty, p => {
  p.AddRules<ItemValidatorBuilder>();
});
```

In this example, the class `ItemValidatorBuilder` is an implementation of `IBuildsValidator<TValidated>` for the generic type that matches the items within the collection `MyCollectionProperty`.

[would then be imported]:ImportingRules.md
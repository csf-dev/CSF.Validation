# Validating items within a collection

Sometimes validation calls for verifying that each item within a collection of items is valid.
Where a member or validated value is a type that implements `IEnumerable<T>`, it is possible to have a validator enumerate that collection and validate each item found.
This technique is accomplished via [the `ForMemberItems()`] and [the `ForValues()`] methods.

[the `ForMemberItems()`]:todo
[the `ForValues()`]:todo

## Prefer `ForMemberItems` when possible

In the majority of cases it should be possible to use `ForMemberItems`.
Use this where the collection is exposed by a single member, like a property.
This is _the recommended approach_ where it can be used.

The `ForValues` method allows you to write arbitrary getter-logic to retrieve the collection.
However, this approach will not associate any rules for the value with any particular member of the validated object.
This could make the results to those rules harder to consume.

## Validating the whole collection

Whilst the techniques described above allows for validation within a collection, [the `ForMember`] and [`ForValue`] techniques remain available.
Indeed, _you may use both techniques_ with a single collection.

This allows you to perform validation upon the items individually and yet also validate the collection as an aggregate.

[the `ForMember`]:todo
[`ForValue`]:todo

## Example: Validating collection items
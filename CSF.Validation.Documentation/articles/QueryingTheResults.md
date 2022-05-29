# Querying validation results

A strongly-typed validator returns an instance of [`IQueryableValidationResult<T>`].
This interface implements `IEnumerable<ValidationRuleResult>` and so it is possible to query it using Linq.
The validation framework also adds a few methods to make querying even easier:

All of the following methods work a lot like `.Where()` in Linq; each returns a filtered collection of results, _without modifying the original result object_.

## `ForMember` narrows the results by member

The [`ForMember`] method is used to filter/narrow a collection of results to only those which relate to a member, or descendent values derived from that member's value.
In essence this allows narrowing of the results by a property or other member.

## `ForMatchingMemberItem` narrows the results by a collection item

The [`ForMatchingMemberItem`] method is similar to `ForMember` (above) except that it works with collections.
As well as an expression identifying the collection property, you also specify the item (from the original validated object) which you wish to narrow-by.

## `WithoutSuccesses` removes successes from the result

In order to provide confidence, every validation result includes a result for every rule which was executed, including rules which had passing results.
The [`WithoutSuccesses`] method filters those results to only rules which did not pass.

## `ForOnlyThisValue` removes results which relate to descendent values

A validation result for a complex object model might include many results relating to descendent values.
The [`ForOnlyThisValue`] method gets only results which relate to the current _root_ of the validation result.
This root is either the primary object being validated, or a different value which has been traversed-to via `ForMember` and/or `ForMatchingMemberItem`.

[`IQueryableValidationResult<T>`]:xref:CSF.Validation.IQueryableValidationResult`1
[`ForMember`]:xref:CSF.Validation.IQueryableValidationResult`1.ForMember``1(System.Linq.Expressions.Expression{System.Func{`0,``0}})
[`ForMatchingMemberItem`]:xref:CSF.Validation.IQueryableValidationResult`1.ForMatchingMemberItem``1(System.Linq.Expressions.Expression{System.Func{`0,System.Collections.Generic.IEnumerable{``0}}},``0)
[`WithoutSuccesses`]:xref:CSF.Validation.IQueryableValidationResult`1.WithoutSuccesses
[`ForOnlyThisValue`]:xref:CSF.Validation.IQueryableValidationResult`1.ForOnlyThisValue

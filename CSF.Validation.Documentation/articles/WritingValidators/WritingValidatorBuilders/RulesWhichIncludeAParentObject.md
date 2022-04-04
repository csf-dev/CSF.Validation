# Rules which receive a parent value

Many rules will validate just a single value. _"This string must be at least 5 characters long"_ or _"This number must not be negative"_ and so on. In both these cases it does not matter where the string or number came from.
For some rules though, they require some context in the form of the object which provided the value under validation. For example _"This date (representing a start date) must be before the end-date from the same parent object"_.

In this case, write your rules using [the interface `IRule<TValue, TParent>`] and add them to the builder using [the `AddRuleWithParent` method] when configuring rules for a member or value.

[the interface `IRule<TValue, TParent>`]: xref:CSF.Validation.Rules.IRule`2
[the `AddRuleWithParent` method]: xref:CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor`2.AddRuleWithParent``1(System.Action{CSF.Validation.ValidatorBuilding.IConfiguresRule{``0}})

## Rules which use more distant ancestors

The mechanism described above provides the rule with access to the value's immediate parent.
If the rule needs access to a more distant parent object then the rule should make use of [the `AncestorContexts` property] of the [`RuleContext`] parameter that is provided to the rule's `GetResultAsync` method.

This property provides access to a depth-ordered collection (parent first, then grandparent, then great-grandparent etc) of the role contexts for ancestors of the current value.

Within these ancestor contexts, [the `Object` property] provides access to the ancestor object itself. There is no type-safe way to access this, so rule logic will need to cast/safe-cast it to the appropriate type.

[the `AncestorContexts` property]: xref:CSF.Validation.Rules.RuleContext.AncestorContexts
[`RuleContext`]: xref:CSF.Validation.Rules.RuleContext
[the `Object` property]: xref:CSF.Validation.Rules.AncestorRuleContext.Object
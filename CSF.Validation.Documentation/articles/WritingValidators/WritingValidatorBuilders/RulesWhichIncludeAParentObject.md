# Rules which receive a parent value

Many rules will validate just a single value. _"This string must be at least 5 characters long"_ or _"this number must not be negative"_ and so on. In both these cases it does not matter where the string or number come from.

For some rules though, they require some context in the form of the object which provided the value under validation. For example _"this date must be after the end-date from the parent object"_.

In this case, write your rules using [the interface `IRule<TValue, TParent>`] and add them to the builder using [the `AddRuleWithParent` method] when configuring rules for a member or value.

[the interface `IRule<TValue, TParent>`]:TODO
[the `AddRuleWithParent` method]:TODO
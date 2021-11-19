# A rule should only fail for one conceptual reason

Good rules should return a failure result for only one reason/scenario.
In this way, it is perhaps **better not to think of a rule** as _"Pass if all of multiple conditions are met"_ but instead to think of a rule as _"Fail if one of multiple conditions is not met"_.
When defining the validator it is easy to associate many rules with a single value, where one rule covers each conceptual failure scenario.

By using separate rules for each conceptual failure scenario, the feedback from validation failures (for example UI messages) can be more relevant to the actual failure reason.
This is more helpful than providing an unspecific list of all the possible failure reasons.
Additionally, rules which only fail for a single reason are easier to reuse.  If a rule class conflates conceptual failure reasons then you might find yourself needing to duplicate logic into further rule classes _"Like Rule X, except allow it to pass if Y"_.

## Rules should not assume a value is mandatory

A very common specialisation of this advice is for validating values that could be `null` or 'missing', like empty strings/collections.
Any rule which validates such a value should return a _Pass_ outcome if that value is `null` or missing.
This way, the same rule may be used whether the value is mandatory/required or not.
In cases where the value is mandatory, you should also enforce that with (for example) a [`NotNull`] rule.

[`NotNull`]:xref:CSF.Validation.Rules.NotNull

## An example

Imagine you are writing a rule for the format of a `string` to validate that it is plausibly a valid US ZIP code.
Such a rule _should return a Pass result_ if the string is `null` or is empty.
This is because "empty" & "missing" are conceptually different to "present, but invalid".
If the rule required the value to be present then it could not be reused to validate a non-mandatory ZIP code.

If the value is mandatory then it is easy to add a [`NotNullOrEmpty`] rule to the validator alongside the format-validation rule.

[`NotNullOrEmpty`]:xref:CSF.Validation.Rules.NotNullOrEmpty

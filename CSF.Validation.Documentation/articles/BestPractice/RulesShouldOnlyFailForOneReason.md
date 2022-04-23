---
uid: RulesShouldOnlyFailForOneReason
---
# A rule should only fail for one conceptual reason

Good rules should return a failure result for only one reason/scenario.
It is better _not_ to think of a rule as _"Pass if all required conditions are met"_ but instead to think of a rule as _"Fail if one condition is not met"_.
When defining the validator it is easy to combine many rules to validate a single value, where one rule covers each conceptual failure scenario.

The advantages of splitting complex rules up are twofold:

* If a rule fails and a user must be shown a feedback message, it is easier to provide a concise & relevant message if the reason for failure is known precisely.
* Rules become more reusable if they do not conflate failure scenarios because they may be used individually.

Because rules work with dependency injection, it is easy to write and share logic between rules if required (just move the logic into a separate injectable service).  Rules may even use other rules as dependencies.  Look at the [`NotNullOrEmpty`] rule as an example of this technique.


## Rules should not assume a value is mandatory

A very common specialisation of this advice is for validating values that could be `null` or 'missing', such as empty strings/collections.
A well-designed rule which validates a nullable value should return a _Pass_ outcome if that value is `null`.

This way, the same rule may be reused _regardless of whether the value is optional or mandatory_.
In cases where the value is mandatory you would add (for example) a [`NotNull`] rule for the same value.

[`NotNull`]:xref:CSF.Validation.Rules.NotNull

## An example

Imagine you are writing a rule for the format of a `string` to validate that it is in the correct format to be a valid US ZIP code.

This rule _should return a Pass result_ if the string is `null` (and perhaps if it is an empty string).
This is because "empty" & "missing" are conceptually different to "present, but invalid".
If the rule failed for a `null` string then it could not be reused to validate an optional ZIP code.

If the value is mandatory then it is easy to add a [`NotNullOrEmpty`] rule to the validator alongside the format-validation rule.

[`NotNullOrEmpty`]:xref:CSF.Validation.Rules.NotNullOrEmpty

---
uid: WritingValidationRules
---
# Writing Validation Rules

A validator is made from validation rules; each rule represents an independent piece of logic that performs a pass/fail test.
In CSF.Validation, a rule is **a .NET class** which implements [one of the two rule interfaces].
The logic within the `GetResultAsync` method(s) of rule classes should usually [return a pass or fail result or throw an exception],

For advanced usage, rules may include [a dictionary of supplemental information] along with their outcome.
Imagine a scenario where some data retrieved from a dependency service (injected into a rule class) provides useful context that should be included in a UI feedback message.

You are also encouraged to read [this advice regarding best practices] for writing validation rules.

[one of the two rule interfaces]:TheRuleInterfaces.md
[return a pass or fail result or throw an exception]:ReturningResultsFromRules.md
[a dictionary of supplemental information]:ResultData.md
[this advice regarding best practices]:../../BestPractice/index.md

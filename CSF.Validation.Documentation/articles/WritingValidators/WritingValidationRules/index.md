---
uid: WritingValidationRules
---
# Writing Validation Rules

A validator is made from validation rules; each rule represents an independent piece of logic that performs a pass/fail test.
In CSF.Validation, a rule is **a .NET class** which implements at least [one of **the two rule interfaces**].
A single rule class may implement both interfaces and/or may implement the same interface for several generic types if appropriate.

## Rules overview

Both rule interfaces define the method `GetResultAsync` (with parameters appropriate to the corresponding generic rule interface).
This method performs a single validation test and should usually [return either a pass/fail result or should throw an exception].

In more advanced scenarios, rules may supplement their result with [a dictionary of arbitrary data].
This is useful when the validation result would benefit from some further contextual information.

You are also encouraged to read [this advice regarding best practices] for writing validation rules.

[one of **the two rule interfaces**]:TheRuleInterfaces.md
[return either a pass/fail result or should throw an exception]:ReturningResultsFromRules.md
[a dictionary of arbitrary data]:ResultData.md
[this advice regarding best practices]:../../BestPractice/index.md

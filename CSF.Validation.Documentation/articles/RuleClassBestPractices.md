---
uid: ValidationRuleBestPractices
---
# Rule class best practices

These are hints and guidance to help you write good [rule classes] that promote reuse and make good validators.

## A rule should only fail for one reason

Good rules should return a failure result for only one reason/scenario. In this way, it is perhaps better to think of a rule not as _"Pass if these conditions are met"_ but instead as _"Fail if one condition is not met"_.

For example, imagine you are writing a rule for the format of a `string` to validate that it is plausibly a valid US ZIP code. This rule should return a _Pass_ result if the string is `null` or is empty. This is because "empty" or "missing" are conceptually different to "present, but invalid".

If a rule were to conflate not-null/not-empty with format-validation then such a rule could not be used with a ZIP code value which is optional. If the value is mandatory then it is easy to combine format-validation with [`NotNull`] & [`NotEmpty`] rules. By using different rules to cover "invalid-format" and "missing", you may also provide better feedback in a UI.

Following on from the above, generally-speaking it is good for any rule which validates a [nullable reference type] to return a _Pass_ outcome if the value is `null`.

[`NotNull`]
[`NotEmpty`]

## Avoid unnecessary error results

Whilst it is OK for validation rule classes to throw unhandled exceptions, which will be translated to _Error_ results by the validator, developers should continue to follow best practices for exceptions.

Error results are treated like failures by the validator, but they should still only be used for truly unexpected/exceptional scenarios. It is more difficult to make use of an Error result than a Failure result.

If your rule has an expectable error condition that relates to the object being validated, then year for this condition and return a _Pass_ result if it is encountered. Then write & use a separate rule to fail specifically on that condition. This also helps avoid confusing validation results by not returning more than one failure for the same conceptual reason.

## Inject dependencies into rules

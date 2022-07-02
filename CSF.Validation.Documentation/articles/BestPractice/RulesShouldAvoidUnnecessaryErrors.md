# Avoid unnecessary error results from rules

Whilst it is OK for validation rule classes to throw unhandled exceptions, which will be translated to _Error_ results by the validator, developers should continue to follow best practices for exceptions.

Error results are treated like failures by the validator, but they should still only be used for truly unexpected/exceptional scenarios. It is more difficult to make meaningful use of an Error result than a Failure result.

If a rule has complex logic which cannot be meaningfully executed for an expected reason then it is best to return a _Pass_ result in this scenario.
Then, in a different validation rule applied to the same value, return a _Fail_ result for that scenario which would have broken the first rule.

This way, we treat the error scenario as a 'conceptually different failure reason' (see above) and return a specific failure for that, instead of raising an error from the more complex rule.

## Example

Here is a simple example of a rule which could raise an error for scenarios which could be reasonable expected.

```csharp
// BAD, raises an error if the string is null
// or has fewer than 5 characters length.

public class FifthCharacterMustBeY : IRule<string>
{
    Task<RuleResult> GetResultAsync(string validated,
                                    RuleContext context,
                                    CancellationToken token = default)
    {
        return validated[4] == 'Y' ? PassAsync() : FailAsync();
    }
}

// GOOD, there's no way this could raise an error now.
// When assigning this rule to a string value, if that value is
// mandatory and must have length of at least 5 then also assign
// a NotNull and a StringLength rule to the same value.

public class FifthCharacterMustBeY : IRule<string>
{
    Task<RuleResult> GetResultAsync(string validated,
                                    RuleContext context,
                                    CancellationToken token = default)
    {
        if(validated is null || validated.Length < 5) return PassAsync();
        return validated[4] == 'Y' ? PassAsync() : FailAsync();
    }
}
```

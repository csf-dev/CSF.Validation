# Returning results from rules

Executing the `GetResultAsync` method(s) of a rule class should most commonly lead to one of two outcomes:

* The method returns a pass result
* The method returns a failure result

In _truly exceptional_ scenarios it is acceptable to either throw an exception from the rule class or to return an error result.

## Returning a pass or failure result

In order to return pass/failure results [it is strongly recommended] to use [the static class **`CommonResults`**].
This class is intended to be used via the `using static` syntax, as demonstrated below.

```csharp
using static CSF.Validation.Rules.CommonResults;

public class NotAString : IRule<object>
{
    public Task<RuleResult> GetResultAsync(object validated,
                                           RuleContext context,
                                           CancellationToken token = default)
    {
        // The FailAsync and PassAsync methods are defined on CommonResults.
        return validated is string ? FailAsync() : PassAsync();
    }
}
```

As well as the pass & fail scenarios here, there are also methods dealing with _error results_; we will cover these below.

[it is strongly recommended]:../../BestPractice/UseCommonRuleResults.md
[the static class **`CommonResults`**]:xref:CSF.Validation.Rules.CommonResults
[`CommonResults`]:xref:CSF.Validation.Rules.CommonResults

### Synchronous or asynchronous operation

The example above is of a rule that runs synchronously; there is nothing to `await` and the `GetResultAsync` method does not need to be made `async`.
All overloads of the `PassAsync`, `FailAsync` & `ErrorAsync` methods upon the [`CommonResults`] class will return completed tasks with a result already available.

Rules may also operate asynchronously.
This is useful where the rule logic requires use of an awaitable method from a dependency, such as reading from a database.
For these results you may use overloads of the `Pass`, `Fail` & `Error` methods from [`CommonResults`].
These methods are more suitable when you wish to use the `async` keyword with the `GetResultAsync` method.

### Returning additional data

All of the methods defined on the `CommonResults` class present an overload that accepts [a dictionary of result data].
This allows advanced usages scenarios in which supplemental information may be returned along with the overall pass, fail or error outcome.

[a dictionary of result data]:ResultData.md

## Throwing exceptions from rule classes

Non-trivial rules can raise unexpected exceptions, it is a simple fact of software development.
For example a rule which must read a value from a database is likely to throw an exception if the database is unavailable.

CSF.Validation attempts to minimuse the amount of boilerplate `try/catch` that developers must write in their rule classes.
Any and all exceptions which originate in a `GetResultAsync` method are caught automatically by the framework.
These exceptions are translated to validation results which have an outcome of [`RuleOutcome.Errored`].
The [`ValidationRuleResult`] resulting from this caught exception will include [a reference to the exception which was thrown by the rule].

[`RuleOutcome.Errored`]:xref:CSF.Validation.Rules.RuleOutcome
[a reference to the exception which was thrown by the rule]:xref:CSF.Validation.Rules.RuleResult.Exception
[`ValidationRuleResult`]:xref:CSF.Validation.ValidationRuleResult

### How the validator treats errors

The validator treats error results similarly to failures; an error result does not count as a pass.
For rules which have dependencies, if any of a rule's dependencies' results is an error then the dependent rule will not be executed.

When one or more validation results has the outcome [`RuleOutcome.Errored`], the framework's default behaviour is to throw an exception at the end of validation.
That exception will contain the complete validation result including all of the error results.
This behaviour may be overridden by specifying [the `RuleThrowingBehaviour` property] on [an instance of `ValidationOptions`] passed to the validator.

[the `RuleThrowingBehaviour` property]:xref:CSF.Validation.ValidationOptions.RuleThrowingBehaviour
[an instance of `ValidationOptions`]:xref:CSF.Validation.ValidationOptions

### Returning error results manually

It is also possible, using [the `CommonResults` class] to manually create and return an error result, via either of the `ErrorAsync()` or `Error()` methods.
The only practical difference between using these methods and allowing the rule `GetResultAsync` method to throw an exception is:

* A manually-returned error result does not require an exception
* A manually-returned error result may contain [a dictionary of result data]

[the `CommonResults` class]:xref:CSF.Validation.Rules.CommonResults
[a dictionary of result data]:ResultData.md
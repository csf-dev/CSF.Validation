# Use the common rule results

Use the static [`CommonResults`] class within your rules to get result instances/tasks of result instances.
You should use the `CommonResults` class as follows.

```csharp
using static CSF.Validation.Rules.CommonResults;

public class MustBeFour : IRule<int>
{
    public ValueTask<RuleResult> GetResultAsync(int validated,
                                                RuleContext context,
                                                CancellationToken token = default)
    {
        return validated == 4 ? PassAsync() : FailAsync();
    }
}
```

[`CommonResults`]:xref:CSF.Validation.Rules.CommonResults

## Optimisation as well as convenience

When omitting the optional parameters (or passing only `null`) to the following methods, the `CommonResults` class offers an optimisation.
In these scenarios [flyweight instances] will be returned instead of allocating new [`RuleResult`] instances.

* [`Pass()`]
* [`PassAsync()`]
* [`Fail()`]
* [`FailAsync()`]
* [`Error()`]
* [`ErrorAsync()`]

This will improve throughput, particularly for validators which include a large number of rules which do not need to return data when they pass or fail.

[flyweight instances]:https://en.wikipedia.org/wiki/Flyweight_pattern
[`RuleResult`]:xref:CSF.Validation.Rules.RuleResult
[`Pass()`]:xref:CSF.Validation.Rules.CommonResults.Pass(System.Collections.Generic.IDictionary{System.String,System.Object})
[`PassAsync()`]:xref:CSF.Validation.Rules.CommonResults.PassAsync(System.Collections.Generic.IDictionary{System.String,System.Object})
[`Fail()`]:xref:CSF.Validation.Rules.CommonResults.Fail(System.Collections.Generic.IDictionary{System.String,System.Object})
[`FailAsync()`]:xref:CSF.Validation.Rules.CommonResults.FailAsync(System.Collections.Generic.IDictionary{System.String,System.Object})
[`Error()`]:xref:CSF.Validation.Rules.CommonResults.Error(System.Exception,System.Collections.Generic.IDictionary{System.String,System.Object})
[`ErrorAsync()`]:xref:CSF.Validation.Rules.CommonResults.ErrorAsync(System.Exception,System.Collections.Generic.IDictionary{System.String,System.Object})

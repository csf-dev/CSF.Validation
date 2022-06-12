# Rule cancellation and timeouts

Many validation rules execute very quickly and will return synchronous results via completed tasks.
The `GetResultAsync` method of [the two rule interfaces] receives a `CancellationToken` though, which allows the developer to deal with task-cancellation more gracefully when a rule might take more than a trivial amount of time to execute.

[the two rule interfaces]:../WritingValidators/WritingValidationRules/TheRuleInterfaces.md

## Do not check for cancellation at the start of a rule

You might be tempted to check for cancellation at the beginning of a validation rule's logic, such as via [the `ThrowIfCancellationRequested` method].
There is no need to do this; it is already executed immediately before executing each rule's logic.

In a similar manner, it is also pointless to check for cancallation at the very end of the rule, before returning a result.

[the `ThrowIfCancellationRequested` method]:https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.throwifcancellationrequested

## Do not check for cancellation in trivial rules

If your rule logic is trivial then there is no need to check for cancellation or to abort its execution early if the token is cancelled.
It does not matter if the rule is allowed to run to completion if it takes only a few milliseconds extra.

## Pass the cancellation token 'downwards'

Where a validation rule's logic makes use of dependencies which provide an asynchronous API, accepting a cancellation token, then pass the same cancellation token to these async methods.
This will allow them to handle cancellation gracefully in their own way.
It will avoid the rule developer to need to write cancellation-handling logic of their own.

## Check for cancellation if dealing with APIs that do not handle cancellation

Legacy APIs, or those written more naively, might not accept a cancellation token.
In this case it makes sense to add cancellation-checking such as `ThrowIfCancellationRequested` between calls to long-running/expensive logic.

## Use the `IHasRuleTimeout` interface to specify a per-rule timeout

The overall validation process: the `ValidateAsync` method accepts a cancellation token.
This may be used to specify a timeout for the overall validation process; if this token is cancelled then the whole validation operation will cease and the validator will raise an exception.

If individual rules risk running for too long (for example, a rule which depends upon an external/network source such as a database server) then an individual per-rule timeout may be specified.
To do this, have you rule class additionally implement [the interface `IHasRuleTimeout`].
This interface adds a `GetTimeout` method which should return a `TimeSpan` (or a `null` reference, indicating that no timeout is applicable).
If a rule implements `IHasRuleTimeout` and it takes longer than that timeout value to execute then:

* The cancellation token passed as a parameter to the rule will be cancelled
* The rule will immediately return an error result
* That error result will contain _either_:
  * An `OperationCanceledException`
  * If it contains no exception then it will have a [`RuleResult.Data`] item with a key equal to "`Validation rule timeout`" and value equal to the `TimeSpan` of the timeout which was exceeded.

Note that in this case, the validation framework does not force the early termination of the rule logic, which could still be running in the background.
This is the job of the rule logic and/or any asynchronous logic which it consumes.
Even when using `IHasRuleTimeout`, it is still important to make use of the cancellation token received in the rule's `GetResultAsync` method, for the purpose of terminating the logic early if/when it has been cancelled.

[the interface `IHasRuleTimeout`]:xref:CSF.Validation.Rules.IHasRuleTimeout
[`RuleResult.Data`]:xref:CSF.Validation.Rules.RuleResult.Data

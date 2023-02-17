---
uid: WritingMessageProviders
---
# Writing feedback message providers

A feedback message provider is an object which implements at least one of these interfaces:

* [`IGetsFailureMessage`]
* [`IGetsFailureMessage<in TValidated>`]
* [`IGetsFailureMessage<in TValidated, in TParent>`]

You will likely notice that these are quite similar to [the rule interfaces], with the addition of a non-generic interface.
It is recommended to decorate separated rule/message provider classes with [`FailureMessageStrategyAttribute`] for both performance and organisational reasons.

Finally, message providers may optionally use one or more of:

* [`IHasFailureMessageUsageCriteria`]
* [`IHasFailureMessageUsageCriteria<in TValidated>`]
* [`IHasFailureMessageUsageCriteria<in TValidated, in TParent>`]

[`IGetsFailureMessage`]:xref:CSF.Validation.Messages.IGetsFailureMessage
[`IGetsFailureMessage<in TValidated>`]:xref:CSF.Validation.Messages.IGetsFailureMessage`1
[`IGetsFailureMessage<in TValidated, in TParent>`]:xref:CSF.Validation.Messages.IGetsFailureMessage`2
[the rule interfaces]:WritingValidators/WritingValidationRules/TheRuleInterfaces.md
[`IHasFailureMessageUsageCriteria`]:xref:CSF.Validation.Messages.IHasFailureMessageUsageCriteria
[`IHasFailureMessageUsageCriteria<in TValidated>`]:xref:CSF.Validation.Messages.IHasFailureMessageUsageCriteria`1
[`IHasFailureMessageUsageCriteria<in TValidated, in TParent>`]:xref:CSF.Validation.Messages.IHasFailureMessageUsageCriteria`2
[`FailureMessageStrategyAttribute`]:xref:CSF.Validation.Messages.FailureMessageStrategyAttribute

## How separated message providers work

When using message provider classes which are separated from the rule classes for which they generate messages, the validation framework makes use of [the **strategy pattern**].
For each [`ValidationRuleResult`], the framework looks for an appropriate message provider - a class which implements one of the three `IGetsFailureMessage` interfaces noted above - and uses the best match in order to get the message for that rule result.
If no matching message provider is found then the rule result is left without a message.

The selection process (to choose the appropriate class) is influnced by a number of factors.
The process is described below.

1. If the rule class implements an appropriate generic `IRuleWithMessage` interface then that class will always be used to get the message.
    * If a match is found this way then it will be used _and the remaining steps below will not be used for that rule result_.
2. If a message provider class is registered with DI that implements a compatible `IGetsFailureMessage` interface (generic or non-generic) then this is considered as a candidate for getting the message.
3. From the candidate message provider classes, any which are decorated with [`FailureMessageStrategyAttribute`] are further tested for compatibility; those which are incompatible are discarded from the list of candidates and those which are compatible according to the attribute are given a higher priority than those which do not have the attribute.
4. The remaining candidates are instantiated via dependency injection; if any of these implement a compatible `IHasFailureMessageUsageCriteria` interface then the logic in the `CanGetFailureMessage` is used to further eliminate incompatible providers. Any provider which implement this criteria interface and which are able to get a failure message are given higher priority than those which do not implement the interface.
5. The highest-priority message provider which remains amongst the candidates is used to get the feedback message.

[the **strategy pattern**]:https://en.wikipedia.org/wiki/Strategy_pattern
[`ValidationRuleResult`]:xref:CSF.Validation.ValidationRuleResult

## Using the `FailureMessageStrategyAttribute`

It is recommended to decorate classes which implement any of the `IGetsFailureMessage` interfaces, but not an `IRuleWithMessage` interface with [`FailureMessageStrategyAttribute`].
This attribute is used to provide some simple criteria indicating the rules for which this message provider is appropriate.
These criteria are indicated via the attribute's settable properties, each serving as a predicate value which must be matched by the rule result.

A message provider class will only be used if it matches all of the provided predicate values for a single attribute (a logical "AND").
The attribute may be applied more than once to any class though, and where it is used multiple times the message provider may be used if would be matched by any of the attributes (a logical "OR").

The failure message strategy attribute performs very well because _"whether or not the provider is a suitable candidate"_ is determined before the message provider has been instantiated from dependency injection.
This way, many unsuitable message providers may be ruled-out before they are created, reducing the amount of work performed in more computationally-expensive later steps.

### Examples

In this simple example, this message provider will only be considered a candidate to provide messages for the rule type `MySampleRule`.

```csharp
[FailureMessageStrategy(RuleType = typeof(MySampleRule))]
public class MySampleRuleMessageProvider : IGetsFailureMessage<int>
{
    public ValueTask<string> GetFailureMessageAsync(int value,
                                                    ValidationRuleResult result,
                                                    CancellationToken token = default)
    {
        // Implementation omitted
    }
}
```

In the following example, the message provider will be considered a candidate to provide messages for rules of type `MustExistInDatabase` when the outcome is `Errored` or for any rule when the validated type is `IEntity`.

```csharp
[FailureMessageStrategy(RuleType = typeof(MustExistInDatabase), Outcome = RuleOutcome.Errored)]
[FailureMessageStrategy(ValidatedType = typeof(IEntity)]
public class DatabaseErrorMessageProvider : IGetsFailureMessage
{
    public ValueTask<string> GetFailureMessageAsync(ValidationRuleResult result,
                                                    CancellationToken token = default)
    {
        // Implementation omitted
    }
}
```

## Using the `IHasFailureMessageUsageCriteria` interfaces

The three `IHasFailureMessageUsageCriteria` interfaces (listed above) provide more flexibility for determining whether or not a message provider class is suitable to be used for a specified validation rule result.
Each of these interfaces provides a `CanGetFailureMessage` method which returns `bool` to indicate whether or not the message provider should be used or not.

Avoid using an `IHasFailureMessageUsageCriteria` interface if the same test may be performed using a `FailureMessageStrategyAttribute`, because these interfaces do not perform quite as well as the attribute-based approach.

When using an `IHasFailureMessageUsageCriteria`, the interface must be of a compatible generic type for the message provider's generic signature.
The criteria interface may be less generic, but it must not have contradictory generic types or else it will not be used.

## Priority of providers

1. A message provider which implements an `IRuleWithMessage` interface will always be used with precedence over any other.
2. A message provider which implements an `IHasFailureMessageUsageCriteria` and is decorated with `FailureMessageStrategyAttribute` and which passes the tests carried-out by both mechanisms will be used with next-highest priority.
3. A message provider which implements an `IHasFailureMessageUsageCriteria` which returns `true` from its `CanGetFailureMessage` method is used with next-highest priority.
4. A message provider which is decorated with `FailureMessageStrategyAttribute` and which is permitted by that logic will be used with next-highest priority.
    * The more properties/predicate values which are set upon the attribute then the higher the priority of the message provider.  _Even with all properties/predicate values set_, a provider decorated only with this attribute will not take priority over a provider that implements an appropriate `IHasFailureMessageUsageCriteria` interface.
5. A message provider which has neither the attribute nor any `IHasFailureMessageUsageCriteria` interface will be used with the lowest priority.

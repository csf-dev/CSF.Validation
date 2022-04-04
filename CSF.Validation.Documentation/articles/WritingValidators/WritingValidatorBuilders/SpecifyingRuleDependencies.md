# Specifying rule dependencies

An advanced feature of this validation framework is the ability to specify rule dependencies.
In simple terms that means _"do not execute this rule if any of these other rules (the dependencies) failed"_.

## What rule dependencies should be used for

Rule dependencies are useful when dealing with validation rules that involve computationally expensive operations, for example database queries.
Imagine we have a validation rule that verifies that a product exists in a database, matching a product ID in the validated object.
If this rule fails (the product does not exist), then further validation rules relating to the product are likely to be pointless and should be skipped because they are certain to fail.

Having rules related to the product depend upon the rule which verifies the product's existence accomplishes this.

## What rule dependencies should not be used for

There is a downside to specifying rule dependencies; it introduces coupling between the rule logic in ways that can be unintuitive for developers to find & read.

If the depended-upon rule is not computationally expensive, so executing its logic multiple times is not problematic, then it is easier to use a normal constructor dependency on the rule logic class.
Consider the following example; you may wish to inject the dependency rule by an interface instead.

```csharp
public class RuleWhichDependsUponAnother : IRule<string>
{
  readonly DependencyRule  dependencyRule;

  public async Task<RuleResult> GetResultAsync(string validated, RuleContext context, CancellationToken token = default)
  {
    var dependencyResult = await dependencyRule.GetResultAsync(validated, context, token);
    if (dependencyResult.Outcome != RuleOutcome.Pass)
      return Fail();

    // Remainder of validation logic here
  }

  public RuleWhichDependsUponAnother(DependencyRule  dependencyRule)
  {
    this.dependencyRule = dependencyRule;
  }
}
```

By using an explicit constructor dependency, it is crystal clear to a developer that this rule depends upon another.
It is not so easy to tell if dependencies are declared externally within the validator configuration.
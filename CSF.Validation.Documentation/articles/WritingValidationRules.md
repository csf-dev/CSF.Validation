---
uid: WritingValidationRules
---
# Writing Validation Rules

A validator is made from validation rules; each rule represents an independent piece of logic that performs a pass/fail test.
In **CSF.Validation** a rule is a .NET class which implements one of the following two rule interfaces.

* [`IRule<in TValidated>`]
* [`IRule<in TValue, in TParent>`]

There is no reason why a rule class cannot implement both interfaces and also no reaosn why a rule class cannot implement either of these interfaces more than once, for different generic types (if it makes sense to do so).
For example, rules which operate upon integer values might implement the interfaces for a variety of integer types, such as `Byte`, `Int16`, `Int32` & `Int64`.

[`IRule<in TValidated>`]:xref:CSF.Validation.Rules.IRule`1
[`IRule<in TValue, in TParent>`]:xref:CSF.Validation.Rules.IRule`2

## Which interface to choose?

The `IRule<in TValidated>` interface is suitable for the majority of validation rules.
The value to be validated is provided to [the `GetResultAsync` method] and the rule logic returns a result based upon that value.

Some validation rules require access to the parent object which is being validated though.
Imagine we are validating the following model.

```csharp
public class LibraryBookLoan
{
    public long BookId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
```

Now, we wish to write a validation rule that the `ReturnDate` (the end of the loan) must not be earlier than the `LoanDate` (the beginning of the loan).
Whilst we could write a class which implements `IRule<LibraryBookLoan>`, such a rule would not be associated with the `ReturnDate` property, as it would need to be applied at the level of the loan object itself.

Instead, here we would implement `IRule<DateTime,LibraryBookLoan>` in our rule class.
[The `GetResultAsync` method which additionally accepts a parent value] will receive the `DateTime` to be validated as the primary value but additionally a reference to the 'parent' `LibraryBookLoan`, giving it access to the other properties of that object.
This then allows us to associate the rule with the `ReturnDate` field in our validator.

[the `GetResultAsync` method]:xref:CSF.Validation.Rules.IRule`1.GetResultAsync(`0,CSF.Validation.Rules.RuleContext,System.Threading.CancellationToken)
[The `GetResultAsync` method which additionally accepts a parent value]:xref:CSF.Validation.Rules.IRule`2.GetResultAsync(`0,`1,CSF.Validation.Rules.RuleContext,System.Threading.CancellationToken)

## Returning results from rules

In normal operation, the `GetResultAsync` method of a rule class should always do one of three things:

* Return a pass result
* Return a failure result
* Throw an exception
  * Alternatively, manually return an error result

### Passing or failing a rule

In order to return pass/failure results a helper class is available; [this class is named `CommonResults`] and it is designed to be used as so.

```csharp
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

public class MyValidationRule : IRule<object>
{
    public Task<RuleResult> GetResultAsync(object validated,
                                           RuleContext context,
                                           CancellationToken token = default)
    {
        // For a pass result:
        return PassAsync();

        // Or for a failure result:
        return FailAsync();
    }
}
```

The `CommonResults` class also has helper methods named simply `Pass` and `Fail` which return results that are not contained within `Task<T>` instances.
These are useful if you wish to make the `GetResultAsync` method `async` and use the `await` keyword.

These methods additionally present an overload in which you may return [a dictionary of result data].
This allows advanced usages scenarios in which arbitrary information, related to the validation rule may be included with the outcome.

[this class is named `CommonResults`]:xref:CSF.Validation.Rules.CommonResults
[a dictionary of result data]:ResultData.md

### Rules which encounter errors

When validation rules are executed by the validation framework, they are executed within a `try/catch` which records any unhandled exceptions raised by the rule classes.
Because of this, rule classes can often avoid boilerplate exception-catching logic of their own; _it is OK for the `GetResultAsync` method to throw an exception_ if it encounters an error.

An unhandled exception from a rule class is converted to an **Error** outcome, which behaves in the same way as a failure outcome.
The error result will have the original exception object available as a property, so that the application can take appropriate action (for example, logging).

It is also possible, using [the `CommonResults` class] to manually create and return an error result, via either of the `ErrorAsync()` or `Error()` methods.
The only practical difference between using these methods and allowing the rule `GetResultAsync` method to throw an exception is:

* A manually-returned error result does not require an exception
* A manually-returned error result may contain [a dictionary of result data]

[the `CommonResults` class]:xref:CSF.Validation.Rules.CommonResults
[a dictionary of result data]:ResultData.md

## Tips for writing good rules

This documentation contains a guide of [best practices for writing rule classes].

[best practices for writing rule classes]:RuleClassBestPractices.md

# The validation rule interfaces

A validation rule class must implement at least one of the following two interfaces.

* [`IRule<in TValidated>`]
* [`IRule<in TValue, in TParent>`]

[`IRule<in TValidated>`]:xref:CSF.Validation.Rules.IRule`1
[`IRule<in TValue, in TParent>`]:xref:CSF.Validation.Rules.IRule`2

## Rules can implement these multiple times

If it makes sense to do so, it is perfectly reasonable for a rule class to implement both interfaces.
Likewise, a rule class may implement either or both interfaces more than once for different generic types if it's appropriate.
Developers might need to use [explicit interface implementation] to avoid ambiguity of overloads.

[explicit interface implementation]:https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/explicit-interface-implementation

### Example: The `IntegerInRange` rule

The rule [`IntegerInRange`] in the standard rules package implements `IRule<TValidated>` for all of the following types:

* `Byte`
* `Int16`
* `Int32`
* `Int64`
* `Nullable<Byte>`
* `Nullable<Int16>`
* `Nullable<Int32>`
* `Nullable<Int64>`

This way the same conceptual rule may be used regardless of the data-type it validates.
This rule may be reused regardless of the type of integer value, or whether that value is nullable or not

[`IntegerInRange`]:xref:CSF.Validation.Rules.IntegerInRange

## Which interface to choose?

The `IRule<in TValidated>` interface is suitable for the majority of validation rules.
The value to be validated is provided to [the `GetResultAsync` method] and the rule logic returns a result based upon that value.

Use the `IRule<in TValue, in TParent>` interface when the validation rule also requires access to the object from which the value was derived.
This is most common when validating properties of a model object.
[The `GetResultAsync` method in `IRule<in TValue, in TParent>`] receives a 'parent' object as an additional parameter.

[the `GetResultAsync` method]:xref:CSF.Validation.Rules.IRule`1.GetResultAsync(`0,CSF.Validation.Rules.RuleContext,System.Threading.CancellationToken)
[The `GetResultAsync` method in `IRule<in TValue, in TParent>`]:xref:CSF.Validation.Rules.IRule`2.GetResultAsync(`0,`1,CSF.Validation.Rules.RuleContext,System.Threading.CancellationToken)

### Example: Validating a library book loan

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

Instead, here we would implement `IRule<DateTime,LibraryBookLoan>` in our rule class, like so.

```csharp
using static CSF.Validation.Rules.CommonResults;

public class MustNotBeBeforeLoanDate : IRule<DateTime, LibraryBookLoan>
{
    public Task<RuleResult> GetResultAsync(DateTime validated,
                                           LibraryBookLoan parent,
                                           RuleContext context,
                                           CancellationToken token = default)
    {
        if(parent is null) return PassAsync();
        return validated >= parent.LoanDate ? PassAsync() : FailAsync();
    }
}
```

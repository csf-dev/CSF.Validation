# Polymorphic Validation

Polymorphic validation is a technique for validating objects of a base type, but applying additional validation configuration (values and rules) at runtime.
This additional validation configuration depends upon the validated object's concrete type.

For example, when validating a `Person` object, a rule might be applied which asserts that the `Name` property is not null or empty.
If that `Person` object is in fact an instance of `Employee` (a subclass of person) then the object should also have a `PayrollNumber` property which should be validated.
Polymorphic validation allows the creation of a validator which validates `Person` objects, but also applies the appropriate validation for `Employee` when the validated object is actually an employee.

## How to use polymorphic validation

Configuring polymorphic validation is easy from a validator builder; use [the `WhenValueIs` method] (or [its equivalent for a value accessor]) to set up validation configuration related to the specified subtype.

## Example

Here is an example of polymorphic validation, matching the hypothetical scenario specified above.

```csharp
public class PersonValidatorBuilder : IBuildsValidator<Person>
{
  public void ConfigureValidator(IConfiguresValidator<Person> config)
  {
    config.ForMember(x => x.Name, m =>
    {
      m.AddRule<NotNullOrEmpty>();
    });

    config.WhenValueIs<Employee>(v =>
    {
      v.ForMember(x => x.PayrollNumber, m =>
      {
        m.AddRule<IntegerInRange>(r =>
        {
          r.ConfigureRule(c => c.Min = 1);
        });
      });
    }
  }
}
```

The validator created from the builder above will validate any `Person` instance, applying a rule to the `Name` property.
For `Person` instances that are also `Employee` instances then the additional rule, relating to the `PayrollNumber` property, will be applied.

## Polymorphic rules may be imported

When using [the `WhenValueIs` method], you may import rules from another validator builder, using [the same technique described elsewhere].
This allows keeping rules specific to particular subclasses contained within their own validator builders.

[the `WhenValueIs` method]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.WhenValueIs``1(System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValidator{``0}})
[its equivalent for a value accessor]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor`2.WhenValueIs``1(System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[the same technique described elsewhere]:ImportingRules.md
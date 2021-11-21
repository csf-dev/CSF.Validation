# Importing rules from another builder

Validator builders are used to specify which rules should be applied to an object and its directly-accessible members/values.
They can be used as a self-contained piece of validation configuration.
Validator builders may also consume the rules and configuration from another builder though; there are a few scenarios where this technique is useful.

* De-duplicating validation configuration for base types
* Creating alternative validation scenarios
* Validing referenced objects

Using a validator builder, importing another builder is accomplished via the `AddRules<TBuilder>()` method from either an instance of [`IConfiguresValidator<TValidated>`] or (for validating referenced objects) from an instance of [`IConfiguresValueAccessor<TValidated, TValue>`].

[`IConfiguresValidator<TValidated>`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.AddRules``1
[`IConfiguresValueAccessor<TValidated, TValue>`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor`2.AddRules``1

## Example: De-duplicating base type validation

This technique is useful when you want to validate members of a class hierarchy.
Consider this very simple example of an object model for validation:

```csharp
public abstract class Pet
{
    public string Name { get; set; }
}

public class PetCat : Pet
{
    public string FurColour { get; set; }
}

public class PetFish : Pet
{
    public int MinimumTankSizeCubicCm { get; set; }
}
```

Now, we wish to validate instances of `PetCat` and `PetFish` but we don't want to duplicate any validation rules for `Pet`.
Here is how this could be laid out using some validator builders.

```csharp
public class PetValidatorBuilder : IBuildsValidator<Pet>
{
    public void ConfigureValidator(IConfiguresValidator<Pet> config)
    {
        config.ForMember(x => x.Name, m =>
        {
            m.AddRule<NotNullOrEmpty>();
        });
    }
}

public class PetCatValidatorBuilder : IBuildsValidator<PetCat>
{
    public void ConfigureValidator(IConfiguresValidator<PetCat> config)
    {
        config.AddRules<PetValidatorBuilder>();

        config.ForMember(x => x.FurColour, m =>
        {
            m.AddRule<NotNullOrEmpty>();
        });
    }
}
```

Notice how the the `PetCatValidatorBuilder` imports the rules from the `PetValidatorBuilder`.
This has the same effect as consuming any and all configuration in the `PetValidatorBuilder` as if it were a part of `PetCatValidatorBuilder`.

## Example: Creating alternate validation scenarios

Imagine that a single object type needs to be validated in more than one conceptual manner.
Imagine you run a library that loans books and members are allowed to take a loan to a maximum initial duration of 12 weeks but they are also allowed to extend a loan but only for 4 additional weeks at a time.
A request for a loan might look like this:

```csharp
public class BookLoanRequest
{
    public long BookId { get; set; }
    public long MemberId { get; set; }
    public int LoanDurationWeeks { get; set; }
}
```

Now we want to validate this model, but under two different scenarios: For an initial loan and for a loan extension.
We do not want to duplicate common rules across those two validator builders though.

```csharp
public class BookLoanRequestCommonBuilder : IBuildsValidator<BookLoanRequest>
{
    public void ConfigureValidator(IConfiguresValidator<BookLoanRequest> config)
    {
        // This example uses two fictitious rules,
        // their implementation is unimportant.
        config.ForMember(x => x.BookId, m =>
        {
            m.AddRule<MustExistInDatabase>();
        });

        config.ForMember(x => x.MemberId, m =>
        {
            m.AddRule<MustBeActiveMember>();
        });
    }
}

public class InitialBookLoanRequestBuilder : IBuildsValidator<BookLoanRequest>
{
    public void ConfigureValidator(IConfiguresValidator<BookLoanRequest> config)
    {
        config.AddRules<BookLoanRequestCommonBuilder>();

        config.ForMember(x => x.LoanDurationWeeks, m =>
        {
            m.AddRule<IntegerInRange>(c =>
            {
                c.ConfigureRule(r =>
                {
                    r.Min = 1;
                    r.Max = 12;
                })
            });
        });
    }
}

public class ExtensionBookLoanRequestBuilder : IBuildsValidator<BookLoanRequest>
{
    public void ConfigureValidator(IConfiguresValidator<BookLoanRequest> config)
    {
        config.AddRules<BookLoanRequestCommonBuilder>();

        config.ForMember(x => x.LoanDurationWeeks, m =>
        {
            m.AddRule<IntegerInRange>(c =>
            {
                c.ConfigureRule(r =>
                {
                    r.Min = 1;
                    r.Max = 4;
                })
            });
        });
    }
}
```

This produces a validator for each of the Initial & Extension loans, both of which consume the rules configured within the common validator builder.

## Example: Validating referenced objects

When validating [object graphs], each nontrivial object-type included in that graph should have its own validator builder.
Then, when validating across references, import the rules from the appropriate builder within [a `ForMember` or `ForValue` or `ForMemberItems` or `ForValues` declaration].

Consider these models

```csharp
public class Vehicle
{
    public DateTime? ManufacturedDate { get; set; }
    public ICollection<Wheel> Wheels { get; set; }
}

public class Wheel
{
    public decimal? DiameterCm { get; set; }
}
```

The validator builders for these might look something like the following:

```csharp
public class VehicleBuilder : IBuildsValidator<Vehicle>
{
    public void ConfigureValidator(IConfiguresValidator<Vehicle> config)
    {
        config.ForMember(x => x.ManufacturedDate, m =>
        {
            m.AddRule<NotNull>();
        });

        config.ForMemberItems(x => x.Wheels, m =>
        {
            m.AddRules<WheelBuilder>();
        });
    }
}

public class WheelBuilder : IBuildsValidator<Wheel>
{
    public void ConfigureValidator(IConfiguresValidator<Wheel> config)
    {
        config.ForMember(x => x.DiameterCm, m =>
        {
            m.AddRule<NotNull>();
        });
    }
}
```

By creating a validator from the `VehicleBuilder`, every one of its wheels will be validated using the rules within `WheelBuilder`.
Such parent/child consumption of validator builders can continue through unlimited levels of relationships.
Incidentally this also demonstrates [the validation of collection items], via the usage of `ForMemberItems`.

The important thing to avoid is **circular consumption**, which will cause an error.
In the example above, the `WheelBuilder` must not import the rules from `VehicleBuilder`.

[object graphs]:https://en.wikipedia.org/wiki/Object_graph
[a `ForMember` or `ForValue` or `ForMemberItems` or `ForValues` declaration]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1
[the validation of collection items]:ValidatingCollectionItems.md

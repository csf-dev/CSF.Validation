# Writing Validator Builders

Validator builders are the simplest & clearest way to define a validator when you wish to describe the validator using .NET code.

A validator builder is a class which implements the interface [`IBuildsValidator<TValidated>`].
Developers then use the `ConfigureValidator` method and the [`IConfiguresValidator<TValidated>`] helper to specify how the validator will function.
Validator builders are type-safe and benefit from IDE autocomplete (aka Intellisense) functionality.

[`IBuildsValidator<TValidated>`]:xref:CSF.Validation.IBuildsValidator`1
[`IConfiguresValidator<TValidated>`]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1

## The basics: Adding rules to a builder

Rules may be added to either or both of the object being validated or values derived from that object.
Here is an example of three ways to add rules to a validator, by using a builder.

```csharp
public class PetValidatorBuilder : IBuildsValidator<Pet>
{
    public void ConfigureValidator(IConfiguresValidator<Pet> config)
    {
        config.AddRule<NotNull>();

        config.ForMember(x => x.Name, member =>
        {
            member.AddRule<NotNullOrEmpty>();
        });

        config.ForValue(x => GetAge(x.BirthDate), val =>
        {
            val.AddRule<IntegerInRange>(conf =>
            {
                conf.ConfigureRule(rule =>
                {
                    rule.Min = 0;
                    rule.Max = 200;
                })
            });
        });
    }

    static int GetAge(DateTime birthDate) { /* Implementation omitted */ }
}
```

In the first example, [the `AddRule` method] is used directly from the `IConfiguresValidator<Pet>` instance.
This would add a rule that implements `IRule<Pet>`, to apply validation logic associated with the complete object instance.
In this case it is the [`NotNull`] rule.

In the second example, [the `ForMember` method] is used to specify a member (in this case, a property) of the `Pet` class.
Let's assume that `Pet.Name` is a property of type `string`.
Rules added within the member lambda target that member's value.  In this case the [`NotNullOrEmpty`] rule will validate this string value.

In the third example, [the `ForValue` method] is used.
`ForValue` allows validation of a completely arbitrary value, not limited to only values returned-by/stored-in a member like a property.
Developers are encouraged to prefer `ForMember` over `ForValue` where possible, as the `ForValue` method comes with some limitations which do not apply to `ForMember`.

Finally, the third example also demonstrates a rule that has configuration.
If you look at the `IntegerInRange` rule, you will see that the rule class has two _settable properties_ describing the minimum and maximum of the permitted range.
The configuration syntax permits setting that state from the builder.

[the `AddRule` method]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.AddRule``1(System.Action{CSF.Validation.ValidatorBuilding.IConfiguresRule{``0}})
[`NotNull`]:xref:CSF.Validation.Rules.NotNull
[the `ForMember` method]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForMember``1(System.Linq.Expressions.Expression{System.Func{`0,``0}},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`NotNullOrEmpty`]:xref:CSF.Validation.Rules.NotNullOrEmpty
[the `ForValue` method]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.ForValue``1(System.Func{`0,``0},System.Action{CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor{`0,``0}})
[`IntegerInRange`]:xref:CSF.Validation.Rules.IntegerInRange

## Further techniques

With the techniques above, we have only seen a part of what a validator builder can accomplish.
The links below will describe the remaining techniques.

* [Importing rules from another builder]
* [Validating items in a collection]
* [Identifying rules and objects]
* [Adding rules which receive a parent object]
* [Dealing with exceptions accessing a member or value]
* [Specifying dependencies between rules]
* [Polymorphic validation]

[Importing rules from another builder]:ImportingRules.md
[Validating items in a collection]:ValidatingCollectionItems.md
[Identifying rules and objects]:RuleAndObjectIdentifiers.md
[Adding rules which receive a parent object]:RulesWhichIncludeAParentObject.md
[Dealing with exceptions accessing a member or value]:HandlingAccessorExceptions.md
[Specifying dependencies between rules]:SpecifyingRuleDependencies.md
[Polymorphic validation]:PolymorphicValidation.md

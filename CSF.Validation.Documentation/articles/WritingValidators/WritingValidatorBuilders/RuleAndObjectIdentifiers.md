# Identifying rules and objects

When validating complex object graphs, especially those which include [collections of validated values], it can be helpful to identify object instances within the validator. This allows easy matching of results back to the correct validated object instance.

Additionally, sometimes it is useful to identify an individual rule with a name. This can be useful if the same rule-type must be applied more than once to the same property.

[collections of validated values]: ValidatingCollectionItems.md

## Identifying object instances

A common problem when validating a complex object graph (such as one which contains collections of validated objects) is matching the validation results back to the corresponding objects.
To simplify this, validated objects may be marked with an identity using [the `UseObjectIdentity` method] of a validator-builder.
Validation results for these objects include the identity with each result, so that the result may be matched with its corresponding validated object.

Object identities in CSF.Validation have no meaning beyond the above.
You may use an identity property/value which is already available upon the object or you may add one.
If you are adding an identity property, consider a `System.Guid`, as it is quick and easy to create unique values.

Identities are ephemeral and do not need to persisted.
All that is important is that for each invocation of the validation logic, the identity of any given validated object remains stable.
If the validation logic is executed twice, then subsequent invocations do not need to have the same identities.

[the `UseObjectIdentity` method]: xref:CSF.Validation.ValidatorBuilding.IConfiguresValidator`1.UseObjectIdentity(System.Func{`0,System.Object})

## Identifying/naming rules

It is quite rare that rules need to be named. Rules are usually identified by their `System.Type` along with the type and perhaps parent type and member name that they validate.

Sometimes the above is not enough, though. For example if the same object or member must be validated with the same rule type more than once, for example with different configurations.

In this case, these rules must be given unique names in order to distinguish them. This is performed by setting [the `Name` property] of the rule-configuration builder. Here is an example:

```csharp
builder.AddRule<MyRuleType>(conf => {
    conf.Name = "Rule name here";
});
```

[the `Name` property]: xref:CSF.Validation.ValidatorBuilding.IConfiguresRule`1.Name
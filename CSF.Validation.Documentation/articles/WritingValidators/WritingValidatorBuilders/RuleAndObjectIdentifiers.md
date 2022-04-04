# Identifying rules and objects

When validating complex object graphs, especially those which include [collections of validated values], it can be helpful to identify object instances within the validator. This allows easy matching of results back to the correct validated object instance.

Additionally, sometimes it is useful to identify an individual rule with a name. This can be useful if the same rule-type must be applied more than once to the same property.

## Identifying object instances

Object instances within the graph of validated objects may have an identifier. The identifier is not used in any special capacity but it is associated with the corresponding result items. This can make it easier to match up results with their validated objects.

Object identifiers may be added to a builder using [the `UseObjectIdentity` method].

Identities do not need to be persistent across separate invocations of the validation logic, as long as they are round-tripped forward and backward between the validator and any logic which consumes the validation result. As such, `Guid` objects make good identifiers.

[the `UseObjectIdentity` method]:xref:TODO

## Identifying/naming rules

It is quite rare that rules need to be named. Rules are usually identified by their `System.Type` along with the type and perhaps parent type and member name that they validate.

Sometimes the above is not enough, though. For example if the same object or member must be validated with the same rule type more than once, for example with different configurations.

In this case, these rules must be given unique names in order to distinguish them. This is performed by setting [the `Name` property] of the rule-configuration builder. Here is an example:

```csharp
builder.AddRule<MyRuleType>(conf => {
    conf.Name = "Rule name here";
});
```

[the `Name` property]:TODO
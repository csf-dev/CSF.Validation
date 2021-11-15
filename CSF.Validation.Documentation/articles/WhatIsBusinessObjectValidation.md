# Business Object Validation

For the sake of this framework, **business objects** are defined as models (typically .NET classes) which contain primarily data.
A business object is not limited to a single instance of a single class, they may actually be a graph of instances of many classes, interconnected by relationships.
For those familiar with <acronym title="Domain Driven Design">DDD</acronym>, models _may be [either entities or value objects]_.

[either entities or value objects]:https://en.wikipedia.org/wiki/Domain-driven_design#Kinds_of_models

## A sample business object to validate

Let's imagine that we are building an international e-commerce web application.
One of its features is to purchase all of the items in the shopping cart.
That business request might be modelled using an object similar to the following.

```csharp
public class PurchaseShoppingCartRequest
{
    public PostalAddress ShippingAddress { get; set; }
    public ICollection<LineItem> LineItems { get; set; }
}

public class PostalAddress
{
    public string StreetLine1 { get; set; }
    public string StreetLine2 { get; set; }
    public string CityOrTownName { get; set; }
    public string StateOrCountyName { get; set; }
    public string ZipOrPostalCode { get; set; }
    public long? CountryId { get; set; }
}

public class LineItem
{
    public long? ProductId { get; set; }
    public int Quantity { get; set; }
}
```

Now let's imagine some of the possibilities for validation:

* When validating the shipping address, the `CountryId` must not be null and must correspond to a country that is known-about by the application.
* When validating a ZIP or postal code, depending upon the selected country, there will country-specific rules for the format of this value, which will likely require a dependency-injected service, such as [strategy pattern].
* Perhaps some products are not available in all countries and should be validated that they can be shipped to the selected country.

[strategy pattern]:https://en.wikipedia.org/wiki/Strategy_pattern

These rules are all non-trivial and at least one of them would typically be implemented by consuming services via dependency injection.
This is the sort of validation that **CSF.Validation** is well-suited to perform.

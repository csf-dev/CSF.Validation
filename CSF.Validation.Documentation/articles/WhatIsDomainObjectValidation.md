# Domain Object Validation

For the sake of this framework, **domain objects** are defined as models (typically .NET classes) which contain state.
Domain object models relate to business-logic concepts and are free from technical/architectural concerns.
The term "domain" borrows somewhat from _[Domain Driven Design]_ but your application/team does not need to be following DDD; if you are then it matters not if these are [entities or value objects].

The important feature of domain objects is that they are not limited to a single instance of a single class.
A domain object to be validated may be a complete graph of related object instances, interconnected by relationships.

[Domain Driven Design]:https://en.wikipedia.org/wiki/Domain-driven_design
[entities or value objects]:https://en.wikipedia.org/wiki/Domain-driven_design#Kinds_of_models

## A sample domain object to validate

Let's imagine that we are building an international e-commerce web application.
One of its features is to purchase all of the items in the shopping cart.
That request might be modelled using an object similar to the following.

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
* When validating a ZIP or postal code there will be country-specific rules for the format of this value.
* Perhaps some products are not available in all countries.  The object should be validated to verify that the selected product may be shipped to the selected country.

These rules are all non-trivial and would typically require the consumption of dependency-injected services.
This is the sort of validation that **CSF.Validation** is well-suited to perform.

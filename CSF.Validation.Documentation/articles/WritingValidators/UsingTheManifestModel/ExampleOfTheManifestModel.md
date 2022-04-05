# An Example of the Manifest Model

This is a worked example of how the Manifest Model might be used to define a validator.
The example model is specified using JSON, assuming a simple JSON mapping to/from [the manifest model classes].

[the manifest model classes]:index.md#The_Manifest_Model_classes

## The object model to be validated

This example assumes that you wish to validate instances of the `ShoppingBasket` class below.
By extension, we will be including instances of the other objects in our validation also.

```csharp
public class ShoppingBasket
{
  public ICollection<BasketItem> Items { get; set; }
}

public class BasketItem
{
  public StoreProduct Product { get; set; }
  public int Quantity { get; set; }
  public Guid ItemId { get; set; }
}

public class StoreProduct
{
  public long Id { get; set; }
}
```

## The validation rules to be applied

In this example (to keep it brief), we want to apply only three validation validation rules to each basket.
we will use the `BasketItem.ItemId` property to identify the items.

* Every item in the basket must have a positive `BasketItem.Quantity`
* Every item in the basket must have a non-null `BasketItem.Product`
* Every `BasketItem.Product` for every item in the basket must have a positive `StoreProduct.Id`
  * But we do not process this rule if the `BasketItem.Quantity` was invalid
  * _In a real-world validation, this might not be a simple check for a positive number, it could be a rule which performs a database query to verify that this is a real/supported product in our store.  This is why we have specified a dependency, to avoid executing this 'expensive' rule if the quantity is invalid._

## Sample Manifest Model JSON

This JSON uses two validation rules from the [CSF.Validation.StandardRules] package: **NotNull** and **IntegerInRange**.
This JSON, converted to a manifest model, would represent the validation described above.

[CSF.Validation.StandardRules]:https://www.nuget.org/packages/CSF.Validation.StandardRules/

```json
{
  "Children": {
    "Items": {
      "IdentityMemberName": "ItemId",
      "EnumerateItems": true,
      "Children": {
        "Quantity": {
          "Rules": [
            {
              "RuleTypeName": "CSF.Validation.Rules.IntegerInRange, CSF.Validation.StandardRules",
              "RulePropertyValues": {
                "Min": 1
              }
            }
          ]
        },
        "Product": {
          "Rules": [
            {
              "RuleTypeName": "CSF.Validation.Rules.NotNull, CSF.Validation.StandardRules",
            }
          ],
          "Children": {
            "Id": {
              "Rules": [
                {
                  "RuleTypeName": "CSF.Validation.Rules.IntegerInRange, CSF.Validation.StandardRules",
                  "RulePropertyValues": {
                    "Min": 1
                  },
                  "Dependencies": [
                    {
                      "AncestorLevels": 2,
                      "MemberName": "Quantity",
                      "RuleTypeName": "CSF.Validation.Rules.IntegerInRange, CSF.Validation.StandardRules"
                    }
                  ]
                }
              ]
            }
          }
        }
      }
    }
  }
}
```

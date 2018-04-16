# CSF Validation
CSF.Validation is a business rule validation framework.
It is intended for use within the business logic layer of your application.

## How this differs from other frameworks
Many validation frameworks (such as [ASP.NET validation controls] or [.NET validation attributes]) are intended for validation which lies close to the user interface.
This is evident in their design, for example the validation failure messages are defined along with the validation rule itself.
The human-readable failure message is a presentation/UI concern and not directly related to the the pass/fail of the rule logic.

[ASP.NET validation controls]: https://msdn.microsoft.com/en-us/library/debza5t0.aspx
[.NET validation attributes]: https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.validationattribute(v=vs.110).aspx

CSF.Validation separates concerns appropriately so that each aspect of the validation process may be controlled separately.
If you wanted a simple validator which you will (architecturally speaking) keep close to your UI, then this framework is probably not for you.

If you want a shared validation framework which will live in your business logic layer, validating requests which come from a wide variety of other layers then this might be what you are looking for.

## Usage
The process of validating an object is:

1. Create a validation manifest, which lists the rules which are desired, along with their configurations
2. Use a validator factory to create a validator from that manifest
3. Use the validator to validate your object

At the first stage, there is a static type named `ManifestBuilder` which can help you with the creation of a validation manifest.
It provides a fluent interface to add and configure rules in the manifest.

## Example
Here is an example of usage, including the creation of your own custom validation rule:

```csharp
// A simple object model which we are going to validate

public interface IDessert
{
  string Name { get; }
  decimal UnitPrice { get; }
}

public interface IPurchaseDessertRequest
{
  IDessert DessertToPurchase { get; }
  decimal MoneyInWallet { get; }
  int DesiredQuantity { get; 
}

// A custom validator type

// using CSF.Validation.Rules;
public class CanAffordDessertRule : Rule<IPurchaseDessertRequest>
{
  protected override RuleOutcome GetOutcome(IPurchaseDessertRequest request)
  {
    if(request == null)
      return Success;
    
    var totalCost = request.DessertToPurchase.UnitPrice * request.DesiredQuantity;
    if(totalCost > request.MoneyInWallet)
      return Failure;
    
    return Success;
  }
}

// Code to construct the validator for this

// using CSF.Validation;
// using CSF.Validation.Manifest;
// using CSF.Validation.Manifest.Fluent;
// using CSF.Validation.Manifest.StockRules;
public IValidator CreatePurchaseDessertRequestValidator()
{
  var builder = ManifestBuilder.Create<IPurchaseDessertRequest>();
  
  builder.AddRule<NotNullRule>();
  
  builder.AddMemberRule<NotNullValueRule>(x => x.DessertToPurchase, c => {
    c.AddDependency<NotNullRule,IPurchaseDessertRequest>();
  });
  
  builder.AddMemberRule<NumericRangeValueRule>(x => x.DesiredQuantity, c => {
    c.Configure(r => {
      r.Min = 1;
    });
    c.AddDependency<NotNullRule,IPurchaseDessertRequest>();
  });
  
  builder.AddMemberRule<CanAffordDessertRule>(c => {
    c.AddDependency<NotNullValueRule,IPurchaseDessertRequest>(x => x.DessertToPurchase);
    c.AddDependency<NumericRangeValueRule,IPurchaseDessertRequest>(x => x.DesiredQuantity);
  });
  
  var factory = new ValidatorFactory();
  return factory.GetValidator(builder.GetManifest());
}

// We get our validation result

var validator = CreatePurchaseDessertRequestValidator();
var request = GetPurchaseDessertRequest();
var result = validator.Validate(request);
```

This example creates a validator with four validation rules:

* A not-null rule ensures that the request is not null
* A not-null member rule ensures that the **DessertToPurchase** property is not null
* A numeric range rule ensures that the **DesiredQuantity** property is at least 1
* A custom rule ensures that the buyer can afford their desserts

Note the use of `.Configure()` in the numeric range rule.
When building a validation manifest you may configure parameters in each individual rule via property setters on that rule.

Additionally, note that most of the rules have dependencies.
The way dependencies work is that a rule which carries a dependency will not be executed (it will be marked as *skipped due to dependency failure* in the results) if any of the rules it depends upon have failed.
In the example above, there is no point executing any of the property rules if the request itself is `null`.
There is also no way to get a meaningful result from the `CanAffordDessertRule` if either the dessert is `null` or if the desired quantity is less than one.

Dependencies will automatically form chains; if a dependency rule is skipped due to a dependency failure of its own then any rules depending upon it will also be skipped.

A validation result contains a list of every validation rule in the manifest and the outcome of every single rule.
Rules which have an outcome of `Success` or `IntentionallySkipped` (used in advanced scenarios) should be considered OK to allow validation to pass.
Any other outcome indicates a validation failure.

## Build status
CI builds are configured via both Travis (for build & test on Linux/Mono) and AppVeyor (Windows/.NET).
Below are links to the most recent build statuses for these two CI platforms.

Platform | Status
-------- | ------
Linux/Mono (Travis) | [![Travis Status](https://travis-ci.org/csf-dev/CSF.Validation.svg?branch=master)](https://travis-ci.org/csf-dev/CSF.Validation)
Windows/.NET (AppVeyor) | [![AppVeyor status](https://ci.appveyor.com/api/projects/status/ra0bx9w820gnudx6?svg=true)](https://ci.appveyor.com/project/craigfowler/csf-validation)

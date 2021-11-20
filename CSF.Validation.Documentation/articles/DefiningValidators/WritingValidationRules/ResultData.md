# Result data

In simple circumstances, a validation rule passes or it fails and there is nothing more to it.
In advanced scenarios it is helpful to include supplemental information about that pass or failure, which may be consumed by any logic that receives the validation result.

In these cases, [the `CommonResults` class] offers overloads of all of its methods which accepts result data.
Result data is an `IDictionary<string,object>` of arbitrary key/value pairs.
This dictionary of result data is [available from the validation result] for whatever purpose it is required later.

[the `CommonResults` class]:xref:CSF.Validation.Rules.CommonResults
[available from the validation result]:xref:CSF.Validation.Rules.RuleResult.Data

## An example: buying alcohol

Let's imagine that we are operating an online mail-order service which operates internationally.
Let's also imagine that amongst our products, we sell alcoholic beverages.
Alcohol is an age-restricted item almost everywhere but the minimum age to purchase alcohol varies from territory to territory.

We wish to write a validation rule that ensures a customer who is underage cannot purchase alcohol.
We also want to provide a useful validation feedback message stating the required age, relevant to the customer's territory.
Here's an example rule which could accomplish this.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

public class MustSatisfyAgeRestrictionForAlcoholPurchase : IRule<OrderLineItem>
{
    readonly IGetsWhetherOrderLineItemIsAlcoholicBeverage isAlcoholicProvider;
    readonly IGetsTerritoryForOrderLineItem territoryProvider;
    readonly IGetsMinimumAgeForAlcoholPurchase minimumAgeProvider;
    readonly IGetsCustomerAgeForOrderLineItem ageProvider;

    public async Task<RuleResult> GetResultAsync(OrderLineItem validated,
                                                 RuleContext context,
                                                 CancellationToken token = default)
    {
        if(validated is null) return Pass();

        var isAlcoholic = await isAlcoholicProvider.IsAnAlcoholicBeverage(validated);
        if(!isAlcoholic) return Pass();
        
        var territory = await territoryProvider.GetTerritory(validated);
        var minimumAge = await minimumAgeProvider.GetMinimumAgeForAlcoholPurchase(territory);
        var customerAge = await ageProvider.GetCustomerAge(validated);
        
        if(customerAge >= minimumAge) return Pass();

        return Fail(new
        {
            { "territory_name", territory.Name },
            { "minimum_age", minimumAge },
        });
    }

    // The constructor is omitted from this example, it would just inject & initialise the
    // services depended-upon above.
}
```

Hopefully the logic of this example should be self-explanatory enough.
Notice how at the end, if the customer is not old enough to make the purchase, we return a failure result.
In this failure result we include two pieces of data:

* The name of the territory
* The minimum age for that territory

Including these two pieces of information would make it easier to produce a more helpful validation feedback message (for example), because we could include this data in that message.

## Result data does not just have to be for messages

A common use for result data is for inclusion in feedback messages.
It may be used for any kind of supplemental data though, where that data is to be consumed by logic that receives the validation result.

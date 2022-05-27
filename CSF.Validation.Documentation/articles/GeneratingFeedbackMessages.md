# Validation feedback messages

Validation is usually used in combination with user-provided/user-entered data; if validation fails then the user must re-submit that data with corrections.
In these cases it is important to help the user to understand why their original submission was invalid and give them advice which helps them to fix the problems.

## Where to write the feedback messages?

In most applications human-readable validation feedback messages are written close to the user interface logic, where the data is entered and submitted.
This is a very sound technique and _usually it is the correct thing to do_.

In some scenarios, where the validation is core to the logic of the application, the generation of feedback messages does not 'belong' with the user interface.
Imagine a server-based application which provides many channels by which data may be submitted for validation.
This could include any or all of a web user interface, iOS/Android apps, a REST-based JSON API and so on.
In this case we would not want to duplicate the message-logic across all of these client layers, we would want it to be written in one re-usable location.
This is what the validation-message-generation functionality of CSF.Validation accomplishes.

## Enabling message generation

By default, _generation of feedback messages is **switched off**_ in CSF.Validation.
If you wish to use it then it must be explicitly enabled by setting the validation option [`EnableMessageGeneration`] to `true`.

## Writing feedback messages for rules

In CSF.Validation, human-readable failure messages generated from classes and each message is associated with a single failed rule.
The [validation rule result] for the failure is inspected by the framework and if a suitable "message provider" class may be found then it is used to get the message associated with that result.

### Message providers combined with rules

The simplest way to write feedback messages is to use an alternative to [the generic IRule interfaces].
Instead of `IRule<TValidated>` or `IRule<TValidated,TParent>`, use [`IRuleWithMessage<TValidated>`] or [`IRuleWithMessage<TValidated,TParent>`].
These "rule with message" interfaces declare an additional method: `GetFailureMessageAsync`.
This method is used to get a failure message which is used when that particular rule fails.

This technique is simple and keeps the failure message close (in the same class) to the rule with which it is associated.
It is recommended as the first technique to try out if you wish to generate validation feedback messages, if you do not have more complex requirements.
When the rule fails, the same instance of the same rule class will always be used to get the message.

The limitation of this technique comes if non-trivial logic is required to get the message.
In that case you might consider separate message providers (below).

### Separate rule & message logic

The alternative technique to the above is to separate the rule logic and the message provider.
Here, you would write classes which implement one or more of [the message provider interfaces] and register them with dependency injection.
You may also use attributes and/or criteria methods to control when a message provider class is used.

[`EnableMessageGeneration`]:xref:CSF.Validation.ValidationOptions.EnableMessageGeneration
[validation rule result]:xref:CSF.Validation.ValidationRuleResult
[the generic IRule interfaces]:WritingValidators/WritingValidationRules/TheRuleInterfaces.md
[`IRuleWithMessage<TValidated>`]:xref:CSF.Validation.Rules.IRuleWithMessage`1
[`IRuleWithMessage<TValidated,TParent>`]:xref:CSF.Validation.Rules.IRuleWithMessage`2
[the message provider interfaces]:WritingMessageProviders.md

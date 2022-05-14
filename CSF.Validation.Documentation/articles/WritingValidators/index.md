# Writing validators

Fundamentally, a validator is an object which is built from a collection of validation rules, where each rule is a .NET class.
When it is used to validate an object or object graph, it runs each of those rules getting a result-per-rule.
If every one of those rules returns a pass result then the overall validation is a pass.  If any rule did not pass then validation is a failure.

To create a validator, you must first define which rules are to be run upon which values.
You may (and are encouraged) to write your own rule classes as well, to suit your application's logic.

## Creating a validator from some rules

As noted above, to create a validator you must define which rules should apply to which values and how those rules should be configured (in the case of rules which have parameters).
This information is called _the validation manifest_ and the framework provides three mechanisms by which to declare it, although typical applications will need use **only one** of them.

1. [A **validator builder**] is the best choice for most applications; the validator is specified using type-safe .NET code.
Developers gain the benefits of features such as compile-time syntax checking and IDE autocomplete (aka Intellisense).
2. If the composition of a validator is to be _defined using data_ then consider using the [**manifest model**].
The manifest model objects are suited for serialization to/from formats such as JSON, XML or relational database data.
3. Developers wishing to use advanced techniques may [create or manipulate a validation manifest directly].
This could be useful for developers who wish to extend the validation framework - building their own _"validation by convention"_ functionality for instance.

Developers are _advised to use the simplest mechanism_ which meets their requirements.

[A **validator builder**]:WritingValidatorBuilders/index.md
[**manifest model**]:UsingTheManifestModel/index.md
[create or manipulate a validation manifest directly]:TheValidationManifest/index.md
[POCOs]:https://en.wikipedia.org/wiki/Plain_old_CLR_object

## Writing rule classes

The rule classes are where each piece of stand-alone validation logic is held.
There are some pre-written rules for common validation scenarios available in [the standard rules package].
The [source code for the standard rules] also offers some worked examples of what a rule class could look like.

Almost every application which uses this framework will need some rule logic of its own though.
Developers are encouraged to write their own rule classes containing whatever logic they need.
You are advised to read [the documentation for writing rule classes] as well as [best practices & guidance] applicable to rules.

[the standard rules package]:https://www.nuget.org/packages/CSF.Validation.StandardRules/
[source code for the standard rules]:https://github.com/csf-dev/CSF.Validation/tree/master/CSF.Validation.StandardRules/Rules
[the documentation for writing rule classes]:WritingValidationRules/index.md
[best practices & guidance]:../BestPractice/index.md#writing-validation-rule-classes
[`IRule<in TValidated>`]:xref:CSF.Validation.Rules.IRule`1
[`IRule<in TValue, in TParent>`]:xref:CSF.Validation.Rules.IRule`2


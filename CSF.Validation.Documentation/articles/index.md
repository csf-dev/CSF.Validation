# Introduction

Using the **CSF.Validation** framework involves following these three conceptual steps.
Each is a link to further detail.

1. [Configure the framework]
2. [Define how objects should be validated]
3. [Perform validation]

[Configure the framework]:ConfigureTheFramework.md
[Define how objects should be validated]:WritingValidators/index.md
[Perform validation]:ConsumingValidators.md

## Recommended usage scenarios

CSF.Validation is intended for scenarios where:

* The validation is non-trivial
* The validation is conceptually a part of your business logic

CSF.Validation is best for validating [domain objects], rather than user-interface-layer DTOs.
[Validation rule classes] are fully compatible with dependency injection.
The architecture is also not dependent-upon our specific to any particular user-interface technology.
Indeed, it may be used when there is no user interface at all.






Imagine an application where a user must submit a complex form with many dependencies between field values.
CSF.Validation excels when it is used to validate rich object models, perhaps including collections of descendent models.
It is also useful where the validation rules to be run involve complex logic or injected dependencies.

This framework provides an organised architecture upon which to write each independent validation rule [following SOLID principles].
It provides both declarative and data-based mechanisms for specifying which of those rules should be executed & a simple API for performing validation and getting the result.

[domain object validation]:WhatIsDomainObjectValidation.md
[following SOLID principles]:https://en.wikipedia.org/wiki/SOLID

## Where this framework might not be useful

CSF.Validation is less useful for trivial validation, such as where a few fields which must not be null or empty and there is nothing more complex to do.
It is also not well-suited for validation that lies architecturally close to a specific user interface technology.
# Introduction

To begin using the **CSF.Validation** framework you must follow these three conceptual steps:

1. [Configure the framework]
2. [Define how objects should be validated]
3. [Perform validation]

[Configure the framework]:ConfigureTheFramework.md
[Define how objects should be validated]:DefiningValidators/index.md
[Perform validation]:ConsumingValidators.md

## Where this framework is useful

CSF.Validation is designed for non-trivial **[domain object validation]**.  It is particularly useful where that validation occurs close-to or as part-of the core application logic.

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

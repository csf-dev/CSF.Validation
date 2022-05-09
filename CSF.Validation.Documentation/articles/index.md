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

CSF.Validation is best-used for validating [domain objects], rather than DTOs which are specific to a particular user interface technology.
By logically arranging validation in the central domain/business logic, disconnected from user interface concerns, it becomes reusable across any user interface or API exposed by the application.

### Where this framework might not be useful

CSF.Validation is less useful for trivial validation, such as validating objects with only a few properties which must not be null or empty.
If there is no complex validation to be performed then this framework is likely to be overkill.

CSF.Validation is also less well-suited for validation which must be architecturally connected with a specific user interface technology.

## Features and benefits

### Compatible with dependency injection

[Validation rule classes] and (if you wish to use them) [validation message providers] are fully compatible with .NET dependency injection.
Rule and failure message logic may use constructor-injected dependencies like the rest of your business logic.

### Flexible

The CSF.Validation architecture is flexible; you may define validators declaratively in code, as data such as JSON or XML.
You may also combine these techniques with advanced logic such as reflection in order to build convention-based validators.

### Low dependency footprint

The architecture is not dependent-upon or specific to any particular user-interface technology.
Indeed, it may be used when there is no user interface at all.
Validation results could be serialised and returned through a JSON web API (for instance).

### Follows SOLID principles

CSF.Validation helps you to [follow the SOLID principles] in your app.
Each individual validation rule is written in a single-responsibility class.

The definition of a validator (which rules should be applied to which values, and how they are to be configured) is *declarative*.

[domain objects]:WhatIsDomainObjectValidation.md
[Validation rule classes]: WritingValidators/WritingValidationRules/index.md
[follow the SOLID principles]:https://en.wikipedia.org/wiki/SOLID
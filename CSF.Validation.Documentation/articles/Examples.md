# Examples

If you would like to see a sample validator then the best place to look is [the source code of the `ValidationManifestValidatorBuilder`].
This class and further validators linked from it are real validators, specifically it describes _a validator which may be used to validate the configuration of a validator!_

This validator is built using [the validator builder technique] and also uses a number of pieces of functionality within the framework.

* Member validation
* Collection item validation
* Importing validation rules from another builder
* Rule dependencies
* Recursive validation

Further examples may be found within the `IntegrationTests` directory of the `CSF.Validation.Tests` project.
In particular the class named `PersonValidatorBuilder` demonstrates:

* Assigning a rule name
* Rule configuration
* Polymorphic validation
* Object identity

[the source code of the `ValidationManifestValidatorBuilder`]:xref:CSF.Validation.ValidatorValidation.ValidationManifestValidatorBuilder
[the validator builder technique]: WritingValidators/WritingValidatorBuilders/index.md

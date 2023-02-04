# Testing validation rules & validators

Validation rules and message classes are inherently easy to unit test.
There are some tricks that might help make it even easier, though.

## Test framework integration

The validation framework itself is tested using **NUnit**.
If you use this framework then you might want to take a look at the following classes from the test project `CSF.Validation.Tests`:

* `CSF.Validation.ValidationResultConstraint`
* `CSF.Validation.Rules.RuleResultConstraint`
* `CSF.Validation.Is`

These classes provide integration with NUnit to write assertions for validation results and the results from individual validation rules.

If you additionally use [AutoFixture] & [Moq] then the class `CSF.Validation.IntegrationTestingAttribute` might also be of interest to you.

[AutoFixture]: https://github.com/AutoFixture/AutoFixture
[Moq]: https://github.com/moq/moq4

## Testing a validator itself

The implementation of the interface [`IValidatesValidationManifest`] may be used to perform a sanity-check upon a validator.
This process does not guarantee that your validator is error-free but it can catch and point out a number of common errors.

If you create/scaffold a service provider within your _integration tests_, then you may resolve this service and use it to "validate your validators" and get a test result.
See the test `ValidationManifestValidatorTests.ValidateAsyncShouldReturnPassResultForItsOwnManifest` for an example of using this service to validate a validator.

[`IValidatesValidationManifest`]:xref:CSF.Validation.ValidatorValidation.IValidatesValidationManifest

# Creating validators

Conceptually, a validator is a collection of validation rules to be executed, along with their interdependencies. In CSF.Validation this is called _a validation manifest_.

Each _[validation rule]_ which is used in a validation manifest should be written within a .NET class. These classes must implement at least one of the two rule interfaces.

Whilst it is possible to create a validation manifest by-hand it's more common to write either [a validation builder] or to use [the validation model]. The builder is by-far the easiest and best approach when the collection and application of the rules can be described reliably using code. The validation model is more suited to dynamic, validation-as-data scenarios. Using the model, the rules which are applied may be stored in a serialised or data-oriented format, thus could be manipulated and altered by the application itself.

[validation rule]: WritingValidationRules.md
[a validation builder]: WritingValidationBuilders.md
[the validation model]: UsingTheValidationModel.md
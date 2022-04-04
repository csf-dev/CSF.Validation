# Ignoring accessor exceptions

When validating derived values or member values, there are occasionally instances when the accessor (executing a method, or a property getter, or arbitrary accessor logic) could raise an exception.
By default this will raise an exception across the validator and halt validation, because value accessors should not throw.
This functionality provides a way to ignore such exceptions.

_Use this functionality with caution; it is usually a sign of poor object design if accessors such as getters raise exceptions. It is much better to create accessors which do not raise exceptions._

## How to ignore exceptions

If you are dealing with a value accessor that could throw and you wish to prevent such exceptions from blocking validation, then use [the `IgnoreExceptions` method] when configuring the accessor. When this method is used, any exception thrown when getting the value will be caught & ignored.

[the `IgnoreExceptions` method]:xref:CSF.Validation.ValidatorBuilding.IConfiguresValueAccessor`2.IgnoreExceptions

## An ignored exception means the value is valid

Note that when ignoring accessor exceptions, validation for the value will not be executed.
This means that the default behaviour for a value which raises and exception is to treat it as **valid**.

If you wish to validate that a getter or other value-accessor does not raise an exception then you should write a validation rule for the object the provides the value. This rule should attempt to get the value, inside a `try`/`catch` block, and should return a failure result is an exception is caught.
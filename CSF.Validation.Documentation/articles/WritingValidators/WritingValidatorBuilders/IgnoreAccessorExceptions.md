# Ignoring accessor exceptions

_Beware using this functionality; it is usually a sign of poor object design if accessors such as getters raise exceptions. It is much better to create accessors which do not raise exceptions._

When validating derived values or member values, there are occasionally instances when the accessor (executing a method, or a property getter, or arbitrary accessor logic) could raise an exception. By default this will raise an exception across the validator and halt validation, because value accessors should not throw.

If you are dealing with a value accessor that could throw and you wish to prevent such exceptions from blocking validation, then use [the `IgnoreExceptions` method] when configuring the accessor. When this method is used, any exception thrown when getting the value will be caught & ignored.

Note that when ignoring accessor exceptions, validation for the value will not be executed.
This means that the value cannot be treated as invalid.

[the `IgnoreExceptions` method]:TODO
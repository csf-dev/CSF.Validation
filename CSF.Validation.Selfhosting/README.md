# Self-hosting validation framework

This package is useful only for applications _which do not make use of dependency injection_.

The types within this package provide a small API to create validators without needing to register the relevant types within application-wide DI.
It uses a small/self-contained DI environment so that non-DI-based apps may consume validators.

If your app does not use dependency injection then it is strongly recommended that you consider migrating towards it.
This package is provided only as a backstop for apps which cannot use it.
In ideal circumstances _this package would never be used_.

For more information, please see [the documentation website].

[the documentation website]:https://csf-dev.github.io/CSF.Validation/

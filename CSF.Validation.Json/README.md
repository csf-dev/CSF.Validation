# Creating validators from JSON

This small package is an enhancement to CSF.Validation which enables creating validators from JSON and/or saving them back to JSON.
To use this functionality within your app, include the following in the app's startup/dependency injection code:

```csharp
services.AddJsonValidationSerializer();
```

You may then inject & consume `CSF.Validation.ISerializesManifestModelToFromJson` in your app.
This service has methods to serialize and deserialize instances of `CSF.Validation.ManifestModel.Value` to/from JSON.
A manifest model value may be used to create a validator via the validator factory: `CSF.Validation.IGetsValidator`.

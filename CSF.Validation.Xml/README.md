# Creating validators from XML

This small package is an enhancement to CSF.Validation which enables creating validators from XML and/or saving them back to XML.
To use this functionality within your app, include the following in the app's startup/dependency injection code:

```csharp
services.AddXmlValidationSerializer();
```

You may then inject & consume `CSF.Validation.ISerializesManifestModelToFromXml` in your app.
This service has methods to serialize and deserialize instances of `CSF.Validation.ManifestModel.Value` to/from XML.
A manifest model value may be used to create a validator via the validator factory: `CSF.Validation.IGetsValidator`.

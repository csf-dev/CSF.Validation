using System;
using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A model which describes the result of <see cref="IConvertsModelValuesToManifestValues.ConvertAllValues(ModelToManifestConversionContext)"/>.
    /// This contains a root <see cref="ManifestItem"/> but also a collection of all of the
    /// manifest values which were converted, along with their original model values.
    /// </summary>
    public class ModelToManifestValueConversionResult
    {
        ICollection<ModelAndManifestValuePair> convertedValues = new List<ModelAndManifestValuePair>();

        /// <summary>
        /// Gets or sets the root manifest value.
        /// </summary>
        public ManifestItem RootValue { get; set; }

        /// <summary>
        /// Gets or sets a collection of all of the manifest values which were converted, along
        /// with their corresponding original model values.
        /// </summary>
        public ICollection<ModelAndManifestValuePair> ConvertedValues
        {
            get => convertedValues;
            set => convertedValues = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
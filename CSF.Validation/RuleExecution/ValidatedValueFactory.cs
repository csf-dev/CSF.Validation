using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A service which gets a <see cref="ValidatedValue"/> from a <see cref="ManifestValue"/> and an object
    /// to be validated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This prepares the validation rules to be executed and extracts all child values from the object to be
    /// validated, traversing its object graph according to the validation manifest.
    /// </para>
    /// </remarks>
    public class ValidatedValueFactory : IGetsValidatedValue
    {
        readonly IGetsValidatedValueFromBasis valueFromBasisFactory;
        readonly IGetsValueToBeValidated valueProvider;
        readonly IGetsEnumerableItemsToBeValidated enumerableProvider;

        /// <summary>
        /// Gets the validated value from the specified manifest value and object to be validated.
        /// </summary>
        /// <param name="manifestValue">The manifest value.</param>
        /// <param name="objectToBeValidated">The object to be validated.</param>
        /// <param name="options">The validation options.</param>
        /// <returns>A validated value, including a hierarchy of descendent values and
        /// the rules which may be executed upon those values.</returns>
        public ValidatedValue GetValidatedValue(ManifestValue manifestValue,
                                                object objectToBeValidated,
                                                ResolvedValidationOptions options)
        {
            if (manifestValue is null)
                throw new ArgumentNullException(nameof(manifestValue));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var openList = new Queue<ValidatedValueBasis>(new [] {
                new ValidatedValueBasis(manifestValue, new SuccessfulGetValueToBeValidatedResponse(objectToBeValidated), null)
            });
            ValidatedValue rootValidatedValue = null;

            while(openList.Any())
            {
                var currentBasis = openList.Dequeue();
                if(currentBasis.IsCircularReference()) continue;

                if(currentBasis.ValidatedValueResponse is IgnoredGetValueToBeValidatedResponse)
                    continue;

                var currentValues = GetValidatedValues(currentBasis);
                if(rootValidatedValue is null && currentValues.Any())
                    rootValidatedValue = currentValues.First();

                foreach(var value in currentValues)
                    FindAndAddChildrenToOpenList(currentBasis, value, openList, options);
            }

            return rootValidatedValue;
        }

        IList<ValidatedValue> GetValidatedValues(ValidatedValueBasis basis)
        {
            if(!(basis.ManifestValue is ManifestCollectionItem))
            {
                var value = valueFromBasisFactory.GetValidatedValue(basis);
                if(!(basis.Parent is null))
                    basis.Parent.ChildValues.Add(value);
                return new[] { value };
            }

            var values = enumerableProvider
                .GetEnumerableItems(basis.GetActualValue(), basis.ManifestValue.ValidatedType)
                .Select((x, idx) => new ValidatedValueBasis(basis.ManifestValue, new SuccessfulGetValueToBeValidatedResponse(x), basis.Parent, idx))
                .Select(valueFromBasisFactory.GetValidatedValue)
                .ToList();
            if(!(basis.Parent is null))
                basis.Parent.CollectionItems = values;
            
            return values;
        }

        void FindAndAddChildrenToOpenList(ValidatedValueBasis currentBasis,
                                          ValidatedValue currentValue,
                                          Queue<ValidatedValueBasis> openList,
                                          ResolvedValidationOptions options)
        {
            if(!currentValue.ValueResponse.IsSuccess)
                return;

            var actualValue = currentValue.GetActualValue();

            if(!(currentBasis.ManifestValue.CollectionItemValue is null || actualValue is null))
                openList.Enqueue(new ValidatedValueBasis(currentBasis.ManifestValue.CollectionItemValue,
                                                         currentValue.ValueResponse,
                                                         currentValue));

            foreach(var childManifestValue in currentBasis.GetChildManifestValues())
            {
                var valueResponse = valueProvider.GetValueToBeValidated(childManifestValue, actualValue, options);
                openList.Enqueue(new ValidatedValueBasis(childManifestValue, valueResponse, currentValue));
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatedValueFactory"/>.
        /// </summary>
        /// <param name="valueFromBasisFactory">A factory that gets the logic from a <see cref="ValidatedValueBasis"/>.</param>
        /// <param name="valueProvider">A service to get the value to be validated.</param>
        /// <param name="enumerableProvider">A service that gets the items of an enumerable object.</param>
        /// <exception cref="ArgumentNullException">If any parameter value is <see langword="null" />.</exception>
        public ValidatedValueFactory(IGetsValidatedValueFromBasis valueFromBasisFactory,
                                     IGetsValueToBeValidated valueProvider,
                                     IGetsEnumerableItemsToBeValidated enumerableProvider)
        {
            this.valueFromBasisFactory = valueFromBasisFactory ?? throw new ArgumentNullException(nameof(valueFromBasisFactory));
            this.valueProvider = valueProvider ?? throw new ArgumentNullException(nameof(valueProvider));
            this.enumerableProvider = enumerableProvider ?? throw new ArgumentNullException(nameof(enumerableProvider));
        }
    }
}
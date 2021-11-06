using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.Validation.Manifest;
using CSF.Validation.Rules;

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
        readonly IGetsValidationLogic validationLogicFactory;

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
                                                ValidationOptions options)
        {
            if (manifestValue is null)
                throw new ArgumentNullException(nameof(manifestValue));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var openList = new Queue<ValidatedValueBasis>(new [] { new ValidatedValueBasis(manifestValue, objectToBeValidated, null) });
            ValidatedValue rootValidatedValue = null;

            while(openList.Any())
            {
                var currentBasis = openList.Dequeue();

                var currentValue = GetValidatedValue(currentBasis);
                if(rootValidatedValue is null) rootValidatedValue = currentValue;

                FindAndAddChildrenToOpenList(currentBasis, currentValue, openList, options);
            }

            return rootValidatedValue;
        }

        static IEnumerable GetEnumerable(object value)
        {
            if(value is IEnumerable enumerable)
                return enumerable;

            var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ObjectMustBeEnumerable"),
                                        nameof(IEnumerable),
                                        value.GetType().FullName);
            throw new ValidationException(message);
        }

        ValidatedValue GetValidatedValue(ValidatedValueBasis basis)
        {
            var valueIdentity = basis.ManifestValue.IdentityAccessor is null
                ? null
                : basis.ManifestValue.IdentityAccessor(basis.ActualValue);
            
            var value = new ValidatedValue
            {
                ManifestValue = basis.ManifestValue,
                ActualValue = basis.ActualValue,
                ValueIdentity = valueIdentity,
                ParentValue = basis.Parent,
                CollectionItemOrder = basis.CollectionOrder,
            };

            if(!(basis.Parent is null))
                basis.Parent.ChildValues.Add(value);

            value.Rules = basis.ManifestValue.Rules
                .Select(manifestRule => new ExecutableRule
                        {
                            ValidatedValue = value,
                            ManifestRule = manifestRule,
                            RuleLogic = validationLogicFactory.GetValidationLogic(manifestRule),
                            RuleIdentifier = new RuleIdentifier(manifestRule, valueIdentity),
                        })
                .ToList();

            return value;
        }

        static void FindAndAddChildrenToOpenList(ValidatedValueBasis currentBasis,
                                                 ValidatedValue currentValue,
                                                 Queue<ValidatedValueBasis> openList,
                                                 ValidationOptions options)
        {
            foreach(var childManifestValue in currentBasis.ManifestValue.Children)
            {
                if(!TryGetActualValue(childManifestValue, currentBasis.ActualValue, options, out var childActualValue))
                    continue;

                if(childManifestValue.EnumerateItems)
                {
                    if(childActualValue is null) continue;
                    var enumerableChildValue = GetEnumerable(childActualValue);

                    long itemOrder = 0;
                    foreach(var item in enumerableChildValue)
                        openList.Enqueue(new ValidatedValueBasis(childManifestValue, item, currentValue, itemOrder++));
                }
                else
                {
                    openList.Enqueue(new ValidatedValueBasis(childManifestValue, childActualValue, currentValue));
                }
            }
        }

        static bool TryGetActualValue(ManifestValue manifestValue,
                                      object parentValue,
                                      ValidationOptions validationOptions,
                                      out object actualValue)
        {
            actualValue = null;
            if(parentValue is null) return false;

            try
            {
                actualValue = manifestValue.AccessorFromParent(parentValue);
                return true;
            }
            catch(Exception e)
            {
                if(!manifestValue.IgnoreAccessorExceptions && !validationOptions.IgnoreValueAccessExceptions)
                {
                    var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("ErrorAccessingValue"),
                                                manifestValue.ValidatedType.FullName);
                    throw new ValidationException(message, e);
                }

                actualValue = GetDefaultOfType(manifestValue.ValidatedType);
                return true;
            }
        }

        static object GetDefaultOfType(Type t) => t.GetTypeInfo().IsValueType ? Activator.CreateInstance(t) : null;

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatedValueFactory"/>.
        /// </summary>
        /// <param name="validationLogicFactory">The validation logic factory.</param>
        /// <exception cref="ArgumentNullException">If either <paramref name="validationLogicFactory"/> is null.</exception>
        public ValidatedValueFactory(IGetsValidationLogic validationLogicFactory)
        {
            this.validationLogicFactory = validationLogicFactory ?? throw new ArgumentNullException(nameof(validationLogicFactory));
        }

        /// <summary>
        /// A small model used just within this class.  It holds the information required in order to get
        /// a <see cref="ValidatedValue"/> at a point in the future.
        /// </summary>
        private sealed class ValidatedValueBasis
        {
            internal ManifestValue ManifestValue { get; }
            internal object ActualValue { get; }
            internal ValidatedValue Parent { get; }
            internal long CollectionOrder { get; }

            /// <summary>
            /// Initialises a new instance of <see cref="ValidatedValueBasis"/>.
            /// </summary>
            /// <param name="manifestValue">The manifest value for this basis.</param>
            /// <param name="actualValue">The actual value for this basis.</param>
            /// <param name="parent">The parent validated value for this basis.</param>
            /// <param name="collectionOrder">An optional collection order for this basis.</param>
            internal ValidatedValueBasis(ManifestValue manifestValue, object actualValue, ValidatedValue parent, long collectionOrder = 0)
            {
                ManifestValue = manifestValue;
                ActualValue = actualValue;
                Parent = parent;
                CollectionOrder = collectionOrder;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A model for contextual information about a validator-builder.
    /// </summary>
    public sealed class ValidatorBuilderContext : IGetsValidationManifest, IEquatable<ValidatorBuilderContext>
    {
        /// <summary>
        /// Gets the <see cref="ManifestItem"/> instance associated with the current context.
        /// </summary>
        public ManifestItem ManifestValue { get; }

        /// <summary>
        /// Gets a collection of contexts contained within the current instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It would be wrong to call these "child" contexts, because they might relate to true children
        /// (in the nomenclature of <see cref="ManifestItem.Children"/>) but they could also
        /// relate to polymorphic types or collection item values.  It is better to just think of
        /// these as contexts that are to be a part of the <see cref="ManifestValue"/> in some manner.
        /// </para>
        /// </remarks>
        public ICollection<ValidatorBuilderContext> Contexts { get; } = new HashSet<ValidatorBuilderContext>();

        /// <summary>
        /// Gets a collection of rule-configuration callbacks for the current instance.
        /// </summary>
        public ICollection<IConfiguresContext> ConfigurationCallbacks { get; } = new HashSet<IConfiguresContext>();

        /// <summary>
        /// Gets a value indicating whether or no the current context is eligible to be recursive.
        /// </summary>
        public bool IsEligibleToBeRecursive { get; private set; } = true;

        /// <summary>
        /// Determines whether or not the current instance is equal to the specified <paramref name="other"/> or not.
        /// </summary>
        /// <param name="other">An object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance is equal to the specified object; <see langword="false" /> otherwise.</returns>
        public bool Equals(ValidatorBuilderContext other)
        {
            if(ReferenceEquals(other, null)) return false;
            if(ReferenceEquals(other, this)) return true;

            return Equals(ManifestValue, other.ManifestValue);
        }

        /// <summary>
        /// Determines whether or not the current instance is equal to the specified <paramref name="obj"/> or not.
        /// </summary>
        /// <param name="obj">An object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance is equal to the specified object; <see langword="false" /> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as ValidatorBuilderContext);

        /// <summary>
        /// Gets a hash code for the current instance.
        /// </summary>
        /// <returns>A hash code</returns>
        public override int GetHashCode() => ManifestValue.GetHashCode();

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString() => $"[{nameof(ValidatorBuilderContext)}: {ManifestValue}]";

        /// <summary>
        /// Converts the current context into a recursive one.
        /// </summary>
        /// <param name="ancestor">The ancestor manifest item into which validation should recurse.</param>
        /// <exception cref="InvalidOperationException">If <see cref="IsEligibleToBeRecursive"/> is <see langword="false" />.</exception>
        public void MakeRecursive(ManifestItem ancestor)
        {
            if(!IsEligibleToBeRecursive)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustNotBeRecursive"), nameof(MakeRecursive));
                throw new InvalidOperationException(message);
            }

            ManifestValue.MakeRecursive(ancestor);
            IsEligibleToBeRecursive = false;
        }

        /// <summary>
        /// Asserts that the current context is not already recursive.  Also marks it ineligible to become recursive.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the current context is already recursive.</exception>
        public void AssertNotRecursive()
        {
            if(ManifestValue.IsRecursive)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustBeRecursive"), nameof(AssertNotRecursive));
                throw new InvalidOperationException(message);
            }
            IsEligibleToBeRecursive = false;
        }

        /// <summary>
        /// Gets a manifest value from the current instance.
        /// </summary>
        /// <returns>A manifest value.</returns>
        public ManifestItem GetManifestValue()
        {
            var manifestValues = (from context in Contexts
                                  where !Equals(context)
                                  select context.GetManifestValue())
                .ToList();
            ManifestValue.CombineWithDescendents(manifestValues);

            return ManifestValue;
        }

        /// <summary>
        /// Recursively calls all of the <see cref="ConfigurationCallbacks"/> upon the current context
        /// and all of its descendents: <see cref="Contexts"/>.
        /// </summary>
        public void RecursivelyConfigure()
        {
            foreach (var configuration in ConfigurationCallbacks)
                configuration.ConfigureContext(this);

            var descendents = Contexts.Where(x => !Equals(x)).ToList();
            foreach (var descendent in descendents)
                descendent.RecursivelyConfigure();
        }

        /// <inheritdoc/>
        public ValidationManifest GetManifest()
        {
            var root = GetManifestValue();
            RecursivelyConfigure();
            var manifest = new ValidationManifest
            {
                RootValue = root,
                ValidatedType = root.ValidatedType,
            };
            root.Parent = manifest;
            return manifest;
        }

        /// <summary>
        /// Gets a context provider from the specified object.
        /// </summary>
        /// <param name="provider">An object which must implement <see cref="IHasValidationBuilderContext"/>.</param>
        /// <returns>A <see cref="IHasValidationBuilderContext"/>.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="provider"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="provider"/> does not implement <see cref="IHasValidationBuilderContext"/>.</exception>
        public static IHasValidationBuilderContext GetContextProvider(object provider)
        {
            if (provider is null)
                throw new ArgumentNullException(nameof(provider));

            try
            {
                return (IHasValidationBuilderContext) provider;
            }
            catch(InvalidCastException e)
            {
                throw new ArgumentException(String.Format(Resources.ExceptionMessages.GetExceptionMessage("MustProvideAContext"),
                                                                  typeof(IHasValidationBuilderContext),
                                                                  nameof(GetContextProvider)),
                                            nameof(provider),
                                            e);
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ValidatorBuilderContext"/>.
        /// </summary>
        /// <param name="manifestValue">The manifest value associated with the current context.</param>
        public ValidatorBuilderContext(ManifestItem manifestValue)
        {
            ManifestValue = manifestValue ?? throw new ArgumentNullException(nameof(manifestValue));
        }
    }
}
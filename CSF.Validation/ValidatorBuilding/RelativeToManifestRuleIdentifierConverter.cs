using System;
using System.Linq;
using CSF.Validation.Manifest;
using static CSF.Validation.Resources.ExceptionMessages;

namespace CSF.Validation.ValidatorBuilding
{
    /// <summary>
    /// A service which maps/converts instances of <see cref="RelativeRuleIdentifier"/> to <see cref="ManifestRuleIdentifier"/>.
    /// </summary>
    public class RelativeToManifestRuleIdentifierConverter : IGetsManifestRuleIdentifierFromRelativeIdentifier
    {
        /// <summary>
        /// Gets the manifest rule identifier from a relative rule identifier.
        /// </summary>
        /// <param name="currentValue">The current manifest value from which the <paramref name="relativeIdentifier"/> should be derived.</param>
        /// <param name="relativeIdentifier">The relative rule identifier.</param>
        /// <returns>A manifest rule identifier.</returns>
        public ManifestRuleIdentifier GetManifestRuleIdentifier(ManifestValueBase currentValue, RelativeRuleIdentifier relativeIdentifier)
        {
            if (currentValue is null)
                throw new ArgumentNullException(nameof(currentValue));
            if (relativeIdentifier is null)
                throw new ArgumentNullException(nameof(relativeIdentifier));

            var manifestValue = GetManifestValue(currentValue, relativeIdentifier);
            return new ManifestRuleIdentifier(manifestValue, relativeIdentifier.RuleType, relativeIdentifier.RuleName);
        }

        static ManifestValueBase GetManifestValue(ManifestValueBase baseValue, RelativeRuleIdentifier relativeIdentifier)
        {
            var current = baseValue;

            for (var i = relativeIdentifier.AncestorLevels; i > 0; i--)
                current = current?.Parent ?? throw GetInsufficientParentsException(relativeIdentifier.AncestorLevels, nameof(relativeIdentifier));

            var memberName = relativeIdentifier.MemberName;
            if(!String.IsNullOrEmpty(memberName))
                current = current.Children.SingleOrDefault(x => x.MemberName == memberName) ?? throw GetCannotFindMemberException(memberName, nameof(relativeIdentifier));

            return current;
        }

        static Exception GetCannotFindMemberException(string memberName, string paramName)
        {
            var message = String.Format(GetExceptionMessage("ManifestValueDoesNotHaveChildMember"), nameof(ManifestValue), memberName);
            return new ArgumentException(message, paramName);
        }

        static ArgumentException GetInsufficientParentsException(int ancestorLevels, string paramName)
        {
            var message = String.Format(GetExceptionMessage("ManifestValueDoesNotHaveSufficientAncestorLevels"), nameof(ManifestValue), ancestorLevels);
            return new ArgumentException(message, paramName);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// Extension methods for <see cref="ManifestItem"/>.
    /// </summary>
    public static class ManifestItemExtensions
    {
        /// <summary>
        /// Gets a collection of all of the ancestor manifest items for the specified items.  Ancestors are returned
        /// closest-first, with more distant ancestors returned afterward.
        /// </summary>
        /// <param name="item">The manifest item.</param>
        /// <returns>A collection of the ancestor items.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="item"/> is <see langword="null" />.</exception>
        public static IEnumerable<ManifestItem> GetAncestors(this ManifestItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            return GetAncestorsPrivate(item);
        }

        static IEnumerable<ManifestItem> GetAncestorsPrivate(ManifestItem item)
        {
            ManifestItem current = item;
            while(!(current is null))
            {
                yield return current;
                current = current.ParentItem;
            }
        }

        /// <summary>
        /// Gets an ancestor from the specified item, at a specified depth.  A depth of one will return the
        /// immediate parent, two will return the grandparent and so on.
        /// </summary>
        /// <param name="item">The manifest item.</param>
        /// <param name="depth">The depth of ancestor to return.</param>
        /// <returns>The ancestor manifest item.</returns>
        /// 
        public static ManifestItem GetAncestor(this ManifestItem item, int depth)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if(depth < 0)
                throw new ArgumentOutOfRangeException(nameof(depth));

            var ancestors = item.GetAncestors();

            try
            {
                return ancestors.Skip(depth).First();
            }
            catch(InvalidOperationException ex)
            {
                var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("NotEnoughAncestors"),
                                            depth,
                                            ancestors.Count() - 1);
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
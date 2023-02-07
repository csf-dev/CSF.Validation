using System;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// A 'flags' enumeration which indicates the possible kinds of <see cref="ManifestItem"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Instead of a class hierarchy, manifest items are marked-up with these flags because a
    /// manifest item may have more than one of these attributes at the same time.
    /// </para>
    /// </remarks>
    [Flags]
    public enum ManifestItemType
    {
        /// <summary>
        /// The manifest item represents a regular value to be validated.
        /// </summary>
        Value                       = 1 << 0,

        /// <summary>
        /// The manifest item represents an item of a collection.
        /// </summary>
        CollectionItem              = 1 << 1,

        /// <summary>
        /// The manifest item represents a polymorphic type for either a value or collection item.
        /// </summary>
        PolymorphicType             = 1 << 2,

        /// <summary>
        /// The manifest item is recursive and 'points back to' an ancestor manifest item.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Note that on its own, this value is not permitted as a value to <see cref="ManifestItem.ItemType"/>.
        /// It is only valid when combined with one of:
        /// </para>
        /// <list type="bullet">
        /// <item><description><see cref="Value"/></description></item>
        /// <item><description><see cref="CollectionItem"/></description></item>
        /// <item><description><see cref="PolymorphicType"/></description></item>
        /// </list>
        /// </remarks>
        Recursive                   = 1 << 3,

        /// <summary>
        /// A convenience value equivalent to the combination of both <see cref="Value"/> &amp; <see cref="Recursive"/>.
        /// </summary>
        RecursiveValue              = Value | Recursive,

        /// <summary>
        /// A convenience value equivalent to the combination of both <see cref="CollectionItem"/> &amp; <see cref="Recursive"/>.
        /// </summary>
        RecursiveCollectionItem     = CollectionItem | Recursive,

        /// <summary>
        /// A convenience value equivalent to the combination of both <see cref="PolymorphicType"/> &amp; <see cref="Recursive"/>.
        /// </summary>
        RecursivePolymorphicType    = PolymorphicType | Recursive
    }
}
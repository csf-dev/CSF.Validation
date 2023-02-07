using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Manifest
{
    /// <summary>
    /// Base class used for values which are validated.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The validation manifest is the model by which validators are described, including how they should
    /// validate objects and values.
    /// </para>
    /// <para>
    /// The validation manifest objects are not particularly suited to serialization,
    /// as they support the use of types that cannot be easily serialized.
    /// If you are looking for a way to create/define a validator using serialized data then please read the
    /// article <xref href="ManifestModelIndexPage?text=Using+the+Manifest+Model"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ManifestRule"/>
    /// <seealso cref="ManifestRuleIdentifier"/>
    /// <seealso cref="ValidationManifest"/>
    /// <seealso cref="ManifestItemType"/>
    public class ManifestItem : IManifestNode
    {
        static readonly ManifestItemType[] permittedTypes = {
            ManifestItemType.Value,
            ManifestItemType.CollectionItem,
            ManifestItemType.PolymorphicType,
            ManifestItemType.RecursiveValue,
            ManifestItemType.RecursiveCollectionItem,
            ManifestItemType.RecursivePolymorphicType
        };

        ICollection<ManifestItem> children = new List<ManifestItem>();
        ICollection<ManifestRule> rules = new List<ManifestRule>();
        ICollection<ManifestItem> polymorphicTypes = new HashSet<ManifestItem>();
        ManifestItemType itemType = ManifestItemType.Value;

        /// <summary>
        /// Gets or sets a value which indicates the type &amp; behaviour of the current manifest item.
        /// </summary>
        public ManifestItemType ItemType
        {
            get => itemType;
            set {
                if(!permittedTypes.Contains(value))
                {
                    var message = String.Format(Resources.ExceptionMessages.GetExceptionMessage("InvalidManifestItemType"),
                                                nameof(ItemType),
                                                typeof(ManifestItemType));
                    throw new ArgumentException(message, nameof(value));
                }
                itemType = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="ItemType"/> includes the flag <see cref="ManifestItemType.Value"/>.
        /// </summary>
        public bool IsValue => ItemType.HasFlag(ManifestItemType.Value);

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="ItemType"/> includes the flag <see cref="ManifestItemType.CollectionItem"/>.
        /// </summary>
        public bool IsCollectionItem => ItemType.HasFlag(ManifestItemType.CollectionItem);

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="ItemType"/> includes the flag <see cref="ManifestItemType.PolymorphicType"/>.
        /// </summary>
        public bool IsPolymorphicType => ItemType.HasFlag(ManifestItemType.PolymorphicType);

        /// <summary>
        /// Gets a value indicating whether or not the <see cref="ItemType"/> includes the flag <see cref="ManifestItemType.Recursive"/>.
        /// </summary>
        public bool IsRecursive => ItemType.HasFlag(ManifestItemType.Recursive);

        /// <summary>
        /// Gets or sets a reference to an ancestor manifest item, to which the current item refers.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see cref="IsRecursive"/> is <see langword="true" /> then this value must not be <see langword="null" />.
        /// Otherwise, it must be <see langword="null" />.
        /// </para>
        /// </remarks>
        public ManifestItem RecursiveAncestor { get; set; }

        /// <summary>
        /// Gets or sets the type of the object which the current manifest value describes.
        /// </summary>
        public Type ValidatedType { get; set; }

        /// <summary>
        /// Gets or sets the parent of the current manifest item.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This must be either a <see cref="ValidationManifest"/> (indicating that this item is the root of the
        /// validation manifest data-structure) or another <see cref="ManifestItem"/>, indicating the parent of the
        /// current item.
        /// </para>
        /// </remarks>
        public IManifestNode Parent { get; set; }

        /// <summary>
        /// Gets or sets a function which retrieves a unique identity of the object being
        /// validated, given a reference to that object being validated.
        /// </summary>
        public Func<object, object> IdentityAccessor { get; set; }

        /// <summary>
        /// Gets or sets an optional value object which indicates how items within a collection are to be validated.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the value representd by the current instance is a collection/enumerable of items then these items may
        /// be validated individually.  In this scenario, the <see cref="ManifestItem.ValidatedType"/> must be a
        /// type that implements <see cref="System.Collections.Generic.IEnumerable{T}"/> for at least one generic type.
        /// </para>
        /// <para>
        /// If this property has a non-null value, then the <see cref="ManifestItem"/> will be used to validate
        /// each item within that collection.
        /// </para>
        /// <para>
        /// If the current manifest value does not represent a collection of items to be validated individually then this
        /// property must be <see langword="null" />.
        /// </para>
        /// </remarks>
        public ManifestItem CollectionItemValue { get; set; }

        /// <summary>
        /// Gets or sets a collection of the immediate descendents of the current manifest value.
        /// </summary>
        public ICollection<ManifestItem> Children
        {
            get => children;
            set => children = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a collection of the rules associated with the current value.
        /// </summary>
        public ICollection<ManifestRule> Rules
        {
            get => rules;
            set => rules = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a mapping of the runtime types to polymorphic validation manifest definitions for those types.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see cref="IsPolymorphicType"/> is <see langword="true" /> then this collection must be empty.
        /// </para>
        /// </remarks>
        public ICollection<ManifestItem> PolymorphicTypes
        {
            get => polymorphicTypes;
            set => polymorphicTypes = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets a function which gets (from the object represented by the <see cref="ManifestItem.Parent"/>)
        /// the value for the current instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see cref="IsValue"/> is <see langword="false" /> then this value is irrelevant and must be <see langword="null" />.
        /// </para>
        /// </remarks>
        public Func<object, object> AccessorFromParent { get; set; }

        /// <summary>
        /// Where the current value represents a member access invocation (such as
        /// a property getter), this property gets or sets the name of that member.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see cref="IsValue"/> is <see langword="false" /> then this value is irrelevant and must be <see langword="null" />.
        /// </para>
        /// </remarks>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets an optional value which indicates the desired behaviour should the <see cref="AccessorFromParent"/> raise an exception.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This option will override the behaviour specified at <see cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        /// for the current manifest value, if this property is set to any non-<see langword="null" /> value.
        /// </para>
        /// <para>
        /// If this property is set to <see langword="null" /> then the behaviour at <see cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        /// will be used.
        /// </para>
        /// <para>
        /// If <see cref="IsValue"/> is <see langword="false" /> then this value is irrelevant and must be <see langword="null" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="ResolvedValidationOptions.AccessorExceptionBehaviour"/>
        public ValueAccessExceptionBehaviour? AccessorExceptionBehaviour { get; set; }

        /// <summary>
        /// Gets a string representation of the current instance.
        /// </summary>
        /// <returns>A string which represents the current instance.</returns>
        public override string ToString()
        {
            var propertyStrings = GetPropertyValuesForToString()
                .Where(x => !(x.Value is null))
                .Select(x => $"{x.Key} = {x.Value}").ToList();
            
            return $"[{GetType().Name}: {String.Join(", ",  propertyStrings)}]";
        }

        /// <summary>
        /// Gets a collection of property values which will be used for the <see cref="ToString()"/> method.
        /// </summary>
        /// <returns>The property names &amp; values</returns>
        IDictionary<string, string> GetPropertyValuesForToString()
        {
            return new Dictionary<string, string>
            {
                { nameof(Type), ValidatedType?.Name },
                { nameof(ManifestItem.MemberName), this.MemberName },
            };
        }

        /// <summary>
        /// Creates a new <see cref="ManifestItem"/> from the specified <paramref name="ancestor"/>,
        /// but with the <see cref="ManifestItemType.Recursive"/> added to its type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Many property values are copied from the <paramref name="ancestor"/> by this method, as the
        /// recursive item has many of the same properties as its ancestor.
        /// </para>
        /// </remarks>
        /// <param name="ancestor">The ancestor item upon which to base the new recursive item.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="ancestor"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="ancestor"/> is already recursive.</exception>
        public static ManifestItem CreateRecursive(ManifestItem ancestor)
        {
            if (ancestor is null)
                throw new ArgumentNullException(nameof(ancestor));

            if(ancestor.IsRecursive)
                throw new ArgumentException(String.Format(Resources.ExceptionMessages.GetExceptionMessage("AlreadyRecursive")), nameof(ancestor));

            return new ManifestItem
            {
                AccessorExceptionBehaviour = ancestor.AccessorExceptionBehaviour,
                Children = ancestor.Children.ToList(),
                ItemType = ancestor.ItemType | ManifestItemType.Recursive,
                CollectionItemValue = ancestor.CollectionItemValue,
                IdentityAccessor = ancestor.IdentityAccessor,
                PolymorphicTypes = ancestor.PolymorphicTypes,
                RecursiveAncestor = ancestor,
                Rules = ancestor.Rules.ToList(),
                ValidatedType = ancestor.ValidatedType,
            };
        }
    }
}
//  
//  Entity.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 CSF Software Limited
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using CSF.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Entities
{
  /// <summary>
  /// <para>
  /// Generic type to store entity references that are serialisable to a persistent data source such as a database.
  /// </para>
  /// </summary>
  /// <typeparam name="T">
  /// Describes the underlying type of the reference to be stored within the current object instance.
  /// </typeparam>
  /// <remarks>
  /// <para>This type supports access to the reference ID within that repository.</para>
  /// </remarks>
  [Serializable]
  public class Entity<TEntity,TIdentity> : IEntity<TEntity,TIdentity> where TEntity : IEntity
  {
    #region fields
    
    private IIdentity<TEntity,TIdentity> _identity;
    
    #endregion
    
    #region IEntity[T] implementation

    /// <summary>
    /// <para>Read-only.  Gets whether or not the current instance holds a valid reference or not.</para>
    /// </summary>
    public virtual bool HasIdentity
    {
      get {
        return _identity != null;
      }
    }
    
    /// <summary>
    /// <para>Gets and sets the unique identifier component of this entity's generic identity.</para>
    /// <para>
    /// Avoid using this property in favour of <see cref="GetIdentity"/> and <c>SetIdentity</c>, see the
    /// remarks for more information.
    /// </para>
    /// <seealso cref="GetIdentity"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Ideally, avoid using this property unless absolutely neccesary.  It exists primarily for providing access to
    /// database-mapping layers that need a property with which to reference the unique identifier.
    /// </para>
    /// </remarks>
    public virtual TIdentity Id
    {
      get {
        TIdentity output = default(TIdentity);
        
        if(_identity != null)
        {
          output = _identity.Value;
        }
        
        return output;
      }
      set {
        try
        {
          _identity = this.CreateIdentity(value);
        }
        catch(ArgumentException)
        {
          _identity = null;
        }
      }
    }
    
    /// <summary>
    /// <para>Gets the reference stored within the current object instance.</para>
    /// </summary>
    /// <returns>
    /// An instance of the underlying reference for the current object.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// If the underlying reference stored within the current object instance is not valid.
    /// </exception>
    public virtual IIdentity<TEntity,TIdentity> GetIdentity()
    {
      if(_identity == null)
      {
        throw new InvalidOperationException("This entity instance does not have an identity.");
      }
      
      return _identity;
    }

    /// <summary>
    /// <para>Overloaded.  Sets the reference stored within the current object instance.</para>
    /// </summary>
    /// <param name="identityValue">
    /// An instance of the underlying reference type for the current object.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If <paramref name="identityValue"/> is not a valid reference.
    /// </exception>
    public virtual void SetIdentity(TIdentity identityValue)
    {
      if(_identity != null)
      {
        throw new InvalidOperationException("The current entity already has an identity.  Identity may not be " +
                                            "changed once set without first calling ClearIdentity().");
      }
      
      _identity = this.CreateIdentity(identityValue);
    }
    
    /// <summary>
    /// <para>Overloaded.  Sets the reference stored within the current object instance.</para>
    /// </summary>
    /// <param name="identityValue">
    /// A <see cref="System.Object"/>
    /// </param>
    public virtual void SetIdentity(object identityValue)
    {
      this.SetIdentity(Convert.ChangeType(identityValue, typeof(TIdentity)));
    }

    /// <summary>
    /// <para>Clears the underlying reference stored within the current instance.</para>
    /// </summary>
    public virtual void ClearIdentity()
    {
      _identity = null;
    }
    
    /// <summary>
    /// <para>
    /// Determines whether or not a given <paramref name="identityValue"/> is valid for storing within the current
    /// object instance.
    /// </para>
    /// </summary>
    /// <param name="identityValue">
    /// An instance of the underlying reference type for the current object.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public virtual bool ValidateIdentity(TIdentity identityValue)
    {
      return this.Validate(identityValue);
    }
    
    #endregion
    
    #region explicit IEntity implementation
    
    object IEntity.Id {
      get {
        return this.Id;
      }
      set {
        this.Id = (TIdentity) value;
      }
    }
    
    IIdentity IEntity.GetIdentity()
    {
      return this.GetIdentity();
    }
    
    void IEntity.SetIdentity(object identityValue)
    {
      this.SetIdentity((TIdentity) Convert.ChangeType(identityValue, typeof(TIdentity)));
    }
    
    bool IEntity.ValidateIdentity(object identityValue)
    {
      return this.ValidateIdentity((TIdentity) identityValue);
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>
    /// Overridden from <see cref="Object"/>, overloaded.  Determines whether the current instance is equal to the
    /// specified <paramref name="obj"/>.
    /// </para>
    /// </summary>
    /// <param name="obj">
    /// A <see cref="System.Object"/> to compare against.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/> indicating whether the <paramref name="obj"/> is equal to the current instance.
    /// </returns>
    public override bool Equals(object obj)
    {
      bool output;
      
      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else if(obj is IEntity)
      {
        output = this.Equals((IEntity) obj);
      }
      else
      {
        output = false;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Overloaded.  Determines whether the current instance is equal to the specified entity reference.
    /// </para>
    /// </summary>
    /// <param name="compareTo">
    /// An <see cref="IEntity"/> to compare against
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/> indicating whether the entity to <paramref name="compareTo"/> is equal to
    /// the current instance.
    /// </returns>
    public virtual bool Equals(IEntity compareTo)
    {
      bool output;
      
      if(Object.ReferenceEquals(this, compareTo))
      {
        output = true;
      }
      else if((object) compareTo == null)
      {
        output = false;
      }
      else if(!this.HasIdentity || !compareTo.HasIdentity)
      {
        output = false;
      }
      else
      {
        output = (this.GetIdentity().Equals(compareTo.GetIdentity()));
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overridden.  Gets a hash code for the current object instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    public override int GetHashCode()
    {
      int output;
      
      if(this.HasIdentity)
      {
        output = _identity.GetHashCode();
      }
      else
      {
        output = base.GetHashCode();
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Creates a human-readable string representation of the current instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public override string ToString()
    {
      string output;
      
      if(this.HasIdentity)
      {
        output = this.GetIdentity().ToString();
      }
      else
      {
        output = String.Format("[{0}: no identity]", this.GetIdentityType().FullName);
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>
    /// Factory method creates a new <see cref="IIdentity"/> instance that uniquely identifies the current instance.
    /// </para>
    /// </summary>
    /// <param name="identityValue">
    /// An identity value of the generic type appropriate to this entity
    /// </param>
    /// <returns>
    /// A generic identity instance
    /// </returns>
    protected virtual IIdentity<TEntity,TIdentity> CreateIdentity(TIdentity identityValue)
    {
      if(!this.Validate(identityValue))
      {
        throw new ArgumentException("Invalid identity value");
      }
      
      return Identity.Create<TEntity,TIdentity>(identityValue);
    }
    
    /// <summary>
    /// <para>Gets the <see cref="System.Type"/> that this entity instance uses to identify itself.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="Type"/>
    /// </returns>
    protected virtual Type GetIdentityType()
    {
      return typeof(TEntity);
    }

    /// <summary>
    /// Gets (setting up if neccesary) a list designed to hold 'reciprocal' references between this entity and related
    /// entities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to either pass-through and return an existing event-bound list for the one-to-many
    /// relationship or to create and configure that list before returning it.  This private overload requires the
    /// source list to be non-null.
    /// </para>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A list that wraps the source list with events.
    /// </returns>
    /// <param name='sourceList'>
    /// The 'source' list of related entities.
    /// </param>
    /// <param name='property'>
    /// An expression that exposes a property (upon the items within the source list) that holds the reference back to
    /// the 'parent' entity.
    /// </param>
    /// <typeparam name='TItem'>
    /// The type of items in the list.
    /// </typeparam>
    private IEventBoundList<TItem> GetOrInitReferenceList<TItem>(IList<TItem> sourceList,
                                                                 Expression<Func<TItem, object>> property)
      where TItem : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      if(sourceList == null)
      {
        throw new ArgumentNullException("sourceList");
      }

      IEventBoundList<TItem> output = sourceList as IEventBoundList<TItem>;

      if(output == null)
      {
        PropertyInfo propInfo = Reflect.Property(property);

        output = sourceList.WrapWithBeforeActions(
          (list, item) => {
            if(item == null)
            {
              throw new ArgumentNullException("item");
            }
            propInfo.SetValue(item, this, null);
            return true;
          },
          (list, item) => {
            if(item == null)
            {
              throw new ArgumentNullException("item");
            }
            bool contained = list.Contains(item);
            if(contained)
            {
              propInfo.SetValue(item, null, null);
            }
            return contained;
          });
      }

      return output;
    }

    /// <summary>
    /// Gets (setting up if neccesary) a list designed to hold 'reciprocal' references between this entity and related
    /// entities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to either pass-through and return an existing event-bound list for the one-to-many
    /// relationship or to create and configure that list before returning it.  If the source list is null then it is
    /// initialised as an empty list before use.
    /// </para>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A list that wraps the source list with events.
    /// </returns>
    /// <param name='wrappedList'>
    /// The existing wrapped list, which may be null.
    /// </param>
    /// <param name='sourceList'>
    /// The 'source' list of related entities.
    /// </param>
    /// <param name='property'>
    /// An expression that exposes a property (upon the items within the source list) that holds the reference back to
    /// the 'parent' entity.
    /// </param>
    /// <typeparam name='TItem'>
    /// The type of items in the list.
    /// </typeparam>
    [Obsolete("This functionality is being moved to the new 'GetOrInitReferenceList' method")]
    protected virtual IList<TItem> GetOneToManyReferenceList<TItem>(ref IList<TItem> wrappedList,
                                                                              ref IList<TItem> sourceList,
                                                                              Expression<Func<TItem, object>> property)
      where TItem : class
    {
      return this.GetOrInitReferenceList(ref wrappedList, ref sourceList, property);
    }

    /// <summary>
    /// Gets (setting up if neccesary) a list designed to hold 'reciprocal' references between this entity and related
    /// entities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is used to either pass-through and return an existing event-bound list for the one-to-many
    /// relationship or to create and configure that list before returning it.  If the source list is null then it is
    /// initialised as an empty list before use.
    /// </para>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A list that wraps the source list with events.
    /// </returns>
    /// <param name='wrappedList'>
    /// The existing wrapped list, which may be null.
    /// </param>
    /// <param name='sourceList'>
    /// The 'source' list of related entities.
    /// </param>
    /// <param name='property'>
    /// An expression that exposes a property (upon the items within the source list) that holds the reference back to
    /// the 'parent' entity.
    /// </param>
    /// <typeparam name='TItem'>
    /// The type of items in the list.
    /// </typeparam>
    protected virtual IList<TItem> GetOrInitReferenceList<TItem>(ref IList<TItem> wrappedList,
                                                                 ref IList<TItem> sourceList,
                                                                 Expression<Func<TItem, object>> property)
      where TItem : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      IEventBoundList<TItem> typedList = wrappedList as IEventBoundList<TItem>;

      if(typedList == null || !typedList.IsWrappedList(sourceList))
      {
        sourceList = sourceList?? new List<TItem>();
        wrappedList = this.GetOrInitReferenceList(sourceList, property);
      }

      return wrappedList;
    }

    /// <summary>
    /// Handles cleanup and 'bookkeeping' tasks when replacing an existing many-to-many reciprocal reference list with
    /// a new list instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <returns>
    /// A list that wraps the replacement list.
    /// </returns>
    /// <param name='wrappedList'>
    /// The old/original wrapped list, which will be cleared gracefully before it is overwritten.
    /// </param>
    /// <param name='replacementList'>
    /// The list with which to replace the old.
    /// </param>
    /// <param name='property'>
    /// An expression that exposes a property (upon the items within the source list) that holds the reference back to
    /// the 'parent' entity.
    /// </param>
    /// <typeparam name='TItem'>
    /// The type of items in the list.
    /// </typeparam>
    [Obsolete("This functionality is being moved to the new 'ReplaceReferenceList' method")]
    protected virtual IList<TItem> ReplaceOneToManyReferenceList<TItem>(IList<TItem> wrappedList,
                                                                        IList<TItem> replacementList,
                                                                        Expression<Func<TItem, object>> property)
      where TItem : class
    {
      this.ReplaceReferenceList(ref wrappedList, replacementList, property);
      return wrappedList;
    }

    /// <summary>
    /// Handles cleanup and 'bookkeeping' tasks when replacing an existing many-to-many reciprocal reference list with
    /// a new list instance.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is very heavily based on the excellent work found at:
    /// <c>https://handcraftsman.wordpress.com/2011/01/05/nhibernate-custom-collection-options/</c>.
    /// </para>
    /// </remarks>
    /// <param name='wrappedList'>
    /// The old/original wrapped list, which will be cleared gracefully before it is overwritten and then replaced with
    /// a new wrapped list.
    /// </param>
    /// <param name='replacementList'>
    /// The list with which to replace the old.
    /// </param>
    /// <param name='property'>
    /// An expression that exposes a property (upon the items within the source list) that holds the reference back to
    /// the 'parent' entity.
    /// </param>
    /// <typeparam name='TItem'>
    /// The type of items in the list.
    /// </typeparam>
    protected virtual void ReplaceReferenceList<TItem>(ref IList<TItem> wrappedList,
                                                       IList<TItem> replacementList,
                                                       Expression<Func<TItem, object>> property)
      where TItem : class
    {
      // See the remarks for IEventBoundList<T> for an important rationale discussion for the generic constraint 'class'

      if(replacementList == null)
      {
        throw new ArgumentNullException("replacementList");
      }

      IEventBoundList<TItem> typedList = wrappedList as IEventBoundList<TItem>;

      if(typedList != null)
      {
        typedList.DetachAll();
      }

      PropertyInfo propInfo = Reflect.Property(property);
      foreach(TItem item in replacementList)
      {
        propInfo.SetValue(item, this, null);
      }

      wrappedList = this.GetOrInitReferenceList(replacementList, property);
    }


    /// <summary>
    /// <para>Private method to validate an identity value.</para>
    /// </summary>
    /// <param name="identityValue">
    /// A value of the generic type of this instance.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    private bool Validate(TIdentity identityValue)
    {
      return !Object.Equals(identityValue, default(TIdentity));
    }
    
    #endregion
    
    #region events and invokers
    
    /// <summary>
    /// <para>
    /// This event is invoked when the current instance becomes dirty (requires update in a presistent data store).
    /// </para>
    /// </summary>
    public virtual event EventHandler Dirty;
    
    /// <summary>
    /// <para>This event is invoked when the current instance is to deleted in a persistent data store.</para>
    /// </summary>
    public virtual event EventHandler Deleted;
    
    /// <summary>
    /// <para>
    /// This event is invoked when the current instance is created (stored within the repository for the first time).
    /// </para>
    /// </summary>
    public virtual event EventHandler Created;
    
    /// <summary>
    /// <para>Event invoker for when this instance becomes dirty.</para>
    /// </summary>
    protected virtual void OnDirty()
    {
      EventArgs ev = new EventArgs();
      
      if(this.Dirty != null)
      {
        this.Dirty(this, ev);
      }
    }
    
    /// <summary>
    /// <para>Event invoker for when this instance is to be deleted.</para>
    /// </summary>
    protected virtual void OnDelete()
    {
      EventArgs ev = new EventArgs();
      
      if(this.Deleted != null)
      {
        this.Deleted(this, ev);
      }
    }
    
    /// <summary>
    /// <para>
    /// Event invoker for when this instance is to be created (stored in the repository for the first time).
    /// </para>
    /// </summary>
    protected virtual void OnCreated()
    {
      EventArgs ev = new EventArgs();
      
      if(this.Created != null)
      {
        this.Created(this, ev);
      }
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// <para>Initialises this instance with an empty/default reference.</para>
    /// </summary>
    public Entity()
    {
      this._identity = null;
    }
    
    #endregion
    
    #region static operator overloads
    
    /// <summary>
    /// <para>Performs equality testing between two entity instances.</para>
    /// </summary>
    /// <remarks>
    /// <para>This equality test does not require type equality between the objects to be compared.</para>
    /// </remarks>
    /// <param name="entity">
    /// An entity instance.
    /// </param>
    /// <param name="obj">
    /// An <see cref="IEntity"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator ==(Entity<TEntity,TIdentity> entity, IEntity obj)
    {
      bool output;
      
      if((object) entity == null)
      {
        output = (obj == null);
      }
      else
      {
        output = entity.Equals(obj);
      }
      
      return output;
    }

    /// <summary>
    /// <para>Performs inequality testing between two entity instances.</para>
    /// </summary>
    /// <remarks>
    /// <para>This equality test does not require type equality between the objects to be compared.</para>
    /// </remarks>
    /// <param name="entity">
    /// An entity instance.
    /// </param>
    /// <param name="obj">
    /// An <see cref="IEntity"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator !=(Entity<TEntity,TIdentity> entity, IEntity obj)
    {
      return !(entity == obj);
    }
    
    #endregion
  }
}


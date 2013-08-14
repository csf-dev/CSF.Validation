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
  public class Entity<TEntity,TIdentity>
    : IEntity<TEntity,TIdentity>,
      IEquatable<IEntity<TEntity,TIdentity>>,
      IEquatable<IEntity>
    where TEntity : IEntity
  {
    #region fields
    
    private Identity<TEntity,TIdentity>? _identity;
    
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
        return _identity.HasValue? _identity.Value.Value : default(TIdentity);
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
    public virtual Identity<TEntity,TIdentity> GetIdentity()
    {
      if(!_identity.HasValue)
      {
        throw new InvalidOperationException("This entity instance does not have an identity.");
      }
      
      return _identity.Value;
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
      if(_identity.HasValue)
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

      IEntity<TEntity,TIdentity> typedObj = obj as IEntity<TEntity,TIdentity>;
      if(typedObj != null)
      {
        output = this.Equals(typedObj);
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

      IEntity<TEntity,TIdentity> typedObj = compareTo as IEntity<TEntity,TIdentity>;
      if(typedObj != null)
      {
        output = this.Equals(typedObj);
      }
      else
      {
        output = false;
      }

      return output;
    }
    
    /// <summary>
    /// <para>
    /// Determines whether the current instance is equal to the specified <paramref name="obj"/>.
    /// </para>
    /// </summary>
    /// <param name="obj">
    /// An entity instance to compare with.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/> indicating whether the <paramref name="obj"/> is equal to the current instance.
    /// </returns>
    public virtual bool Equals(IEntity<TEntity,TIdentity> obj)
    {
      bool output;

      if(obj == null)
      {
        output = false;
      }
      else if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else if(!this.HasIdentity || !obj.HasIdentity)
      {
        output = false;
      }
      else
      {
        output = (this.GetIdentity() == obj.GetIdentity());
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
    /// Creates and validates a new identity instance from the given value.
    /// </summary>
    /// <param name="identityValue">
    /// An identity value of the generic type appropriate to this entity
    /// </param>
    /// <returns>
    /// A generic identity instance
    /// </returns>
    protected virtual Identity<TEntity,TIdentity> CreateIdentity(TIdentity identityValue)
    {
      if(!this.Validate(identityValue))
      {
        throw new ArgumentException("Invalid identity value");
      }
      
      return new Identity<TEntity,TIdentity>(identityValue);
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
    /// Determines equality between two entity instances.
    /// </summary>
    /// <param name="objectA">
    /// An entity instance.
    /// </param>
    /// <param name="objectB">
    /// An entity instance.
    /// </param>
    public static bool operator ==(Entity<TEntity,TIdentity> objectA, IEntity<TEntity,TIdentity> objectB)
    {
      bool output;

      if((object) objectA == null)
      {
        output = ((object) objectB == null);
      }
      else
      {
        output = objectA.Equals(objectB);
      }

      return output;
    }

    /// <summary>
    /// Determines inequality between two entity instances.
    /// </summary>
    /// <param name="objectA">
    /// An entity instance.
    /// </param>
    /// <param name="objectB">
    /// An entity instance.
    /// </param>
    public static bool operator !=(Entity<TEntity,TIdentity> objectA, IEntity<TEntity,TIdentity> objectB)
    {
      return !(objectA == objectB);
    }

    /// <summary>
    /// <para>Performs equality testing between two entity instances.</para>
    /// </summary>
    /// <remarks>
    /// <para>This equality test does not require type equality between the objects to be compared.</para>
    /// </remarks>
    /// <param name="objectA">
    /// An entity instance.
    /// </param>
    /// <param name="objectB">
    /// An <see cref="IEntity"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator ==(Entity<TEntity,TIdentity> objectA, IEntity objectB)
    {
      bool output;
      
      if((object) objectA == null)
      {
        output = ((object) objectB == null);
      }
      else
      {
        output = objectA.Equals(objectB);
      }
      
      return output;
    }

    /// <summary>
    /// <para>Performs inequality testing between two entity instances.</para>
    /// </summary>
    /// <remarks>
    /// <para>This equality test does not require type equality between the objects to be compared.</para>
    /// </remarks>
    /// <param name="objectA">
    /// An entity instance.
    /// </param>
    /// <param name="objectB">
    /// An <see cref="IEntity"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool operator !=(Entity<TEntity,TIdentity> objectA, IEntity objectB)
    {
      return !(objectA == objectB);
    }

    #endregion
  }
}


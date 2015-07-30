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
  /// Generic base type for an entity object which may be stored in a persistent data source.
  /// </summary>
  /// <typeparam name="TIdentity">The identity type for the current instance.</typeparam>
  [Serializable]
  public class Entity<TIdentity> : IEntity, IEquatable<IEntity>, IEquatable<Entity<TIdentity>>
  {
    #region fields
    
    private IIdentity _identity;
    private int? _cachedHashCode;
    
    #endregion
    
    #region properties

    /// <summary>
    /// Gets a value indicating whether this instance has an identity or not.
    /// </summary>
    /// <value><c>true</c> if this instance has an identity; otherwise, <c>false</c>.</value>
    public virtual bool HasIdentity
    {
      get {
        return _identity != null;
      }
    }
    
    /// <summary>
    /// Gets or sets the identity value for the current instance.
    /// </summary>
    /// <value>The identity.</value>
    public virtual TIdentity Identity
    {
      get {
        return (_identity != null)? (TIdentity) _identity.Value : default(TIdentity);
      }
      set {
        if(Object.Equals(value, default(TIdentity)))
        {
          _identity = null;
        }
        else
        {
          _identity = CSF.Entities.Identity.Create<TIdentity>(value, this.GetType());
        }
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the raw identity instance contained within the current entity, favour the extension method 'GetIdentity'
    /// instead.
    /// </summary>
    /// <returns>The identity value.</returns>
    public virtual IIdentity GetRawIdentity()
    {
      if(!this.HasIdentity)
      {
        throw new InvalidOperationException("The current instance must have an identity, check using HasIdentity().");
      }

      return _identity;
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      bool output;

      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else
      {
        var other = obj as Entity<TIdentity>;
        output = ((object) other != null)? this.Equals(other) : false;
      }

      return output;
    }
    
    /// <summary>
    /// Determines whether the specified <see cref="CSF.Entities.IEntity"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </summary>
    /// <param name="obj">The <see cref="CSF.Entities.IEntity"/> to compare with the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Entities.IEntity"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool Equals(IEntity obj)
    {
      bool output;

      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else
      {
        var other = obj as Entity<TIdentity>;
        output = ((object) other != null)? this.Equals(other) : false;
      }

      return output;
    }
    
    /// <summary>
    /// Determines whether the specified <see cref="T:CSF.Entities.Entity{TIdentity}"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </summary>
    /// <param name="other">The <see cref="T:CSF.Entities.Entity{TIdentity}"/> to compare with the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="T:CSF.Entities.Entity{TIdentity}"/> is equal to the current
    /// <see cref="T:CSF.Entities.Entity{TIdentity}"/>; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool Equals(Entity<TIdentity> other)
    {
      bool output;

      if(Object.ReferenceEquals(this, other))
      {
        output = true;
      }
      else if((object) other != null)
      {
        output = (this.HasIdentity
                  && other.HasIdentity
                  && this.GetRawIdentity().Equals(other.GetRawIdentity()));
      }
      else
      {
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="T:CSF.Entities.Entity{TIdentity}"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
    /// hash table.
    /// </returns>
    public override int GetHashCode()
    {
      if(!_cachedHashCode.HasValue)
      {
        if(this.HasIdentity)
        {
          _cachedHashCode = _identity.GetHashCode();
        }
        else
        {
          _cachedHashCode = base.GetHashCode();
        }
      }

      return _cachedHashCode.Value;
    }
    
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="T:CSF.Entities.Entity{TIdentity}"/>.
    /// </returns>
    public override string ToString()
    {
      string output;
      
      if(this.HasIdentity)
      {
        output = this.GetRawIdentity().ToString();
      }
      else
      {
        output = String.Format("[{0}#(no identity)]", this.GetType().FullName);
      }
      
      return output;
    }

    #endregion

    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Entities.Entity{TIdentity}"/> class.
    /// </summary>
    public Entity()
    {
      _identity = null;
      _cachedHashCode = null;
    }
    
    #endregion
    
    #region static operator overloads

    /// <summary>
    /// Operator overload for testing equality between entity instances.
    /// </summary>
    /// <param name="objectA">An entity instance.</param>
    /// <param name="objectB">An entity instance.</param>
    public static bool operator ==(Entity<TIdentity> objectA, Entity<TIdentity> objectB)
    {
      bool output;

      if(Object.ReferenceEquals(objectA, objectB))
      {
        output = true;
      }
      else if((object) objectA != null)
      {
        output = (objectA.HasIdentity
                  && objectB.HasIdentity
                  && objectA.GetRawIdentity().Equals(objectB.GetRawIdentity()));
      }
      else
      {
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Operator overload for testing inequality between entity instances.
    /// </summary>
    /// <param name="objectA">An entity instance.</param>
    /// <param name="objectB">An entity instance.</param>
    public static bool operator !=(Entity<TIdentity> objectA, Entity<TIdentity> objectB)
    {
      return !(objectA == objectB);
    }

    /// <summary>
    /// Operator overload for testing equality between entity instances.
    /// </summary>
    /// <param name="objectA">An entity instance.</param>
    /// <param name="objectB">An entity instance.</param>
    public static bool operator ==(Entity<TIdentity> objectA, IEntity objectB)
    {
      bool output;

      if(Object.ReferenceEquals(objectA, objectB))
      {
        output = true;
      }
      else if((object) objectA != null)
      {
        output = (objectA.HasIdentity
                  && objectB.HasIdentity
                  && objectA.GetRawIdentity().Equals(objectB.GetRawIdentity()));
      }
      else
      {
        output = false;
      }

      return output;
    }

    /// <summary>
    /// Operator overload for testing inequality between entity instances.
    /// </summary>
    /// <param name="objectA">An entity instance.</param>
    /// <param name="objectB">An entity instance.</param>
    public static bool operator !=(Entity<TIdentity> objectA, IEntity objectB)
    {
      return !(objectA == objectB);
    }

    /// <summary>
    /// Operator overload for testing equality between entity instances.
    /// </summary>
    /// <param name="objectA">An entity instance.</param>
    /// <param name="objectB">An entity instance.</param>
    public static bool operator ==(IEntity objectA, Entity<TIdentity> objectB)
    {
      return objectB == objectA;
    }

    /// <summary>
    /// Operator overload for testing inequality between entity instances.
    /// </summary>
    /// <param name="objectA">An entity instance.</param>
    /// <param name="objectB">An entity instance.</param>
    public static bool operator !=(IEntity objectA, Entity<TIdentity> objectB)
    {
      return !(objectB == objectA);
    }

    #endregion
  }
}


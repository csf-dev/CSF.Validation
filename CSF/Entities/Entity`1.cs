//
// Entity`2.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


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
  public class Entity<TIdentity> : IEntity, IEquatable<IEntity>
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
        var other = obj as IEntity;
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
      else if((object) obj == null)
      {
        output = false;
      }
      else if(!this.HasIdentity
              || !obj.HasIdentity)
      {
        output = false;
      }
      else
      {
        IIdentity
          myIdentity = this.GetRawIdentity(),
          theirIdentity = obj.GetRawIdentity();

        output = myIdentity.Equals(theirIdentity);
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

    /// <summary>
    /// Gets the raw identity instance contained within the current entity, favour the extension method 'GetIdentity'
    /// instead.
    /// </summary>
    /// <returns>The identity value.</returns>
    protected virtual IIdentity GetRawIdentity()
    {
      if(!this.HasIdentity)
      {
        throw new InvalidOperationException("The current instance must have an identity, check using HasIdentity().");
      }

      return _identity;
    }

    #endregion

    #region explicit interface implementations

    IIdentity IEntity.GetRawIdentity()
    {
      return this.GetRawIdentity();
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

      if((object) objectA != null)
      {
        output = objectA.Equals(objectB);
      }
      else
      {
        output = Object.ReferenceEquals(objectA, objectB);
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

      if((object) objectA != null)
      {
        output = objectA.Equals(objectB);
      }
      else
      {
        output = Object.ReferenceEquals(objectA, objectB);
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


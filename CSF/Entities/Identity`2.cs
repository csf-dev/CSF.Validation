//  
//  Identity`T.cs
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

namespace CSF.Entities
{
  /// <summary>
  /// <para>
  /// Describes an immutable identity for an <see cref="IEntity"/>.  This serves as a globally unique identifier for
  /// that entity within the domain.
  /// </para>
  /// </summary>
  /// <typeparam name="T">
  /// The underlying type of the <see cref="Value"/> that sets an entity aside from all other entities of the same
  /// <see cref="Type"/>
  /// </typeparam>
  [Serializable]
  public struct Identity<TEntity,TIdentifier>
    : IIdentity<TEntity>,
      IEquatable<IIdentity<TEntity>>,
      IEquatable<Identity<TEntity,TIdentifier>>
    where TEntity : IEntity
  {
    #region fields
    
    private TIdentifier _value;
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para>Read-only.  A <see cref="System.Type"/> that is associated with this identity instance.</para>
    /// </summary>
    public Type EntityType
    {
      get {
        return typeof(TEntity);
      }
    }

    /// <summary>
    /// <para>Read-only.  Gets the type of the identifier <see cref="Value"/>.</para>
    /// </summary>
    public Type IdentifierType
    {
      get {
        return typeof(TIdentifier);
      }
    }
    
    /// <summary>
    /// <para>
    /// Read-only.  An <see cref="System.Object"/> of the appropriate generic type that uniquely identifies this entity
    /// amongst all other entities of the same <see cref="Type"/>.
    /// </para>
    /// </summary>
    public TIdentifier Value
    {
      get {
        return _value;
      }
    }

    object IIdentity<TEntity>.Value
    {
      get {
        return this.Value;
      }
    }
    
    #endregion

    #region methods

    /// <summary>
    /// <para>Overridden, overloaded.  Determines equality with the given <see cref="System.Object"/>.</para>
    /// </summary>
    /// <param name="obj">
    /// A <see cref="System.Object"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public override bool Equals (object obj)
    {
      bool output;
      
      if(obj is Identity<TEntity,TIdentifier>)
      {
        output = this.Equals((Identity<TEntity,TIdentifier>) obj);
      }
      else
      {
        output = false;
      }
      
      return output;
    }

    /// <summary>
    /// Determines whether the specified identity is equal to the current instance.
    /// </summary>
    /// <param name='other'>
    /// The identity to compare with the current instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified identity is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals (IIdentity<TEntity> other)
    {
      bool output;
      
      if(other is Identity<TEntity,TIdentifier>)
      {
        output = this.Equals((Identity<TEntity,TIdentifier>) other);
      }
      else
      {
        output = false;
      }
      
      return output;
    }

    /// <summary>
    /// Determines whether the specified identity is equal to the current instance.
    /// </summary>
    /// <param name='other'>
    /// The identity to compare with the current instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified identity is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals (Identity<TEntity,TIdentifier> other)
    {
      return this.Value.Equals(other.Value);
    }

    /// <summary>
    /// <para>Overridden.  Computes a hash code for this identity instance.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/>
    /// </returns>
    public override int GetHashCode ()
    {
      int typeHash, identityHash;
      
      typeHash = this.EntityType.GetHashCode();
      identityHash = this.Value.GetHashCode();
      
      return (typeHash ^ identityHash);
    }
    
    /// <summary>
    /// <para>Overridden, gets a <see cref="System.String"/> representation of this identity.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    public override string ToString ()
    {
      return string.Format ("[{0}: {1}]", this.EntityType.FullName, this.Value.ToString());
    }

    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Constructs a new identity instance.</para>
    /// </summary>
    /// <param name="identity">
    /// An identifir of the appropriate generic type
    /// </param>
    public Identity(TIdentifier identity)
    {
      _value = identity;
    }
    
    #endregion
    
    #region operator overloads

    /// <summary>
    /// Operator overload for testing equality between identity instances.
    /// </summary>
    /// <param name="objectA">
    /// An identity instance.
    /// </param>
    /// <param name="objectB">
    /// An identity instance.
    /// </param>
    public static bool operator ==(Identity<TEntity,TIdentifier> objectA, Identity<TEntity,TIdentifier> objectB)
    {
      return (objectA.Equals(objectB));
    }

    /// <summary>
    /// Operator overload for testing inequality between identity instances.
    /// </summary>
    /// <param name="objectA">
    /// An identity instance.
    /// </param>
    /// <param name="objectB">
    /// An identity instance.
    /// </param>
    public static bool operator !=(Identity<TEntity,TIdentifier> objectA, Identity<TEntity,TIdentifier> objectB)
    {
      return !(objectA == objectB);
    }

    /// <summary>
    /// Operator overload for testing equality between identity instances.
    /// </summary>
    /// <param name="objectA">
    /// An identity instance.
    /// </param>
    /// <param name="objectB">
    /// An identity instance.
    /// </param>
    public static bool operator ==(Identity<TEntity,TIdentifier> objectA, IIdentity<TEntity> objectB)
    {
      return (objectA.Equals(objectB));
    }

    /// <summary>
    /// Operator overload for testing inequality between identity instances.
    /// </summary>
    /// <param name="objectA">
    /// An identity instance.
    /// </param>
    /// <param name="objectB">
    /// An identity instance.
    /// </param>
    public static bool operator !=(Identity<TEntity,TIdentifier> objectA, IIdentity<TEntity> objectB)
    {
      return !(objectA == objectB);
    }

    /// <summary>
    /// Operator overload for testing equality between identity instances.
    /// </summary>
    /// <param name="objectA">
    /// An identity instance.
    /// </param>
    /// <param name="objectB">
    /// An identity instance.
    /// </param>
    public static bool operator ==(IIdentity<TEntity> objectA, Identity<TEntity,TIdentifier> objectB)
    {
      return (objectB == objectA);
    }

    /// <summary>
    /// Operator overload for testing inequality between identity instances.
    /// </summary>
    /// <param name="objectA">
    /// An identity instance.
    /// </param>
    /// <param name="objectB">
    /// An identity instance.
    /// </param>
    public static bool operator !=(IIdentity<TEntity> objectA, Identity<TEntity,TIdentifier> objectB)
    {
      return !(objectA == objectB);
    }
    
    #endregion
  }
}


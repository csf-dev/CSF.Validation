//
// Identity`2.cs
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

namespace CSF.Entities
{
  /// <summary>
  /// Describes an immutable identity for an <see cref="T:Entity{TIdentity}"/>.  This serves as a unique identifier for
  /// that entity instance within the object model.
  /// </summary>
  /// <typeparam name="TIdentity">The underlying type of the identity <see cref="Value"/>.</typeparam>
  /// <typeparam name="TEntity">The type of <see cref="T:Entity{TIdentity}"/> that this object represents.</typeparam>
  [Serializable]
  public class Identity<TIdentity,TEntity> :  IIdentity<TEntity>,
                                              IEquatable<IIdentity<TEntity>>,
                                              IEquatable<Identity<TIdentity,TEntity>>
                                              where TEntity : IEntity
  {
    #region fields
    
    private TIdentity _value;
    
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
    /// Gets the underlying type of <see cref="Value"/>.
    /// </summary>
    /// <value>The identity type.</value>
    public Type IdentityType
    {
      get {
        return typeof(IIdentity);
      }
    }

    /// <summary>
    /// <para>
    /// Read-only.  An <see cref="System.Object"/> of the appropriate generic type that uniquely identifies this entity
    /// amongst all other entities of the same <see cref="Type"/>.
    /// </para>
    /// </summary>
    public TIdentity Value
    {
      get {
        return _value;
      }
    }

    object IIdentity.Value
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

      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else
      {
        Identity<TIdentity,TEntity> other = obj as Identity<TIdentity,TEntity>;
        output = ((object) other != null)? this.Equals(other) : false;
      }

      return output;
    }

    /// <summary>
    /// Determines whether the specified identity is equal to the current instance.
    /// </summary>
    /// <param name='obj'>The identity to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the specified identity is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals (IIdentity obj)
    {
      bool output;

      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else
      {
        Identity<TIdentity,TEntity> other = obj as Identity<TIdentity,TEntity>;
        output = ((object) other != null)? this.Equals(other) : false;
      }

      return output;
    }

    /// <summary>
    /// Determines whether the specified identity is equal to the current instance.
    /// </summary>
    /// <param name='obj'>The identity to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the specified identity is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals (IIdentity<TEntity> obj)
    {
      bool output;

      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else
      {
        Identity<TIdentity,TEntity> other = obj as Identity<TIdentity,TEntity>;
        output = ((object) other != null)? this.Equals(other) : false;
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
    public bool Equals (Identity<TIdentity,TEntity> other)
    {
      bool output;

      if(Object.ReferenceEquals(this, other))
      {
        output = true;
      }
      else
      {
        output = ((object) other != null)? this.Value.Equals(other.Value) : false;
      }

      return output;
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
      return string.Format ("[{0}#{1}]", this.EntityType.FullName, this.Value.ToString());
    }

    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Entities.Identity{TIdentity,TEntity}"/> class.
    /// </summary>
    /// <param name="identity">The identity value.</param>
    public Identity(TIdentity identity)
    {
      _value = identity;
    }
    
    #endregion
    
    #region operator overloads

    /// <summary>
    /// Operator overload for testing equality between identity instances.
    /// </summary>
    /// <param name="objectA">An identity instance.</param>
    /// <param name="objectB">An identity instance.</param>
    public static bool operator ==(Identity<TIdentity,TEntity> objectA, Identity<TIdentity,TEntity> objectB)
    {
      return ((object) objectA != null || (object) objectB == null)
             && objectA.Equals(objectB);
    }

    /// <summary>
    /// Operator overload for testing inequality between identity instances.
    /// </summary>
    /// <param name="objectA">An identity instance.</param>
    /// <param name="objectB">An identity instance.</param>
    public static bool operator !=(Identity<TIdentity,TEntity> objectA, Identity<TIdentity,TEntity> objectB)
    {
      return !(objectA == objectB);
    }

    /// <summary>
    /// Operator overload for testing equality between identity instances.
    /// </summary>
    /// <param name="objectA">An identity instance.</param>
    /// <param name="objectB">An identity instance.</param>
    public static bool operator ==(Identity<TIdentity,TEntity> objectA, IIdentity<TEntity> objectB)
    {
      return ((object) objectA != null || (object) objectB == null)
             && objectA.Equals(objectB);
    }

    /// <summary>
    /// Operator overload for testing inequality between identity instances.
    /// </summary>
    /// <param name="objectA">An identity instance.</param>
    /// <param name="objectB">An identity instance.</param>
    public static bool operator !=(Identity<TIdentity,TEntity> objectA, IIdentity<TEntity> objectB)
    {
      return !(objectA == objectB);
    }

    /// <summary>
    /// Operator overload for testing equality between identity instances.
    /// </summary>
    /// <param name="objectA">An identity instance.</param>
    /// <param name="objectB">An identity instance.</param>
    public static bool operator ==(IIdentity<TEntity> objectA, Identity<TIdentity,TEntity> objectB)
    {
      return objectB == objectA;
    }

    /// <summary>
    /// Operator overload for testing inequality between identity instances.
    /// </summary>
    /// <param name="objectA">An identity instance.</param>
    /// <param name="objectB">An identity instance.</param>
    public static bool operator !=(IIdentity<TEntity> objectA, Identity<TIdentity,TEntity> objectB)
    {
      return !(objectA == objectB);
    }
    
    #endregion
  }
}


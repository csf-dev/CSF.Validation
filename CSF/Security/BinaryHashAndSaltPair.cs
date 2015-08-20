//
// BinaryHashAndSaltPair.cs
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

namespace CSF.Security
{
  /// <summary>
  /// Immutable type represents a password hash and salt pair based on binary data.
  /// </summary>
  public class BinaryHashAndSaltPair : IHashAndSaltPair
  {
    #region fields
    
    private byte[] _hash, _salt;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets the password hash.
    /// </summary>
    /// <value>
    /// The password hash.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public byte[] Hash
    {
      get {
        return _hash;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _hash = value;
      }
    }

    /// <summary>
    /// Gets the password hash.
    /// </summary>
    /// <value>
    /// The password hash.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public byte[] Salt
    {
      get {
        return _salt;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _salt = value;
      }
    }
    
    #endregion
    
    #region IHashAndSaltPair implementation
    
    string IHashAndSaltPair.GetHash()
    {
      return Convert.ToBase64String(this.Hash);
    }
    
    string IHashAndSaltPair.GetSalt()
    {
      return Convert.ToBase64String(this.Salt);
    }
    
    #endregion
    
    #region constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.BinaryHashAndSaltPair"/> class.
    /// </summary>
    /// <param name='hash'>
    /// The hash.
    /// </param>
    /// <param name='salt'>
    /// The salt.
    /// </param>
    public BinaryHashAndSaltPair(byte[] hash, byte[] salt)
    {
      this.Hash = hash;
      this.Salt = salt;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Security.BinaryHashAndSaltPair"/> class.
    /// </summary>
    /// <param name='base64Hash'>
    /// The hash, encoded as a Base64 string.
    /// </param>
    /// <param name='base64Salt'>
    /// The salt, encoded as a Base64 string.
    /// </param>
    public BinaryHashAndSaltPair(string base64Hash, string base64Salt)
    {
      this.Hash = Convert.FromBase64String(base64Hash);
      this.Salt = Convert.FromBase64String(base64Salt);
    }
    
    #endregion
  }
}


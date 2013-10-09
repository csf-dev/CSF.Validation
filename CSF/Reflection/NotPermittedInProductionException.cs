//
//  NotPermittedInProductionException.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2013 Craig Fowler
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
using System.Runtime.Serialization;


namespace CSF.Reflection
{
  /// <summary>
  /// Exception type thrown when functionality that is marked as forbidden in production (non-debug) builds is used.
  /// </summary>
  [Serializable]
  public class NotPermittedInProductionException : Exception
  {
    #region constants

    private const string
      DEFAULT_MESSAGE_NO_TYPE           = "Functionality that is marked forbidden in production builds must not be " +
                                          "used.",
      DEFAULT_MESSAGE_WITH_TYPE         = "Functionality that is marked forbidden in production builds must not be " +
                                          "used.  See the 'Type' property for the type that is forbidden.",
      TYPE_KEY                          = "Forbidden type";

    #endregion

    #region properties

    /// <summary>
    /// Gets the type that is marked as forbidden.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public Type Type
    {
      get {
        return this.Data[TYPE_KEY] as Type;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    public NotPermittedInProductionException() : this(DEFAULT_MESSAGE_NO_TYPE, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    public NotPermittedInProductionException(string message) : this(message, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public NotPermittedInProductionException(Exception inner) : this(DEFAULT_MESSAGE_NO_TYPE, inner) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public NotPermittedInProductionException(string message, Exception inner) : base(message, inner) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='type'>
    /// The type that contained the forbidden functionality.
    /// </param>
    public NotPermittedInProductionException(Type type) : this(DEFAULT_MESSAGE_WITH_TYPE, null)
    {
      if(type == null)
      {
        throw new ArgumentNullException("type");
      }

      this.Data[TYPE_KEY] = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Reflection.NotPermittedInProductionException"/> class.
    /// </summary>
    /// <param name='info'>
    /// Serialization info.
    /// </param>
    /// <param name='cx'>
    /// Streaming context.
    /// </param>
    public NotPermittedInProductionException(SerializationInfo info, StreamingContext cx) : base(info, cx) {}

    #endregion
  }
}


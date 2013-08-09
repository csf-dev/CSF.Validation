//
//  IdentityParser2.cs
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

namespace CSF.Entities
{
  /// <summary>
  /// Implementation of an identity parser that parses identity instances for a specific entity type and identity-type.
  /// </summary>
  public class IdentityParser<TEntity,TIdentity> : IIdentityParser<TEntity> where TEntity : IEntity
  {
    #region IIdentityParser implementation

    /// <summary>
    ///  Parse the specified input as an identity instance. 
    /// </summary>
    /// <param name='input'>
    ///  The identity value to parse. 
    /// </param>
    public IIdentity<TEntity> Parse (object input)
    {
      TIdentity parsedIdentifier = (TIdentity) Convert.ChangeType(input, typeof(TIdentity));
      return new Identity<TEntity,TIdentity>(parsedIdentifier);
    }

    /// <summary>
    /// Attempts to parse the specified input as an identity instance.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the parsing succeeded; <c>false</c> otherwise.
    /// </returns>
    /// <param name='input'>
    /// The identity value to parse.
    /// </param>
    /// <param name='output'>
    /// The parsed identity.
    /// </param>
    public bool TryParse (object input, out IIdentity<TEntity> output)
    {
      TIdentity parsedIdentifier = default(TIdentity);
      bool result = false;

      output = default(IIdentity<TEntity>);

      try
      {
        parsedIdentifier = (TIdentity) Convert.ChangeType(input, typeof(TIdentity));
        result = true;
      }
      catch(Exception) {}

      if(result)
      {
        output = new Identity<TEntity,TIdentity>(parsedIdentifier);
      }

      return result;
    }

    #endregion
  }
}


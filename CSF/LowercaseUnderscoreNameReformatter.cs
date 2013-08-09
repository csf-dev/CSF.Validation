//
//  LowercaseUnderscoreNameReformatter.cs
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
using System.Text.RegularExpressions;

namespace CSF
{
  /// <summary>
  /// Implementation of <see cref="NameReformatter"/> that transforms <c>PascalCase</c> to
  /// <c>lowercase_underscore_separated</c>.
  /// </summary>
  public class LowercaseUnderscoreNameReformatter : NameReformatter
  {
    #region constants

    private static readonly Regex
      UppercaseCharacters                         = new Regex("([A-Z])", RegexOptions.Compiled),
      FirstUppercaseCharacter                     = new Regex("^([A-Z])", RegexOptions.Compiled);

    #endregion

    #region methods

    /// <summary>
    ///  Formats the specified name using the rules defined by the current implementation. 
    /// </summary>
    /// <param name='objectName'>
    ///  The name to re-format. 
    /// </param>
    public override string Format (string objectName)
    {
      string output = objectName;

      // Assuming the name starts with an uppercase character (it should) then just lowercase it
      output = FirstUppercaseCharacter.Replace(output, x => x.Value.ToLowerInvariant());

      // All other uppercase characters are prefixed with an underscore and then lowercase'd
      output = UppercaseCharacters.Replace(output, x => String.Format("_{0}", x.Value.ToLowerInvariant()));

      return output;
    }

    #endregion
  }
}


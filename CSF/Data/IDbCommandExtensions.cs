//
//  IDbCommandExtensions.cs
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
using System.Data;

namespace CSF.Data
{
  /// <summary>
  /// Extension methods for <c>IDbCommand</c> instances.
  /// </summary>
  public static class IDbCommandExtensions
  {
    /// <summary>
    /// Convenience extension method adds a named parameter to a <c>IDbCommand</c> instance.
    /// </summary>
    /// <param name='command'>
    /// The command to which the parameter is to be added.
    /// </param>
    /// <param name='name'>
    /// The name of the parameter.
    /// </param>
    /// <param name='value'>
    /// The parameter's value.
    /// </param>
    public static void AddParameter(this IDbCommand command, string name, object value)
    {
      if(command == null)
      {
        throw new ArgumentNullException("command");
      }

      var param = command.CreateParameter();
      param.ParameterName = name;
      param.Value = value;
      command.Parameters.Add(param);
    }
  }
}


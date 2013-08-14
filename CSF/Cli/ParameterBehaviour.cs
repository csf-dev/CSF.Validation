//  
//  ParameterBehaviour.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 Craig Fowler
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

namespace CSF.Cli
{
  /// <summary>
  /// Enumerates the possible behaviours for an <see cref="IParameter"/>.
  /// </summary>
  public enum ParameterBehaviour
  {
    /// <summary>
    /// Indicates that the parameter is a switch only and may not have an associated value.  It is either present or not
    /// present.
    /// </summary>
    Switch,
    
    /// <summary>
    /// Indicates that the parameter may have an optional value associated with it but that the value is not mandatory.
    /// </summary>
    ValueOptional,
    
    /// <summary>
    /// Indicates that the parameter has an associated value and that this value is mandatory.
    /// </summary>
    ValueRequired
  }
}


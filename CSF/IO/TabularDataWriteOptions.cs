//  
//  TabularDataWriteOptions.cs
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

namespace CSF.IO
{
  /// <summary>
  /// Enumerates the options available when writing tabular data.
  /// </summary>
  [Flags]
  public enum TabularDataWriteOptions
  {
    /// <summary>
    /// Indicates no special options selected.
    /// </summary>
    None                      = 0,
    
    /// <summary>
    /// Indicates that all values should be quoted, even if quotes are not neccesarily required.
    /// </summary>
    AlwaysQuote               = 1
  }
}


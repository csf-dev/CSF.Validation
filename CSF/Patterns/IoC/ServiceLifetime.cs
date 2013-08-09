//  
//  ServiceLifetime.cs
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

namespace CSF.Patterns.IoC
{
  /// <summary>
  /// Enumerates the possible lifetimes of a service.
  /// </summary>
  public enum ServiceLifetime
  {
    /// <summary>
    /// There is only a single instance of a service created for an entire application domain.
    /// </summary>
    Singleton                 = 1,
    
    /// <summary>
    /// An instance of the service is created per thread.
    /// </summary>
    PerThread,
    
    /// <summary>
    /// An instance of the service is created per <see cref="System.Web.HttpContext"/>.
    /// </summary>
    PerHttpContext,
    
    /// <summary>
    /// A new instance of the service is created every time that it is requested.
    /// </summary>
    Transient
  }
}


//
//  ServiceModel.cs
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

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Type representing a model of a 'null' service layer interation, with a container for a common model.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type does not really mdoel a service layer interaction at all.  There is no property for a request or a
  /// response.
  /// </para>
  /// </remarks>
  public class ServiceModel<TCommon> : IServiceModel<TCommon>
    where TCommon : class
  {
    #region fields

    private TCommon _common;

    #endregion

    #region properties

    /// <summary>
    /// Gets the common model component.
    /// </summary>
    /// <value>
    /// The common model.
    /// </value>
    public virtual TCommon Common
    {
      get {
        return _common;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of this model type.
    /// </summary>
    /// <param name='common'>
    /// A common model component.
    /// </param>
    public ServiceModel(TCommon common)
    {
      if(common == null)
      {
        throw new ArgumentNullException("common");
      }

      _common = common;
    }

    #endregion
  }
}


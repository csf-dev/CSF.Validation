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
  /// Type representing a model of a 'request only' service layer interation, with a container for a common model.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This type provides the 'common' model container as well as access to a request, but no response.
  /// </para>
  /// </remarks>
  public class ServiceModel<TCommon,TRequest> : ServiceModel<TCommon>
    where TCommon : class
    where TRequest : IRequest
  {
    #region fields

    private TRequest _request;

    #endregion

    #region properties

    /// <summary>
    /// Gets the service layer request.
    /// </summary>
    /// <value>
    /// The request.
    /// </value>
    public virtual TRequest Request
    {
      get {
        return _request;
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
    /// <param name='request'>
    /// The service layer request.
    /// </param>
    public ServiceModel(TCommon common, TRequest request) : base(common)
    {
      if(request == null)
      {
        throw new ArgumentNullException("request");
      }

      _request = request;
    }

    #endregion
  }
}


using System;

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Type representing a model of a typical service layer request/response, as well as a container for a 'common'
  /// model.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This should be the most frequently-used service layer model.  It describes a request, a response and a common
  /// model component.
  /// </para>
  /// </remarks>
  public class ServiceModel<TCommon,TRequest,TResponse> : ServiceModel<TCommon,TRequest>
    where TCommon : class
    where TRequest : IRequest<TResponse>
    where TResponse : Response
  {
    #region fields

    private TResponse _response;

    #endregion

    #region properties

    /// <summary>
    /// Gets the service layer response.
    /// </summary>
    /// <value>
    /// The response.
    /// </value>
    public virtual TResponse Response
    {
      get {
        return _response;
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
    /// <param name='response'>
    /// The service layer response.
    /// </param>
    public ServiceModel(TCommon common, TRequest request, TResponse response) : base(common, request)
    {
      if(response == null)
      {
        throw new ArgumentNullException("response");
      }

      _response = response;
    }

    #endregion
  }
}


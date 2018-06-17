using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoreTools.Http
{
  /// <summary>
  /// Represents an object that converts the 'access_token' query string parameter to an 'Authorization' header.
  /// This is required to provide a user with authorized access to a SignalR hub.
  /// </summary>
  public class QueryStringAuthHandler
  {
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryStringAuthHandler"/> class using the specified parameter.
    /// </summary>
    /// <param name="next">An object used to invoke the next middleware in the application's request pipeline</param>
    public QueryStringAuthHandler(RequestDelegate next)
    {
      _next = next;
    }

    /// <summary>
    /// If no 'Authorization' header is present, converts the incomming request's query string
    /// 'access_token' parameter to an Authorization header so the rest of the chain can authorize
    /// the request correctly.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task Invoke(HttpContext context)
    {
      if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
      {
        var qs = context.Request.QueryString;

        if (qs.HasValue)
        {
          var token = qs.Value
            .Split('&')
            .SingleOrDefault(x => x.Contains("access_token"))?
            .Split('=')
            .Last();

          if (!string.IsNullOrWhiteSpace(token))
          {
            context.Request.Headers.Add("Authorization", "Bearer " + token);
          }
        }
      }
      
      await _next(context);
    }
  }
}

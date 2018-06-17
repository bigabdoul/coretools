using CoreTools.Http;
using Microsoft.AspNetCore.Builder;

namespace CoreTools.Extensions
{
    /// <summary>
    /// Exposes extension methods for classes that implement <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="QueryStringAuthHandler"/> to the application's request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseQueryStringAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<QueryStringAuthHandler>();
        }
    }
}

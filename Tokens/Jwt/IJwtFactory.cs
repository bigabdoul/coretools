using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreTools.Tokens.Jwt
{
    /// <summary>
    /// Specifies the contract required for factory classes that generate JSON Web Tokens (JWT) and claims.
    /// </summary>
    public interface IJwtFactory
    {
        /// <summary>
        /// Asynchronously generate an encoded JSON Web Token (JWT).
        /// </summary>
        /// <param name="userName">The user name for whom to generate the token.</param>
        /// <param name="identity">The identity for whom to generate the token.</param>
        /// <returns></returns>
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);

        /// <summary>
        /// Generate an instance of the <see cref="ClaimsIdentity"/> class using the specified user name and identifier.
        /// </summary>
        /// <param name="userName">The user name for whom to generate the claims.</param>
        /// <param name="id">The user identifier for whom to generate the claims.</param>
        /// <returns></returns>
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
    }
}

using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreTools.Tokens.Jwt
{
    /// <summary>
    /// Provides JWT extension methods.
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// Generates a JSON Web Token using the specified parameters asynchronously.
        /// </summary>
        /// <param name="identity">The identity to encode.</param>
        /// <param name="userId">The user identifier for the token.</param>
        /// <param name="userName">The user name for the token.</param>
        /// <param name="role">The user role for the token.</param>
        /// <param name="factory">An object that generates the encoded token.</param>
        /// <param name="options">An object that indicates validity of the token.</param>
        /// <returns></returns>
        public static async Task<IJwtToken> GenerateJwtAsync(this ClaimsIdentity identity, string userId, string userName, string role, IJwtFactory factory, JwtIssuerOptions options)
        {
            return new JwtToken
            {
                Id = userId,
                AuthToken = await factory.GenerateEncodedToken(userName, identity),
                ExpiresIn = (int)options.ValidFor.TotalSeconds,
                Role = role,
            };
        }
    }
}

using Newtonsoft.Json;

namespace CoreTools.Tokens.Jwt
{
    /// <summary>
    /// Represents a serializable JSON Web Token.
    /// </summary>
    public class JwtToken : IJwtToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtToken"/> class.
        /// </summary>
        public JwtToken()
        {
        }

        /// <summary>
        /// Gets or sets the token's user identifier.
        /// </summary>
        [JsonProperty("id")]
        public virtual string Id { get; set; }

        /// <summary>
        /// Gets or sets the token's user role.
        /// </summary>
        [JsonProperty("role")]
        public virtual string Role { get; set; }

        /// <summary>
        /// Gets or sets the encoded token.
        /// </summary>
        [JsonProperty("auth_token")]
        public virtual string AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the timespan (in seconds) the token will be valid for.
        /// </summary>
        [JsonProperty("expires_in")]
        public virtual int ExpiresIn { get; set; }

        /// <summary>
        /// Serializes and returns the current instance as a JSON-encoded string.
        /// </summary>
        /// <returns></returns>
        public virtual string Json() => JsonConvert.SerializeObject(this);
    }
}

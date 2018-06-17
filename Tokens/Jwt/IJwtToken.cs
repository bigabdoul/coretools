namespace CoreTools.Tokens.Jwt
{
    /// <summary>
    /// Specifies the contract required for a serializable JSON Web Token.
    /// </summary>
    public interface IJwtToken
    {
        /// <summary>
        /// The token's user identifier.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The token's user role.
        /// </summary>
        string Role { get; set; }

        /// <summary>
        /// The encoded authorization token.
        /// </summary>
        string AuthToken { get; set; }

        /// <summary>
        /// The timespan (in seconds) the token will be valid for.
        /// </summary>
        int ExpiresIn { get; set; }

        /// <summary>
        /// Serialize and return the token as a JSON-encoded string.
        /// </summary>
        /// <returns></returns>
        string Json();
    }
}

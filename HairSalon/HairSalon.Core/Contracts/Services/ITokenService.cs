using System.Security.Claims;

namespace HairSalon.Core.Contracts.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates an access token based on the provided claims.
        /// </summary>
        /// <param name="claims">The claims to include in the access token.</param>
        /// <returns>The generated access token.</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}

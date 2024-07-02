using SecurityService.Client;
using SecurityService.DataTransferObjects.Responses;
using Shared.Logger;

namespace EstateManagement.BusinessLogic.Common
{
    using Shared.General;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// Converts to datetime.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Guid guid)
        {
            var bytes = guid.ToByteArray();

            Array.Resize(ref bytes, 8);

            return new DateTime(BitConverter.ToInt64(bytes));
        }

        /// <summary>
        /// Converts to guid.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static Guid ToGuid(this DateTime dt)
        {
            var bytes = BitConverter.GetBytes(dt.Ticks);

            Array.Resize(ref bytes, 16);

            return new Guid(bytes);
        }

        public static Boolean NeedsRefreshed(this TokenResponse tokenResponse, Int32 minimumTimeLeft = 2)
        {
            return tokenResponse.Expires.UtcDateTime.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(2);
        }

        #endregion
    }

    public static class Helpers
    {
        [ExcludeFromCodeCoverage]
        public static async Task<TokenResponse> GetToken(TokenResponse currentToken,
            ISecurityServiceClient securityServiceClient, CancellationToken cancellationToken)
        {
            // Get a token to talk to the estate service
            String clientId = ConfigurationReader.GetValue("AppSettings", "ClientId");
            String clientSecret = ConfigurationReader.GetValue("AppSettings", "ClientSecret");
            Logger.LogDebug($"Client Id is {clientId}");
            Logger.LogDebug($"Client Secret is {clientSecret}");

            if (currentToken == null)
            {
                TokenResponse token = await securityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            if (currentToken.NeedsRefreshed())
            {
                Logger.LogInformation($"Token is about to expire at {currentToken.Expires.DateTime:O}");
                TokenResponse token = await securityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
                Logger.LogInformation($"Token is {token.AccessToken}");
                return token;
            }

            return currentToken;
        }
    }
}
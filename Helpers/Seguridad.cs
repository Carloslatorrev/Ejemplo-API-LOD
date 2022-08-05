using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;

namespace LODApi.Helpers
{
    public static class UrlToken
    {
        private const int BYTE_LENGTH = 32;

        /// <summary>
        /// Generate a fixed length token that can be used in url without endcoding it
        /// </summary>
        /// <returns></returns>
        public static string GenerateToken()
        {
            // get secure array bytes
            byte[] secureArray = GenerateRandomBytes();

            // convert in an url safe string
            string urlToken = WebEncoders.Base64UrlEncode(secureArray);

            return urlToken;
        }

        /// <summary>
        /// Generate a cryptographically secure array of bytes with a fixed length
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateRandomBytes()
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                byte[] byteArray = new byte[BYTE_LENGTH];
                provider.GetBytes(byteArray);

                return byteArray;
            }
        }
    }   
}
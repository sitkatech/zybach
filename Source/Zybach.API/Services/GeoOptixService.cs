using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Zybach.EFModels.Entities;

namespace Zybach.API.Services
{
    public class GeoOptixService
    {
        public static async Task<HttpResponseMessage> GetGeoOptixResponse(string url, string geoOptixUsername,  string geoOptixPassword, string authorityURL, string geoOptixClientID, string geoOptixClientSecret, ZybachDbContext _dbContext)
        {
            var currentToken = _dbContext.GeoOptixAccessToken.FirstOrDefault();
            if (currentToken == null || (currentToken.GeoOptixAccessTokenExpiryDate - DateTime.Now).Hours < 2)
            {
                var tokenResponse = await KeystoneService.GetKeystoneAuthorizationToken(geoOptixUsername,
                    geoOptixPassword, authorityURL,
                    geoOptixClientID, geoOptixClientSecret);
                
                var geoOptixTokenEntry = new GeoOptixAccessToken()
                {
                    GeoOptixAccessTokenValue = tokenResponse?.AccessToken,
                    GeoOptixAccessTokenExpiryDate = DateTime.Now.AddSeconds((int)tokenResponse?.ExpiresIn)
                };

                if (currentToken != null)
                {
                    _dbContext.GeoOptixAccessToken.Remove(currentToken);
                }
                _dbContext.GeoOptixAccessToken.Add(geoOptixTokenEntry);
                _dbContext.SaveChanges();
                currentToken = geoOptixTokenEntry;
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", currentToken.GeoOptixAccessTokenValue);
            return await client.GetAsync(url);

        }
    }
}
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Zybach.EFModels.Entities;

namespace Zybach.API.Services
{
    public class GeoOptixService
    {
        private readonly string geoOptixUsername;
        private readonly string geoOptixPassword;
        private readonly string authorityURL;
        private readonly string geoOptixClientID;
        private readonly string geoOptixClientSecret;
        private readonly string geoOptixHostName;
        private ZybachDbContext _dbContext;

        public GeoOptixService(ZybachConfiguration _zybachConfiguration, ZybachDbContext dbContext)
        {
            geoOptixUsername = _zybachConfiguration.GEOOPTIX_USERNAME;
            geoOptixPassword = _zybachConfiguration.GEOOPTIX_PASSWORD;
            authorityURL = _zybachConfiguration.KEYSTONE_AUTHORITY_URL;
            geoOptixClientID = _zybachConfiguration.GEOOPTIX_CLIENT_ID;
            geoOptixClientSecret = _zybachConfiguration.GEOOPTIX_CLIENT_SECRET;
            geoOptixHostName = _zybachConfiguration.GEOOPTIX_HOST_NAME;
            _dbContext = dbContext;
        }

        public async Task<HttpResponseMessage> GetWells()
        {
            var url = $"{geoOptixHostName}/project-overview-web/water-data-program/sites";
            return await GetGeoOptixResponse(url);
        }

        public async Task<HttpResponseMessage> GetWell(string siteCanonicalName)
        {
            var url = $"{geoOptixHostName}/project-overview-web/water-data-program/sites/{siteCanonicalName}";
            return await GetGeoOptixResponse(url);
        }

        public async Task<HttpResponseMessage> GetGeoOptixResponse(string url)
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

        public async Task<HttpResponseMessage> GetSensorsForWell(string siteCanonicalName)
        {
            var url = $"{geoOptixHostName}/project-overview-web/water-data-program/sites/{siteCanonicalName}/stations";
            return await GetGeoOptixResponse(url);
        }

        public async Task<HttpResponseMessage> GetSensorFolders(string sensorCanonicalName, string wellCanonicalName)
        {
            var url =
                $"{geoOptixHostName}/projects/water-data-program/sites/{wellCanonicalName}/stations/{sensorCanonicalName}/folders";
            return await GetGeoOptixResponse(url);
        }

        public async Task<HttpResponseMessage> GetTimeSeriesData(string folderCanonicalName, string sensorCanonicalName, string wellCanonicalName)
        {
            var url =
                $"{geoOptixHostName}/projects/water-data-program/sites/{wellCanonicalName}/stations/{sensorCanonicalName}/folders/{folderCanonicalName}/files/data.json/download";
            return await GetGeoOptixResponse(url);
        }
    }
}
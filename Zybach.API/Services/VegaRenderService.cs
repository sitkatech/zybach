using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Zybach.API.Services;

namespace VegaRenderService
{
    public class VegaRenderService
    {
        private static HttpClient _client { get; set; }
        private static ILogger<VegaRenderService> _logger { get; set; }

        public VegaRenderService(ILogger<VegaRenderService> logger, IOptions<ZybachConfiguration> zybachConfiguration)
        {
            _logger = logger;
            _client = new HttpClient()
            {
                BaseAddress = new Uri(zybachConfiguration.Value.VEGA_RENDER_URL)
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));

        }

        public async Task<byte[]> PrintPNG(string spec)
        {
            _logger.LogInformation($"Handling PrintPNG request with BaseAddress {_client.BaseAddress}");
            try
            {
                var response = await _client.PostAsJsonAsync("/", new { spec = spec });
                response.EnsureSuccessStatusCode();
                var png = response.Content.ReadAsByteArrayAsync();
                return await png;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Had an error using the vega render service {ex.Message}");
                throw;
            }

        }

    }
}
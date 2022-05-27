using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace VegaRenderService
{
    public class VegaRenderService
    {
        private static HttpClient _client { get; set; }
        private static ILogger _logger { get; set; }

        public VegaRenderService(string baseUri, ILogger logger)
        {
            _logger = logger;
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseUri)
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));
        }

        public async Task<byte[]> PrintPNG(string spec)
        {
            _logger.Information($"Handling PrintPNG request with BaseAddress {_client.BaseAddress}");
            var response = await _client.PostAsJsonAsync("/", new { spec = spec });
            var png = response.Content.ReadAsByteArrayAsync();
            return await png;
        }

    }
}
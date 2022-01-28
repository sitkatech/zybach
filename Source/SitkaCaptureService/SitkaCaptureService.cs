using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace SitkaCaptureService
{
    public class SitkaCaptureService
    {
        private static HttpClient _client { get; set; }
        private static ILogger _logger { get; set; }

        public SitkaCaptureService(string baseUri, ILogger logger)
        {
            _logger = logger;
            _client = new HttpClient()
            {
                BaseAddress = new Uri(baseUri)
            };
        }

        public async Task<byte[]> PrintPDF(CapturePostData postData)
        {
            _logger.Information($"Handling PrintPDF request with BaseAddress {_client.BaseAddress}");
            var response = await _client.PostAsJsonAsync("/pdf", postData);
            var pdf = response.Content.ReadAsByteArrayAsync();
            return await pdf;
        }

        public async Task<byte[]> PrintImage(CapturePostData postData)
        {
            _logger.Information($"Handling PrintImage request with BaseAddress {_client.BaseAddress}");
            var response = await _client.PostAsJsonAsync("/image", postData);
            var image = response.Content.ReadAsByteArrayAsync();
            return await image;
        }

    }
}
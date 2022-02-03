using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VegaRenderServiceCollectionExtensions
    {
        public static IServiceCollection AddVegaRenderService(this IServiceCollection services, string baseUri, ILogger logger)
        {
            services.AddTransient(s => new VegaRenderService.VegaRenderService(baseUri, logger));

            return services;
        }

    }
}

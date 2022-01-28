using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SitkaCaptureServiceCollectionExtensions
    {
        public static IServiceCollection AddSitkaCaptureService(this IServiceCollection services, string baseUri, ILogger logger)
        {
            services.AddTransient(s => new SitkaCaptureService.SitkaCaptureService(baseUri, logger));

            return services;
        }

    }
}

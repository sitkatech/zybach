using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using Hangfire;
using Hangfire.SqlServer;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Zybach.API.Services;
using Zybach.API.Services.Telemetry;
using Zybach.EFModels.Entities;
using SendGrid.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;
using Zybach.API.Services.Authorization;
using Zybach.API.Services.Notifications;
using ILogger = Serilog.ILogger;
using System.Collections.Generic;

namespace Zybach.API
{
    public class Startup
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly IWebHostEnvironment _environment;
        private string _instrumentationKey;
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            _environment = environment;

            _instrumentationKey = Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];

            if (!string.IsNullOrWhiteSpace(_instrumentationKey))
            {
                _telemetryClient = new TelemetryClient(TelemetryConfiguration.CreateDefault())
                {
                    InstrumentationKey = _instrumentationKey
                };
            }
            else
            {
                _telemetryClient = new TelemetryClient(TelemetryConfiguration.CreateDefault());
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(_instrumentationKey);
            services.AddControllers().AddNewtonsoftJson(opt =>
                {
                    if (!_environment.IsProduction())
                    {
                        opt.SerializerSettings.Formatting = Formatting.Indented;
                    }
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    var resolver = opt.SerializerSettings.ContractResolver;
                    if (resolver != null)
                    {
                        if (resolver is DefaultContractResolver defaultResolver)
                        {
                            defaultResolver.NamingStrategy = null;
                        }
                    }
                });

            services.Configure<ZybachConfiguration>(Configuration);
            var zybachConfiguration = Configuration.Get<ZybachConfiguration>();
            services.AddHttpClient<GeoOptixService>(c =>
            {
                c.BaseAddress = new Uri(zybachConfiguration.GEOOPTIX_HOSTNAME);
                c.Timeout = TimeSpan.FromMinutes(30);
                c.DefaultRequestHeaders.Add("x-geooptix-token", zybachConfiguration.GEOOPTIX_API_KEY);
            });

            services.AddHttpClient<GeoOptixSearchService>(c =>
            {
                c.BaseAddress = new Uri(zybachConfiguration.GEOOPTIX_SEARCH_HOSTNAME);
                c.Timeout = TimeSpan.FromMinutes(30);
                c.DefaultRequestHeaders.Add("x-geooptix-token", zybachConfiguration.GEOOPTIX_API_KEY);
            });

            services.AddHttpClient<AgHubService>(c =>
            {
                c.BaseAddress = new Uri(zybachConfiguration.AGHUB_API_BASE_URL);
                c.Timeout = TimeSpan.FromMinutes(30);
                c.DefaultRequestHeaders.Add("x-api-key", zybachConfiguration.AGHUB_API_KEY);
            });

            services.AddHttpClient<GETService>(c =>
            {
                c.BaseAddress = new Uri(zybachConfiguration.GET_API_BASE_URL);
                c.Timeout = TimeSpan.FromMinutes(30);
                c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", zybachConfiguration.GET_API_SUBSCRIPTION_KEY);
                //Allows us to follow a URL and get more information on why a request failed
                c.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");
            });
            services.AddHttpClient<OpenETService>(c =>
            {
                c.BaseAddress = new Uri(zybachConfiguration.OpenETAPIBaseUrl);
                c.Timeout = TimeSpan.FromMinutes(30);
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(zybachConfiguration.OPENET_API_KEY);
            });

            services.AddScoped<InfluxDBService>();
            services.AddScoped<WellService>();
            services.AddScoped<SupportTicketNotificationService>();

            var keystoneHost = zybachConfiguration.KEYSTONE_HOST;

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    if (_environment.IsDevelopment())
                    {
                        // NOTE: CG 3/22 - This allows the self-signed cert on Keystone to work locally.
                        options.BackchannelHttpHandler = new HttpClientHandler()
                        {
                            ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
                        };
                        //These allow the use of the container name and the url when developing.
                        options.TokenValidationParameters.ValidateIssuer = false;
                    }
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.Authority = keystoneHost;
                    options.RequireHttpsMetadata = false;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler
                    {
                        MapInboundClaims = false
                    });
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                });

            services.AddDbContext<ZybachDbContext>(c =>
            {
                c.UseSqlServer(zybachConfiguration.DB_CONNECTION_STRING, x =>
                {
                    x.CommandTimeout((int) TimeSpan.FromMinutes(3).TotalSeconds);
                    x.UseNetTopologySuite();
                });
            });

            services.AddSingleton(Configuration);
            services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();
            services.AddSingleton<ITelemetryInitializer, UserInfoTelemetryInitializer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var logger = GetSerilogLogger();
            services.AddSingleton(logger);

            services.AddSendGrid(options => { options.ApiKey = zybachConfiguration.SendGridApiKey; });

            services.AddTransient(s => new KeystoneService(s.GetService<IHttpContextAccessor>(), keystoneHost));

            services.AddSingleton<SitkaSmtpClientService>();

            services.AddHttpClient("OpenETClient", c =>
            {
                c.BaseAddress = new Uri(zybachConfiguration.OpenETAPIBaseUrl);
                c.Timeout = new TimeSpan(60 * TimeSpan.TicksPerSecond);
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(zybachConfiguration.OPENET_API_KEY);
            });

            services.AddHttpClient("GenericClient", c =>
            {
                c.Timeout = new TimeSpan(60 * TimeSpan.TicksPerSecond);
            });

            services.AddScoped(s => s.GetService<IHttpContextAccessor>().HttpContext);
            services.AddScoped(s => UserContext.GetUserFromHttpContext(s.GetService<ZybachDbContext>(), s.GetService<IHttpContextAccessor>().HttpContext));
            services.AddScoped<IOpenETTriggerBucketRefreshJob, OpenETTriggerBucketRefreshJob>();


            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(zybachConfiguration.DB_CONNECTION_STRING, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer(x =>
            {
                x.WorkerCount = 1;
            });

            services.AddControllers();

            services.AddVegaRenderService(zybachConfiguration.VEGA_RENDER_URL, logger);

            services.AddSwaggerGen(c =>
            {
                // extra options here if you wanted
            });

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddHealthChecks().AddDbContextCheck<ZybachDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory, ILogger logger)
        {
            loggerFactory.AddSerilog(logger);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(policy =>
            {
                //TODO: don't allow all origins
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(TelemetryHelper.PostBodyTelemetryMiddleware);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/");
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter(Configuration) }
            });

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            HangfireJobScheduler.ScheduleRecurringJobs();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            var modules = app.ApplicationServices.GetServices<ITelemetryModule>();
            var dependencyModule = modules.OfType<DependencyTrackingTelemetryModule>().FirstOrDefault();

            if (dependencyModule != null)
            {
                var domains = dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains;
                domains.Add("core.windows.net");
                domains.Add("10.0.75.1");
            }

            app.UseSwagger();

            app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
        }
        private void OnShutdown()
        {
            _telemetryClient.Flush();
            Thread.Sleep(1000);
        }

        private ILogger GetSerilogLogger()
        {
            var outputTemplate = $"[{_environment.EnvironmentName}] {{Timestamp:yyyy-MM-dd HH:mm:ss zzz}} {{Level}} | {{RequestId}}-{{SourceContext}}: {{Message}}{{NewLine}}{{Exception}}";
            var serilogLogger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console(outputTemplate: outputTemplate);

            if (!_environment.IsDevelopment())
            {
                serilogLogger.WriteTo.ApplicationInsights(_telemetryClient, new TraceTelemetryConverter());
            }

            return serilogLogger.CreateLogger();
        }
    }
}

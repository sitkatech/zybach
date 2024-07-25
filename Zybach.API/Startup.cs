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
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Zybach.API.Logging;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.API.Services.Notifications;
using Zybach.API.Services.Telemetry;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            services.AddScoped<IAzureStorage, AzureStorage>();
            services.AddScoped<BlobService>();

            services.AddHttpClient<PrismAPIService>(c =>
            {
                c.BaseAddress = new Uri(zybachConfiguration.PRISM_API_BASE_URL);
                c.Timeout = TimeSpan.FromMinutes(30);
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

            services.AddTransient<VegaRenderService.VegaRenderService>();

            services.AddSwaggerGen(c =>
            {
                // extra options here if you wanted
            });

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddHealthChecks().AddDbContextCheck<ZybachDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
                opts.GetLevel = LogHelper.CustomGetLevel;
            });
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
            app.UseMiddleware<LogHelper>();

            app.Use(TelemetryHelper.PostBodyTelemetryMiddleware);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz");
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorizationFilter(Configuration) }
            });

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            HangfireJobScheduler.ScheduleRecurringJobs();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            app.UseSwagger();

            app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
        }
        private void OnShutdown()
        {
            Thread.Sleep(1000);
        }
        
    }
}

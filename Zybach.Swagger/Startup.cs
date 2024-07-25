using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Zybach.EFModels.Entities;
using Zybach.Swagger.Logging;
using ILogger = Serilog.ILogger;

namespace Zybach.Swagger
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
            services.Configure<ZybachSwaggerConfiguration>(Configuration);
            var zybachConfiguration = Configuration.Get<ZybachSwaggerConfiguration>();

            services.AddDbContext<ZybachDbContext>(c =>
            {
                c.UseSqlServer(zybachConfiguration.DB_CONNECTION_STRING, x =>
                {
                    x.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
                    x.UseNetTopologySuite();
                });
            });

            services.AddSingleton(Configuration);
            services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<InfluxDBService>();
            services.AddScoped(s => s.GetService<IHttpContextAccessor>().HttpContext);
            services.AddControllers();


            #region Swagger
            // Base swagger services
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1.0",
                    Title = "Twin Platte Groundwater Managers Platform API",
                    Description = "Before you start using the Twin Platte Groundwater Managers Platform API, you will need to obtain an API key from the project team. TheTwin Platte Groundwater Managers Platform REST API provides resource-oriented urls to fetch data as JSON.",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Example Contact",
                    //    Url = new Uri("https://example.com/contact")
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Example License",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "ApiKey must appear in header",
                    Type = SecuritySchemeType.ApiKey,
                    Name = ApiKeyAttribute.ApiKeyName,
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                };
                options.AddSecurityRequirement(requirement);
            });
            #endregion

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

            app.UseMiddleware<LogHelper>();
            #region Swagger
            // Register swagger middleware and enable the swagger UI which will be 
            // accessible at https://<apihostname>/swagger
            // NOTE: There is no auth on these endpoints out of the box.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "V1");
                c.RoutePrefix = "docs";
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz");
            });

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

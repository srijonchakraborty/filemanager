using AutoMapper;
using FileManagerAPI.AutoMapper.Profiles;
using FileManagerAPI.Common;
using FileManagerAPI.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerAPI
{
    public class Startup
    {
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; private set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;
            HostingEnvironment = env;
            Configuration = configuration;
            _appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            _logger.LogDebug("Startup::Constructor::Settings loaded");
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FileManagerAPI", Version = "v1" });
            });


            _logger.LogTrace("Startup::ConfigureServices");

            try
            {
                if (_appSettings.IsValid())
                {
                    _logger.LogDebug("Startup::ConfigureServices::valid AppSettings");

                    #region App settings

                    services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

                    #endregion

                    #region Controllers

                    services.AddControllers(options =>
                    {
                        options.Filters.Add(new ProducesAttribute("application/json"));
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

                    #endregion

                    #region API versioning

                    services.AddApiVersioning(options =>
                    {
                        options.ReportApiVersions = true;
                        options.DefaultApiVersion = new ApiVersion(1, 0);
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.ApiVersionReader = new UrlSegmentApiVersionReader();
                    });

                    #endregion

                    #region CORS

                    string[] cors = _appSettings.CorsSetting.Split(',');
                    services.AddCors(options =>
                    {
                        options.DefaultPolicyName = "CORSPolicy";
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.WithOrigins(cors)
                                .SetIsOriginAllowedToAllowWildcardSubdomains()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
                    });
                    #endregion

                    #region Versioned Api Explorer

                    //Format the version as "'v'major[.minor][-status]"
                    services.AddVersionedApiExplorer(options =>
                    {
                        options.GroupNameFormat = "'v'VVV";
                        options.SubstituteApiVersionInUrl = true;
                    });

                    #endregion


                    #region Caching

                    services.AddDistributedMemoryCache();

                    #endregion

                    #region Session

                    services.AddSession(options =>
                    {
                        options.IdleTimeout = TimeSpan.FromMinutes(60);
                    });

                    #endregion

                    #region JWT Authentication

                    var key = Encoding.ASCII.GetBytes(_appSettings.CryptoKey);
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

                    #endregion

                    #region Antiforgery

                    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
                    services.AddAntiforgery(options =>
                    {
                        options.SuppressXFrameOptionsHeader = false;
                        options.HeaderName = "SJNF.FILE.X-XSRF-TOKEN";
                    });

                    #endregion

                    #region Auto Mapper Configurations

                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new APIMappingProfile());
                    });

                    IMapper mapper = mappingConfig.CreateMapper();
                    services.AddSingleton(mapper);

                    #endregion

                    #region SWAGGER


                    #endregion

                    #region Mappings

                    services.ConfigureMappings();

                    #endregion

                    #region Business Service  

                    services.ConfigureBusinessServices(Configuration);

                    #endregion

                    _logger.LogDebug("Startup::ConfigureServices::ApiVersioning, Swagger and DI settings");
                }
                else
                {
                    _logger.LogDebug("Startup::ConfigureServices::invalid AppSettings");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            _logger.LogTrace("Startup::Configure");
            _logger.LogDebug($"Startup::Configure::Environment:{env.EnvironmentName}");

            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler(a => a.Run(async context =>
                    {
                        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = feature.Error;
                        var code = HttpStatusCode.InternalServerError;

                        if (exception is ArgumentNullException)
                            code = HttpStatusCode.BadRequest;
                        else if (exception is ArgumentException)
                            code = HttpStatusCode.BadRequest;
                        else if (exception is UnauthorizedAccessException)
                            code = HttpStatusCode.Unauthorized;

                        _logger.LogError($"GLOBAL ERROR HANDLER::HTTP:{code}::{exception.Message}");

                        var result = JsonConvert.SerializeObject(exception, Formatting.Indented);

                        context.Response.Clear();
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(result);
                    }));

                    app.UseHsts();
                }

                app.UseCors("CORSPolicy");
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();

                #region Session

                app.UseSession();

                #endregion

                #region For Authentication

                app.UseAuthentication();
                app.UseAuthorization();

                #endregion

                #region X-Frame-Options

                //app.Use(async (context, next) =>
                //{
                //    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                //    await next();
                //});

                #endregion

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
                app.UseRequestLocalization();

                #region Swagger

                if (_appSettings.IsValid())
                {
                    if (_appSettings.Swagger.Enabled)
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI(options =>
                        {
                            foreach (var description in provider.ApiVersionDescriptions)
                            {

                                string sjp = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
                                options.SwaggerEndpoint($"{sjp}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                            }
                        });
                    }
                }

                #endregion

                app.Run(async (context) =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<!DOCTYPE html><html lang=\"en\"><head><title>SMC Tender API (.Net 5)</title></head><body><h3>SMC API is Running</h3>");
                    await context.Response.WriteAsync($"<p>Request Url: {context.Request.GetDisplayUrl()}<p>");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}

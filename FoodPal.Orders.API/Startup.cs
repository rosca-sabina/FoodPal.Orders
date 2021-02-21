using FluentValidation;
using FoodPal.Orders.API.Filters;
using FoodPal.Orders.API.Versioning;
using FoodPal.Orders.Data;
using FoodPal.Orders.Mappers;
using FoodPal.Orders.MessageBroker.Contracts;
using FoodPal.Orders.MessageBroker.ServiceBus;
using FoodPal.Orders.Services;
using FoodPal.Orders.Services.Contracts;
using FoodPal.Orders.Services.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace FoodPal.Orders.API
{
    public class Startup
    {
        private readonly int _majorVersion;
        private readonly string _majorVersionString;

        private const string ApiTitle = "FoodPal - Orders API";
        private const string ApiDescription = "FoodPal - Orders API microservice";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _majorVersion = VersioningInfo.MajorVersion;
            _majorVersionString = $"v{_majorVersion}";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddControllers();

            #region AutoMapper
            services.AddAutoMapper(typeof(AbstractProfile).Assembly);
            #endregion

            #region API Versioning
            // Register API versioning
            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.DefaultApiVersion = new ApiVersion(_majorVersion, 0);
            });

            // Register Version API explorer
            services.AddVersionedApiExplorer(options =>
            {
                options.DefaultApiVersion = new ApiVersion(_majorVersion, 0);

                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
            #endregion

            #region FluentValidation
            services.AddValidatorsFromAssembly(typeof(InternalValidator<>).Assembly);
            #endregion

            #region Services registration
            services.AddTransient<IExceptionFilter, ExceptionFilter>();

            services.Configure<MessageBrokerConnectionSettings>(x => Configuration.Bind("MessageBrokerSettings", x));

            services
                .AddTransient<IMessageBroker, ServiceBusMessageBroker>()
                .AddTransient<IOrderService, OrderService>()
                .AddTransient<IOrderItemsService, OrderItemService>()
                .AddTransient<IDeliveryDetailsService, DeliveryDetailsService>();

            var dbConnectionString = Configuration.GetConnectionString("OrdersConnectionString");

            services
                .AddTransient<IOrdersContextFactory, OrdersContextFactory>()
                .AddTransient<IOrdersUnitOfWork, OrdersUnitOfWork>(sp => new OrdersUnitOfWork(sp.GetService<IOrdersContextFactory>().CreateDbContext(dbConnectionString)));

            #endregion

            services.AddMvc(x =>
            {
                x.EnableEndpointRouting = false;
                x.Filters.Add(new ProducesAttribute("application/json"));
                x.Filters.AddService(typeof(IExceptionFilter));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_majorVersionString, new OpenApiInfo
                {
                    Version = _majorVersionString,
                    Title = ApiTitle,
                    Description = ApiDescription
                });

                // Set the comments path for the Swagger JSON and UI.
                /*c.IncludeXmlComments(GetXmlCommentsFilePath(), includeControllerXmlComments: true);
                c.IncludeXmlComments(GetDtosXmlCommentsFilePath());*/
            });
        }

        /// <summary>
        /// Docs path
        /// </summary>
        /// <returns></returns>
        protected static string GetXmlCommentsFilePath()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            return xmlPath;
        }

        /// <summary>
        /// DTOs doc path
        /// </summary>
        /// <returns></returns>
        protected static string GetDtosXmlCommentsFilePath()
        {
            var xmlFile = $"FoodPal.Orders.DTOs.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            return xmlPath;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Environment: {Environment}", env.EnvironmentName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodPal Orders API v1"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

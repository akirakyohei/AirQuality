using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AirQualityService.Setting;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using AirQualityService.Data.Interface;
using AirQualityService.Data;
using AutoMapper;
using AirQualityService.Helpers;
using Microsoft.OpenApi.Models;
using AirQualityService.Services;

namespace AirQualityService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            MQTTClientSettings mQTTClientSettings = new MQTTClientSettings();
            Configuration.GetSection(nameof(MQTTClientSettings)).Bind(mQTTClientSettings);
            MQTTClientSettings.instance = mQTTClientSettings;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMqttClientHostedService();
            services.AddSingleton<ExtarmalService>();
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AirQuality",
                    Description = "Team 21"
                });

            });

            services.AddCors();

            services.Configure<AirQualityDatabaseSettings>(
                Configuration.GetSection(nameof(AirQualityDatabaseSettings)));

            services.AddSingleton<IAirQualityDatabaseSettings>(opt =>
            opt.GetRequiredService<IOptions<AirQualityDatabaseSettings>>().Value);
            services.AddSingleton<ICityReporitory, CityRepository>();
            services.AddSingleton<IPointRepository, PointRepository>();
            services.AddSingleton<IAirQualityRepository, AirQualityRepository>();
            services.AddSingleton<AirQualityContext>();
            services.AddTransient<IInitiallizeData, InitiallizeData>();
            services.AddAutoMapper(config => { config.AddProfile<MapperProfile>(); }, AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(config => { config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Air Quality API Version1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

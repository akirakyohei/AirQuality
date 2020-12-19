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
using AirQualityService.Helpers.@interface;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using AirQualityService.ViewModels;
using AirQualityService.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.SpaServices.AngularCli;

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

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddMvc(opt => opt.EnableEndpointRouting = false);
            services.AddMqttClientHostedService();
            services.AddScoped<ExtarmalService>();
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

            //services.AddSpaStaticFiles(config =>
            //{
            //    config.RootPath = "AirApp";
            //});

            services.AddCors();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "jndjfhvdkf",
                    ValidAudience = "hfbvfhgvklnckjnrubsdx",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("indfivfglfivfusdfdg"))
                };
            });

            services.Configure<AirQualityDatabaseSettings>(
                Configuration.GetSection(nameof(AirQualityDatabaseSettings)));

            services.AddSingleton<IAirQualityDatabaseSettings>(opt =>
            opt.GetRequiredService<IOptions<AirQualityDatabaseSettings>>().Value);

            services.AddTransient<AirQualityContext>();
            services.AddSingleton<ICityReporitory, CityRepository>();
            services.AddSingleton<IAirQualityRepository, AirQualityRepository>();
            services.AddSingleton<IPointRepository, PointRepository>();
            services.AddSingleton<IReportAirQualityInDayRepository, ReportAirQualityRepository>();
            services.AddSingleton<IAQIHelper, AQIHelper>();
            services.AddTransient<IInitiallizeData, InitiallizeData>();

            //add quartz service
            services.AddSingleton<IJobFactory, AwakeUpDeviceJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<AwakeUpDeviceJob>();
            services.AddSingleton<ReportEndDayJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(AwakeUpDeviceJob),
                //cronExpression: "0 0 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23 ? * * *"
                //cronExpression: "0 0 * ? * * *"
                cronExpression: "0/20 * * ? * * *"
                ));

            services.AddSingleton(new JobSchedule(
                jobType: typeof(ReportEndDayJob),
                cronExpression: "0 30 0 ? * * *"
                //cronExpression: "0/5 * * * * ? *"
                ));
            services.AddHttpClient("ibm", config =>
            {

                config.DefaultRequestHeaders.Add("Content-Type", "application/json");
                config.DefaultRequestHeaders.Add("Authorization", "Basic YS1mNHZ0OTMtOWRlZHljbW1hazpUVTUpQVJaN3FmQ3dGQ00mKHg=");

            });
            services.AddScoped<IIBMClientHelper, IBMClientHelper>();
            services.AddHostedService<QuartzHostedService>();


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
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Air Quality API Version1");
            });

            //app.UseSpa(config =>
            //{
            //    config.Options.SourcePath = Path.Join(env.ContentRootPath, "./../AirApp");
            //    if (env.IsDevelopment())
            //    {
            //        config.UseAngularCliServer(npmScript: "start");
            //    }
            //});


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

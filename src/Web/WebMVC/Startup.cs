﻿using MassTransit;
using RabbitMQ.Client;
using WebMVC.Application.Services;
using WebMVC.Infrastructure;
using WebMVC.Infrastructure.Data.Repositories;

namespace WebMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionString"],
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }));
            
            services.AddScoped<IPlateRepository, PlateRepository>();
            services.AddScoped<IPlateService, PlateService>();
            
            services.AddControllers();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddMassTransit(x =>
            {
                //x.AddConsumer<ConsumerClass>();

                //ADD CONSUMERS HERE
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["EventBusConnection"], "/", h =>
                    {
                        if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                        {
                            h.Username(Configuration["EventBusUserName"]);
                        }

                        if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                        {
                            h.Password(Configuration["EventBusPassword"]);
                        }
                    });

                    cfg.ConfigureEndpoints(context);
                    cfg.ExchangeType = ExchangeType.Fanout;
                });
            });

            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            app.UseStaticFiles();
            app.UseForwardedHeaders();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}

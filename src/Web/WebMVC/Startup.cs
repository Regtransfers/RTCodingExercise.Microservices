using MassTransit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RabbitMQ.Client;
using WebMVC.Services;

namespace RTCodingExercise.WebMVC
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
            services.AddHttpClient("PlatesApi", client =>
            {
                client.BaseAddress = new Uri("http://catalog-api");
            });

            services.AddScoped<IPlatesService, PlatesService>();

            services.AddControllers();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSession();
            services.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();
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
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}

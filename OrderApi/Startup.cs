using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using zipkin4net.Transport.Http;
using ZipkinExtensions;

namespace OrderApi
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
            services.AddControllers();
            services.AddHttpClient(ServiceName.ProductService, client =>
            {
                client.BaseAddress = new Uri($"http://localhost:5002");
            })
            //.AddHttpMessageHandler<NacosDiscoveryDelegatingHandler>()
            .AddHttpMessageHandler(provider => TracingHandler.WithoutInnerHandler(ServiceName.OrderService));

        }

        /// <summary>
        /// autofac configure
        /// https://github.com/1100100/Dapper.Extensions
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddZipkin(ServiceName.OrderService);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //在管道中注册zipkin
            app.UseZipkin(lifetime, loggerFactory, ServiceName.OrderService, "http://localhost:9411/");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace ZipkinExtensions
{
    public static class EmptyClass
    {
        [System.Obsolete("改用AutofacExtensions.AddZipkin")]
        public static IServiceCollection AddZipkin(this IServiceCollection services)
        {
            return services.AddSingleton<TraceObserver>()
                           //.AddSingleton<ITraceDiagnosticListener, HttpDiagnosticListener>()//不支持构造函数传参
                           ;
        }

        public static IApplicationBuilder UseZipkin(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ILoggerFactory loggerFactory, string serviceName, string zipkinUrl)
        {
            System.Diagnostics.DiagnosticListener.AllListeners.Subscribe(app.ApplicationServices.GetService<TraceObserver>());
            lifetime.ApplicationStarted.Register(() =>
            {
                TraceManager.SamplingRate = 1.0f;//记录数据密度，1.0代表全部记录
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender(zipkinUrl, "application/json");

                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());
                var consoleTracer = new zipkin4net.Tracers.ConsoleTracer();


                TraceManager.RegisterTracer(tracer);
                TraceManager.RegisterTracer(consoleTracer);
                TraceManager.Start(logger);

            });
            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());
            app.UseTracing(serviceName);//这边的名字可自定义
            return app;
        }
    }
}

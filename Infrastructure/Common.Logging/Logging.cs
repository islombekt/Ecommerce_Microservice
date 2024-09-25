
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> cfg => (context, loggerConf) =>
        {
            var env = context.HostingEnvironment;
            loggerConf.MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Console();

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConf.MinimumLevel.Override("Catalog", Serilog.Events.LogEventLevel.Debug);
                loggerConf.MinimumLevel.Override("Basket", Serilog.Events.LogEventLevel.Debug);
                loggerConf.MinimumLevel.Override("Discount", Serilog.Events.LogEventLevel.Debug);
                loggerConf.MinimumLevel.Override("Ordering", Serilog.Events.LogEventLevel.Debug);
            }  
        };
    }
}

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

[assembly: FunctionsStartup(typeof(FunctionsOpenTelemetry.Startup))]
namespace FunctionsOpenTelemetry
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var resourceBuilder = ResourceBuilder.CreateDefault().AddService("example-azure-function").AddTelemetrySdk();

            var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetResourceBuilder(resourceBuilder)
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter()
                .Build();

            builder.Services.AddSingleton(tracerProvider);

            //// Enable Metrics with OpenTelemetry
            //var openTelemetryMeterProvider = Sdk.CreateMeterProviderBuilder()
            //    .SetResourceBuilder(openTelemetryResourceBuilder)
            //    .AddAspNetCoreInstrumentation()
            //    .AddConsoleExporter()
            //    .Build();

            //builder.Services.AddSingleton(openTelemetryMeterProvider);


            //// Enable Logging with OpenTelemetry
            //builder.Services.AddLogging((loggingBuilder) =>
            //{
            //    // Only Warning or above will be sent to Opentelemetry
            //    loggingBuilder.AddFilter<OpenTelemetryLoggerProvider>("*", LogLevel.Warning);
            //}
            //);

            //builder.Services.AddSingleton<ILoggerProvider, OpenTelemetryLoggerProvider>();
            //builder.Services.Configure<OpenTelemetryLoggerOptions>((openTelemetryLoggerOptions) =>
            //{
            //    openTelemetryLoggerOptions.SetResourceBuilder(openTelemetryResourceBuilder);
            //    openTelemetryLoggerOptions.IncludeFormattedMessage = true;
            //    openTelemetryLoggerOptions.AddConsoleExporter();
            //}
            //);
        }
    }
}

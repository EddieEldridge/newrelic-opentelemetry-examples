using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

// OTEL_EXPORTER_OTLP_ENDPOINT=http://staging-otlp.nr-data.net:4317
// OTEL_EXPORTER_OTLP_HEADERS=api-key=<license_key>
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService("example-azure-function-isolated", null, null, false)
    .AddTelemetrySdk();

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("MyActivitySource")
    .AddHttpClientInstrumentation()
    .AddConsoleExporter()
    .AddOtlpExporter(options =>
    {
        options.Endpoint = new Uri($"{Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT")}");
        options.Headers = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_HEADERS");
    })
    .Build();

host.Run();

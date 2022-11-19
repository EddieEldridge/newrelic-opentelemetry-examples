using System.Diagnostics;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class IsolatedFunction
    {
        private static ActivitySource ActivitySource = new ActivitySource("MyActivitySource");
        private readonly ILogger _logger;
        private static HttpClient HttpClient = new HttpClient();

        public IsolatedFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<IsolatedFunction>();
        }

        [Function("IsolatedFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext context)
        {
            using var activity = ActivitySource.StartActivity(context.FunctionDefinition.Name);

            activity?.SetTag("faas.trigger", "http");
            activity?.SetTag("faas.execution", context.InvocationId);

            await HttpClient.GetStringAsync("https://www.newrelic.com");

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}

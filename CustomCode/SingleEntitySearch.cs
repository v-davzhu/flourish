using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using PowerScript;

namespace ProjectFlourish.CustomConnectorCode
{
    // LOCAL TESTING - simulate triggering of custom connector operations (endpoint -> operation inside script)
    public static class SingleEntitySearch
    {
        public static Script script = new Script();

        [FunctionName("singleEntitySearch")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            script.Context = new ScriptContext();
            script.Context.OperationId = "SingleEntitySearch";
            script.Context.Request = req;

            return await script.ExecuteAsync();
        }
    }
}

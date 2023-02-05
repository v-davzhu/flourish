using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Net;
using System.Text;
using PowerScript;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ProjectFlourish.CustomConnectorCode
{
    // LOCAL TESTING - simulate triggering of custom connector operations (endpoint -> operation inside script)
    public static class SupportedEntities
    {

        public static Script script = new Script();

        [FunctionName("entities")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            script.Context = new ScriptContext();
            script.Context.OperationId = "GetEntities";
            script.Context.Request = req;

            return await script.ExecuteAsync();

        }
    }
}

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

namespace ProjectFlourish.CustomConnectorCode
{
    public static class SupportedEntities
    {
        [FunctionName("Entities")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            HttpRequestMessage httpRequest3S = new HttpRequestMessage(method: HttpMethod.Get, "https://projectflourish.azurewebsites.net/api/Entities3S");

            HttpClient httpClient = new HttpClient();
            var response3S = await httpClient.SendAsync(httpRequest3S);

            var entities3S = await response3S.Content.ReadAsStringAsync();

            var stringContent = new StringContent(entities3S, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = stringContent;

            return response;
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;

namespace PowerScript
{
    public abstract class ScriptBase
    {
        // Context object
        public IScriptContext Context { get; }

        // CancellationToken for the execution
        public CancellationToken CancellationToken { get; }

        // Helper: Creates a StringContent object from the serialized JSON
        public abstract StringContent CreateJsonContent(string serializedJson);

        // Abstract method for your code
        public abstract Task<HttpResponseMessage> ExecuteAsync();
    }

    public interface IScriptContext
    {
        // Correlation Id
        string CorrelationId { get; }

        // Connector Operation Id
        string OperationId { get; }

        // Incoming request
        HttpRequestMessage Request { get; }

        // Logger instance
        ILogger Logger { get; }

        // Used to send an HTTP request
        // Use this method to send requests instead of HttpClient.SendAsync
        Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken);
    }

    public class Script : ScriptBase
    {
        public override StringContent CreateJsonContent(string serializedJson)
        {
            throw new NotImplementedException();
        }

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {

            // we check action url
            if (this.Context.Request.RequestUri.ToString().Contains("/Entities"))
                return await SupportedEntities.Run(this.Context.Request, this.Context.Logger);

            // call into action-class
            return null;
        }

        public static class SupportedEntities
        {
            [FunctionName("Entities")]
            public static async Task<HttpResponseMessage> Run(HttpRequestMessage req,
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
}
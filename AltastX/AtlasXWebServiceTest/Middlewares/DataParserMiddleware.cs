using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace atlastX.Middlewares
{
    public class DataParserMiddleware
    {
        private const string JsonContentType = "application/json";
        private const string FormContentType = "application/x-www-form-urlencoded";

        private readonly IEnumerable<string> _paths;
        private readonly RequestDelegate _next;
        private readonly string _endpoint;

        public DataParserMiddleware(RequestDelegate next, string endpoint, IEnumerable<string> paths)
        {
            _next = next;
            _endpoint = endpoint;
            _paths = paths;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpClient client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });

            if (_paths.Any(p => context.Request.Path.Value.StartsWith(p, StringComparison.InvariantCultureIgnoreCase)))
            {
                HttpRequest request = context.Request;
                string url = $"{_endpoint}{request.Path}{request.QueryString}";
                HttpRequestMessage httpRequest = await CreateProxyHttpRequest(context.Request, new Uri(url));
                HttpResponseMessage response = await client.SendAsync(httpRequest);
                await CopyProxyHttpResponse(context, response);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task<HttpRequestMessage> CreateProxyHttpRequest(HttpRequest request, Uri uri)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = new HttpMethod(request.Method)
            };

            // Copy the request headers
            foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> header in request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && requestMessage.Content != null)
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
            requestMessage.Headers.Host = uri.Authority;


            string body = string.Empty;
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
            }

            bool isJson = request.ContentType != null && request.ContentType.Contains(JsonContentType, StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(body))
            {
                requestMessage.Content = new StringContent(body, Encoding.UTF8, isJson ? JsonContentType : FormContentType);
            }

            return requestMessage;
        }

        private async Task CopyProxyHttpResponse(HttpContext context, HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            HttpResponse response = context.Response;

            string sourceHost = context.Request.Host.Value;
            string destinationHost = responseMessage.RequestMessage.RequestUri.Authority;

            response.StatusCode = (int)responseMessage.StatusCode;
            foreach (KeyValuePair<string, IEnumerable<string>> header in responseMessage?.Headers)
            {
                if (header.Key == "Location")
                {
                    response.Headers[header.Key] = header.Value.Select(value => value.Replace(destinationHost, sourceHost)).ToArray();
                }
                else
                {
                    response.Headers[header.Key] = header.Value.ToArray();
                }

            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in responseMessage?.Content?.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            // Removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("transfer-encoding");

            using (Stream responseStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                await responseStream.CopyToAsync(response.Body, context.RequestAborted);
            }
        }
    }
}
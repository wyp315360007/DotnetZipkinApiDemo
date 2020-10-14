using Microsoft.Extensions.DiagnosticAdapter;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using zipkin4net;
using zipkin4net.Propagation;
using zipkin4net.Tracers.Zipkin.Thrift;

namespace ZipkinExtensions
{
    /// <summary>
    /// 增加记录日志内容
    /// </summary>
    public class HttpDiagnosticListener : ITraceDiagnosticListener
    {
        public string DiagnosticName => "HttpHandlerDiagnosticListener";

        private ClientTrace clientTrace;
        private readonly IInjector<HttpHeaders> _injector = Propagations.B3String.Injector<HttpHeaders>((carrier, key, value) => carrier.Add(key, value));
        private string _serviceName;
        public HttpDiagnosticListener(string serviceName)
        {
            this._serviceName = serviceName;
        }

        [DiagnosticName("System.Net.Http.Request")]
        public void HttpRequest(HttpRequestMessage request)
        {
            clientTrace = new ClientTrace(this._serviceName, request.Method.Method);
            if (clientTrace.Trace != null)
            {
                _injector.Inject(clientTrace.Trace.CurrentSpan, request.Headers);
            }
        }

        [DiagnosticName("System.Net.Http.Response")]
        public void HttpResponse(HttpResponseMessage response)
        {
            if (clientTrace.Trace != null)
            {
                ///要增加记录的日志内容
                clientTrace.AddAnnotation(Annotations.Tag(zipkinCoreConstants.HTTP_PATH, response.RequestMessage.RequestUri.LocalPath));
                clientTrace.AddAnnotation(Annotations.Tag(zipkinCoreConstants.HTTP_METHOD, response.RequestMessage.Method.Method));
                clientTrace.AddAnnotation(Annotations.Tag(zipkinCoreConstants.HTTP_HOST, response.RequestMessage.RequestUri.Host));
                if (!response.IsSuccessStatusCode)
                {
                    clientTrace.AddAnnotation(Annotations.Tag(zipkinCoreConstants.HTTP_STATUS_CODE, ((int)response.StatusCode).ToString()));
                }
            }
        }

        [DiagnosticName("System.Net.Http.Exception")]
        public void HttpException(HttpRequestMessage request, Exception exception)
        {
        }
    }
}

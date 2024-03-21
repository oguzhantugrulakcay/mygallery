using Serilog;

namespace mygallery.Infrastuctures
{
    public class UnhandledExceptionMiddleware(RequestDelegate Next)
    {
        private readonly RequestDelegate next = Next;
        private static string _body;
        private static Exception _exception;
        public async Task Invoke(HttpContext context
        //, IDiagnosticContext diagnosticContext
        )
        {
            context.Request.EnableBuffering();

            var reader = new StreamReader(context.Request.Body);

            _body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0L;


            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                var innerExCount = 0;

                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    innerExCount++;
                }

                if (innerExCount > 0)
                {
                    _exception = exception;
                }

                throw;
            }
        }



        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Scheme", request.Scheme);

            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            var endpoint = httpContext.GetEndpoint();

            if (endpoint != null)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }

            httpContext.Request.EnableBuffering();

            if (string.IsNullOrWhiteSpace(_body))
            {
                var reader = new StreamReader(httpContext.Request.Body);
                _body = reader.ReadToEnd();
            }

            diagnosticContext.Set("Body", _body);

            httpContext.Request.Body.Position = 0L;


            if (httpContext.User.Claims.Any())
            {
                //var currentUser = httpContext.User.GetUser();

                //diagnosticContext.Set("UserId", currentUser.UserId);
                //diagnosticContext.Set("FullName", currentUser.FullName);
                //diagnosticContext.Set("CompanyId", currentUser.CompanyId);
                //diagnosticContext.Set("UserType", currentUser.UserType);
            }


            if (_exception != null)
            {
                diagnosticContext.Set("InnermostException", _exception);
            }

            var ua = httpContext.Request.Headers.UserAgent.FirstOrDefault() ?? "";

            if (!string.IsNullOrWhiteSpace(ua))
            {
                diagnosticContext.Set("Agent", ua);
            }
        }
    }
}

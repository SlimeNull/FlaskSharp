using System;

namespace FlaskSharp
{
    public class HttpRequest : HttpMessage
    {
        internal HttpRequest(HttpMethod method, Uri url)
        {
            Method = method;
            Url = url;
        }

        public HttpMethod Method { get; }
        public Uri Url { get; }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;

namespace FlaskSharp
{
    public class HttpResponse : HttpMessage
    {
        private HttpStatus status;

        internal HttpResponse() : this(HttpStatus.Ok, null)
        { }

        internal HttpResponse(HttpStatus status) : this(status, null)
        { }

        internal HttpResponse(HttpStatus status, string? message)
        {
            Status = status;

#if NET6_0_OR_GREATER
            Message = message ?? Enum.GetName<HttpStatus>(status) ?? string.Empty;
#else
            Message = message ?? Enum.GetName(typeof(HttpStatus), status) ?? string.Empty;
#endif
        }

        public HttpStatus Status
        {
            get => status;
            set
            {
                status = value;

#if NET6_0_OR_GREATER
                Message = Enum.GetName(value) ?? Message;
#else
                Message = Enum.GetName(typeof(HttpStatus), value) ?? Message;
#endif
            }
        }

        public string Message { get; set; }

        internal async Task WriteToAsync(Stream dest)
        {
            using StreamWriter writer = new StreamWriter(dest);

            await writer.WriteAsync($"HTTP/1.1 {(int)Status} {Message}");
            await writer.WriteAsync("\r\n");

            Headers["Content-Length"] = $"{Body.Length}";
            foreach (var kv in Headers)
            {
                await writer.WriteAsync($"{kv.Key}: {kv.Value}");
                await writer.WriteAsync("\r\n");
            }

            await writer.WriteAsync("\r\n");
            await writer.FlushAsync();

            Body.Seek(0, SeekOrigin.Begin);
            await Body.CopyToAsync(dest);
            await dest.FlushAsync();
        }
        public static HttpResponse Continue => new HttpResponse(HttpStatus.Continue);
        public static HttpResponse SwitchingProtocols => new HttpResponse(HttpStatus.SwitchingProtocols);
        public static HttpResponse Ok => new HttpResponse(HttpStatus.Ok);
        public static HttpResponse Created => new HttpResponse(HttpStatus.Created);
        public static HttpResponse NoContent => new HttpResponse(HttpStatus.NoContent);
        public static HttpResponse MovedPermanently => new HttpResponse(HttpStatus.MovedPermanently);
        public static HttpResponse Found => new HttpResponse(HttpStatus.Found);
        public static HttpResponse NotModified => new HttpResponse(HttpStatus.NotModified);
        public static HttpResponse BadRequest => new HttpResponse(HttpStatus.BadRequest);
        public static HttpResponse Unauthorized => new HttpResponse(HttpStatus.Unauthorized);
        public static HttpResponse Forbidden => new HttpResponse(HttpStatus.Forbidden);
        public static HttpResponse NotFound => new HttpResponse(HttpStatus.NotFound);
        public static HttpResponse InternalServerError => new HttpResponse(HttpStatus.InternalServerError);
        public static HttpResponse BadGateway => new HttpResponse(HttpStatus.BadGateway);
        public static HttpResponse ServiceUnavailable => new HttpResponse(HttpStatus.ServiceUnavailable);
    }
}
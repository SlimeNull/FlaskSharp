namespace FlaskSharp
{
    public abstract class HttpMessage
    {
        public HttpMessage()
        {
            Body = new HttpMessageBodyStream(this);
        }

        public HttpMessageBodyStream Body { get; }

        public HttpHeaders Headers { get; } = new HttpHeaders();
    }
}
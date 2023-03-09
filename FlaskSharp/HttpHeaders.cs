using System.Runtime.Serialization;

namespace FlaskSharp
{
    public class HttpHeaders : Dictionary<string, string>
    {
        public HttpHeaders()
        {
        }

        public HttpHeaders(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public HttpHeaders(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
        {
        }

        public HttpHeaders(IEqualityComparer<string>? comparer) : base(comparer)
        {
        }

        public HttpHeaders(int capacity) : base(capacity)
        {
        }

        public HttpHeaders(IDictionary<string, string> dictionary, IEqualityComparer<string>? comparer) : base(dictionary, comparer)
        {
        }

        public HttpHeaders(IEnumerable<KeyValuePair<string, string>> collection, IEqualityComparer<string>? comparer) : base(collection, comparer)
        {
        }

        public HttpHeaders(int capacity, IEqualityComparer<string>? comparer) : base(capacity, comparer)
        {
        }

        protected HttpHeaders(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public string Host
        {
            get => TryGetValue("Host", out string? value) ? value : string.Empty;
            set => this["Host"] = value;
        }

        public string Referer
        {
            get => TryGetValue("Referer", out string? value) ? value : string.Empty;
            set => this["Referer"] = value;
        }

        public string Authorization
        {
            get => TryGetValue("Authorization", out string? value) ? value : string.Empty;
            set => this["Authorization"] = value;
        }

        public string Cookie
        {
            get => TryGetValue("Cookie", out string? value) ? value : string.Empty;
            set => this["Cookie"] = value;
        }

        public string Location
        {
            get => TryGetValue("Location", out string? value) ? value : string.Empty;
            set => this["Location"] = value;
        }

        public string UserAgent
        {
            get => TryGetValue("User-Agent", out string? value) ? value : string.Empty;
            set => this["User-Agent"] = value;
        }

        public string ContentType
        {
            get => TryGetValue("Content-Type", out string? value) ? value : string.Empty;
            set => this["Content-Type"] = value;
        }

        public long ContentLength 
        { 
            get => TryGetValue("Content-Length", out string? valueStr) && long.TryParse(valueStr, out long value) ? value : 0;
            set => this["Content-Length"] = Convert.ToString(value);
        }
    }
}
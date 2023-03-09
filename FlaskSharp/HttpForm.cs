using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FlaskSharp
{
    public abstract class HttpForm : Dictionary<string, string>
    {
        public HttpForm()
        {
        }

        public HttpForm(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public HttpForm(IEqualityComparer<string>? comparer) : base(comparer)
        {
        }

        public HttpForm(int capacity) : base(capacity)
        {
        }

        public HttpForm(IDictionary<string, string> dictionary, IEqualityComparer<string>? comparer) : base(dictionary, comparer)
        {
        }

        public HttpForm(int capacity, IEqualityComparer<string>? comparer) : base(capacity, comparer)
        {
        }

        protected HttpForm(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

#if NET6_0_OR_GREATER

        public HttpForm(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
        {
        }

        public HttpForm(IEnumerable<KeyValuePair<string, string>> collection, IEqualityComparer<string>? comparer) : base(collection, comparer)
        {
        }
#endif
    }
}
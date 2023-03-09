using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FlaskSharp
{

    public class HttpUrlEncodedForm : HttpForm
    {
        public HttpUrlEncodedForm()
        {
        }

        public HttpUrlEncodedForm(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public HttpUrlEncodedForm(IDictionary<string, string> dictionary, IEqualityComparer<string>? comparer) : base(dictionary, comparer)
        {
        }

        public HttpUrlEncodedForm(IEqualityComparer<string>? comparer) : base(comparer)
        {
        }

        public HttpUrlEncodedForm(int capacity) : base(capacity)
        {
        }

        public HttpUrlEncodedForm(int capacity, IEqualityComparer<string>? comparer) : base(capacity, comparer)
        {
        }

        protected HttpUrlEncodedForm(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

#if NET6_0_OR_GREATER

        public HttpUrlEncodedForm(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
        {
        }

        public HttpUrlEncodedForm(IEnumerable<KeyValuePair<string, string>> collection, IEqualityComparer<string>? comparer) : base(collection, comparer)
        {
        }

#endif


        private static readonly char[] eqSplitor = new[]{ '=' };

        public static HttpUrlEncodedForm FromFormString(string formString)
        {
            var form = new HttpUrlEncodedForm();
            var pairs = formString.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split(eqSplitor, 2);
                if (keyValue.Length != 2)
                    continue;

                form.Add(Uri.UnescapeDataString(keyValue[0]), Uri.UnescapeDataString(keyValue[1]));
            }
            return form;
        }

        public void PopulateFromFormString(string formString)
        {
            var pairs = formString.Split('&');

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split(eqSplitor, 2);
                if (keyValue.Length != 2)
                    continue;

                Add(Uri.UnescapeDataString(keyValue[0]), Uri.UnescapeDataString(keyValue[1]));
            }
        }

        public string ToFormString()
        {
            var enumerator = GetEnumerator();
            if (!enumerator.MoveNext())
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            while (enumerator.MoveNext())
            {
                sb.Append('&');
                sb.Append($"{Uri.EscapeDataString(enumerator.Current.Key)}={Uri.EscapeDataString(enumerator.Current.Value)}");
            }

            return sb.ToString();
        }
    }
}
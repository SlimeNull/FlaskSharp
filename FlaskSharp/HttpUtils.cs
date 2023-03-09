using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskSharp
{
    internal static class HttpUtils
    {
        public static bool TryGetRequestInformation(
            string request,
#if NET6_0_OR_GREATER
            [NotNullWhen(true)] out string? method,
            [NotNullWhen(true)] out string? url,
            [NotNullWhen(true)] out string? version)
#else
            out string method,
            out string url,
            out string version)
#endif
        {
            method = url = version = null!;

            int methodEnd = request.IndexOf(' ');
            if (methodEnd == -1)
                return false;

            method = request.Substring(0, methodEnd);

            int urlStart = methodEnd + 1;
            int urlEnd = request.IndexOf(' ', urlStart);
            if (urlEnd == -1)
                return false;

            url = request.Substring(urlStart, urlEnd - urlStart);

            int versionStart = urlEnd + 1;
            int versionEnd = request.IndexOf(' ', versionStart);
            if (versionEnd == -1)
                versionEnd = request.Length;

            version = request.Substring(versionStart, versionEnd - versionStart);

            return true;
        }

        public static HttpHeaders ParseHeaders(IEnumerable<string> headers)
        {
            HttpHeaders rst = new HttpHeaders();

            foreach (string header in headers)
            {
                int colon = header.IndexOf(':');
                if (colon == -1)
                    continue;

                string key = header.Substring(0, colon).Trim();
                string value = header.Substring(colon + 1).Trim();

                rst[Uri.UnescapeDataString(key)] = Uri.UnescapeDataString(value);
            }

            return rst;
        }


    }
}

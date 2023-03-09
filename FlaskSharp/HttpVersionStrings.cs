using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlaskSharp
{
    public static class HttpVersionStrings
    {
        static HttpVersionStrings()
        {
            version2str = new Dictionary<HttpVersion, string>();
            str2version = new Dictionary<string, HttpVersion>();

            PropertyInfo[] props =
                typeof(HttpVersionStrings).GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType != typeof(string))
                    continue;

                if (Enum.TryParse(prop.Name, out HttpVersion versionEnum))
                {
                    string str = prop.GetValue(null) as string ?? throw new Exception("Impossible");

                    version2str[versionEnum] = str;
                    str2version[str] = versionEnum;
                }
            }
        }

        public static readonly string Version1_0 = "HTTP/1.0";
        public static readonly string Version1_1 = "HTTP/1.1";

        private static Dictionary<HttpVersion, string> version2str;
        private static Dictionary<string, HttpVersion> str2version;

        public static bool TryParse(string version, out HttpVersion httpVersion)
        {
            return str2version.TryGetValue(version, out httpVersion);
        }

        public static string? GetString(HttpVersion version)
        {
            if (version2str.TryGetValue(version, out var str))
                return str;

            return null;
        }
    }
}
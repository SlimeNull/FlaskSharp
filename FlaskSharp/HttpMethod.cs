using System;

namespace FlaskSharp
{
    [Flags]
    public enum HttpMethod
    {
        Get    = 1,
        Post   = 2,
        Put    = 4,
        Delete = 8
    }
}
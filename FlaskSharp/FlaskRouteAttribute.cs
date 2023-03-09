using System;

namespace FlaskSharp
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FlaskRouteAttribute : Attribute
    {
        public FlaskRouteAttribute(string route)
        {
            Route = route;
        }

        public string Route { get; }
        public HttpMethod Methods { get; set; } = HttpMethod.Get;
    }
}
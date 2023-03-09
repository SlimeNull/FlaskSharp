namespace FlaskSharp
{
    public enum HttpStatus
    {
        /// <summary>
        /// 表示服务器已经接收到请求头，并且客户端可以发送请求体。
        /// </summary>
        Continue = 100,

        /// <summary>
        /// 表示服务器已经理解了客户端的请求，并将通过Upgrade消息头通知客户端切换协议。
        /// </summary>
        SwitchingProtocols = 101,



        /// <summary>
        /// 请求已经成功处理，并返回响应。
        /// </summary>
        Ok = 200,

        /// <summary>
        /// 表示请求已经成功处理，并在服务器上创建了一个新的资源。
        /// </summary>
        Created = 201,

        /// <summary>
        /// 表示请求已经成功处理，但是没有响应内容。
        /// </summary>
        NoContent = 204,

        

        /// <summary>
        /// 表示请求的资源已经被永久移动到新的URL。
        /// </summary>
        MovedPermanently = 301,

        /// <summary>
        /// 表示请求的资源已经被临时移动到新的URL。
        /// </summary>
        Found = 302,

        /// <summary>
        /// 表示客户端缓存的资源仍然有效，可以直接使用缓存的资源。
        /// </summary>
        NotModified = 304,

        

        /// <summary>
        /// 表示请求存在语法错误或者无法被服务器理解。
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// 表示请求需要身份验证，但是客户端没有提供有效的身份验证信息。
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 表示客户端没有权限访问请求的资源。
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 表示请求的资源不存在。
        /// </summary>
        NotFound = 404,

        

        /// <summary>
        /// 表示服务器在处理请求时出现了未知的错误。
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// 表示服务器作为网关或代理角色时，从上游服务器接收到的响应无效。
        /// </summary>
        BadGateway = 502,

        /// <summary>
        /// 表示服务器暂时无法处理请求，通常是由于服务器过载或正在维护。
        /// </summary>
        ServiceUnavailable = 503
    }
}
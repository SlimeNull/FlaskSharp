using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace FlaskSharp
{
    public class FlaskApp
    {
        private TcpListener? listener;

        private Task? mainLoop;
        private CancellationTokenSource? cancellationTokenSource;

        private readonly List<FlaskRouteMethod> flaskRouteMethods;

        record FlaskRouteMethod(MethodInfo Method, ParameterInfo[] Parameters, FlaskRouteAttribute Attribute);

        public FlaskApp()
        {
            InitMethods(out flaskRouteMethods);
        }

        public IPAddress ListenAddress { get; set; } = IPAddress.Any;
        public int ListenPort { get; set; } = 5000;

        private void InitMethods(out List<FlaskRouteMethod> flaskRouteMethods)
        {
            Type thisType = GetType();
            MethodInfo[] methods = thisType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            flaskRouteMethods = new List<FlaskRouteMethod>();

            foreach (MethodInfo method in methods)
            {
                var attribute = method.GetCustomAttribute<FlaskRouteAttribute>();
                if (attribute == null)
                    continue;

                flaskRouteMethods.Add(new FlaskRouteMethod(method, method.GetParameters(), attribute));
            }
        }

        private async Task MainLoopAsync()
        {
            if (listener == null || cancellationTokenSource == null)
                return;

            CancellationToken cancellationToken =
                cancellationTokenSource.Token;

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await HandleConnectionAsync(listener);
            }

            listener.Stop();
            listener = null;

            mainLoop = null;
            cancellationTokenSource = null;
        }

        private Task HandleConnectionAsync(TcpListener listener)
        {
            async Task InternalHandleConnectionAsync(Task<TcpClient> accept)
            {
                using TcpClient client = await accept;
                await ProcessConnectionAsync(client);
            }

            Task<TcpClient> accept =
                listener.AcceptTcpClientAsync();

            _ = InternalHandleConnectionAsync(accept);
            return accept;
        }

        private async Task ProcessConnectionAsync(TcpClient client)
        {
            using NetworkStream networkStream = client.GetStream();
            using StreamReader streamReader = new StreamReader(networkStream);

            string? requestLine =
                await streamReader.ReadLineAsync();
            if (requestLine == null)
            {
                await HttpResponse.BadRequest.WriteToAsync(networkStream);
                return;
            }

            if (!HttpUtils.TryGetRequestInformation(requestLine, out string? methodStr, out string? url, out string? versionStr))
            {
                await HttpResponse.BadRequest.WriteToAsync(networkStream);
                return;
            }

            if (!Enum.TryParse<HttpMethod>(methodStr, true, out HttpMethod method))
            {
                await HttpResponse.BadRequest.WriteToAsync(networkStream);
                return;
            }

            List<string> headerStrs = new List<string>();
            while (true)
            {
                string? header =
                    await streamReader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(header))
                    break;

                headerStrs.Add(header);
            }

            HttpHeaders headers = HttpUtils.ParseHeaders(headerStrs);
            if (string.IsNullOrWhiteSpace(headers.Host))
            {
                await HttpResponse.BadRequest.WriteToAsync(networkStream);
                return;
            }

            Uri requestUri = new Uri($"http://{headers.Host}{url}");

            byte[] bodyBuffer = new byte[headers.ContentLength];
            await networkStream.ReadEnoughAsync(bodyBuffer, 0, bodyBuffer.Length);

            MemoryStream body = new MemoryStream(bodyBuffer);

            HttpRequest request = new HttpRequest(method, requestUri);
            HttpResponse response = new HttpResponse();

            await ProcessRequestAsync(request, response);
            await response.WriteToAsync(networkStream);
        }

        private async Task ProcessRequestAsync(HttpRequest request, HttpResponse response)
        {
            HttpUrlEncodedForm query =
                HttpUrlEncodedForm.FromFormString(request.Url.Query.TrimStart('?'));

            if (request.Headers.ContentType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
            {
                string formString = request.Body.GetText();
                query.PopulateFromFormString(formString);
            }

            bool handled = false;
            foreach (var method in flaskRouteMethods)
            {
                if (request.Url.AbsolutePath.Equals(method.Attribute.Route, StringComparison.OrdinalIgnoreCase))
                {
                    handled = true;
                    object?[] parameterObjects = new object[method.Parameters.Length];

                    for (int i = 0; i < method.Parameters.Length; i++)
                    {
                        ParameterInfo paramInfo = method.Parameters[i];
                        if (paramInfo.ParameterType == typeof(HttpRequest))
                        {
                            parameterObjects[i] = request;
                            continue;
                        }

                        if (paramInfo.ParameterType == typeof(HttpResponse))
                        {
                            parameterObjects[i] = response;
                            continue;
                        }

                        if (paramInfo.ParameterType == typeof(HttpForm))
                        {
                            parameterObjects[i] = query;
                            continue;
                        }

                        if (paramInfo.Name != null && query.TryGetValue(paramInfo.Name, out var queryValue))
                        {
                            if (paramInfo.ParameterType == typeof(string))
                            {
                                parameterObjects[i] = queryValue;
                                continue;
                            }

                            if (paramInfo.ParameterType == typeof(byte))
                            {
                                if (byte.TryParse(queryValue, out byte value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(short))
                            {
                                if (short.TryParse(queryValue, out short value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(ushort))
                            {
                                if (ushort.TryParse(queryValue, out ushort value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(int))
                            {
                                if (int.TryParse(queryValue, out int value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(uint))
                            {
                                if (uint.TryParse(queryValue, out uint value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(long))
                            {
                                if (long.TryParse(queryValue, out long value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(ulong))
                            {
                                if (ulong.TryParse(queryValue, out ulong value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(float))
                            {
                                if (float.TryParse(queryValue, out float value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(double))
                            {
                                if (double.TryParse(queryValue, out double value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(decimal))
                            {
                                if (decimal.TryParse(queryValue, out decimal value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(bool))
                            {
                                if (bool.TryParse(queryValue, out bool value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(DateTime))
                            {
                                if (DateTime.TryParse(queryValue, out DateTime value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(DateTimeOffset))
                            {
                                if (DateTimeOffset.TryParse(queryValue, out DateTimeOffset value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }

                            if (paramInfo.ParameterType == typeof(TimeSpan))
                            {
                                if (TimeSpan.TryParse(queryValue, out TimeSpan value))
                                {
                                    parameterObjects[i] = value;
                                    continue;
                                }
                            }
                        }

                        parameterObjects[i] = paramInfo.DefaultValue ??
                            ReflectionUtils.GetDefaultValue(paramInfo.ParameterType);
                    }

                    object? retValue =
                        method.Method.Invoke(this, parameterObjects);

                    if (retValue == null)
                        break;

                    Type retValueType =
                        retValue.GetType();

                    //if (retValue is Task retTask)
                    //{
                    //    await retTask;
                    //    Task<string>

                    //    if (retValueType.Ba)
                    //}

                    string retValueText;

                    if (retValueType == typeof(string))
                        retValueText = (string)retValue;
                    else if (retValueType.IsPrimitive)
                        retValueText = retValue.ToString() ?? string.Empty;
                    else
                        retValueText = JsonSerializer.Serialize(retValue, retValueType);

                    StreamWriter bodyWriter = new StreamWriter(response.Body);
                    await bodyWriter.WriteAsync(retValueText);
                    await bodyWriter.FlushAsync();
                    break;
                }
            }

            if (!handled)
                response.Status = HttpStatus.NotFound;
        }

        public Task StartAsync()
        {
            if (listener != null)
                throw new InvalidOperationException("Already started");

            listener = new TcpListener(ListenAddress, ListenPort);
            listener.Start();

            cancellationTokenSource = new CancellationTokenSource();
            mainLoop = MainLoopAsync();
            return Task.CompletedTask;
        }

        public async Task WaitForShutdownAsync()
        {
            if (mainLoop == null)
                throw new InvalidOperationException("Not started yet");

            await mainLoop;
        }

        public async Task StopAsync()
        {
            if (mainLoop == null || cancellationTokenSource == null)
                throw new InvalidOperationException("Not started yet");

            cancellationTokenSource.Cancel();
            await WaitForShutdownAsync();
        }

        public async Task RunAsync()
        {
            await StartAsync();
            await WaitForShutdownAsync();
        }

        public void Start() => StartAsync().Wait();
        public void WaitForShutdown() => WaitForShutdownAsync().Wait();
        public void Stop() => StopAsync().Wait();
        public void Run() => RunAsync().Wait();
    }
}
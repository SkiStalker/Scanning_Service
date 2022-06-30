using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Net.WebSockets;

namespace Task2_Kapserskiy
{
    class ScanServiceNetwork
    {
        public bool Alive { get; private set; }
        ScanService? scanService;
        readonly HttpListener httpListener;
        readonly CancellationToken cancelled;
        TaskCompletionSource<object>? cancellationSignal = null;
        public ScanServiceNetwork()
        {
            scanService = new ScanService();
            httpListener = new HttpListener();
            cancelled = new CancellationToken();
            cancellationSignal = new TaskCompletionSource<object>();

            cancelled.Register((state) =>
            {
                if (state == null)
                    return;
                ((TaskCompletionSource<object>)state).TrySetResult(new object());
            }, cancellationSignal);

            httpListener.Prefixes.Add("http://localhost:5000/api/");
            httpListener.Start();
        }
        public async void Start()
        {
            Alive = true;
            while (Alive)
            {
                if (cancellationSignal == null)
                    throw new Exception("Null cancellation signal");

                Task<HttpListenerContext> contextTask = httpListener.GetContextAsync();

                if (contextTask != await Task.WhenAny(contextTask, cancellationSignal.Task).ConfigureAwait(false))
                    break;

                HttpListenerContext context = contextTask.Result;

                byte[] data = new byte[context.Request.ContentLength64];

                context.Request.InputStream.Read(data, 0, data.Length);

                ScanRequest? scanRequest = JsonSerializer.Deserialize<ScanRequest>(Encoding.UTF8.GetString(data));

                switch (context.Request.HttpMethod)
                {
                    case "GET":
                        {
                            new Thread(() => { GetRequest(context.Response, scanRequest); }).Start();
                            break;
                        }
                    case "POST":
                        {
                            new Thread(() => { PostRequest(context.Response, scanRequest); }).Start();
                            break;
                        }
                    default:
                        {
                            new Thread(() => { DefaultResponse(context.Response); }).Start();
                            break;
                        }
                }
            }
        }

        void DefaultResponse(HttpListenerResponse? response)
        {
            SendDto(new ScanDto(message: "Unknown request"), response);
        }

        void GetRequest(HttpListenerResponse? response, ScanRequest? scanRequest)
        {
            if (scanService != null)
            {
                ScanDto scanDto = scanService.GetTask(scanRequest);
                SendDto(scanDto, response);
            }
            else
            {
                SendDto(new ScanDto(message: "Internal service error"), response);
            }
        }

        void PostRequest(HttpListenerResponse? response, ScanRequest? scanRequest)
        {
            if (scanService != null)
            {
                ScanDto scanDto = scanService.AddTask(scanRequest);
                SendDto(scanDto, response);
            }
            else
            {
                SendDto(new ScanDto(message: "Internal service error"), response);
            }
        }

        void SendDto(ScanDto scanDto, HttpListenerResponse? response)
        {
            if (Alive)
            {
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(scanDto));
                response?.OutputStream.Write(data, 0, data.Length);
                response?.Close();
            }
        }

        public void Stop()
        {
            Alive = false;
            cancellationSignal?.SetCanceled();
            httpListener.Close();
            scanService?.Dispose();
        }
    }
}

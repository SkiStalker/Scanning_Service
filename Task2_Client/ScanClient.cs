using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Task2_Kapserskiy
{
    class ScanClient
    {
        UdpClient client;
        public ScanClient()
        {
            client = new UdpClient(5001);
        }
        ~ScanClient()
        {
            client.Close();
        }

        static HttpResponseMessage SendRequest(ScanRequest scanRequest, HttpMethod method)
        {
            HttpRequestMessage msg = CreateHttpMessage(scanRequest, method);
            HttpClient clt = new HttpClient();
            return clt.Send(msg);
        }

        static HttpRequestMessage CreateHttpMessage(ScanRequest scanRequest, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://localhost:5000/api/");
            request.Method = method;
            request.Content = new StringContent(JsonSerializer.Serialize(scanRequest));
            request.Headers.Add("Accept", "application/json");
            return request;
        }
        static ScanDto? GetDto(HttpResponseMessage message)
        {
            var json = message.Content.ReadAsStringAsync();
            json.Wait();
            ScanDto? scanDto = JsonSerializer.Deserialize<ScanDto>(json.Result);
            return scanDto;
        }

        public ScanDto? Scan(string path)
        {
            ScanRequest scanRequest = new ScanRequest(path);
            HttpResponseMessage message = SendRequest(scanRequest, HttpMethod.Post);
            return GetDto(message);
        }

        public ScanDto? Status(string id)
        {
            ScanRequest scanRequest = new ScanRequest(id);
            HttpResponseMessage message = SendRequest(scanRequest, HttpMethod.Get);
            return GetDto(message);
        }
    }
}

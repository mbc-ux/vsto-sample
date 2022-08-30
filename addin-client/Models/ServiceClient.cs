using System;
using System.IO;
using System.Net;

namespace Vsto.Sample.Client.Models
{
    public interface IServiceClient : ICanPublishData
    {
        string Endpoint { get; set; }
        void LoadData();
        event Action<string> ActionLogged;
    }

    public class ServiceClient : IServiceClient
    {
        private void Log(string log)
        {
            ActionLogged?.Invoke(log);
            ActionLogged?.Invoke(Environment.NewLine);
        }

        public string Endpoint { get; set; }
        public async void LoadData()
        {
            Log(@"Calling service...");
            var request = WebRequest.Create(Endpoint);
            Log("Authentication Level: " + request.AuthenticationLevel);
            Log("Content Type: " + request.ContentType);
            Log("Content Length: " + request.ContentLength);
            Log("Request URI: " + request.RequestUri);
            using (var response = request.GetResponse())
            {
                Log(@"Reading response...");
                Log("Content Type: " + response.ContentType);
                Log("Content Length: " + response.ContentLength);
                using (var stream = response.GetResponseStream())
                {
                    using(var reader = new StreamReader(stream))
                    {
                        var responseContent = reader.ReadToEnd();
                        Log("Content: " + responseContent);
                        DataPublished?.Invoke(responseContent);
                    }
                }
            }
            Log(@"Called service..." + Environment.NewLine);
        }

        public event Action<string> ActionLogged;
        public event Action<object> DataPublished;
    }
}

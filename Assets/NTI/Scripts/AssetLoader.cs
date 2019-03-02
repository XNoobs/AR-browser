using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Assertions;


namespace NTI.Scripts
{
    public class AssetLoader
    {
        private const string Localhost = "127.0.0.1";
        private const string Xnoobsru = "157.230.133.157";
        
        private readonly IPAddress _host = IPAddress.Parse(Localhost);
        private readonly int _port;

        public AssetLoader(int port)
        {
            // ToDo: use any server free port
            this._port = port;
        }

        public async void Run()
        {
           using (var output = File.OpenWrite("map.osm"))
           {
               try
               {
                   var httpRequest = (HttpWebRequest) WebRequest.Create(Localhost + "/ar/get_model");
                   httpRequest.Method = WebRequestMethods.Http.Get;

                   var httpResponse = (HttpWebResponse) httpRequest.GetResponse();
                   var httpResponseStream = httpResponse.GetResponseStream();

                   Assert.IsNotNull(httpResponseStream);
                   await httpResponseStream.CopyToAsync(output);
               }
               catch(HttpRequestException e)
               {
                   Console.WriteLine(e.Message);
               }
           }
        }
    }
}
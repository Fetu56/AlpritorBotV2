using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlpritorBotV2.ListenerModule
{
    public static class TokenListener
    {
        private static readonly string ResponseHtlm = Resources.ResourceHtml.AccessToken;
        private static HttpListener? _server;
        private static string? _accessToken;

        public async static Task<string> GetAccessToken()
        {
            _server = new HttpListener();
            _server.Prefixes.Add("http://localhost:3001/");
            _server.Start();

            await Task.Run(() => {
                Process browserStartProccess = new();
                browserStartProccess.StartInfo.UseShellExecute = true;
                browserStartProccess.StartInfo.FileName = $"https://id.twitch.tv/oauth2/authorize?response_type=token&client_id={ConfigurationManager.AppSettings["clientId"]!}&redirect_uri=http://localhost:3001&scope=channel:read:redemptions+channel:manage:broadcast+channel:manage:redemptions+channel:manage:polls+channel:manage:predictions+channel:read:polls+channel:read:predictions+channel:manage:raids+channel:read:vips+channel:manage:vips";
                browserStartProccess.Start();
            });

            _server.BeginGetContext(Callback, null);

            while(_accessToken == null)
            {
                Thread.Sleep(1000);
            }
            
            _server.Stop();

            return _accessToken;
        }


        static void Callback(IAsyncResult res)
        {
            var ctx = _server!.EndGetContext(res);
           
            byte[] buffer = Encoding.UTF8.GetBytes(ResponseHtlm);
            ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
            ctx.Response.OutputStream.Close();

            var accessToken = ctx.Request.QueryString.GetValues("access_token");
            if(accessToken != null)
            {
                _accessToken = accessToken[0];
            }
            else
            {
                _server.BeginGetContext(Callback, null);
            }
        }
    }
}

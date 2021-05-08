
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HotUpdateScripts
{

    /// <summary>
    /// 重写的WebClient类
    /// </summary>
    public class ShiYuanWebClient : WebClient
    {
        readonly int _timeout;
        public ShiYuanWebClient(int timeout = 40)
        {
            if (null == ServicePointManager.ServerCertificateValidationCallback)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }

            _timeout = timeout * 1000;
        }

        //the above does not work for async requests, lets override the method
        public new async Task<string> DownloadStringTaskAsync(Uri address)
        {
            return await RunWithTimeout(base.DownloadStringTaskAsync(address));
        }

        public new async Task<string> UploadStringTaskAsync(string address, string data)
        {
            return await RunWithTimeout(base.UploadStringTaskAsync(address, data));
        }
        public new async Task DownloadFileTaskAsync(string address, string data)
        {
            await RunWithTimeout(base.DownloadFileTaskAsync(address, data));
        }
        public new async Task DownloadFileTaskAsync(Uri address, string fileName)
        {
            await RunWithTimeout(base.DownloadFileTaskAsync(address, fileName));
        }
        private async Task<T> RunWithTimeout<T>(Task<T> task)
        {
            if (task == await Task.WhenAny(task, Task.Delay(_timeout)))
                return await task;
            else
            {
                this.CancelAsync();
                throw new TimeoutException();
            }
        }
        private async Task RunWithTimeout(Task task)
        {
            if (task == await Task.WhenAny(task, Task.Delay(_timeout)))
                await task;
            else
            {
                this.CancelAsync();
                throw new TimeoutException();
            }
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //request.ProtocolVersion = HttpVersion.Version10;
            request.Timeout = _timeout;
            request.ReadWriteTimeout = _timeout;
            request.Proxy = null;
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            return base.GetWebResponse(request);
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //总是接受,通过Https验证
            return true;
        }
    }
}
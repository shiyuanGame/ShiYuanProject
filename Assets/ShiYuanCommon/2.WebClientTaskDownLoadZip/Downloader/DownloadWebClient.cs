
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HotUpdateScripts
{
    /// <summary>
    /// 重写的WebClient类
    /// </summary>
    public class DownloadWebClient : WebClient
    {
        readonly int _timeout;
        public DownloadWebClient(int timeout = 60)
        {
            if (null == ServicePointManager.ServerCertificateValidationCallback)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }

            _timeout = timeout * 1000;
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
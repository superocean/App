using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace DealSearchEngine.Utility
{
    public class HttpSessionForGZipEncoding
    {
        //This static parameter is set true when we do in fresh price, we need the time out for one request
        public static bool _bIsNeedTimeOut = false;
        //This is the IP which use in downloading
        public static string sIP = "-1";
        private string sContent = "";
        private CookieCollection _cookies = new CookieCollection();
        private WebHeaderCollection _headers = new WebHeaderCollection();
        public string sReferer = "";
        public string sAccept = "";
        public string sContentType = "";
        public int _iTimeOut = 30 * 1000; //Time out is 30s
        private int iDefaultTimeout = 60 * 1000; // Default time out is 60s

        public HttpSessionForGZipEncoding()
        {

        }
        public CookieCollection Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }
        public WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }
        public string GetMethodDownload(string sUrl, bool isKeepAlive, bool isAutoRedirect, bool isUseHeader, bool isUseCookie)
        {
            try
            {
                sContent = "";
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(sUrl);
                httpRequest.Method = "GET";
                httpRequest.AllowAutoRedirect = isAutoRedirect;
                httpRequest.KeepAlive = isKeepAlive;
                httpRequest.ServicePoint.ConnectionLimit = 500;
                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                httpRequest.CookieContainer = new CookieContainer();
                if (!string.IsNullOrEmpty(sAccept))
                    httpRequest.Accept = sAccept;

                if (_bIsNeedTimeOut) httpRequest.Timeout = _iTimeOut;
                else httpRequest.Timeout = iDefaultTimeout;
                if (_headers != null & isUseHeader == true)
                {
                    string key;
                    string keyvalue;
                    for (int i = 0; i < _headers.Count; i++)
                    {

                        key = _headers.Keys[i];
                        keyvalue = _headers[i];
                        httpRequest.Headers.Add(key, keyvalue);
                    }
                }


                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream answer = httpResponse.GetResponseStream();
                StreamReader _answer = new StreamReader(answer);
                _headers = httpResponse.Headers;
                sContent = _answer.ReadToEnd();
                Cookies = httpResponse.Cookies;
                httpResponse.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return sContent;
        }
        public string PostMethodDownload(string sUrl, string sPost, bool isKeepAlive, bool isAutoRedirect, bool isUseHeader, bool isUseCookie)
        {
            try
            {
                sContent = "";
                byte[] buffer = Encoding.ASCII.GetBytes(sPost);
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(sUrl);
                httpRequest.Method = "POST";
                httpRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                httpRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";
                httpRequest.AllowAutoRedirect = isAutoRedirect;
                httpRequest.KeepAlive = isKeepAlive;
                httpRequest.ServicePoint.ConnectionLimit = 500;
                httpRequest.ContentLength = buffer.Length;
                httpRequest.CookieContainer = new CookieContainer();

                Stream postData = httpRequest.GetRequestStream();
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream answer = httpResponse.GetResponseStream();
                _headers = httpResponse.Headers;
                // StreamReader _answer = new StreamReader(answer, encoding);
                StreamReader _answer = new StreamReader(answer);
                sContent = _answer.ReadToEnd();
                Cookies = httpResponse.Cookies;
                httpResponse.Close();
            }

            catch (Exception)
            {
                throw;
            }
            return sContent;
        }
    }
}
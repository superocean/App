using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Net;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Configuration;
using System.Diagnostics;

namespace DealSearchEngine.Utility
{
    
    public class HttpSession
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

        public HttpSession()
        {
            //Read IP from config file
            if (!String.IsNullOrEmpty(sIP))
                if (sIP == "-1")
                    sIP = System.Configuration.ConfigurationManager.AppSettings["IP"];
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
                //Begin check to use default IP or use IP from config file
                if ((String.IsNullOrEmpty(sIP) == false) && (sIP != "-1"))
                    httpRequest.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(Bind);
                //End check
                httpRequest.Method = "GET";
                if (!sContentType.Equals(""))
                    httpRequest.ContentType = sContentType;
                else
                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                if (!sReferer.Equals(""))
                    httpRequest.Referer = sReferer;
                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows XP)";
                httpRequest.AllowAutoRedirect = isAutoRedirect;
                httpRequest.KeepAlive = isKeepAlive;
                httpRequest.ServicePoint.ConnectionLimit = 500;
                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                httpRequest.CookieContainer = new CookieContainer();
                if (!string.IsNullOrEmpty(sAccept))
                    httpRequest.Accept = sAccept;

                if (_bIsNeedTimeOut) httpRequest.Timeout = _iTimeOut;
                else httpRequest.Timeout = iDefaultTimeout;
                if (_cookies != null && isUseCookie == true)
                {
                    httpRequest.CookieContainer.Add(_cookies);
                }
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

                //Add gzip 
                if (httpRequest.Headers == null)
                {
                    httpRequest.Headers = new WebHeaderCollection();
                }
                httpRequest.Headers.Add("Accept-Encoding", "gzip,deflate");


                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                //string sCharacterSet = "UTF-8";
                //if (httpResponse.CharacterSet != "") sCharacterSet = httpResponse.CharacterSet;
                //Encoding encoding = Encoding.GetEncoding(sCharacterSet);
                Stream answer = httpResponse.GetResponseStream();

                //Unzip
                if (httpResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    answer = new GZipStream(answer, CompressionMode.Decompress);
                }
                else if (httpResponse.ContentEncoding.ToLower().Contains("deflate"))
                {
                    answer = new DeflateStream(answer, CompressionMode.Decompress);
                }
                //
                StreamReader _answer = new StreamReader(answer);
                _headers = httpResponse.Headers;
                foreach (Cookie cook in httpResponse.Cookies)
                    _cookies.Add(cook);
                //_cookies = httpResponse.Cookies;
                sContent = _answer.ReadToEnd();
                httpResponse.Close();
            }
            catch (Exception e)
            {
                /*T  temp remove Microsoft.Practices.EnterpriseLibrary.Logging */

                //LogEntry en = new LogEntry();
                //en.Message = "ERROR: could not download content from url:" + sUrl + "--" + e.Message;
                //Logger.Write(en);
                System.Diagnostics.Trace.WriteLine("ERROR: could not download content from url:" + sUrl + "--" + e.Message);
                //throw e;
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
                //Begin check to use default IP or use IP from config file
                if ((String.IsNullOrEmpty(sIP) == false) && (sIP != "-1"))
                    httpRequest.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(Bind);
                //End check

                httpRequest.Method = "POST";
                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows XP)";
                if (!sContentType.Equals(""))
                    httpRequest.ContentType = sContentType;
                else
                    httpRequest.ContentType = "application/x-www-form-urlencoded";
                if (!sReferer.Equals(""))
                    httpRequest.Referer = sReferer;
                if (_bIsNeedTimeOut) httpRequest.Timeout = _iTimeOut;
                else httpRequest.Timeout = iDefaultTimeout;
                httpRequest.AllowAutoRedirect = isAutoRedirect;
                httpRequest.KeepAlive = isKeepAlive;
                httpRequest.ServicePoint.ConnectionLimit = 500;
                httpRequest.ContentLength = buffer.Length;
                httpRequest.CookieContainer = new CookieContainer();


                if (_cookies != null && isUseCookie == true)
                {
                    httpRequest.CookieContainer.Add(_cookies);
                }
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
                //Add gzip 
                if (httpRequest.Headers == null)
                {
                    httpRequest.Headers = new WebHeaderCollection();
                }
                httpRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
                //
                Stream postData = httpRequest.GetRequestStream();
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                //string sCharacterSet = "UTF-8";
                //if (httpResponse.CharacterSet != "") sCharacterSet = httpResponse.CharacterSet;
                //Encoding encoding = Encoding.GetEncoding(sCharacterSet);
                Stream answer = httpResponse.GetResponseStream();

                //Unzip
                if (httpResponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    answer = new GZipStream(answer, CompressionMode.Decompress);
                }
                else if (httpResponse.ContentEncoding.ToLower().Contains("deflate"))
                {
                    answer = new DeflateStream(answer, CompressionMode.Decompress);
                }
                foreach (Cookie cook in httpResponse.Cookies)
                    _cookies.Add(cook);
                //_cookies = httpResponse.Cookies;
                _headers = httpResponse.Headers;
                // StreamReader _answer = new StreamReader(answer, encoding);
                StreamReader _answer = new StreamReader(answer);
                sContent = _answer.ReadToEnd();
                httpResponse.Close();
            }

            catch (Exception e)
            {
                /*T temp remove Microsoft.Practices.EnterpriseLibrary.Logging*/
                //LogEntry en = new LogEntry();
                //en.Message = "ERROR: could not download content from url:" + sUrl + "--" + e.Message;
                //Logger.Write(en);
                System.Diagnostics.Trace.WriteLine("ERROR: could not download content from url:" + sUrl + "--" + e.Message);
            }
            return sContent;
        }
        IPEndPoint Bind(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
        {
            IPAddress address = IPAddress.Parse(sIP); //Replace with any of your machine's external IPs
            //The second parameter is 0: meaning that the client port will be assigned by windows
            IPEndPoint ie = new IPEndPoint(address, 0);
            return ie;

        }

        
    }
}
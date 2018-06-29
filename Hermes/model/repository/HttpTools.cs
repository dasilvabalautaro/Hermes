using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Hermes.model.repository
{
    class HttpTools
    {
        #region constants
        public const string SERVICE_REGISTER_USER = "http://www.globalhiddenodds.com/index.php/registeruser";
        public const string SERVICE_REGISTER_LICENSE = "http://www.globalhiddenodds.com/index.php/registerlicense";
        public const string SERVICE_REGISTER_CONFIGURATION = "http://www.globalhiddenodds.com/index.php/registerconfiguration";
        public const string SERVICE_VERIFY_LOGIN = "http://www.globalhiddenodds.com/index.php/verifylogin";
        public const string SERVICE_UPDATE_VIDEO = "http://www.globalhiddenodds.com/index.php/updateurlvideo";
        #endregion

        #region variables
        private string addressServer;
        private string service;
        private string result;
        public delegate void ResultOperationDelegate(string result);
        public event ResultOperationDelegate OnOperationRequest;
        private Dictionary<string, object> data = new Dictionary<string, object>();

        public string AddressServer
        {
            get
            {
                return addressServer;
            }

            set
            {
                addressServer = value;
            }
        }

        public string Service
        {
            get
            {
                return service;
            }

            set
            {
                service = value;
            }
        }

        public Dictionary<string, object> Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        public string Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
                if (OnOperationRequest != null)
                {
                    OnOperationRequest(result);
                }
            }
        }

        #endregion

        #region methods
        public void sendServerWeb()
        {
            try
            {

                var request = HttpWebRequest.Create(Service)
                    as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.BeginGetRequestStream(new
                    AsyncCallback(GetRequestStreamCallback), request);
            }
            catch (WebException we)
            {
                throw new ArgumentException(we.Message);
            }


        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

                Stream postStream = request.EndGetRequestStream(asynchronousResult);
                string postData = JsonConvert.SerializeObject(data, Formatting.None);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();

                request.BeginGetResponse(new AsyncCallback(GetResponceStreamCallback), 
                    request);
            }
            catch (WebException we)
            {
                Console.WriteLine(we.Message);
            }
        }

        void GetResponceStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
            {
                Result = httpWebStreamReader.ReadToEnd();
            }

        }

        public void sendServerWeb(Dictionary<string, string> mapCookies)
        {
            try
            {
                string cookies = "";
                foreach (KeyValuePair<string, string> pair in mapCookies)
                {
                    if (!string.IsNullOrEmpty(cookies))
                    {
                        cookies += ";";
                    }
                    cookies += pair.Key + "=" + pair.Value;
                }
                var request = HttpWebRequest.Create(Service) as HttpWebRequest;
                request.Method = "GET";
                request.Headers["Set-Cookie"] = cookies;
                request.BeginGetResponse(new AsyncCallback(GetResponceStreamCallback), request);

            }
            catch (WebException we)
            {
                throw new ArgumentException(we.Message);
            }


        }
        public bool checkForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }


            #endregion
        }
    }
}

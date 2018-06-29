using Hermes.locatable_resources;
using Hermes.model.data;
using Hermes.model.data.repository;
using Hermes.model.repository;
using Hermes.presentation.view;
using Hermes.tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.presentation.presenter
{
    class HttpToolsPresenter
    {
        #region variables
        HttpTools httptools = new HttpTools();
        public Subject<string> subjectResult = new Subject<string>();
        public Subject<string> subjectError = new Subject<string>();
        private RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();
        private CriptoUtil criptoUtil = new CriptoUtil();
        #endregion

        #region methods

        public HttpToolsPresenter()
        {
            httptools.OnOperationRequest += new HttpTools
                .ResultOperationDelegate(sendResult);
        }

        private void sendResult(string result)
        {

            subjectResult.OnNext(result);

        }
        
        public void setBody(Dictionary<string, object> mapParams)
        {
            httptools.Data = mapParams;
        }

        public void setService(string service)
        {
            httptools.Service = service;
        }

        public void sendRequest()
        {
            try
            {
                httptools.sendServerWeb();
            }
            catch (ArgumentException ie)
            {           
                subjectError.OnNext(ie.Message);
            }   
        }

        public string getValueOfKeyResponseJson(string jsonString, string key)
        {
            string id = "";
            JObject json = JObject.Parse(jsonString);
            JToken success = json["success"];
            bool successResult = (bool)success;
            if (successResult)
            {
                JToken result = json["result"];
                id = (string)result.Value<string>(key);
            }
            else
            {
                JToken error = json["error"];
                if (error != null)
                {
                    string msgError = (string)error.Value<string>("message");
                    subjectError.OnNext(msgError);
                }
            }
            return id;

        }

        public bool checkForInternetConnection()
        {
            return httptools.checkForInternetConnection();
        }

        public bool isExistValuesConnection()
        {            
            if (!string.IsNullOrEmpty(registryValueDataReader
                .getKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                frmRegisterMobile.USER_MOBILE_KEY)))
            {
                return true;
            }else
            {
                return false;
            }
                
        }

        public string getUserMobile()
        {
            return registryValueDataReader
                    .getKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                    frmRegisterMobile.USER_MOBILE_KEY);
        }

        public string getPasswordMobile()
        {
            return registryValueDataReader
                    .getKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                    frmRegisterMobile.PASSWORD_MOBILE_KEY);
        }

        public string getTokenMobile()
        {
            return registryValueDataReader
                    .getKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                    frmRegisterMobile.TOKEN_MOBILE_KEY);
        }

        public string getIdConfigurationMobile()
        {
            return registryValueDataReader
                    .getKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                    frmRegisterMobile.ID_CONFIGURATION_MOBILE_KEY);
        }

        public void getToken()
        {
            if (checkForInternetConnection())
            {
                if (isExistValuesConnection())
                {
                    string name = getUserMobile();
                    string password = getPasswordMobile();
                    password = criptoUtil.desencript(password);
                    httptools.Service = HttpTools.SERVICE_VERIFY_LOGIN;
                    httptools.Data = buildParamsToken(name, password);
                    sendRequest();
                }
            }
            else
            {
                subjectError.OnNext(StringResources.network_fail);
            }
        }

        public void setBodyForVideo(string address)
        {
            if (checkForInternetConnection())
            {
                if (isExistValuesConnection())
                {
                    string token = getTokenMobile();
                    string id_config = getIdConfigurationMobile();                    
                    httptools.Service = HttpTools.SERVICE_UPDATE_VIDEO;
                    httptools.Data = buildParamsUrlVideo(token, id_config, address);                                        
                }
            }
            else
            {
                subjectError.OnNext(StringResources.network_fail);
            }
        }

        private Dictionary<string, object> buildParamsToken(string name, 
            string password)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.FIELD_NAME, name);
            mapParams.Add(User.FIELD_PASS, password);
            return mapParams;
        }

        private Dictionary<string, object> buildParamsUrlVideo(string token,
            string id_configuration, string address)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.FIELD_TOKEN, token);
            mapParams.Add(User.FIELD_ID, id_configuration);
            mapParams.Add(User.FIELD_URL_VIDEO, address);
            return mapParams;
        }

        public void sendRequestGet(Dictionary<string, string> mapCookies)
        {
            try
            {
                httptools.sendServerWeb(mapCookies);
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);
            }
        }
        #endregion

    }
}

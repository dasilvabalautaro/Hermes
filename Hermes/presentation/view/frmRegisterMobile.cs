using Hermes.locatable_resources;
using Hermes.model.data;
using Hermes.model.data.repository;
using Hermes.model.repository;
using Hermes.presentation.presenter;
using Hermes.tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hermes.presentation.view
{
    public partial class frmRegisterMobile : Form
    {
        #region variables
        public string strNameMenu;
        private ControlManager controlManager = new ControlManager();
        private StatusStrip status;
        private int step = 0;
        private CriptoUtil criptoUtil = new CriptoUtil();
        private HttpToolsPresenter httpToolsPresenter = new HttpToolsPresenter();
        private delegate void MessageResultDelegate(string resultInput, StatusStrip ssMain);
        private MessageResultDelegate delegateCatchResult;
        IDisposable subscriptionRequest;
        IDisposable subscriptionRequestError;
        #endregion
        #region constants
        public const string USER_MOBILE_KEY = "user_mobile";
        public const string PASSWORD_MOBILE_KEY = "password_mobile";
        public const string TOKEN_MOBILE_KEY = "token_mobile";
        public const string LICENSE_ID_MOBILE_KEY = "license_id_mobile";
        public const string USER_ID_MOBILE_KEY = "user_id_mobile";
        public const string ID_CONFIGURATION_MOBILE_KEY = "configuration_id_mobile";
        #endregion
        public frmRegisterMobile()
        {
            InitializeComponent();
        }

        private void frmRegisterMobile_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen
                .WorkingArea.Width - this.Width) / 2);
            status = controlManager.getStatusStripMain(mdiMain.NAME);
            initControls();
            subscriptionReactive();
        }

        private void initControls()
        {
            this.delegateCatchResult = new MessageResultDelegate(addMessageResult);
        }

        private void subscriptionReactive()
        {

            subscriptionRequest = httpToolsPresenter
                .subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionRequestError = httpToolsPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));           
        }

        private void launchMessageStatus(string result)
        {            
            this.Invoke(this.delegateCatchResult,
               new Object[] { result, status });
        }
        private void addMessageResult(string resultInput, StatusStrip ssMain)
        {

            if (!string.IsNullOrEmpty(resultInput))
            {
                string id = httpToolsPresenter
                    .getValueOfKeyResponseJson(resultInput, "id");
                if (!string.IsNullOrEmpty(id)){
                    switch (this.step)
                    {
                        case 0:
                            this.step++;
                            saveUserMobileRegistry(id);
                            executeRequest(buildParamsLicense(id), 
                                HttpTools.SERVICE_REGISTER_LICENSE);
                            break;
                        case 1:
                            this.step++;
                            saveIdLicenseRegistry(id);
                            executeRequest(buildParamsConfiguration(id),
                                HttpTools.SERVICE_REGISTER_CONFIGURATION);
                            break;
                        case 2:
                            controlManager.stopProgressStatusStrip(1, ssMain);
                            this.step = 0;
                            saveIdConfigurationRegistry(id);
                            break;
                        default:
                            break;
                    }
                }else
                {
                    controlManager.stopProgressStatusStrip(1, ssMain);
                }
               
                Console.Write("Id User:" + id + " Step: " + this.step.ToString() + "\n");  
                              
                controlManager.setValueTextStatusStrip(StringResources
                    .result_ok, 0, ssMain);               
            }
            else
            {
                controlManager.stopProgressStatusStrip(1, ssMain);
            }

        }

        private void saveIdConfigurationRegistry(string id)
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();
            registryValueDataReader.setKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                ID_CONFIGURATION_MOBILE_KEY,
                id);
        }

        private void saveIdLicenseRegistry(string id)
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();
            registryValueDataReader.setKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                LICENSE_ID_MOBILE_KEY,
                id);
        }
        private void saveUserMobileRegistry(string id)
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();
            registryValueDataReader.setKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                USER_MOBILE_KEY,
                txtUser.Text);
            string password = criptoUtil.encript(txtPassword.Text.ToString());
            registryValueDataReader.setKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                PASSWORD_MOBILE_KEY,
                password);
            registryValueDataReader.setKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                TOKEN_MOBILE_KEY,
                "1");
            registryValueDataReader.setKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                USER_ID_MOBILE_KEY,
                id);
            
        }
        private void launchMessageError(string result)
        {
            MessageBox.Show(result);
        }

        private bool validateInput()
        {
            if (!string.IsNullOrEmpty(txtUser.Text.ToString().Trim()) &&
                !string.IsNullOrEmpty(txtDescription.Text.ToString().Trim()) &&
                !string.IsNullOrEmpty(txtPassword.Text.ToString().Trim()))
            {
                return true;
            }

            return false;
        }

        private Dictionary<string, object> buildParamsConfiguration(string id)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.FIELD_APP_ID, id);
            mapParams.Add(User.FIELD_URL_VIDEO, "http");            
            return mapParams;
        }

        private Dictionary<string, object> buildParams()
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.FIELD_NAME, txtUser.Text);
            mapParams.Add(User.FIELD_PASS, txtPassword.Text);
            mapParams.Add(User.FIELD_DESCRIPTION, txtDescription.Text);
            return mapParams;
        }

        private Dictionary<string, object> buildParamsLicense(string idUser)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.FIELD_NAME, "hermes");
            mapParams.Add(User.FIELD_QUANTITY, "1");
            mapParams.Add(User.FIELD_LICENSE, "123456");
            mapParams.Add(User.FIELD_USER, idUser);
            return mapParams;
        }
        private void executeRequest(Dictionary<string, object> mapParams, 
            string service)
        {
            httpToolsPresenter.setBody(mapParams);
            httpToolsPresenter.setService(service);
            httpToolsPresenter.sendRequest();
        }

        private void frmRegisterMobile_FormClosing(object sender, FormClosingEventArgs e)
        {
            subscriptionRequest.Dispose();
            subscriptionRequestError.Dispose();
            controlManager.setValueTextStatusStrip("", 0, status);
            controlManager.enabledOptionMenu(strNameMenu, mdiMain.NAME);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (validateInput())
            {
                controlManager.startProgressStatusStrip(1, status);
                executeRequest(buildParams(), HttpTools.SERVICE_REGISTER_USER);
            }
            else
            {
                controlManager.setValueTextStatusStrip(StringResources.params_empty,
                    0, status);
            }
        }
    }
}

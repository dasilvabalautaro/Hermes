using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hermes.locatable_resources;
using Hermes.model.data;
using Hermes.presentation.presenter;
using Hermes.tools;

namespace Hermes.presentation.view
{
    public partial class frmLogin : Form
    {
        #region variables
        public string strNameMenu;
        private ControlManager controlManager = new ControlManager();
        private StatusStrip status;
        private AccessUserPresenter accessUserPresenter = new AccessUserPresenter();
        private bool result;
        private delegate void MessageStatusDelegate(bool resultInput, StatusStrip ssMain);
        private MessageStatusDelegate delegateCatchMessage;
        IDisposable subscriptionAccess;
        IDisposable subscriptionAccessError;
        #endregion
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2);
            status = controlManager.getStatusStripMain(mdiMain.NAME);
            this.delegateCatchMessage = new MessageStatusDelegate(addMessageStatus);
            subscriptionReactive();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        private void addMessageStatus(bool resultInput, StatusStrip ssMain)
        {
            if (resultInput)
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .user_ok, 0, ssMain);
                setMenu();
                this.Close();
            }else
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .user_nok, 0, ssMain);
            }

        }

        private void setMenu()
        {
            if(User.instance.Type == 1)
            {
                controlManager.enabledMenuAdministrator(mdiMain.NAME);
            }
            if (User.instance.Type == 2)
            {
                controlManager.enabledMenuOperator(mdiMain.NAME);
            }
        }

        private void subscriptionReactive()
        {

            subscriptionAccess = accessUserPresenter.subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionAccessError = accessUserPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));

        }

        private void launchMessageError(string result)
        {
            MessageBox.Show(result);
        }

        private void launchMessageStatus(bool result)
        {
            this.result = result;
            this.Invoke(this.delegateCatchMessage,
               new Object[] { this.result, status });
        }

        private bool validateInput()
        {
            if (string.IsNullOrEmpty(txtUser.Text) ||
                string.IsNullOrEmpty(txtPassword.Text))
            {
                return false;
            }
            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Dictionary<string, object> buildParams()
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.PARAM_USERNAME, txtUser.Text.ToString().Trim());
            //string password = criptoUtil.encript(txtPassword.Text.ToString().Trim());
            //Console.WriteLine(password);
            //mapParams.Add(User.PARAM_PASS, password);
            accessUserPresenter.Password = txtPassword.Text.ToString().Trim();
            return mapParams;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            controlManager.setValueTextStatusStrip("", 0, status);
            if (validateInput())
            {
                accessUserPresenter.setParams(buildParams());
                Task t = Task.Factory.StartNew(new
                    Action(accessUserPresenter.getUser));
            }else
            {
                controlManager.setValueTextStatusStrip(StringResources.params_empty,
                    0, status);
            }
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            subscriptionAccess.Dispose();
            subscriptionAccessError.Dispose();            
            controlManager.enabledOptionMenu(strNameMenu, mdiMain.NAME);
        }
    }
}

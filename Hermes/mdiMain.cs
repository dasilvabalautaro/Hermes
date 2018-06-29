using Hermes.locatable_resources;
using Hermes.model.data.repository;
using Hermes.presentation.presenter;
using Hermes.presentation.view;
using Hermes.tools;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hermes
{
    public partial class mdiMain : Form
    {
        #region constants
        public const string NAME = "mdiMain";
        public const string MENU_PRODUCTS = "productosToolStripMenuItem";
        public const string MENU_CONFIGURATION = "herramientasToolStripMenuItem";
        public const string MENU_USERS = "usuariosToolStripMenuItem";
        public const string MENU_REGISTER = "registroToolStripMenuItem";
        public const string MENU_SESION = "cerrarSesiónToolStripMenuItem";
        public const string MENU_FILE = "archivoToolStripMenuItem";
        public const string MENU_DATABASE = "sQLServerToolStripMenuItem";
        public const string MENU_ACCESS = "ingresarToolStripMenuItem";
        public const string MENU_MOBILE = "mobileToolStripMenuItem";
        #endregion

        #region variables
        private ControlManager controlManager = new ControlManager();
        private AccessUserPresenter accessUserPresenter = new AccessUserPresenter();
        private bool result;
        private delegate void MessageStatusDelegate(bool resultInput, StatusStrip ssMain);
        private MessageStatusDelegate delegateCatchMessage;
        private HttpToolsPresenter httpToolsPresenter = new HttpToolsPresenter();
        private delegate void MessageResultDelegate(string resultInput, StatusStrip ssMain);
        private MessageResultDelegate delegateCatchResult;
        IDisposable subscriptionRequest;
        IDisposable subscriptionRequestError;
        IDisposable subscriptionAccess;
        IDisposable subscriptionAccessError;
        #endregion

        public mdiMain()
        {
            InitializeComponent();
        }

        private void mdiMain_Load(object sender, EventArgs e)
        {
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2);
            controlManager.createStatusBar(this, ssMain);
            this.delegateCatchMessage = new MessageStatusDelegate(addMessageStatus);
            this.delegateCatchResult = new MessageResultDelegate(addMessageResult);
            subscriptionReactive();
        }

        private void addMessageResult(string resultInput, StatusStrip ssMain)
        {
            string token = httpToolsPresenter
                    .getValueOfKeyResponseJson(resultInput, "token");
            if (!string.IsNullOrEmpty(token))
            {
                saveTokenConfigurationRegistry(token);
            }
        }

        private void saveTokenConfigurationRegistry(string token)
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();
            registryValueDataReader.setKeyValueRegistry(RepositorySqlServer.PATH_KEY,
                frmRegisterMobile.TOKEN_MOBILE_KEY,
                token);
        }

        private void verifyDatabase()
        {
            Task t = Task.Factory.StartNew(new
                    Action(accessUserPresenter.isExistDatabase));
        }
        private void addMessageStatus(bool resultInput, StatusStrip ssMain)
        {

            if (resultInput)
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .database_ok, 0, ssMain);
                ingresarToolStripMenuItem.PerformClick();
                
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
            subscriptionRequest = httpToolsPresenter
                .subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionRequestError = httpToolsPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));
        }

        private void launchMessageError(string result)
        {
            controlManager.enabledMenuCreateDB(mdiMain.NAME);
            MessageBox.Show(result);
        }

        private void launchMessageStatus(bool result)
        {
            this.result = result;
            this.Invoke(this.delegateCatchMessage,
               new Object[] { this.result, ssMain });
        }

        private void launchMessageStatus(string result)
        {            
            this.Invoke(this.delegateCatchResult,
               new Object[] {result, ssMain });
        }

        private void mdiMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            subscriptionAccess.Dispose();
            subscriptionAccessError.Dispose();
        }

        private void mdiMain_Shown(object sender, EventArgs e)
        {
            verifyDatabase();
            getToken();
        }

        private void getToken()
        {
            Task t = Task.Factory.StartNew(new
                    Action(httpToolsPresenter.getToken));
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void ingresarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlManager.setValueTextStatusStrip("", 0, ssMain);
            frmLogin frmWork = new frmLogin() { MdiParent = this };
            frmWork.strNameMenu = "ingresarToolStripMenuItem";
            ingresarToolStripMenuItem.Enabled = false;
            frmWork.Show();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlManager.setValueTextStatusStrip("", 0, ssMain);
            frmRegisterUser frmWork = new frmRegisterUser() { MdiParent = this };
            frmWork.strNameMenu = "usuariosToolStripMenuItem";
            usuariosToolStripMenuItem.Enabled = false;
            frmWork.Show();
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlManager.setValueTextStatusStrip("", 0, ssMain);
            frmRegisterSales frmWork = new frmRegisterSales()
            {
                MdiParent = this
            };
            frmWork.strNameMenu = "ventasToolStripMenuItem";
            ventasToolStripMenuItem.Enabled = false;
            frmWork.Show();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlManager.setValueTextStatusStrip("", 0, ssMain);
            frmProduct frmWork = new frmProduct()
            {
                MdiParent = this
            };
            frmWork.strNameMenu = "productosToolStripMenuItem";
            productosToolStripMenuItem.Enabled = false;
            frmWork.Show();
        }

        private void sQLServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfigurationDatabase frmWork = new frmConfigurationDatabase() { MdiParent = this };
            frmWork.strNameMenu = "sQLServerToolStripMenuItem";
            sQLServerToolStripMenuItem.Enabled = false;
            frmWork.Show();
        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBrowser frmWork = new frmBrowser() { MdiParent = this };
            frmWork.strNameMenu = "videoToolStripMenuItem";
            videoToolStripMenuItem.Enabled = false;
            frmWork.Show();
        }

        private void mobileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegisterMobile frmWork = new frmRegisterMobile() { MdiParent = this };
            frmWork.strNameMenu = "mobileToolStripMenuItem";
            mobileToolStripMenuItem.Enabled = false;
            frmWork.Show();
        }
    }
}

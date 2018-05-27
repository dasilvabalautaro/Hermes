using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hermes.locatable_resources;
using Hermes.model.data.repository;
using Hermes.presentation.presenter;
using Hermes.tools;

namespace Hermes.presentation.view
{
    public partial class frmConfigurationDatabase : Form
    {
        #region constants
        private const int TIME_OUT = 20;
        private const string TBL_MASTER = "master";
        #endregion
        #region variables
        private CreateDatabasePresenter createDatabasePresenter = new CreateDatabasePresenter();
        private GetServersPresenter getServersPresenter = new GetServersPresenter();
        private ConnectDatabasePresenter connectDatabasePresenter = new ConnectDatabasePresenter();
        private ControlManager controlManager = new ControlManager();
        private StatusStrip status;
        private bool result;
        private delegate void MessageStatusDelegate(bool resultInput, StatusStrip ssMain);
        private MessageStatusDelegate delegateCatchMessage;
        public string strNameMenu;
        public delegate void AddListDelegate(List<string> list);
        public AddListDelegate listDelegate;
        IDisposable subscriptionConnect;
        IDisposable subscription;
        IDisposable subscriptionConnectError;
        IDisposable subscriptionCreate;
        IDisposable subscriptionCreateError;
        //CompositeDisposable disposable = new CompositeDisposable();
        #endregion
        public frmConfigurationDatabase()
        {
            InitializeComponent();
           

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

        private void frmConfigurationDatabase_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2);
            status = controlManager.getStatusStripMain(mdiMain.NAME);           
            this.listDelegate = new AddListDelegate(addListControl);
            this.delegateCatchMessage = new MessageStatusDelegate(addMessageStatus);
            
            subscriptionReactive();

            getListServers();

        }

        private void subscriptionReactive()
        {
            
            subscription = getServersPresenter.subject.Subscribe(
                        r => setControlWithServers(r),
                        () => Console.WriteLine("Get List Completed."));
            subscriptionConnect = connectDatabasePresenter.subject.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionConnectError = connectDatabasePresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));

            subscriptionCreate = createDatabasePresenter.subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionCreateError = createDatabasePresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));
            
        }

        private void addMessageStatus(bool resultInput, StatusStrip ssMain)
        {

            if (resultInput)
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .result_ok, 0, ssMain);
                if (!cmdCrearDatabase.Enabled)
                {
                    cmdCrearDatabase.Enabled = true;
                    cmdConnectDatabase.Enabled = true;
                }
                
            }
           
        }

        private void getListServers()
        {
            controlManager.startProgressStatusStrip(1, status);
            timer.Enabled = true;
            lblCount.Text = "0";
            Task t = Task.Factory.StartNew(new
                Action(getServersPresenter.getListServers));
        }

        private void addListControl(List<string> list)
        {
            controlManager.setComboBox(cboServers, list);
            cboServers.Refresh();
            lblCount.Text = "Ok";
            timer.Stop();
            timer.Enabled = false;
            controlManager.stopProgressStatusStrip(1, status);
        }
        private void setControlWithServers(List<string> list)
        {
            cboServers.Invoke(this.listDelegate, new Object[] { list });
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Int16 i = 0;
            bool flagError = false;
            try
            {
                i = Convert.ToInt16(lblCount.Text);
                i += 1;
                lblCount.Text = Convert.ToString(i);           
            }
            catch (FormatException f)
            {
                Console.WriteLine(f.Message);
                flagError = true;
            }
            catch (OverflowException o)
            {
                Console.WriteLine(o.Message);
                flagError = true;
            }
            finally
            {
                if (i == TIME_OUT || flagError) timer.Stop();
            }
        }    

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Dictionary<string, object> buildParams(string nameDatabase)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            
            if (rbAuthenticationSQLServer.Checked == true)
            {
                if (txtLogin.TextLength != 0 & txtPassword.TextLength != 0)
                {
                    mapParams.Add(RepositorySqlServer.INTEGRATED_SECURITY_KEY, 0);
                    mapParams.Add(RepositorySqlServer.USER_KEY, txtLogin.Text.Trim());
                    mapParams.Add(RepositorySqlServer.PASSWORD_KEY, txtPassword.Text.Trim());

                }
                else
                {
                    throw new ArgumentNullException("Field Empty");             
                   
                }
            }
            else
            {
                mapParams.Add(RepositorySqlServer.INTEGRATED_SECURITY_KEY, 1);
                mapParams.Add(RepositorySqlServer.USER_KEY, "sa");
                mapParams.Add(RepositorySqlServer.PASSWORD_KEY, "");
            }

            if(string.IsNullOrEmpty(nameDatabase.Trim()))
            {
                throw new ArgumentNullException("Field Empty");
            }

            mapParams.Add(RepositorySqlServer.SERVER_NAME_KEY, cboServers.Text);
            mapParams.Add(RepositorySqlServer.NAME_KEY_DATABASE, nameDatabase.Trim());
            mapParams.Add(RepositorySqlServer.DATABASE_NEW, txtNameDatabase.Text.Trim());
            return mapParams;
        }

        private void frmConfigurationDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            subscription.Dispose();
            subscriptionConnect.Dispose();
            subscriptionConnectError.Dispose();
            subscriptionCreate.Dispose();
            subscriptionCreateError.Dispose();
            controlManager.setValueTextStatusStrip("", 0, status);           
            controlManager.enabledOptionSubMenu(strNameMenu, mdiMain.NAME);
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            try
            {
                
                controlManager.setValueTextStatusStrip("", 0, status);
                cmdCrearDatabase.Enabled = false;
                cmdConnectDatabase.Enabled = false;                
                connectDatabasePresenter.setParamsConfiguration(buildParams(TBL_MASTER));
                
                Task t = Task.Factory.StartNew(new
                    Action(connectDatabasePresenter.connectTest));                                              
            }
            catch(ArgumentNullException ae){
                Console.WriteLine(ae.Message);
            }
            
        }

        private void cmdCrearDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                controlManager.setValueTextStatusStrip("", 0, status);
                createDatabasePresenter.setParamsConfiguration(buildParams(TBL_MASTER));

                Task t = Task.Factory.StartNew(new
                    Action(createDatabasePresenter.createDatabase));
            }
            catch (ArgumentNullException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }

        private void cmdConnectDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                controlManager.setValueTextStatusStrip("", 0, status);
                connectDatabasePresenter.setParamsConfiguration(buildParams(txtNameDatabase.Text));

                Task t = Task.Factory.StartNew(new
                    Action(connectDatabasePresenter.connectDatabaseExisting));
            }
            catch (ArgumentNullException ae)
            {
                Console.WriteLine(ae.Message);
            }
        }
    }
}

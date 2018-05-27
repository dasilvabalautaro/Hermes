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
    public partial class frmRegisterUser : Form
    {
        #region variables
        public string strNameMenu;
        private ControlManager controlManager = new ControlManager();        
        private StatusStrip status;
        private CriptoUtil criptoUtil = new CriptoUtil();
        private Operator operatorUser = new Operator();
        private RegisterUserPresenter registerUserPresenter = new RegisterUserPresenter();
        private bool result;
        private List<Operator> listOperators;
        private delegate void MessageStatusDelegate(bool resultInput, StatusStrip ssMain);
        private MessageStatusDelegate delegateCatchMessage;
        private delegate void ListOperatorDelegate(List<Operator> list, 
            ListView lwv);
        private ListOperatorDelegate delegateListOperator;
        IDisposable subscriptionRegister;
        IDisposable subscriptionRegisterError;
        IDisposable subscriptionList;
        #endregion
        public frmRegisterUser()
        {
            InitializeComponent();
        }

        private void frmRegisterUser_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2);
            initControls();
            subscriptionReactive();
            getListOperators();
        }

        private void initControls()
        {
            status = controlManager.getStatusStripMain(mdiMain.NAME);
            lvwUsers.Columns.Add("Usuario", 120, HorizontalAlignment.Left);
            lvwUsers.Columns.Add("Nombres", 160, HorizontalAlignment.Left);
            lvwUsers.Columns.Add("Apellidos", 160, HorizontalAlignment.Left);
            this.delegateCatchMessage = new MessageStatusDelegate(addMessageStatus);
            this.delegateListOperator = new ListOperatorDelegate(addListOperator);
            cboTypeUser.SelectedIndex = -1;
        }

        private void subscriptionReactive()
        {

            subscriptionRegister = registerUserPresenter.subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionRegisterError = registerUserPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));
            subscriptionList = registerUserPresenter
                .subjectOperators.Subscribe(
                        l => launchBuildList(l),
                        () => Console.WriteLine("List Operator."));

        }

        private void getListOperators()
        {
            registerUserPresenter.setSQL(User.instance.SqlList);
            Task t = Task.Factory.StartNew(new
                    Action(registerUserPresenter.getListOperators));
        }

        private void addListOperator(List<Operator> list,
            ListView lwv)
        {
            lwv.Items.Clear();
            foreach (Operator op in list)
            {
                if(op.Id != 1)
                {
                    ListViewItem lvi = new ListViewItem(op.UserName);
                    lvi.SubItems.Add(op.FirstName);
                    lvi.SubItems.Add(op.LastName);
                    lvi.Tag = op.Id;
                    lwv.Items.Add(lvi);
                }
            }
        }

        private void launchBuildList(List<Operator> list)
        {
            this.listOperators = list;
            this.Invoke(this.delegateListOperator,
               new Object[] { this.listOperators, lvwUsers});
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
        private void addMessageStatus(bool resultInput, StatusStrip ssMain)
        {

            if (resultInput)
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .result_ok, 0, ssMain);
                getListOperators();
                clearControls();

            }

        }

        private void clearControls()
        {
            txtLastNames.Text = string.Empty;
            txtNames.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserName.Text = string.Empty;
            cboTypeUser.SelectedIndex = -1;
        }

        private Dictionary<string, object> buildParamsAdd()
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.PARAM_USERTYPEID, operatorUser.Type + 1);
            mapParams.Add(User.PARAM_USERNAME, operatorUser.UserName);
            string password = criptoUtil.encript(operatorUser.Password);
            //string d = criptoUtil.desencript(password);
            mapParams.Add(User.PARAM_PASS, password);
            mapParams.Add(User.PARAM_FIRSTNAME, operatorUser.FirstName);
            mapParams.Add(User.PARAM_LASTNAMES, operatorUser.LastName);

            return mapParams;
        }

        private Dictionary<string, object> buildParamsDelete()
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(User.PARAM_USERID, this.operatorUser.Id);           

            return mapParams;
        }

        private bool validateInput()
        {
            if (!string.IsNullOrEmpty(txtNames.Text.ToString().Trim()) &&
                !string.IsNullOrEmpty(txtLastNames.Text.ToString().Trim()) &&
                !string.IsNullOrEmpty(txtUserName.Text.ToString().Trim()) &&
                !string.IsNullOrEmpty(txtPassword.Text.ToString().Trim()) &&
                cboTypeUser.SelectedIndex != -1)
            {
                return true;
            }

            return false;
        }

        private void frmRegisterUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            subscriptionRegister.Dispose();
            subscriptionRegisterError.Dispose();
            subscriptionList.Dispose();           
            controlManager.enabledOptionMenu(strNameMenu, mdiMain.NAME);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNames_TextChanged(object sender, EventArgs e)
        {
            operatorUser.FirstName = txtNames.Text.ToString();
        }

        private void txtLastNames_TextChanged(object sender, EventArgs e)
        {
            operatorUser.LastName = txtLastNames.Text.ToString();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            operatorUser.UserName = txtUserName.Text.ToString();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            operatorUser.Password = txtPassword.Text.ToString();
        }

        private void cboTypeUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTypeUser.SelectedIndex != -1)
            {
                operatorUser.Type = cboTypeUser.SelectedIndex;
            }
        }

        private void executeNonSql(Dictionary<string, object> mapParams,
            string sql)
        {
            registerUserPresenter.setParams(mapParams);
            registerUserPresenter.setSQL(sql);
            Task t = Task.Factory.StartNew(new
               Action(registerUserPresenter.executeNonSql));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (validateInput())
            {
                executeNonSql(buildParamsAdd(), User.instance.SqlAdd);
            }
            else
            {
                controlManager.setValueTextStatusStrip(StringResources.params_empty,
                    0, status);
            }
        }

    
        private void lvwUsers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.ItemIndex > -1)
            {
                this.operatorUser = this.listOperators[e.ItemIndex + 1];
                txtLastNames.Text = this.operatorUser.LastName;
                txtNames.Text = this.operatorUser.FirstName;
                txtUserName.Text = this.operatorUser.UserName;
                txtPassword.Text = this.operatorUser.Password;
                txtNames.Tag = this.operatorUser.Id;
                cboTypeUser.SelectedIndex = this.operatorUser.Type - 1;
            }
            
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if(txtNames.Tag != null)
            {
                executeNonSql(buildParamsDelete(), User.instance.SqlDel);
            }
            else
            {
                controlManager.setValueTextStatusStrip(StringResources.params_empty,
                    0, status);
            }
        }
    }
}

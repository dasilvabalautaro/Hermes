using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hermes.model.data;
using Hermes.presentation.presenter;
using Hermes.tools;
using Hermes.locatable_resources;

namespace Hermes.presentation.view
{
    public partial class frmRegisterSales : Form
    {
        #region variables
        public string strNameMenu;
        private ControlManager controlManager = new ControlManager();
        private StatusStrip status;
        private PlugComPresenter plugComPresenter = new PlugComPresenter();
        private RegisterProductPresenter registerProductPresenter = new RegisterProductPresenter();
        private RegisterSalePresenter registerSalePresenter = new RegisterSalePresenter();
        private RegisterPayPresenter registerPayPresenter = new RegisterPayPresenter();
        private Product product = new Product();
        private Sale sale = new Sale();
        private Pay pay = new Pay();
        private List<Product> listProduct;
        private string result;
        private bool resultCrud;
        private delegate void PortDataDelegate(string resultInput, 
            TextBox text);
        private PortDataDelegate delegatePortData;
        private delegate void MessageStatusDelegate(bool resultInput,
            StatusStrip ssMain);
        private MessageStatusDelegate delegateCatchMessage;
        private delegate void GetMaxIdDelegate(int id,
            StatusStrip ssMain);
        private GetMaxIdDelegate delegateCatchMaxId;
        private delegate void MessageStatusPayDelegate(bool resultInput,
           StatusStrip ssMain);
        private MessageStatusPayDelegate delegateCatchMessagePay;
        private delegate void ListProductDelegate(List<Product> list,
            ComboBox cbo);
        private ListProductDelegate delegateListProduct;
        private delegate void ListDebtDelegate(DataTable list,
            ListView lvw);
        private ListDebtDelegate delegateListDebt;

        IDisposable subscriptionPlugData;
        IDisposable subscriptionPlugError;
        IDisposable subscriptionRegister;
        IDisposable subscriptionRegisterError;
        IDisposable subscriptionList;
        IDisposable subscriptionMax;
        IDisposable subscriptionRegisterPay;
        IDisposable subscriptionRegisterPayError;
        IDisposable subscriptionRegisterPayListDebt;
        #endregion
        public frmRegisterSales()
        {
            InitializeComponent();
        }

        private void frmRegisterSales_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2);
            status = controlManager.getStatusStripMain(mdiMain.NAME);
            this.delegatePortData = new PortDataDelegate(addPortData);
            this.delegateCatchMessage = new MessageStatusDelegate(addMessageStatus);
            this.delegateListProduct = new ListProductDelegate(addListProduct);
            this.delegateCatchMessagePay = new MessageStatusPayDelegate(addMessageStatusPay);
            this.delegateCatchMaxId = new GetMaxIdDelegate(getMaxIdSale);
            this.delegateListDebt = new ListDebtDelegate(getListDebt);
            subscriptionReactive();
            initControls();
            getListProducts();
        }

        private void initControls()
        {
            lvwPays.Columns.Add("Pago", 100, HorizontalAlignment.Center);
            lvwPays.Columns.Add("Monto", 180, HorizontalAlignment.Center);
            lvwPays.Tag = 0;
            lvwDebts.Columns.Add("Código", 100, HorizontalAlignment.Center);
            lvwDebts.Columns.Add("Saldo", 180, HorizontalAlignment.Center);
            txtUser.Text = User.instance.FirstName +
                " " + User.instance.LastName;
            txtDate.Text = controlManager.setDateLocale();
            txtQuantity.Text = "1";
            sale.UserId = User.instance.Id;
            pay.UserId = User.instance.Id;
        }

        private void getListProducts()
        {
            registerProductPresenter.setSQL(product.SqlList);
            Task t = Task.Factory.StartNew(new
                    Action(registerProductPresenter.getListProducts));
        }

        private void addMessageStatus(bool resultInput, StatusStrip ssMain)
        {

            if (resultInput)
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .save_sale, 0, ssMain);
                registerSalePresenter.setSQL(sale.SqlMax);
                Task t = Task.Factory.StartNew(new
                    Action(registerSalePresenter.getMaxId));

                //btnPrint.Enabled = true;
            }

        }

        private void addMessageStatusPay(bool resultInput, StatusStrip ssMain)
        {

            if (resultInput)
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .result_ok, 0, ssMain);               
                btnPrint.Enabled = true;
            }

        }

        private void addPortData(string resultInput, TextBox text)
        {
            text.Text = resultInput;
        }
        private void addListProduct(List<Product> list,
          ComboBox cbo)
        {
            cbo.Items.Clear();
            
            foreach (Product po in list)
            {
                cbo.Items.Add(po.Description);
            }
        }
        private void subscriptionReactive()
        {
            subscriptionRegister = registerSalePresenter.subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionRegisterError = registerSalePresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));

            subscriptionPlugData = plugComPresenter.subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionPlugError = plugComPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));
            subscriptionList = registerProductPresenter
                .subjectProducts.Subscribe(
                        l => launchBuildList(l),
                        () => Console.WriteLine("List Operator."));
            subscriptionMax = registerSalePresenter.subjectMax.Subscribe(
                        r => launchMaxId(r),
                        () => Console.WriteLine("Get Max Completed."));
            subscriptionRegisterPay = registerPayPresenter.subjectResult.Subscribe(
                        r => launchMessageStatusPay(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionRegisterPayError = registerPayPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));
            subscriptionRegisterPayListDebt = registerPayPresenter
                .subjectDatatable.Subscribe(
                        t => launchListDebt(t),
                        () => Console.WriteLine("List Debt Operation."));
        }

        private void launchListDebt(DataTable list)
        {          
            this.Invoke(this.delegateListDebt,
               new Object[] { list, lvwDebts});
        }   

        private void launchMaxId(int max)
        {           
            this.Invoke(this.delegateCatchMaxId,
               new Object[] { max, status});           
        }

        private void getListDebt(DataTable list, ListView lvw)
        {
            lvw.Items.Clear();
            foreach (DataRow dr in list.Rows)
            {
                ListViewItem item = new ListViewItem(dr[0].ToString(),
                    lvw.Items.Count);
                item.SubItems.Add(dr[1].ToString());
                lvw.Items.Add(item);
            }
               
        }

        private void getMaxIdSale(int id, StatusStrip ssMain)
        {
            if (id != 0)
            {
                pay.IdSale = id;
                if (getSumPays() > 0)
                {
                    controlManager.setValueTextStatusStrip("", 0, ssMain);
                    registerPayPresenter.paramsForAddPay(pay);
                    registerPayPresenter.setSQL(pay.SqlAdd);
                    Task t = Task.Factory.StartNew(new
                        Action(registerPayPresenter.executeNonSql));
                }
                else
                {
                    controlManager.setValueTextStatusStrip(StringResources
                    .result_ok, 0, ssMain);
                    btnPrint.Enabled = true;
                }
            }
        }

        private void launchMessageStatus(bool result)
        {
            this.resultCrud = result;
            this.Invoke(this.delegateCatchMessage,
               new Object[] { this.resultCrud, status });
        }

        private void launchMessageStatusPay(bool result)
        {
            this.resultCrud = result;
            this.Invoke(this.delegateCatchMessagePay,
               new Object[] { this.resultCrud, status });
        }

        private void launchBuildList(List<Product> list)
        {
            this.listProduct = list;
            this.Invoke(this.delegateListProduct,
               new Object[] { this.listProduct, cboProduct });
        }

        private void launchMessageError(string result)
        {
            MessageBox.Show(result);
        }

        private void launchMessageStatus(string result)
        {
            this.result = result;
            this.Invoke(this.delegatePortData,
               new Object[] { this.result, txtDataCaptured});
        }

        private void openPort()
        {
            plugComPresenter.plugCom.PortName = "COM3";
            plugComPresenter.openPort();
        }

        private void frmRegisterSales_FormClosing(object sender, FormClosingEventArgs e)
        {           
            subscriptionPlugError.Dispose();
            subscriptionPlugData.Dispose();            
            subscriptionRegister.Dispose();
            subscriptionRegisterError.Dispose();
            subscriptionList.Dispose();
            subscriptionMax.Dispose();
            subscriptionRegisterPay.Dispose();
            subscriptionRegisterPayError.Dispose();
            plugComPresenter.Dispose();
            controlManager.setValueTextStatusStrip("", 0, status);
            controlManager.enabledOptionMenu(strNameMenu, mdiMain.NAME);
        }

        private void frmRegisterSales_Shown(object sender, EventArgs e)
        {
            openPort();
            executeGetDebts();
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboProduct.SelectedIndex != -1)
            {
                Product pdt = listProduct[cboProduct.SelectedIndex];
                txtPrice.Text = pdt.Price.ToString();
                txtQuantity.Text = "1";
                sale.Product = pdt.Id;
                sale.Price = pdt.Price;
            }
            
             
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || (char.IsControl(e.KeyChar)))
            {               
                e.Handled = false;

            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtQuantity_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuantity.Text.ToString()))
            {
                txtQuantity.Text = "1";
            }
            sale.Quantity = Convert.ToInt16(txtQuantity.Text.ToString());
        }

        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            float value = Convert.ToSingle(txtPrice.Text.ToString()) *
                Convert.ToSingle(txtWeight.Text.ToString());
            txtTotal.Text = value.ToString();
            sale.Weight = Convert.ToSingle(txtWeight.Text.ToString());
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            sale.Total = Convert.ToSingle(txtTotal.Text.ToString());
            txtDifPays.Text = Convert.ToString(getDifPays());
        }

        private void txtClient_Validated(object sender, EventArgs e)
        {
            sale.Client = txtClient.Text.ToString();
        }

        private void txtObservations_Validated(object sender, EventArgs e)
        {
            sale.Observations = txtObservations.Text.ToString();
        }

        private bool validateInput()
        {
            if ((txtWeight.Text.ToString().Trim() != "0") &&
                (cboProduct.SelectedIndex != -1 && 
                !string.IsNullOrEmpty(cboProduct.Text.ToString())))
            {
                return true;
            }

            return false;
        }

        private void btnAddSale_Click(object sender, EventArgs e)
        {
            if (validateInput())
            {
                btnAddSale.Enabled = false;
                controlManager.setValueTextStatusStrip("", 0, status);
                registerSalePresenter.paramsForAddSale(sale);
                registerSalePresenter.setSQL(sale.SqlAdd);
                Task t = Task.Factory.StartNew(new
                    Action(registerSalePresenter.executeNonSql));               
            }
            else
            {
                controlManager.setValueTextStatusStrip(StringResources.params_empty,
                    0, status);
            }
        }

        private void clearControls()
        {
            cboProduct.SelectedIndex = -1;
            txtPrice.Text = "0";
            txtQuantity.Text = "1";
            txtWeight.Text = "0";
            txtTotal.Text = "0";
            txtClient.Text = string.Empty;
            txtObservations.Text = string.Empty;
            txtSumPays.Text = "0";
            txtDifPays.Text = "0";
            btnPrint.Enabled = false;
            lvwPays.Items.Clear();
            btnAddSale.Enabled = true;
            lvwPays.Tag = 0;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            clearControls();
        }

        private void btnWeight_Click(object sender, EventArgs e)
        {
            txtWeight.Text = txtDataCaptured.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) ||
                (char.IsControl(e.KeyChar)) ||
                e.KeyChar == Convert.ToChar("."))
            {
                e.Handled = false;

            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtPay_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPay.Text.ToString()))
            {
                txtPay.Text = "0";
            }
        }

        private void btnAddPay_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt16(lvwPays.Tag) == 0)
            {
                lvwPays.Tag = 1;
                addPayList();
            }else
            {
                updatePayList();
            }
            
        }

        private void addPayList()
        {
            ListViewItem item = new ListViewItem((lvwPays.Items.Count + 1).ToString(),
                lvwPays.Items.Count);
            item.SubItems.Add(txtPay.Text);
            lvwPays.Items.Add(item);
            txtSumPays.Text = Convert.ToString(getSumPays());
            txtDifPays.Text = Convert.ToString(getDifPays());
            pay.Mount = Convert.ToSingle(txtPay.Text.ToString());
            txtPay.Text = "0";
        }

        private void updatePayList()
        {
            lvwPays.Items[lvwPays.Items.Count - 1]
                   .SubItems[1].Text = txtPay.Text;
            txtSumPays.Text = Convert.ToString(getSumPays());
            txtDifPays.Text = Convert.ToString(getDifPays());
            pay.Mount = Convert.ToSingle(txtPay.Text.ToString());
            txtPay.Text = "0";
        }

        private decimal getSumPays()
        {
            decimal decReturn = 0;
            foreach (ListViewItem lvi in lvwPays.Items)
            {
                decReturn += Convert.ToDecimal(lvi.SubItems[1].Text);
            }
            return decReturn;
        }

        private decimal getDifPays()
        {
            return (Convert.ToDecimal(txtTotal.Text) - 
                Convert.ToDecimal(txtSumPays.Text));
        }

        private void lvwPays_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                txtSumPays.Text = Convert.ToString(Convert.
                    ToDecimal(txtSumPays.Text) - 
                    Convert.ToDecimal(lvwPays
                    .SelectedItems[0].SubItems[1].Text));
                lvwPays.SelectedItems[0].Remove();
                lvwPays.Refresh();
            }
        }

        private void executeGetDebts()
        {
            registerPayPresenter.setSQL(pay.SqlList);
            Task t = Task.Factory.StartNew(new
                Action(registerPayPresenter.getListDebt));
        }

        private void lvwDebts_ItemSelectionChanged(object sender, 
            ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int id = Convert.ToInt16(e.Item.Text);
                Console.Write(id);
            }
        }
    
    }
}

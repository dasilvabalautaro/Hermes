using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hermes.locatable_resources;
using Hermes.model.data;
using Hermes.presentation.presenter;
using Hermes.tools;

namespace Hermes.presentation.view
{
    public partial class frmProduct : Form
    {
        #region variables
        public string strNameMenu;
        private ControlManager controlManager = new ControlManager();
        private StatusStrip status;
        private RegisterProductPresenter registerProductPresenter = new RegisterProductPresenter();
        private bool result;
        private Product product = new Product();
        private List<Product> listProduct;
        private delegate void MessageStatusDelegate(bool resultInput, 
            StatusStrip ssMain);
        private MessageStatusDelegate delegateCatchMessage;
        private delegate void ListProductDelegate(List<Product> list,
            ListView lwv);
        private ListProductDelegate delegateListProduct;
        IDisposable subscriptionRegister;
        IDisposable subscriptionRegisterError;
        IDisposable subscriptionList;
        #endregion
        public frmProduct()
        {
            InitializeComponent();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            this.Top = 0;
            this.Left = (int)((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2);
            initControls();
            subscriptionReactive();
            getListProducts();

        }

        private void initControls()
        {
            status = controlManager.getStatusStripMain(mdiMain.NAME);
            lvwProduct.Columns.Add("Producto", 220, HorizontalAlignment.Left);
            lvwProduct.Columns.Add("Precio", 60, HorizontalAlignment.Left);           
            this.delegateCatchMessage = new MessageStatusDelegate(addMessageStatus);
            this.delegateListProduct = new ListProductDelegate(addListProduct);
           
        }
        private void addListProduct(List<Product> list,
           ListView lwv)
        {
            lwv.Items.Clear();
            foreach (Product po in list)
            {               
                    ListViewItem lvi = new ListViewItem(po.Description);
                    lvi.SubItems.Add(po.Price.ToString());                   
                    lvi.Tag = po.Id;
                    lwv.Items.Add(lvi);
               
            }
        }
        private void addMessageStatus(bool resultInput, StatusStrip ssMain)
        {

            if (resultInput)
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .result_ok, 0, ssMain);
                getListProducts();
                clearControls();
            }

        }

        private void clearControls()
        {
            
            txtProduct.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtProduct.Tag = 0;
        }

        private void subscriptionReactive()
        {

            subscriptionRegister = registerProductPresenter.subjectResult.Subscribe(
                        r => launchMessageStatus(r),
                        () => Console.WriteLine("Operation Completed."));
            subscriptionRegisterError = registerProductPresenter
                .subjectError.Subscribe(
                        s => launchMessageError(s),
                        () => Console.WriteLine("Error Operation."));
            subscriptionList = registerProductPresenter
                .subjectProducts.Subscribe(
                        l => launchBuildList(l),
                        () => Console.WriteLine("List Operator."));

        }

        private void launchMessageStatus(bool result)
        {
            this.result = result;
            this.Invoke(this.delegateCatchMessage,
               new Object[] { this.result, status });
        }

        private void launchMessageError(string result)
        {
            MessageBox.Show(result);
        }
        private void getListProducts()
        {
            registerProductPresenter.setSQL(product.SqlList);
            Task t = Task.Factory.StartNew(new
                    Action(registerProductPresenter.getListProducts));
        }

        private void launchBuildList(List<Product> list)
        {
            this.listProduct = list;
            this.Invoke(this.delegateListProduct,
               new Object[] { this.listProduct, lvwProduct });
        }

        private Dictionary<string, object> buildParamsAdd()
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(Product.PARAM_PRICE, product.Price);
            mapParams.Add(Product.PARAM_DESCRIPTION, product.Description);         
            return mapParams;
        }

        private Dictionary<string, object> buildParamsDelete()
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(Product.PARAM_ID, this.product.Id);

            return mapParams;
        }

        private Dictionary<string, object> buildParamsUpate(string prm, 
            object value)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(Product.PARAM_ID, this.product.Id);
            mapParams.Add(prm, value);
            return mapParams;
        }

        private bool validateInput()
        {
            if (!string.IsNullOrEmpty(txtProduct.Text.ToString().Trim()) &&
                !string.IsNullOrEmpty(txtPrice.Text.ToString().Trim()))
            {
                return true;
            }

            return false;
        }

        private void frmProduct_FormClosing(object sender, FormClosingEventArgs e)
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

        private void updateProduct()
        {         
            if(txtProduct.Text.ToString().Trim() != product.Description)
            {
                product.Description = txtProduct.Text.ToString().Trim();
                product.updateProduct(Product.FIELD_DESCRIPTION,
                   Product.PARAM_DESCRIPTION);
                executeNonSql(buildParamsUpate(Product.PARAM_DESCRIPTION,
                    product.Description), product.SqlUpdate);
            }
            if(Convert.ToSingle(txtPrice.Text.ToString()) != product.Price)
            {
                product.Price = Convert.ToSingle(txtPrice.Text.ToString());
                product.updateProduct(Product.FIELD_PRICE,
                    Product.PARAM_PRICE);
                executeNonSql(buildParamsUpate(Product.PARAM_PRICE,
                    product.Price), product.SqlUpdate);
            }

        }
         
        private void executeNonSql(Dictionary<string, object> mapParams,
           string sql)
        {
            registerProductPresenter.setParams(mapParams);
            registerProductPresenter.setSQL(sql);
            Task t = Task.Factory.StartNew(new
               Action(registerProductPresenter.executeNonSql));
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (validateInput())
            {
                product.Description = txtProduct.Text.ToString().Trim();
                product.Price = Convert.ToSingle(txtPrice.Text.ToString());
                executeNonSql(buildParamsAdd(), product.SqlAdd);
            }
            else
            {
                controlManager.setValueTextStatusStrip(StringResources.params_empty,
                    0, status);
            }
        }

        private void lvwProduct_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                this.product = this.listProduct[e.ItemIndex];
                txtProduct.Text = this.product.Description;
                txtPrice.Text = this.product.Price.ToString();
                txtProduct.Tag = this.product.Id;

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(txtProduct.Tag) != 0)
            {
                executeNonSql(buildParamsDelete(), product.SqlDel);
            }
            else
            {
                controlManager.setValueTextStatusStrip(StringResources
                    .params_empty,
                    0, status);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(txtProduct.Tag) != 0 && validateInput())
            {
                controlManager.setValueTextStatusStrip("", 0, status);
                btnRefresh.Enabled = false;
                updateProduct();
                clearControls();
                Thread.Sleep(1000);
                getListProducts();
                btnRefresh.Enabled = true;
            }
            else
            {
                controlManager.setValueTextStatusStrip(StringResources
                .update_invalid, 0, status);
            }
        }
      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Data;
using System.Threading.Tasks;
using Hermes.domain.interactor;
using Hermes.model.data;
using System.Globalization;

namespace Hermes.presentation.presenter
{
    class RegisterProductPresenter
    {
        #region variables
        CRUDUseCase crudUseCase = new CRUDUseCase();
        public Subject<bool> subjectResult = new Subject<bool>();
        public Subject<string> subjectError = new Subject<string>();
        public Subject<List<Product>> subjectProducts = new Subject<List<Product>>();
        private List<Product> listProduct = new List<Product>();
        #endregion

        #region methods
        public RegisterProductPresenter()
        {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(sendResult);
            crudUseCase.OnDatatableDatabase += new CRUDUseCase
                .ResultDatatableDelegate(setProducts);
        }

        private void sendResult(bool result)
        {

            subjectResult.OnNext(result);

        }

        private void setProducts(DataTable dataProducts)
        {
            this.listProduct.Clear();
            foreach (DataRow dr in dataProducts.Rows)
            {

                Product product = new Product();
                product.Id = Convert.ToInt32(dr["id"]);
                product.Description = dr["description"].ToString();
                product.Price = Convert.ToSingle(dr["price"]);
                this.listProduct.Add(product);
            }

            subjectProducts.OnNext(this.listProduct);
        }

        public void getListProducts()
        {
            try
            {
                crudUseCase.executeGetList();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);

            }
        }

        public void setParams(Dictionary<string, object> mapParams)
        {
            crudUseCase.addParams(mapParams);
        }

        public void setSQL(string sql)
        {
            crudUseCase.Sql = sql;
        }

        public void executeNonSql()
        {
            try
            {
                crudUseCase.executeNonSql();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);

            }
        }


        #endregion
    }
}

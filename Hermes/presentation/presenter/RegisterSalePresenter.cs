using Hermes.domain.interactor;
using Hermes.model.data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.presentation.presenter
{
    class RegisterSalePresenter
    {
        #region variables
        CRUDUseCase crudUseCase = new CRUDUseCase();
        public Subject<bool> subjectResult = new Subject<bool>();
        public Subject<string> subjectError = new Subject<string>();
        public Subject<int> subjectMax = new Subject<int>();
        public Subject<Sale> subjectSale = new Subject<Sale>();
       
        #endregion

        #region methods
        public RegisterSalePresenter()
        {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(sendResult);
            crudUseCase.OnDatatableDatabase += new CRUDUseCase
                .ResultDatatableDelegate(setDataList);
        }

        private void sendResult(bool result)
        {

            subjectResult.OnNext(result);

        }

        private Dictionary<string, object> buildParamsAdd(Sale sale)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(Sale.PARAM_PRODUCT, sale.Product);
            mapParams.Add(Sale.PARAM_QUANTITY, sale.Quantity);
            mapParams.Add(Sale.PARAM_WEIGHT, sale.Weight);
            mapParams.Add(Sale.PARAM_CLIENT, sale.Client);
            mapParams.Add(Sale.PARAM_TOTAL, sale.Total);
            mapParams.Add(Sale.PARAM_USER, sale.UserId);
            mapParams.Add(Sale.PARAM_PRICE, sale.Price);
            mapParams.Add(Sale.PARAM_OBSERVATIONS, sale.Observations);
            return mapParams;
        }

        public void paramsForAddSale(Sale sale)
        {
            setParams(buildParamsAdd(sale));
        }

        public void paramsForGetSale(int id)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(Sale.PARAM_ID, id);
            setParams(mapParams);
        }

        private void setParams(Dictionary<string, object> mapParams)
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

        public void getDataTable()
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

        private void setSale(DataRow dataRow)
        {
            Sale sale = new Sale();
            sale.Id = Convert.ToInt32(dataRow[Sale.FIELD_ID]);
            sale.Product = Convert.ToInt32(dataRow[Sale.FIELD_PRODUCT]);
            sale.Quantity = Convert.ToInt32(dataRow[Sale.FIELD_QUANTITY]);
            sale.DateSale = dataRow[Sale.FIELD_DATE].ToString();
            sale.Weight = Convert.ToSingle(dataRow[Sale.FIELD_WEIGHT]);
            sale.Client = dataRow[Sale.FIELD_CLIENT].ToString();
            sale.Price = Convert.ToSingle(dataRow[Sale.FIELD_PRICE]);
            sale.Total = Convert.ToSingle(dataRow[Sale.FIELD_TOTAL]);
            sale.UserId = Convert.ToInt32(dataRow[Sale.FIELD_USER]);
            sale.Observations = dataRow[Sale.FIELD_OBSERVATIONS].ToString();
            subjectSale.OnNext(sale);
        }

        private void setDataList(DataTable dataList)
        {
            int max = 0;

            if(dataList.Rows.Count > 0)
            {
                if(dataList.Rows[0].ItemArray.Length > 1)
                {
                    setSale(dataList.Rows[0]);
                }
                else
                {
                    max = Convert.ToInt32(dataList.Rows[0].ItemArray[0]);
                    subjectMax.OnNext(max);
                }                
            }            
        }

        #endregion
    }
}

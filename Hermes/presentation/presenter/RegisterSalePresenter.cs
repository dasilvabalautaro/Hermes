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
        #endregion

        #region methods
        public RegisterSalePresenter()
        {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(sendResult);
            crudUseCase.OnDatatableDatabase += new CRUDUseCase
                .ResultDatatableDelegate(setMaxId);
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

        public void getMaxId()
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

        private void setMaxId(DataTable dataMax)
        {
            int max = 0;
            if(dataMax.Rows.Count > 0)
            {
                max = Convert.ToInt32(dataMax.Rows[0].ItemArray[0]);

            }

            subjectMax.OnNext(max);
        }

        #endregion
    }
}

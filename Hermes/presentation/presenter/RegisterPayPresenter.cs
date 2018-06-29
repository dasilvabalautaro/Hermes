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
    class RegisterPayPresenter
    {
        #region variables
        CRUDUseCase crudUseCase = new CRUDUseCase();
        public Subject<bool> subjectResult = new Subject<bool>();
        public Subject<string> subjectError = new Subject<string>();
        public Subject<DataTable> subjectDatatable = new Subject<DataTable>();

        #endregion

        #region methods
        public RegisterPayPresenter()
        {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(sendResult);
            crudUseCase.OnDatatableDatabase += new CRUDUseCase
                .ResultDatatableDelegate(setDataTable);
        }

        private void setDataTable(DataTable list)
        {
            subjectDatatable.OnNext(list);
        }
      
        private void sendResult(bool result)
        {

            subjectResult.OnNext(result);

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

        public void paramsForGetPaysOfId(int id)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(Pay.PARAM_ID_SALE, id);
            setParams(mapParams);
        }

        public void paramsForAddPay(Pay pay)
        {
            setParams(buildParamsAdd(pay));
        }

        private Dictionary<string, object> buildParamsAdd(Pay pay)
        {
            Dictionary<string, object> mapParams = new Dictionary<string, object>();
            mapParams.Add(Pay.PARAM_ID_SALE, pay.IdSale);
            mapParams.Add(Pay.PARAM_MOUNT, pay.Mount);
            mapParams.Add(Pay.PARAM_USER, pay.UserId);           
            return mapParams;
        }

        private void setParams(Dictionary<string, object> mapParams)
        {
            crudUseCase.addParams(mapParams);
        }

        public void setSQL(string sql)
        {
            crudUseCase.Sql = sql;
        }

        public void setNameTable(string name)
        {
            crudUseCase.NameTable = name;
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
        #endregion
    }
}

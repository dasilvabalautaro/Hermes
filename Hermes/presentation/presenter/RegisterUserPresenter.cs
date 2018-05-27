using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Hermes.domain.interactor;
using Hermes.model.data;

namespace Hermes.presentation.presenter
{
    class RegisterUserPresenter
    {
        #region variables
        CRUDUseCase crudUseCase = new CRUDUseCase();
        public Subject<bool> subjectResult = new Subject<bool>();
        public Subject<string> subjectError = new Subject<string>();
        public Subject<List<Operator>> subjectOperators = new Subject<List<Operator>>();
        private List<Operator> listUsers = new List<Operator>();
        #endregion

        #region methods
        public RegisterUserPresenter() {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(sendResult);
            crudUseCase.OnDatatableDatabase += new CRUDUseCase
                .ResultDatatableDelegate(setOperators);
        }

        private void sendResult(bool result)
        {

            subjectResult.OnNext(result);

        }

        private void setOperators(DataTable dataOperators)
        {
            this.listUsers.Clear();
            foreach (DataRow dr in dataOperators.Rows)
            {
                Operator operatorUser = new Operator();
                operatorUser.Id = Convert.ToInt32(dr["id"]);
                operatorUser.Type = Convert.ToInt32(dr["type"]);
                operatorUser.UserName = dr["nameUser"].ToString();
                operatorUser.Password = dr["password"].ToString();
                operatorUser.FirstName = dr["firstName"].ToString();
                operatorUser.LastName = dr["lastName"].ToString();
                this.listUsers.Add(operatorUser);
            }

            subjectOperators.OnNext(this.listUsers);
        }

        public void getListOperators()
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

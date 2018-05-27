using System;
using System.Collections.Generic;
using System.Data;
using System.Reactive.Subjects;
using Hermes.domain.interactor;
using Hermes.model.data;
using Hermes.tools;

namespace Hermes.presentation.presenter
{
    class AccessUserPresenter
    {
        #region variables
        CRUDUseCase crudUseCase = new CRUDUseCase();
        private CriptoUtil criptoUtil = new CriptoUtil();
        public Subject<bool> subjectResult = new Subject<bool>();
        public Subject<string> subjectError = new Subject<string>();
        private string password;

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }
        #endregion

        #region methods
        public AccessUserPresenter()
        {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(sendResult);
            crudUseCase.OnDatatableDatabase += new CRUDUseCase
                .ResultDatatableDelegate(setUser);
        }

        private void sendResult(bool result)
        {

            subjectResult.OnNext(result);
            
        }

        private void setUser(DataTable dataUser)
        {
            bool result = false;
                       
            if(dataUser.Rows.Count > 0)
            {
                for(int i = 0; i < dataUser.Rows.Count; i++)
                {
                    DataRow rowUser = dataUser.Rows[i];
                    string pass = rowUser[User.FIELD_PASS].ToString();
                    //string d = criptoUtil.desencript(pass);
                    if (criptoUtil.desencript(pass) == Password)
                    {
                        User.instance.Id = Convert.ToInt16(rowUser[User.FIELD_USERID]);
                        User.instance.Type = Convert.ToInt16(rowUser[User.FIELD_USERTYPEID]);
                        User.instance.UserName = rowUser[User.FIELD_USERNAME].ToString();
                        User.instance.Password = rowUser[User.FIELD_PASS].ToString();
                        User.instance.FirstName = rowUser[User.FIELD_FIRSTNAME].ToString() ?? "";
                        User.instance.LastName = rowUser[User.FIELD_LASTNAMES].ToString() ?? "";
                        result = true;
                        break;
                    }
                }
            }
            subjectResult.OnNext(result);
        }

        public void setParams(Dictionary<string, object> mapParams)
        {
            crudUseCase.addParams(mapParams);
        }

        public void getUser()
        {
            try
            {
                crudUseCase.Sql = User.instance.Sqllogin;
                
                crudUseCase.executeGetList();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);
                
            }

        }

        public void isExistDatabase()
        {
            try
            {
                crudUseCase.verifyIfExistDatabase();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);

            }
        }
        ~AccessUserPresenter()
        {
            subjectResult.Dispose();
            subjectError.Dispose();
        }
        #endregion
    }
}

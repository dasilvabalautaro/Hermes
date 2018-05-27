using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Hermes.domain.interactor;
using Hermes.locatable_resources;

namespace Hermes.presentation
{
    class CreateDatabasePresenter
    {
        #region variables 
        CRUDUseCase crudUseCase = new CRUDUseCase();
        public Subject<bool> subjectResult = new Subject<bool>();
        public Subject<string> subjectError = new Subject<string>();        
        
        #endregion

        #region methods

        public CreateDatabasePresenter()
        {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(setResult);

        }

        private void setResult(bool result)
        {
            subjectResult.OnNext(result);           

        }

        public bool setParamsConfiguration(Dictionary<string, object> mapParams)
        {
            if(mapParams.Count > 0)
            {
                crudUseCase.MapParam = mapParams;
                return true;
            }else
            {
                subjectError.OnNext(StringResources.params_empty);               
                
                return false;
            }
        }

        public void createDatabase()
        {
            try
            {
                crudUseCase.create();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);               
            }
            
        }

        #endregion
    }
}

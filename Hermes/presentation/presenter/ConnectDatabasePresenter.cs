using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Hermes.domain.interactor;

namespace Hermes.presentation.presenter
{
    class ConnectDatabasePresenter
    {
        #region variables
        private CRUDUseCase crudUseCase = new CRUDUseCase();
        public Subject<bool> subject = new Subject<bool>();
        public Subject<string> subjectError = new Subject<string>();
        #endregion

        #region methods
        public ConnectDatabasePresenter()
        {
            crudUseCase.OnOperationDatabase += new CRUDUseCase
                .ResultOperationDelegate(sendResult);
        }

        private void sendResult(bool result)
        {      
            
            subject.OnNext(result);
            //subject.OnCompleted();
        }

        public void setParamsConfiguration(Dictionary<string, object> mapParams)
        {
           
            crudUseCase.MapParam = mapParams;
           
        }

        public void connectTest()
        {
            try
            {
                crudUseCase.connectTest();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);
                //subjectError.OnCompleted();
            }
            
        }

        public void connectDatabaseExisting()
        {
            try
            {
                crudUseCase.connectTest();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);
                //subjectError.OnCompleted();
            }

        }

        ~ConnectDatabasePresenter()
        {
            subject.Dispose();
            subjectError.Dispose();
        }

        #endregion
    }
}

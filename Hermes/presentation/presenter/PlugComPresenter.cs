using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Hermes.model.repository;

namespace Hermes.presentation.presenter
{
    class PlugComPresenter: IDisposable
    {
        #region variables
        public PlugCom plugCom = new PlugCom();
        public Subject<string> subjectResult = new Subject<string>();
        public Subject<string> subjectError = new Subject<string>();
        #endregion

        #region methods
        public PlugComPresenter()
        {
            plugCom.OnReadPort += new PlugCom
                .ResultReadPortDelegate(sendResult);
        }

        private void sendResult(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                subjectResult.OnNext(result);
            }            
        }

        public void openPort()
        {
            try
            {
                plugCom.open();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);

            }
            
        }

        public void closePort()
        {
            try
            {
             
                plugCom.close();
            }
            catch (ArgumentException ie)
            {
                subjectError.OnNext(ie.Message);

            }

        }

        public void Dispose()
        {
            plugCom.Dispose();
        }
        #endregion
    }
}

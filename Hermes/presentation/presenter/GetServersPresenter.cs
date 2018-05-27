using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Hermes.domain.interactor;

namespace Hermes.presentation.presenter
{
    class GetServersPresenter
    {
        #region variables 
        CRUDUseCase crudUseCase = new CRUDUseCase();
        public Subject<List<string>> subject = new Subject<List<string>>();

        #endregion

        #region methods
        public GetServersPresenter()
        {
            crudUseCase.OnBuilList += new CRUDUseCase
                .ResultListDelegate(sendList);
        }

        private void sendList(List<string> list)
        {
            subject.OnNext(list);
            subject.OnCompleted();
        }

        public void getListServers()
        {
            crudUseCase.getListServers();
        }
        #endregion
    }
}

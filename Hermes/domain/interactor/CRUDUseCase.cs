using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hermes.model.data;
using Hermes.model.data.repository;

namespace Hermes.domain.interactor
{
    class CRUDUseCase
    {
        #region variables
        private bool _result;
        private string errorMessage = "";
        private string _sql;
        private DataTable _dataResult;
        Dictionary<string, object> _mapParam;
        List<string> _servers;
        private Store store = new Store(RepositorySqlServer.instance);
        public delegate void ResultOperationDelegate(bool result);
        public event ResultOperationDelegate OnOperationDatabase;
        public delegate void ResultDatatableDelegate(DataTable result);
        public event ResultDatatableDelegate OnDatatableDatabase;
        public delegate void ResultListDelegate(List<string> list);
        public event ResultListDelegate OnBuilList;

        public bool Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
                if (OnOperationDatabase != null)
                {
                    OnOperationDatabase(_result);
                }
            }
        }

        public DataTable DataResult
        {
            get
            {
                return _dataResult;
            }

            set
            {
                _dataResult = value;
                if (OnDatatableDatabase != null)
                {
                    OnDatatableDatabase(_dataResult);
                }
            }
        }

        public Dictionary<string, object> MapParam
        {
            get
            {
                return _mapParam;
            }

            set
            {
                _mapParam = value;               
            }
        }

        public string Sql
        {
            get
            {
                return _sql;
            }

            set
            {
                _sql = value;
                store.Sql = _sql;
            }
        }

        public List<string> Servers
        {
            get
            {
                return _servers;
            }

            set
            {
                _servers = value;
                if (OnBuilList != null)
                {
                    OnBuilList(_servers);
                }

            }
        }
        #endregion

        #region methods
        public CRUDUseCase() { }

        public void executeGetList()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            try
            {
                Task<DataTable> task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        return store.getDataTable();
                    }
                    catch (ArgumentException ie)
                    {
                        errorMessage = ie.Message;
                        tokenSource.Cancel();
                    }
                    return null;
                }, token);
               
                if (!token.IsCancellationRequested)
                {
                    DataResult = task.Result;
                }
                else
                {
                    token.ThrowIfCancellationRequested();
                }

            }
            catch (OperationCanceledException ie)
            {
                throw new ArgumentException(String.Concat(ie.Message,
                    errorMessage));
            }
            finally
            {
                errorMessage = "";
                tokenSource.Dispose();
            }
        }

        public void addParams(Dictionary<string, object> prms)
        {
            store.addParamsSQL(prms);
        }

        public void executeNonSql()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            try
            {
                //Task<bool> task = Task.Factory.StartNew(obj =>
                Task<bool> task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        //Dictionary<string, object> prm = (Dictionary<string, object>)obj;
                        //store.addParamsSQL(prm);
                        //store.Sql = User.instance.SqlAdd;
                        return store.executeNonSQL();
                    }
                    catch (ArgumentException ie)
                    {
                        errorMessage = ie.Message;
                        tokenSource.Cancel();
                    }
                    return false;
                }, token);
                //}, MapParam, token);
                
                if (!token.IsCancellationRequested)
                {
                    Result = task.Result;
                }
                else
                {
                    token.ThrowIfCancellationRequested();
                }

            }
            catch (OperationCanceledException ie)
            {
                throw new ArgumentException(String.Concat(ie.Message,
                    errorMessage));
            }
            finally
            {
                errorMessage = "";
                tokenSource.Dispose();
            }
        }

        public void verifyIfExistDatabase()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            try
            {
                Task<bool> task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        return store.isExitsDatabase();

                    }
                    catch (ArgumentException ie)
                    {
                        errorMessage = ie.Message;
                        tokenSource.Cancel();
                        return false;
                    }
                }, token);

                task.Wait();
                if (!token.IsCancellationRequested)
                {
                    Result = task.Result;
                }
                else
                {
                    token.ThrowIfCancellationRequested();
                }

            }
            catch (OperationCanceledException ie)
            {
                throw new ArgumentException(String.Concat(ie.Message,
                    errorMessage));
            }
            finally
            {
                errorMessage = "";
                tokenSource.Dispose();
            }

        }

        public void connectTest()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            //Task<bool> task = Task.Factory.StartNew(new Func<bool>(connectTest));
            try
            {
                Task<bool> task = Task.Factory.StartNew(obj =>
                {
                    try
                    {
                        Dictionary<string, object> prm = (Dictionary<string, object>)obj;
                        store.valuesOfConnection(prm);
                        return store.connect();

                    }
                    catch (ArgumentException ie)
                    {
                        errorMessage = ie.Message;
                        tokenSource.Cancel();
                    }
                    return false;
                }, MapParam, token);

                task.Wait();
                if (!token.IsCancellationRequested)
                {
                    Result = task.Result;
                }
                else
                {
                    token.ThrowIfCancellationRequested();
                }

            }
            catch (OperationCanceledException ie)
            {
                throw new ArgumentException(String.Concat(ie.Message,
                    errorMessage));
            }
            finally
            {
                errorMessage = "";
                tokenSource.Dispose();
            }

        }

        public void setConfiguration()
        {
            store.setValuesConfigurationInRegistry();
        }

        public void connect()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            try
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        store.getValuesConfiguration();
                        store.connect();
                    }
                    catch (ArgumentException ie)
                    {
                        errorMessage = ie.Message;
                        tokenSource.Cancel();
                    }
                }, token);

                task.Wait();
                if (!token.IsCancellationRequested)
                {
                    Result = true;
                }
                else
                {
                    token.ThrowIfCancellationRequested();
                }

            }
            catch (OperationCanceledException ie)
            {
                throw new ArgumentException(String.Concat(ie.Message,
                    errorMessage));
            }
            finally
            {
                errorMessage = "";
                tokenSource.Dispose();
            }

        }

        public void executeIsConnect()
        {
            Task<bool> task = Task.Factory.StartNew(new Func<bool>(store.isConnect));
            task.Wait();
            Result = task.Result;
        }

        public void create()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;           
            try
            {
                Task<bool> task = Task.Factory.StartNew(obj =>
                {
                    try
                    {
                        Dictionary<string, object> prm = (Dictionary<string, object>)obj;
                        store.valuesOfConnection(prm);
                        return store.create();

                    }
                    catch (ArgumentException ie)
                    {
                        errorMessage = ie.Message;
                        tokenSource.Cancel();
                    }
                    return false;
                }, MapParam, token);

                task.Wait();
                if (!token.IsCancellationRequested)
                {
                    Result = task.Result;
                }
                else
                {
                    token.ThrowIfCancellationRequested();
                }

            }
            catch (OperationCanceledException ie)
            {
                throw new ArgumentException(String.Concat(ie.Message,
                    errorMessage));
            }
            finally
            {
                errorMessage = "";
                tokenSource.Dispose();
            }

        }

        public void getListServers()
        {
            Task<List<string>> task = Task.Factory
                .StartNew(new Func<List<string>>(store.getListServers));
           
            Servers = task.Result;
        }

        #endregion

    }
}

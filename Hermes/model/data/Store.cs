using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Hermes.model.data.repository;

namespace Hermes.model.data
{
    class Store
    {
        #region variables
        private string _sql = string.Empty;        
        RepositoryDatabase repository;
        public string Sql
        {
            get
            {
                return _sql;
            }

            set
            {
                _sql = value;
                RepositorySqlServer.instance.Sql = _sql;
            }
        }      
    
        #endregion

        #region methods

        public Store(RepositoryDatabase repository) {
            this.repository = repository;
        }

        public bool create()
        {
            try
            {
                return this.repository.create();
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }
            
        }

        public void valuesOfConnection(Dictionary<string, object> mapParams)
        {
            this.repository.setValuesOfConnection(mapParams);
        }

        public List<string> getListServers()
        {
            return this.repository.getServers();
        }

        public bool connect()
        {
            try
            {
                return this.repository.connect();
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }
            
        }

        public bool getValuesConfiguration()
        {
            try
            {
                return RepositorySqlServer.instance.getValuesConfiguration();
            }
            catch (FieldAccessException fe)
            {
                throw new ArgumentException(fe.Message);
            }
            
        }

        public bool isConnect()
        {
            return this.repository.isCnn();
        }     

        public void setValuesConfigurationInRegistry()
        {
            this.repository.setValuesConfigurationInRegistry();
        }

        public void addParamsSQL(Dictionary<string, object> mapParams)
        {
            foreach (KeyValuePair<string, object> pair in mapParams)
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = pair.Key.ToString();
                Type type = pair.Value.GetType();
                if (type.Equals(typeof(int)))
                    param.SqlDbType = SqlDbType.Int;
                else if (type.Equals(typeof(string)))
                    param.SqlDbType = SqlDbType.VarChar;
                else if (type.Equals(typeof(double)))
                    param.SqlDbType = SqlDbType.Decimal;
                else if (type.Equals(typeof(bool)))
                    param.SqlDbType = SqlDbType.Bit;
                else
                    Console.WriteLine("'{0}' is another data type.", type);
                param.Value = (object)pair.Value ?? DBNull.Value;
                this.repository.insertParams(param.ParameterName,
                    param.SqlDbType, param.Value);

            }
        }

        public DataTable getDataTable()
        {
            try
            {
                return this.repository.getDataTable();
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }
        }

        public bool executeNonSQL()
        {
            try
            {
                return this.repository.executeNonSQL();
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }
        }

        public bool isExitsDatabase()
        {
            try
            {
                if (getValuesConfiguration() && connect())
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }

        }
      
        ~Store()  
        {
                     
        }

        #endregion

    }
}

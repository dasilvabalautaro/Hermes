using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.model.data.repository
{
    abstract class RepositoryDatabase
    {
        #region constants
        protected const string FOLDER_SQL = "SQL";
        public const string PATH_KEY = "SOFTWARE\\HIDDENODDS\\HERMES";
        #endregion
        #region methods
        abstract public bool connect();
        abstract public bool create();
        abstract public bool isCnn();
        abstract public DataTable getDataTable();      
        abstract public void closeConnection();
        abstract public bool getValuesConfiguration();
        abstract public bool executeNonSQL(bool isProcedure = false);
        abstract public void setValuesConfigurationInRegistry();
        abstract public void setValuesOfConnection(Dictionary<string, object> mapParams);
        abstract public void insertParams(string varName, SqlDbType varType, object value);
        abstract public void insertParams(string varName, System.Data.OleDb.OleDbType varType, object value);
        abstract public List<string> getServers();
        #endregion
    }
}

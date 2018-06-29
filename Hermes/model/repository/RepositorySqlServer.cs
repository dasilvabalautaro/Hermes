using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using Hermes.model.repository;
using Hermes.tools;
using System.Data.OleDb;

namespace Hermes.model.data.repository
{
    sealed class RepositorySqlServer : RepositoryDatabase, IDisposable
    {

        #region constants
        
        private const string INSERT_PARAMETERS_FILE = "INSERT_REGISTRIES_PARAMS.sql";
        private const string CREATE_DATABASE_FILE = "CREATEDB.Log";
        private const string SQL_PROCEDURE_CREATE = "CREATETABLESDB";
        private const string SQL_PROCEDURE_INSERT_PARAMS = "INSERTPARAMSDB";    
        public const string NAME_KEY_DATABASE = "Database";
        public const string INTEGRATED_SECURITY_KEY = "Integrated_Security";
        public const string PASSWORD_KEY = "Password";
        public const string SERVER_NAME_KEY = "ServerName";
        public const string USER_KEY = "User";    
        public const string DATABASE_NEW = "new_database";
        
        #endregion

        #region variables
        private SqlConnection _cnn = new SqlConnection();
        private RepositoryDisk repositoryDisk = new RepositoryDisk();
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private string _nameConnection = string.Empty;       
        private string _databaseName = string.Empty;
        private string _newDatabaseName = string.Empty;
        private string _serverName = string.Empty;
        private short _persistSecurityInfo = 0;
        private short _integratedSecurity = 1;
        private string _user = string.Empty;
        private string _password = string.Empty;
        private string _sql = string.Empty;
        private string _connectionString = string.Empty;
        private string _credentials = string.Empty;
        public static readonly RepositorySqlServer instance = new RepositorySqlServer();
        public string DatabaseName
        {
            get
            {
                return _databaseName;
            }

            set
            {
                _databaseName = value;
            }
        }

        public string ServerName
        {
            get
            {
                return _serverName;
            }

            set
            {
                _serverName = value;
            }
        }

        public short PersistSecurityInfo
        {
            get
            {
                return _persistSecurityInfo;
            }

            set
            {
                _persistSecurityInfo = value;
            }
        }

        public short IntegratedSecurity
        {
            get
            {
                return _integratedSecurity;
            }

            set
            {
                _integratedSecurity = value;
            }
        }

        public string User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
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
            }
        }

        public string NewDatabaseName
        {
            get
            {
                return _newDatabaseName;
            }

            set
            {
                _newDatabaseName = value;
            }
        }
        #endregion

        private RepositorySqlServer() { }

        public override void closeConnection()
        {
            if (_cnn != null)
            {
                if (_cnn.State == ConnectionState.Open)
                {
                    _cnn.Close();
                }
            }
        }

        public override bool connect()
        {
            if (_cnn != null)
            {
                if (_cnn.State == ConnectionState.Open)
                {
                    _cnn.Close();
                }
            }

            if (IntegratedSecurity == 1)
            {

                _cnn.ConnectionString = "Persist Security Info=" +
                    Convert.ToBoolean(PersistSecurityInfo).ToString() +
                    ";Integrated Security=" +
                    Convert.ToBoolean(IntegratedSecurity).ToString() +
                    ";Initial Catalog=" + DatabaseName +
                    ";Data Source=" + ServerName;
            }
            else
            {
                _cnn.ConnectionString = "Persist Security Info=" + Convert.ToBoolean(PersistSecurityInfo).ToString() +
                                        ";User ID=" + User +
                                        ";Password=" + Password +
                                        ";Initial Catalog=" + DatabaseName +
                                        ";Data Source=" + ServerName;
            }
            try
            {
                _cnn.Open();
            }
            catch (SqlException e)
            {
                throw new ArgumentException(e.Message);

            }
            catch (InvalidOperationException ie)
            {
                throw new ArgumentException(ie.Message);

            }
            return true;

        }


        public override bool create()
        {                                   
            try
            {
                string sqlCreateTables = repositoryDisk.readTextFile(Application.StartupPath +
                           "\\" + FOLDER_SQL + "\\" + CREATE_DATABASE_FILE);
                string sqlInsertParams = repositoryDisk.readTextFile(Application.StartupPath +
                                "\\" + FOLDER_SQL + "\\" + INSERT_PARAMETERS_FILE);
                Sql = "CREATE DATABASE " + NewDatabaseName;
                connect();
                executeNonSQL(false);
                DatabaseName = NewDatabaseName;
                connect();
                buildTables(sqlCreateTables, SQL_PROCEDURE_CREATE);
                buildTables(sqlInsertParams, SQL_PROCEDURE_INSERT_PARAMS);
                setValuesConfigurationInRegistry();
                return true;
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);

            }
            catch(FileNotFoundException fe)
            {
                throw new ArgumentException(fe.Message);
            }
           
        }

        public override bool executeNonSQL(bool isProcedure = false)
        {          
            SqlCommand cmd = new SqlCommand(Sql, _cnn);

            foreach (SqlParameter prm in parameters)
            {
                cmd.Parameters.Add(prm);
            }

            if (isProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            try
            {

                cmd.ExecuteNonQuery();

            }
            catch (SqlException se)
            {

                throw new ArgumentException(se.Message);
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }
            catch (InvalidOperationException oe)
            {
                throw new ArgumentException(oe.Message);
            }
            finally
            {
                parameters.Clear();
            }

            return true;
        }

        public override DataTable getDataTable(string nameTable = "table")
        {
           
            try
            {
                SqlCommand cmd = new SqlCommand(Sql, _cnn);
                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }

                SqlDataAdapter adapter;
                DataTable dt = new DataTable();
                dt.TableName = nameTable;
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {                    
                    return dt;
                }else
                {
                    parameters.Clear();
                    throw new ArgumentException(" Data table empty.");
                }

            }
            catch (SqlException se)
            {
                throw new ArgumentException(se.Message);
            }
            catch (InvalidOperationException ie)
            {
                throw new ArgumentException(ie.Message);
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }
            finally
            {
                parameters.Clear();
            }
        }

        private List<string> getInstancesSQLSERVER()
        {
            List<string> sqlInstances = new List<string>();

            System.Data.Sql.SqlDataSourceEnumerator instance = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            System.Data.DataTable dataTable = instance.GetDataSources();
            foreach (DataRow row in dataTable.Rows)
            {

                string instanceName = row["ServerName"].ToString();

                if (!sqlInstances.Contains(instanceName) && !instanceName.Contains(Environment.MachineName))
                {
                    sqlInstances.Add(instanceName);
                }
            }


            List<string> lclInstances = GetLocalSqlServerInstanceNames();
            foreach (var lclInstance in lclInstances)
            {

                string instanceName = Environment.MachineName;
                if (!sqlInstances.Contains(instanceName)) sqlInstances.Add(instanceName);
            }
            sqlInstances.Sort();

            return sqlInstances;
        }

        public override bool getValuesConfiguration()
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();

            if (!string.IsNullOrEmpty(registryValueDataReader
                .getKeyValueRegistry(PATH_KEY, NAME_KEY_DATABASE)))
                DatabaseName = registryValueDataReader
                    .getKeyValueRegistry(PATH_KEY, NAME_KEY_DATABASE);
            else
                throw new FieldAccessException("Get Value Configuration empty.");

            if (!string.IsNullOrEmpty(registryValueDataReader
                .getKeyValueRegistry(PATH_KEY, INTEGRATED_SECURITY_KEY)))
                IntegratedSecurity = Convert.ToInt16(registryValueDataReader
                    .getKeyValueRegistry(PATH_KEY, INTEGRATED_SECURITY_KEY));

            if (!string.IsNullOrEmpty(registryValueDataReader
                .getKeyValueRegistry(PATH_KEY, PASSWORD_KEY)))
                Password = registryValueDataReader.getKeyValueRegistry(PATH_KEY, 
                    PASSWORD_KEY);

            if (!string.IsNullOrEmpty(registryValueDataReader
                .getKeyValueRegistry(PATH_KEY, SERVER_NAME_KEY)))
                ServerName = registryValueDataReader.getKeyValueRegistry(PATH_KEY, 
                    SERVER_NAME_KEY);

            if (!string.IsNullOrEmpty(registryValueDataReader
                .getKeyValueRegistry(PATH_KEY, USER_KEY)))
                User = registryValueDataReader.getKeyValueRegistry(PATH_KEY, USER_KEY);

            return true;
        }

        public override void insertParams(string varName, SqlDbType varType, object value)
        {
            SqlParameter prm = new SqlParameter(varName, varType) { Value = value };

            parameters.Add(prm);
        }

        public override bool isCnn()
        {
            bool blnReturn = true;

            if (_cnn != null)
            {
                if (_cnn.State != ConnectionState.Open)
                {
                    blnReturn = false;

                }
            }
            else
            {
                blnReturn = false;

            }

            return blnReturn;
        }

        public override void setValuesConfigurationInRegistry()
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();
            registryValueDataReader.setKeyValueRegistry(PATH_KEY,
                INTEGRATED_SECURITY_KEY,
                IntegratedSecurity);
            registryValueDataReader.setKeyValueRegistry(PATH_KEY,
                NAME_KEY_DATABASE, DatabaseName);
            registryValueDataReader.setKeyValueRegistry(PATH_KEY,
                PASSWORD_KEY, Password);
            registryValueDataReader.setKeyValueRegistry(PATH_KEY,
                SERVER_NAME_KEY, ServerName);
            registryValueDataReader.setKeyValueRegistry(PATH_KEY,
                USER_KEY, User);
        }
      
        private bool buildTables(string sqlBuildTables, string nameProcedure)
        {            
            try
            {
                Sql = sqlBuildTables;
                executeNonSQL(false);
                Sql = nameProcedure;
                executeNonSQL(true);
                return true;
            }
            catch (ArgumentException ie)
            {
                throw new ArgumentException(ie.Message);
            }                  
        }

        private List<string> GetLocalSqlServerInstanceNames()
        {
            RegistryValueDataReader registryValueDataReader = new RegistryValueDataReader();

            string[] instances64Bit = registryValueDataReader
                .ReadRegistryValueData(tools.RegistryHive.Wow64,
                                    Registry.LocalMachine,
                                    @"SOFTWARE\Microsoft\Microsoft SQL Server",
                                    "InstalledInstances");

            string[] instances32Bit = registryValueDataReader
                .ReadRegistryValueData(tools.RegistryHive.Wow6432,
                                    Registry.LocalMachine,
                                    @"SOFTWARE\Microsoft\Microsoft SQL Server",
                                    "InstalledInstances");


            List<string> localInstanceNames = new List<string>(instances64Bit);
            foreach (var item in instances32Bit)
            {
                if (!localInstanceNames.Contains(item)) localInstanceNames.Add(item);
            }

            return localInstanceNames;
        }

        public override void setValuesOfConnection(Dictionary<string, object> 
            mapParams)
        {
            try
            {
                DatabaseName = mapParams[NAME_KEY_DATABASE] as string;
                ServerName = mapParams[SERVER_NAME_KEY] as string;
                IntegratedSecurity = Convert.ToInt16(mapParams[INTEGRATED_SECURITY_KEY]);
                User = mapParams[USER_KEY] as string;
                Password = mapParams[PASSWORD_KEY] as string;
                NewDatabaseName = mapParams[DATABASE_NEW] as string;
            }
            catch(KeyNotFoundException ke)
            {
                throw new ArgumentException(ke.Message);
            }
            
        }

        public override List<string> getServers()
        {
            return getInstancesSQLSERVER();
        }       
        void IDisposable.Dispose()
        {
            this.closeConnection();
            _cnn.Dispose();
        }

        public override void insertParams(string varName, OleDbType varType, object value)
        {
            throw new NotImplementedException();
        }
    }

}

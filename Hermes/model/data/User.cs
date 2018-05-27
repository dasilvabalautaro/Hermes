using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.model.data
{
    sealed class User
    {
        #region constants
        public const string TABLE_NAME = "USERS";
        public const string FIELD_USERID = "id";
        public const string FIELD_USERTYPEID = "type";
        public const string FIELD_USERNAME = "nameUser";
        public const string FIELD_PASS = "password";
        public const string FIELD_FIRSTNAME = "firstName";
        public const string FIELD_LASTNAMES = "lastName";
        public const string PARAM_USERID = "@userId";
        public const string PARAM_USERTYPEID = "@userTypeId";
        public const string PARAM_USERNAME = "@userName";
        public const string PARAM_PASS = "@pass";
        public const string PARAM_FIRSTNAME = "@firstName";
        public const string PARAM_LASTNAMES = "@lastNames";

        #endregion
        #region variables
        private int _id;
        private int _type;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _password = string.Empty;
        private string _userName = string.Empty;
        private string _sqllogin = string.Empty;
        private string _sqlList = string.Empty;
        private string _sqlAdd = string.Empty;
        private string _sqlDel = string.Empty;
        public static readonly User instance = new User();

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public int Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
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

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }

        public string Sqllogin
        {
            get
            {
                return _sqllogin;
            }

            set
            {
                _sqllogin = value;
            }
        }

        public string SqlList
        {
            get
            {
                return _sqlList;
            }

            set
            {
                _sqlList = value;
            }
        }

        public string SqlAdd
        {
            get
            {
                return _sqlAdd;
            }

            set
            {
                _sqlAdd = value;
            }
        }

        public string SqlDel
        {
            get
            {
                return _sqlDel;
            }

            set
            {
                _sqlDel = value;
            }
        }
        #endregion

        #region methods
        private User()
        {
            Sqllogin = string.Format("SELECT * FROM {0} WHERE {1} = {2}",
                TABLE_NAME, FIELD_USERNAME, PARAM_USERNAME);
            SqlList = string.Format("SELECT * FROM {0}", TABLE_NAME);
            SqlAdd = string.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}) " +
                "VALUES ({6}, {7}, {8}, {9}, {10})", TABLE_NAME, FIELD_USERTYPEID,
                FIELD_USERNAME, FIELD_PASS, FIELD_FIRSTNAME, FIELD_LASTNAMES, PARAM_USERTYPEID,
                PARAM_USERNAME, PARAM_PASS, PARAM_FIRSTNAME, PARAM_LASTNAMES);
            SqlDel = string.Format("DELETE FROM {0} WHERE {1} = {2}", TABLE_NAME,
                FIELD_USERID, PARAM_USERID);
        }

        #endregion
    }
}

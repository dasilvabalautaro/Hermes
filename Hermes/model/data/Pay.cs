using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.model.data
{
    sealed class Pay
    {
        #region constants
        public const string TABLE_NAME = "PAYMENTS";        
        public const string FIELD_ID = "id";
        public const string FIELD_ID_SALE = "idSale";
        public const string FIELD_MOUNT = "mount";        
        public const string FIELD_DATE = "datePay";        
        public const string FIELD_USER = "userId";        
        public const string PARAM_ID = "@id";
        public const string PARAM_ID_SALE = "@idSale";
        public const string PARAM_MOUNT = "@mount";        
        public const string PARAM_DATE = "@datePay";       
        public const string PARAM_USER = "@userId";        
        #endregion

        #region variables
        private int _id;
        private int _idSale;        
        private float _mount;
        private int _userId;
        private string _sqlList = string.Empty;
        private string _sqlAdd = string.Empty;
        private string _sqlDel = string.Empty;

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

        public int IdSale
        {
            get
            {
                return _idSale;
            }

            set
            {
                _idSale = value;
            }
        }

        public float Mount
        {
            get
            {
                return _mount;
            }

            set
            {
                _mount = value;
            }
        }

        public int UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
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
        public Pay()
        {
            SqlAdd = string.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}) " +
                "VALUES ({5}, {6}, {7}, {8})", TABLE_NAME, FIELD_ID_SALE,
                FIELD_MOUNT, FIELD_DATE, FIELD_USER,
                PARAM_ID_SALE, PARAM_MOUNT, "GetDate()", PARAM_USER);

            SqlList = string.Format("SELECT T1.id, (T1.total - SUM(ISNULL(T2.mount, 0))) AS debt " +
                "INTO #AUXFILTER FROM SALES AS T1 LEFT JOIN PAYMENTS AS T2 ON T1.id = T2.idSale " +
                "GROUP BY T1.id, T1.total; SELECT * FROM #AUXFILTER WHERE debt > 0;");

        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.model.data
{
    sealed class Product
    {
        #region constants
        public const string TABLE_NAME = "PPRODUCTTYPE";
        public const string FIELD_ID = "id";
        public const string FIELD_PRICE = "price";
        public const string FIELD_DESCRIPTION = "description";
        public const string PARAM_ID = "@id";
        public const string PARAM_PRICE = "@price";
        public const string PARAM_DESCRIPTION = "@description";
        #endregion

        #region variables
        private int _id;
        private string _description = string.Empty;
        private float _price;
        private string _sqlList = string.Empty;
        private string _sqlAdd = string.Empty;
        private string _sqlDel = string.Empty;
        private string _sqlUpdate = string.Empty;      

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

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public float Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
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

        public string SqlUpdate
        {
            get
            {
                return _sqlUpdate;
            }

            set
            {
                _sqlUpdate = value;
            }
        }
        #endregion

        #region methods
        public Product()
        {
            SqlList = string.Format("SELECT * FROM {0}", TABLE_NAME);
            SqlAdd = string.Format("INSERT INTO {0} ({1}, {2}) " +
                "VALUES ({3}, {4})", TABLE_NAME, FIELD_PRICE,
                FIELD_DESCRIPTION, PARAM_PRICE, PARAM_DESCRIPTION);
            SqlDel = string.Format("DELETE FROM {0} WHERE {1} = {2}", TABLE_NAME,
                FIELD_ID, PARAM_ID);

        }

        public void updateProduct(string fieldname, string value)
        {
            SqlUpdate = string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4}",
                TABLE_NAME, fieldname, value, FIELD_ID, PARAM_ID);
        }

        #endregion
    }
}

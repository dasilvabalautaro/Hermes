using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.model.data
{
    sealed class Sale
    {
        #region constants
        public const string TABLE_NAME = "SALES";
        public const string FIELD_ID = "id";
        public const string FIELD_PRICE = "price";
        public const string FIELD_PRODUCT = "product";
        public const string FIELD_QUANTITY = "quantity";
        public const string FIELD_DATE = "dateSale";
        public const string FIELD_WEIGHT = "weight";
        public const string FIELD_CLIENT = "client";
        public const string FIELD_USER = "userId";
        public const string FIELD_OBSERVATIONS = "observations";
        public const string FIELD_IMAGE = "image";
        public const string FIELD_TOTAL = "total";
        public const string PARAM_ID = "@id";
        public const string PARAM_PRICE = "@price";
        public const string PARAM_OBSERVATIONS = "@observations";
        public const string PARAM_PRODUCT = "@product";
        public const string PARAM_QUANTITY = "@quantity";
        public const string PARAM_DATE = "@dateSale";
        public const string PARAM_WEIGHT = "@weight";
        public const string PARAM_CLIENT = "@client";
        public const string PARAM_USER = "@userId";
        public const string PARAM_IMAGE = "@image";
        public const string PARAM_TOTAL = "@total";
        #endregion

        #region variables
        private int _id;
        private string _observations = string.Empty;
        private float _price;
        private float _total;
        private int _product;
        private int _quantity;
        private string _dateSale = string.Empty;
        private float _weight;
        private string _client = string.Empty;
        private int _userId;
        private byte[] _image;        
        private string _sqlAdd = string.Empty;
        private string _sqlDel = string.Empty;
        private string _sqlUpdate = string.Empty;
        private string _sqlMax = string.Empty;

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

        public string Observations
        {
            get
            {
                return _observations;
            }

            set
            {
                _observations = value;
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

        public int Product
        {
            get
            {
                return _product;
            }

            set
            {
                _product = value;
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                _quantity = value;
            }
        }

        public string DateSale
        {
            get
            {
                return _dateSale;
            }

            set
            {
                _dateSale = value;
            }
        }

        public float Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                _weight = value;
            }
        }

        public string Client
        {
            get
            {
                return _client;
            }

            set
            {
                _client = value;
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

        public byte[] Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
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

        public float Total
        {
            get
            {
                return _total;
            }

            set
            {
                _total = value;
            }
        }

        public string SqlMax
        {
            get
            {
                return _sqlMax;
            }

            set
            {
                _sqlMax = value;
            }
        }
        #endregion

        #region methods
        public Sale()
        {
            SqlAdd = string.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}) " +
                "VALUES ({10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18})", 
                TABLE_NAME, FIELD_PRODUCT, FIELD_QUANTITY, FIELD_DATE,
                FIELD_WEIGHT, FIELD_CLIENT, FIELD_PRICE, FIELD_USER, FIELD_OBSERVATIONS, FIELD_TOTAL, 
                PARAM_PRODUCT, PARAM_QUANTITY, "GetDate()", PARAM_WEIGHT, 
                PARAM_CLIENT, PARAM_PRICE, PARAM_USER, PARAM_OBSERVATIONS, PARAM_TOTAL);
            SqlMax = string.Format("SELECT MAX({0}) FROM {1}", FIELD_ID, TABLE_NAME);

        }

        #endregion

    }
}

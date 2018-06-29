using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.model.data
{
    class LineWrite
    {
        #region variables
        private string _content = string.Empty;
        private int _alignment;
        private int _style;
        #endregion

        #region constants
        public const int PARENT_START = 0;
        public const int PARENT_END = 1;
        public const int STYLE_REGULAR = 0;
        public const int STYLE_BOLD = 1;

        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
            }
        }

        public int Alignment
        {
            get
            {
                return _alignment;
            }

            set
            {
                _alignment = value;
            }
        }

        public int Style
        {
            get
            {
                return _style;
            }

            set
            {
                _style = value;
            }
        }
        #endregion

        #region methods
        public LineWrite()
        {
            Alignment = PARENT_START;
            Style = STYLE_REGULAR;
        }

        #endregion
    }
}

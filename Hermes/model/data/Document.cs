using Hermes.tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hermes.model.data
{
    class Document: PrintDocument
    {
        #region variables
        private StringFormat strFormat = new StringFormat();
        private Font fontNormal;
        private Font fontBold;        
        private int _left = 4;
        private int _top = 4;
        private int _width = 274;
        private int _height = 17;
        private int _fontSize = 11;
        private int _spaceHeight = 20;
        private Bitmap _bmpQr;
        private List<LineWrite> _list;
        private ControlManager controlManager = new ControlManager();
        private PictureBox picQR = new PictureBox();
      
        public int Left
        {
            get
            {
                return _left;
            }

            set
            {
                _left = value;
            }
        }

        public int Top
        {
            get
            {
                return _top;
            }

            set
            {
                _top = value;
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
            }
        }

        public int FontSize
        {
            get
            {
                return _fontSize;
            }

            set
            {
                _fontSize = value;
            }
        }

        internal List<LineWrite> List
        {
            get
            {
                return _list;
            }

            set
            {
                _list = value;
            }
        }

        public Bitmap BmpQr
        {
            get
            {
                return _bmpQr;
            }

            set
            {
                _bmpQr = value;
                picQR.Image = _bmpQr;
            }
        }

        public int SpaceHeight
        {
            get
            {
                return _spaceHeight;
            }

            set
            {
                _spaceHeight = value;
            }
        }
        #endregion

        #region methods
        public Document()
        {
            fontNormal = new Font("Calibri", FontSize, FontStyle.Regular, GraphicsUnit.Point);
            fontBold = new Font("Calibri", FontSize, FontStyle.Bold, GraphicsUnit.Point);
            strFormat.Alignment = StringAlignment.Far;
            this.PrintPage += Document_PrintPage;
            this.PrinterSettings.PrinterName = controlManager
                .getPrintDefault();
            picQR.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Document_PrintPage(object sender, PrintPageEventArgs e)
        {
            draw(e);
        }

        private int nextLevel(int level)
        {
            return Top + (SpaceHeight * level);
        }

        public void draw(PrintPageEventArgs e)
        {
            int index = 0;
            Font font;
            foreach(LineWrite lw in List)
            {
                if (lw.Style == LineWrite.STYLE_BOLD)
                {
                    font = this.fontBold;
                }else
                {
                    font = this.fontNormal;
                }


                if (lw.Alignment == LineWrite.PARENT_START)
                {
                    e.Graphics.DrawString(lw.Content,
                         font, Brushes.Black, Left, nextLevel(index));
                    index++;
                }

                if (lw.Alignment == LineWrite.PARENT_END)
                {
                    index--;

                    e.Graphics.DrawString(lw.Content,
                        font, Brushes.Black, 
                        new RectangleF(0, 
                        nextLevel(index), Width, Height), strFormat);
                    index++;
                }
            }
            index++;
            e.Graphics.DrawImage(picQR.Image, 
                30, nextLevel(index), 140, 140);

        }

        #endregion

    }
}

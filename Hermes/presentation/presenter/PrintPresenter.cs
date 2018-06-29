using Hermes.model.data;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Hermes.presentation.presenter
{
    class PrintPresenter
    {
        #region variables
        public Document document = new Document();
        private List<LineWrite> list = new List<LineWrite>();
        #endregion

        #region methods
        public PrintPresenter() { }

        public void buildContent(Sale sale, 
            string nameProduct, string debt)
        {
            LineWrite title = new LineWrite();
            title.Content = "Almacen Nuevo";
            title.Style = LineWrite.STYLE_BOLD;
            list.Add(title);
            LineWrite business = new LineWrite();
            business.Content = "CANDELARIA";
            business.Style = LineWrite.STYLE_BOLD;
            list.Add(business);
            LineWrite dateLabel = new LineWrite();
            dateLabel.Content = "La Paz: ";
            dateLabel.Style = LineWrite.STYLE_BOLD;
            list.Add(dateLabel);
            LineWrite dateValue = new LineWrite();
            dateValue.Content = sale.DateSale;
            dateValue.Alignment = LineWrite.PARENT_END;
            list.Add(dateValue);
            LineWrite clientLabel = new LineWrite();
            clientLabel.Content = "Señor(es): ";
            clientLabel.Style = LineWrite.STYLE_BOLD;
            list.Add(clientLabel);
            LineWrite clientValue = new LineWrite();
            clientValue.Content = sale.Client;
            clientValue.Alignment = LineWrite.PARENT_END;
            list.Add(clientValue);
            LineWrite detailLabel = new LineWrite();
            detailLabel.Content = "Por lo siguiente: ";
            list.Add(detailLabel);
            LineWrite headLabel = new LineWrite();
            headLabel.Content = "Cant.        Detalle         Precio        Peso(Kg.)";
            headLabel.Style = LineWrite.STYLE_BOLD;
            list.Add(headLabel);
            LineWrite saleValues = new LineWrite();
            saleValues.Content = string.Format("  {0}              {1}            {2}               {3}",
                sale.Quantity.ToString(), nameProduct, 
                sale.Price.ToString(), sale.Weight.ToString());
            list.Add(saleValues);
            LineWrite totalLabel = new LineWrite();
            totalLabel.Content = "Total: ";
            totalLabel.Style = LineWrite.STYLE_BOLD;
            list.Add(totalLabel);
            LineWrite totalValue = new LineWrite();
            totalValue.Content = sale.Total.ToString() + " Bs.";
            totalValue.Alignment = LineWrite.PARENT_END;
            list.Add(totalValue);
            LineWrite debtLabel = new LineWrite();
            debtLabel.Content = "Saldo: ";
            debtLabel.Style = LineWrite.STYLE_BOLD;
            list.Add(debtLabel);
            LineWrite debtValue = new LineWrite();
            debtValue.Content = debt + " Bs.";
            debtValue.Alignment = LineWrite.PARENT_END;
            list.Add(debtValue);
            document.List = list;
            document.BmpQr = (System.Drawing.Bitmap)QRGen(
                getStringDataForQR(sale, debt),
                Convert.ToInt32(8));           
        }

        private Image QRGen(string input, int qrlevel)
        {

            MessagingToolkit.QRCode.Codec.QRCodeEncoder qe = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            qe.QRCodeVersion = qrlevel;
            System.Drawing.Bitmap bm = qe.Encode(input);
            return bm;
        }

        private string getStringDataForQR(Sale sale, string debt)
        {
            string strReturn = string.Empty;
            strReturn = sale.DateSale + '|' +
                    sale.UserId.ToString() + '|' + sale.Weight.ToString() + '|' + 
                    sale.Total.ToString() + 
                    '|' + debt;           
            return strReturn;
        }
        #endregion
    }
}

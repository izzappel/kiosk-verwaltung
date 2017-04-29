using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;
using System.Drawing.Printing;
using KioskVerwaltung.BusinessObjects;
using System.Drawing;
using System.Windows.Documents;

namespace KioskVerwaltung.Printing
{
    public class Printer
    {
        private DataAccess.DataAccess dataAccess;
        private Sale saleToPrint = null;
        private Font titleFont, normalFont, boldFont;

        public Printer()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            titleFont = new Font("Calibri", 16, FontStyle.Bold, GraphicsUnit.Point);
            normalFont = new Font("Calibri", 14, FontStyle.Regular, GraphicsUnit.Point);
            boldFont = new Font("Calibri", 14, FontStyle.Bold, GraphicsUnit.Point);
        }

        public PrintDocument CreateSalePrintDocument(Sale sale)
        {
            PrintDocument document = new PrintDocument();
            document.DocumentName = String.Format("Tagesabrechnung vom {0:dd.MM.yyyy}", sale.Date);
            document.PrintPage += OnPrintSaleDocument;
            saleToPrint = sale;
            return document;
        }

        private void OnPrintSaleDocument(object sender, PrintPageEventArgs e)
        {
            if (saleToPrint != null)
            {
                Graphics g = e.Graphics;

                g.DrawString(String.Format("Tagesabrechnung vom {0:dd.MM.yyyy}", saleToPrint.Date), titleFont, Brushes.Black, 50, 100);

                Brush normalBrush = Brushes.Black;
                Brush privateBrush = Brushes.Green;
                Brush creditCardBrush = Brushes.Blue;
                Brush deductionBrush = Brushes.Red;
                float x = 50;
                float y = 150;
                float deltaY = 30;

                g.DrawString("Produkt", boldFont, normalBrush, x, y);
                g.DrawString("Rabatt/Reduktion", boldFont, normalBrush, x + 200, y);
                g.DrawString("Preis", boldFont, normalBrush, x + 400, y);
                y += 25;
                g.DrawLine(new Pen(normalBrush), x, y, x + 500, y);
                y += 10;
                
                foreach (var saleProduct in saleToPrint.SaleProducts)
                {
                    Brush brush = normalBrush;
                    if (saleProduct.IsPaidByCreditCard) {  brush = creditCardBrush; } 
                    if (saleProduct.IsPrivate)  { brush = privateBrush; }

                    g.DrawString(saleProduct.Name, normalFont, brush, x, y);
                    g.DrawString(saleProduct.Deduction, normalFont, deductionBrush, x + 200, y);
                    g.DrawString(String.Format("{0:0.00 CHF}", saleProduct.Price), normalFont, brush, x + 400, y);

                    y += deltaY;
                }
                g.DrawLine(new Pen(normalBrush), x, y, x + 500, y);
                y += 10;
                g.DrawString("Total Bargeld:", boldFont, normalBrush, x, y);
                g.DrawString(String.Format("{0:0.00 CHF}", saleToPrint.TotalCash), boldFont, normalBrush, x + 400, y);
                
                y += deltaY;
                
                g.DrawString("Total Kreditkarte:", boldFont, creditCardBrush, x, y);
                g.DrawString(String.Format("{0:0.00 CHF}", saleToPrint.TotalCreditCard), boldFont, normalBrush, x + 400, y);

                y += deltaY;
                g.DrawLine(new Pen(normalBrush), x, y, x + 500, y);
                y += 10;
               
                g.DrawString("Total:", boldFont, normalBrush, x, y);
                g.DrawString(String.Format("{0:0.00 CHF}", saleToPrint.Total), boldFont, normalBrush, x + 400, y);

                y += deltaY;
                g.DrawLine(new Pen(normalBrush), x, y, x + 500, y);
                y += 5;
                g.DrawLine(new Pen(normalBrush), x, y, x + 500, y);
                y += 10;
               
                g.DrawString("Total Privatbezug:", boldFont, privateBrush, x, y);
                g.DrawString(String.Format("{0:0.00 CHF}", saleToPrint.TotalPrivate), boldFont, normalBrush, x + 400, y);
            }
            saleToPrint = null;
        }

    }
}

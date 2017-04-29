using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    public partial class StockPrintFlowDocument : FlowDocument
    {
        public StockPrintFlowDocument(IList<Product> products)
        {
            InitializeComponent();
            this.PagePadding = new Thickness(50);

            foreach (var product in products)
            {
                Floater floater = new Floater();
                Paragraph item = new Paragraph(floater);
                item.KeepTogether = true;
                item.LineHeight = 4;

                Table table = new Table();
                TableRowGroup rowGroup = new TableRowGroup();

                var title = new Paragraph(new Run(product.Name));
                title.FontWeight = FontWeights.Bold;
                var cell1 = new TableCell(title);

                var conseignmentsTitle = new Paragraph(new Run("Lieferungen:"));
                conseignmentsTitle.FontWeight = FontWeights.Bold;
                var cell2 = new TableCell(conseignmentsTitle);

                var row1 = new TableRow();
                row1.Cells.Add(cell1);
                row1.Cells.Add(cell2);
                rowGroup.Rows.Add(row1);

                var price = new Paragraph();
                if (!product.HasConsignmentPrice)
                {
                    price = new Paragraph(new Run(string.Format("Aktueller Preis: {0:0.00} CHF", product.Price)));
                }

                Barcode barcode = new Barcode();
                barcode.Data = product.Barcode;
                barcode.Height = 90;
                barcode.Width = 200;
                barcode.HorizontalAlignment = HorizontalAlignment.Left;
                var barcodeUiContainer = new BlockUIContainer(barcode);
                var barcodeSection = new Section(barcodeUiContainer);

                var cell3 = new TableCell();
                cell3.Blocks.Add(price);
                cell3.Blocks.Add(barcodeSection);


                //Lieferungen
                var tableConsignments = new Table();
                var tableRowGroupConsingments = new TableRowGroup();

                var tableRowHeader = new TableRow();
                tableRowHeader.Cells.Add(new TableCell(new Paragraph(new Run("Anzahl"))));
                tableRowHeader.Cells.Add(new TableCell( (product.HasExpirationDate) ? new Paragraph(new Run("Ablaufdatum")) : new Paragraph() ));
                tableRowHeader.Cells.Add(new TableCell((product.HasConsignmentPrice) ? new Paragraph(new Run("Preis")) { TextAlignment = System.Windows.TextAlignment.Right } : new Paragraph()));

                tableRowGroupConsingments.Rows.Add(tableRowHeader);

                foreach (var consingment in product.Consignments)
                {
                    var tableRowItem = new TableRow();
                    tableRowItem.Cells.Add(new TableCell(new Paragraph(new Run(consingment.NumberOfContent.ToString()))));
                    tableRowItem.Cells.Add(new TableCell((product.HasExpirationDate) ? new Paragraph(new Run(consingment.ExpirationDateString)) : new Paragraph() ));
                    tableRowItem.Cells.Add(new TableCell((product.HasConsignmentPrice) ? new Paragraph(new Run(string.Format("{0:0.00} CHF", consingment.Price))) { TextAlignment = System.Windows.TextAlignment.Right } : new Paragraph()));

                    tableRowGroupConsingments.Rows.Add(tableRowItem);
                }

                var tableRowSeparator = new TableRow();
                tableRowSeparator.Cells.Add(new TableCell(new BlockUIContainer(new Separator() { HorizontalAlignment = HorizontalAlignment.Stretch })) { ColumnSpan = 3 });
                tableRowGroupConsingments.Rows.Add(tableRowSeparator);

                var tableRowLast = new TableRow();
                tableRowLast.Cells.Add(new TableCell(new Paragraph(new Run(product.Stock.ToString()) { FontWeight = FontWeights.Bold })));
                tableRowLast.Cells.Add(new TableCell(new Paragraph()));
                tableRowLast.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", product.TotalPrice)) { FontWeight = FontWeights.Bold }) { TextAlignment = System.Windows.TextAlignment.Right }));
                tableRowGroupConsingments.Rows.Add(tableRowLast);

                tableConsignments.RowGroups.Add(tableRowGroupConsingments);

                var cell4 = new TableCell(tableConsignments);

                var row2 = new TableRow();
                row2.Cells.Add(cell3);
                row2.Cells.Add(cell4);
                rowGroup.Rows.Add(row2);

                table.RowGroups.Add(rowGroup);
                floater.Blocks.Add(table);
                floater.Blocks.Add(new BlockUIContainer(new Separator() { HorizontalAlignment = HorizontalAlignment.Stretch }));

                ListSection.Blocks.Add(item);
            }
        }


       
    }


}

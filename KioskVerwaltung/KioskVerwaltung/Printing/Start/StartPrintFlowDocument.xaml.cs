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
using System.Windows.Shapes;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für StartPrintFlowDocument.xaml
    /// </summary>
    public partial class StartPrintFlowDocument : FlowDocument
    {
        public StartPrintFlowDocument(IList<Product> expiringProducts, IList<Product> shortInStockProducts)
        {
            InitializeComponent();
            this.PagePadding = new Thickness(50);

            var table = new Table();
            var tableRowGroup = new TableRowGroup();

            foreach (var product in expiringProducts)
            {
                var row = new TableRow();
               
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run("Beim Produkt "));
                paragraph.Inlines.Add(new Run(product.Name) { FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run(" laufen folgende Lieferungen ab:"));
                row.Cells.Add(new TableCell(paragraph) { ColumnSpan = 2 });
                tableRowGroup.Rows.Add(row);

                foreach (var consignment in product.Consignments)
                {
                    row = new TableRow();
                    paragraph = new Paragraph();
                    paragraph.Inlines.Add(new Run(consignment.NumberOfContent.ToString()) { FontWeight = FontWeights.Bold });
                    paragraph.Inlines.Add(new Run(" Packungen am "));
                    paragraph.Inlines.Add(new Run(consignment.ExpirationDateString) { FontWeight = FontWeights.Bold });
                    row.Cells.Add(new TableCell());
                    row.Cells.Add(new TableCell(paragraph));
                    tableRowGroup.Rows.Add(row);
                }
            }

            foreach (var product in shortInStockProducts)
            {
                var row = new TableRow();

                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run("Das Produkt "));
                paragraph.Inlines.Add(new Run(product.Name) { FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run(" hat nur noch "));
                paragraph.Inlines.Add(new Run(product.Stock.ToString() + " Packungen") { FontWeight = FontWeights.Bold });
                paragraph.Inlines.Add(new Run(" an Vorrat."));
                row.Cells.Add(new TableCell(paragraph) { ColumnSpan = 2 });
                tableRowGroup.Rows.Add(row);
            }

            table.RowGroups.Add(tableRowGroup);
            Content.Blocks.Add(table);
        }
    }
}

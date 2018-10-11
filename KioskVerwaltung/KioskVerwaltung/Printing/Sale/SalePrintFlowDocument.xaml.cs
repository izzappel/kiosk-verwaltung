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
    /// Interaktionslogik für SalePrintFlowDocument.xaml
    /// </summary>
    public partial class SalePrintFlowDocument : FlowDocument
    {
        public SalePrintFlowDocument(IList<SaleProduct> saleProducts)
        {
            InitializeComponent();
            this.PagePadding = new Thickness(50);

            Sale sale = new Sale();
            sale.SaleProducts = new System.Collections.ObjectModel.ObservableCollection<SaleProduct>(saleProducts);

            Content.Blocks.Add(new Paragraph(new Run(string.Format("Verkauf vom {0:dd.MM.yyyy}", DateTime.Now))) { FontWeight = FontWeights.Bold });

            var table = new Table();
			table.FontSize = 11;
			var tableRowGroup = new TableRowGroup();

            foreach (var saleProduct in saleProducts)
            {
                TableRow tableRow = new TableRow();

                tableRow.Cells.Add(new TableCell(new Paragraph(new Run(saleProduct.Name)) { LineHeight = 25 }));
                tableRow.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("({0:0.00} CHF)", saleProduct.Price))) { }));
                tableRow.Cells.Add(new TableCell(new Paragraph(new Run(saleProduct.Deduction)) { Foreground = new SolidColorBrush(Colors.Red)}));
                tableRow.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", saleProduct.SellPrice))) { TextAlignment = System.Windows.TextAlignment.Right }));
                if(saleProduct.IsPaidByCreditCard) 
                {
                    tableRow.Cells.Add(new TableCell(new BlockUIContainer(new Image() { Source = new BitmapImage(new Uri("pack://application:,,/KioskVerwaltung;component/Icons/creditcard.png")), Width = 25, Height = 25 })));
                    tableRow.Foreground = new SolidColorBrush(Colors.Blue);
                }
                else if (saleProduct.IsPrivate)
                {
                    tableRow.Cells.Add(new TableCell(new BlockUIContainer(new Image() { Source = new BitmapImage(new Uri("pack://application:,,/KioskVerwaltung;component/Icons/private.png")), Width = 25, Height = 25 })));
                    tableRow.Foreground = new SolidColorBrush(Colors.Green);
				}
				else if (saleProduct.IsForGuest)
				{
					tableRow.Cells.Add(new TableCell(new BlockUIContainer(new Image() { Source = new BitmapImage(new Uri("pack://application:,,/KioskVerwaltung;component/Icons/for_guest.png")), Width = 25, Height = 25 })));
					tableRow.Foreground = new SolidColorBrush(Colors.Red);
				}
				else
                {
                    tableRow.Cells.Add(new TableCell());
                }
                tableRowGroup.Rows.Add(tableRow);
            }

            var seperatorRow = new TableRow();
            seperatorRow.Cells.Add(new TableCell(new BlockUIContainer(new Separator() { HorizontalAlignment = HorizontalAlignment.Stretch })) { ColumnSpan = 5 });
            tableRowGroup.Rows.Add(seperatorRow);
            table.RowGroups.Add(tableRowGroup);
            Content.Blocks.Add(table);

            table = new Table();
			table.FontSize = 11;
            tableRowGroup = new TableRowGroup();

            var row = new TableRow();
            row.Cells.Add(new TableCell(new Paragraph(new Run("Privatbezug:"))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", sale.TotalPrivate)))));
            row.Cells.Add(new TableCell(new Paragraph(new Run("Bargeld:"))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", sale.TotalCash))) { TextAlignment = System.Windows.TextAlignment.Right }));
            tableRowGroup.Rows.Add(row);

            row = new TableRow();
			row.Cells.Add(new TableCell(new Paragraph(new Run("Für Gäste:"))));
			row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", sale.TotalForGuest)))));
			row.Cells.Add(new TableCell(new Paragraph(new Run("Kreditkarte:"))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", sale.TotalCreditCard))) { TextAlignment = System.Windows.TextAlignment.Right }));
            tableRowGroup.Rows.Add(row);

            row = new TableRow();
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell(new BlockUIContainer(new Separator() { HorizontalAlignment = HorizontalAlignment.Stretch })) { ColumnSpan = 2 });
            tableRowGroup.Rows.Add(row);

            row = new TableRow();
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("Total vom {0:dd.MM.yyyy}:", DateTime.Now)) { FontWeight = FontWeights.Bold })));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", sale.Total)) { FontWeight = FontWeights.Bold }) { TextAlignment = System.Windows.TextAlignment.Right }));
            tableRowGroup.Rows.Add(row);

            row = new TableRow();
            row.Cells.Add(new TableCell() { LineHeight = 1 });
            row.Cells.Add(new TableCell() { LineHeight = 1 });
            row.Cells.Add(new TableCell(new BlockUIContainer(new Separator() { HorizontalAlignment = HorizontalAlignment.Stretch })) { ColumnSpan = 2, LineHeight = 1 });
            tableRowGroup.Rows.Add(row);
            row = new TableRow();
            row.Cells.Add(new TableCell() { LineHeight = 1 });
            row.Cells.Add(new TableCell() { LineHeight = 1 });
            row.Cells.Add(new TableCell(new BlockUIContainer(new Separator() { HorizontalAlignment = HorizontalAlignment.Stretch })) { ColumnSpan = 2, LineHeight = 1 });
            tableRowGroup.Rows.Add(row);

            table.RowGroups.Add(tableRowGroup);
            Content.Blocks.Add(table);
        }
    }
}

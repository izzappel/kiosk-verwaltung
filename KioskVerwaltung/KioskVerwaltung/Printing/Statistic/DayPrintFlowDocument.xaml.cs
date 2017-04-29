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

namespace KioskVerwaltung.Printing
{
    /// <summary>
    /// Interaktionslogik für DayPrintFlowDocument.xaml
    /// </summary>
    public partial class DayPrintFlowDocument : FlowDocument
    {
        public DayPrintFlowDocument(DayStatisticViewModel dayViewModel)
        {
            InitializeComponent();
            this.PagePadding = new Thickness(50);

            Content.Blocks.Add(new Paragraph(new Run(string.Format("Monatsrapport: {0:dd.MM.yyyy}", dayViewModel.Day)) { FontWeight = FontWeights.Bold }));

            var table = new Table();
            table.Columns.Add(new TableColumn() { Width = new GridLength(30) });
            table.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            table.Columns.Add(new TableColumn() { Width = new GridLength(80) });
            table.Columns.Add(new TableColumn() { Width = new GridLength(80) });
            table.Columns.Add(new TableColumn() { Width = new GridLength(100) });
            table.Columns.Add(new TableColumn() { Width = new GridLength(100) });
            table.Columns.Add(new TableColumn() { Width = new GridLength(100) });
            table.Columns.Add(new TableColumn() { Width = new GridLength(100) });
            var tableRowGroup = new TableRowGroup();

            var row = new TableRow();
            row.Cells.Add(new TableCell(new Paragraph(new Run("Anz.") { FontWeight = FontWeights.Bold })));
            row.Cells.Add(new TableCell(new Paragraph(new Run("Name") { FontWeight = FontWeights.Bold })));
            row.Cells.Add(new TableCell(new Paragraph()));
            row.Cells.Add(new TableCell(new Paragraph()));
            row.Cells.Add(new TableCell(new Paragraph(new Run("Bargeld") { FontWeight = FontWeights.Bold }) { TextAlignment = System.Windows.TextAlignment.Right }));
            row.Cells.Add(new TableCell(new Paragraph(new Run("Kreditk.") { FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Blue) }) { TextAlignment = System.Windows.TextAlignment.Right }));
            row.Cells.Add(new TableCell(new Paragraph(new Run("Total") { FontWeight = FontWeights.Bold }) { TextAlignment = System.Windows.TextAlignment.Right }));
            row.Cells.Add(new TableCell(new Paragraph(new Run("Privat") { FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Green) }) { TextAlignment = System.Windows.TextAlignment.Right }));
            tableRowGroup.Rows.Add(row);

            foreach (var saleProduct in dayViewModel.TotalSaleProducts)
            {
                row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run(saleProduct.Count.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(saleProduct.Name)) { TextAlignment = System.Windows.TextAlignment.Left }));
                row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("({0:0.00} CHF)", saleProduct.Price))) { TextAlignment = System.Windows.TextAlignment.Right }));
                row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("à {0:0.00} CHF", saleProduct.SellPrice))) { TextAlignment = System.Windows.TextAlignment.Right }));
                row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", saleProduct.TotalCash))) { TextAlignment = System.Windows.TextAlignment.Right }));
                row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", saleProduct.TotalCreditCard)) { Foreground = new SolidColorBrush(Colors.Blue) }) { TextAlignment = System.Windows.TextAlignment.Right }));
                row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", saleProduct.Total)) { FontWeight = FontWeights.Bold }) { TextAlignment = System.Windows.TextAlignment.Right }));
                row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", saleProduct.TotalPrivate)) { Foreground = new SolidColorBrush(Colors.Green) }) { TextAlignment = System.Windows.TextAlignment.Right }));
                tableRowGroup.Rows.Add(row);
            }

            row = new TableRow();
            row.Cells.Add(new TableCell(new BlockUIContainer(new Separator() { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch })) { ColumnSpan = 8 });
            tableRowGroup.Rows.Add(row);

            table.RowGroups.Add(tableRowGroup);
            Content.Blocks.Add(table);

            table = new Table();
            table.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            table.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            table.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            table.Columns.Add(new TableColumn() { Width = new GridLength(150) });
            tableRowGroup = new TableRowGroup();

            row = new TableRow();
            row.Cells.Add(new TableCell(new Paragraph(new Run("Privatbezug:"))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", dayViewModel.TotalPrivate)))));
            row.Cells.Add(new TableCell(new Paragraph(new Run("Bargeld:"))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", dayViewModel.TotalCash))) { TextAlignment = System.Windows.TextAlignment.Right }));
            tableRowGroup.Rows.Add(row);

            row = new TableRow();
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell(new Paragraph(new Run("Kreditkarte:"))));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", dayViewModel.TotalCreditCard))) { TextAlignment = System.Windows.TextAlignment.Right }));
            tableRowGroup.Rows.Add(row);

            row = new TableRow();
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell(new BlockUIContainer(new Separator() { HorizontalAlignment = HorizontalAlignment.Stretch })) { ColumnSpan = 2 });
            tableRowGroup.Rows.Add(row);

            row = new TableRow();
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell());
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("Total vom {0:dd.MM.yyyy}:", dayViewModel.Day)) { FontWeight = FontWeights.Bold }) { TextAlignment = System.Windows.TextAlignment.Left }));
            row.Cells.Add(new TableCell(new Paragraph(new Run(string.Format("{0:0.00} CHF", dayViewModel.Total)) { FontWeight = FontWeights.Bold }) { TextAlignment = System.Windows.TextAlignment.Right }));
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

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
    /// Interaktionslogik für SettingsPrintFlowDocument.xaml
    /// </summary>
    public partial class SettingsPrintFlowDocument : FlowDocument
    {
        public SettingsPrintFlowDocument(IList<BarcodeSetting> barcodeSettings)
        {
            InitializeComponent();
            this.PagePadding = new Thickness(50);

            var table = new Table();
            var tableRowGroup = new TableRowGroup();


            var row = new TableRow();

            int i = 0;
            foreach (var barcodeSetting in barcodeSettings)
            {
                var cell = new TableCell();
                cell.Blocks.Add(new Paragraph(new Run(barcodeSetting.Name)) { TextAlignment = System.Windows.TextAlignment.Center });
                cell.Blocks.Add(new BlockUIContainer(new Barcode() { Data = barcodeSetting.Barcode, Height = 100, Width = 180 }));

                row.Cells.Add(cell);

                i++;
                if (i % 3 == 0)
                {
                    tableRowGroup.Rows.Add(row);
                    row = new TableRow();
                }
            }

            if (i % 3 != 0)
            {
                tableRowGroup.Rows.Add(row);
                row = new TableRow();
            }

            table.RowGroups.Add(tableRowGroup);
            Content.Blocks.Add(table);
        }
    }
}

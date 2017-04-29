using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    public class BarcodePrintPaginator: DocumentPaginator
    {
        private IList<BarcodeSetting> barcodeSettings;
        private Size pageSize;

        private int pageCount;

        private int maxBarcodesPerRow;
        private int maxRowsPerPage;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inventoryItems">The list of inventory items to display.</param>
        /// <param name="pageSize">The size of the page in pixels.</param>
        public BarcodePrintPaginator(IList<BarcodeSetting> barcodeSettings)
        {
            this.barcodeSettings = barcodeSettings;
            this.pageSize = new Size(800, 1056);
            PaginateInventoryItems();
        }

        /// <summary>
        /// Computes the page count based on the number of sale products items
        /// and the page size.
        /// </summary>
        private void PaginateInventoryItems()
        {
            int margins = 200;
            maxBarcodesPerRow = (int)((pageSize.Width - margins) / 200);
            maxRowsPerPage = (int)((pageSize.Height - margins) / 150);
            pageCount = (int)Math.Ceiling(((double)barcodeSettings.Count() / maxBarcodesPerRow) / maxRowsPerPage);
        }

        /// <summary>
        /// Gets a range of inventory items from an array.
        /// </summary>
        /// <param name="array">The inventory items array.</param>
        /// <param name="start">Start index.</param>
        /// <param name="end">End index.</param>
        /// <returns></returns>
        private static IList<BarcodeSetting> GetRange(IList<BarcodeSetting> items, int start, int end)
        {
            List<BarcodeSetting> barcodeItems = new List<BarcodeSetting>();
            for (int i = start; i < end; i++)
            {
                if (i >= items.Count())
                {
                    break;
                }
                barcodeItems.Add(items[i]);
            }
            return barcodeItems;
        }

        #region DocumentPaginator Members

        /// <summary>
        /// When overridden in a derived class, gets the DocumentPage for the
        /// specified page number.
        /// </summary>
        /// <param name="pageNumber">
        /// The zero-based page number of the document page that is needed.
        /// </param>
        /// <returns>
        /// The DocumentPage for the specified pageNumber, or DocumentPage.Missing
        /// if the page does not exist.
        /// </returns>
        public override DocumentPage GetPage(int pageNumber)
        {
            // Compute the range of inventory items to display
            int start = pageNumber * maxRowsPerPage * maxBarcodesPerRow;
            int end = start + maxRowsPerPage * maxBarcodesPerRow;

            BarcodeSettingsPage page = new BarcodeSettingsPage(GetRange(barcodeSettings, start, end), pageSize, pageNumber, (pageNumber + 1 == pageCount));
            page.Measure(pageSize);
            page.Arrange(new Rect(pageSize));

            return new DocumentPage(page);
        }
        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether
        /// PageCount is the total number of pages.
        /// </summary>
        public override bool IsPageCountValid
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a count of the number of
        /// pages currently formatted.
        /// </summary>
        public override int PageCount
        {
            get { return pageCount; }
        }
        /// <summary>
        /// When overridden in a derived class, gets or sets the suggested width
        /// and height of each page.
        /// </summary>
        public override System.Windows.Size PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (pageSize.Equals(value) != true)
                {
                    pageSize = value;
                    PaginateInventoryItems();
                }
            }
        }
        /// <summary>
        /// When overridden in a derived class, returns the element being paginated.
        /// </summary>
        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }

        #endregion
    }
}

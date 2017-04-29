using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    public class StockPrintPaginator: DocumentPaginator
    {
        private IList<Product> products;
        private Size pageSize;

        private Dictionary<int, int> paging;
        private Dictionary<int, StockPageViewModel> pages;

        private int pageCount;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inventoryItems">The list of inventory items to display.</param>
        /// <param name="pageSize">The size of the page in pixels.</param>
        public StockPrintPaginator(IList<Product> products)
        {
            this.products = products;
            this.pageSize = new Size(800, 1120);
            paging = new Dictionary<int, int>();
            PaginateProductItems();
        }

        /// <summary>
        /// Computes the page count based on the number of sale products items
        /// and the page size.
        /// </summary>
        private void PaginateProductItems()
        {
            pages = new Dictionary<int, StockPageViewModel>();
            
            int startProduct = 0;
            int pageNr = 0;
            int margins = 100;
            double height = pageSize.Height - margins;

            paging.Add(pageNr, startProduct);
            pageNr++;

            foreach (var product in products)
            {
                double itemHeight = 0;
                StockItem item = new StockItem();
                item.Product = product;
                item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                item.Arrange(new Rect(item.DesiredSize));
                itemHeight = item.ActualHeight;


                height = height - itemHeight; 
                if (height < 0)
                {
                    paging.Add(pageNr, startProduct);
                    pageNr++;
                    height = pageSize.Height - margins - itemHeight;
                }
                startProduct++;
            }
            pageCount = pageNr;
        }

        /// <summary>
        /// Gets a range of inventory items from an array.
        /// </summary>
        /// <param name="array">The inventory items array.</param>
        /// <param name="start">Start index.</param>
        /// <param name="end">End index.</param>
        /// <returns></returns>
        private static IList<Product> GetRange(IList<Product> items, int start, int end)
        {
            List<Product> productItems = new List<Product>();
            for (int i = start; i < end; i++)
            {
                if (i >= items.Count())
                {
                    break;
                }
                productItems.Add(items[i]);
            }
            return productItems;
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
            int start = paging[pageNumber];
            int end = products.Count;
            if (paging.ContainsKey(pageNumber + 1))
            {
                end = paging[pageNumber + 1];
            }

            StockViewPage page = new StockViewPage(GetRange(products, start, end), pageSize, pageNumber, (pageNumber + 1 == pageCount));
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
                    PaginateProductItems();
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

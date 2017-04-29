using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    public class SalePrintPaginator : DocumentPaginator
    {
        private IList<SaleProduct> saleProducts;
        private Size pageSize;
        private int pageCount;
        private int maxRowsPerPage;

        public SalePrintPaginator(IList<SaleProduct> saleProducts)
        {
            this.saleProducts = saleProducts;
            this.pageSize = new Size(800, 1120);
            PaginateSaleProductItems();
        }

        private void PaginateSaleProductItems()
        {
            int margins = 0;
            int itemHeight = 25;
            maxRowsPerPage = (int)((pageSize.Height - margins) / itemHeight);

            pageCount = (int)Math.Ceiling((double)saleProducts.Count() / maxRowsPerPage);

            int lastPageMargins = 100 + margins;
            int startLastPage = (pageCount - 1) * maxRowsPerPage;
            int countItemsOnLastPage = saleProducts.Count - 1 - startLastPage;
            int lastPageHeight = countItemsOnLastPage * itemHeight;
            if (lastPageHeight > pageSize.Height - lastPageMargins)
            {
                pageCount++;
            }
        }

        private static IList<SaleProduct> GetRange(IList<SaleProduct> items, int start, int end)
        {
            List<SaleProduct> saleProductItems = new List<SaleProduct>();
            for (int i = start; i < end; i++)
            {
                if (i >= items.Count())
                {
                    break;
                }
                saleProductItems.Add(items[i]);
            }
            return saleProductItems;
        }

        #region DocumentPaginator Members

        public override DocumentPage GetPage(int pageNumber)
        {
            // Compute the range of SaleProduct items to display
            int start = pageNumber * maxRowsPerPage;
            int end = start + maxRowsPerPage;
            bool isLastPage = (pageNumber + 1 == pageCount);
            if (!isLastPage)
            {
                SaleViewPrintPage page = new SaleViewPrintPage(GetRange(saleProducts, start, end), pageSize, pageNumber);
                page.Arrange(new Rect(pageSize));
                return new DocumentPage(page);

            }
            else
            {
                SaleViewLastPrintPage page = new SaleViewLastPrintPage(GetRange(saleProducts, start, end), saleProducts, pageSize, pageNumber);
                page.Arrange(new Rect(pageSize));
                return new DocumentPage(page);
            }
        }

        public override bool IsPageCountValid
        {
            get { return true; }
        }
        public override int PageCount
        {
            get { return pageCount; }
        }
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
                    PaginateSaleProductItems();
                }
            }
        }
        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }

        #endregion
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing
{
    public class DayPrintPaginator : DocumentPaginator
    {
        private DayStatisticViewModel viewModel;
        private Size pageSize;
        private int pageCount;
        private int maxRowsPerPage;

        public DayPrintPaginator(DayStatisticViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.pageSize = new Size(800, 1056);
            PaginateSaleProductItems();
        }

        private void PaginateSaleProductItems()
        {
            int margins = 0;
            int itemHeight = 40;
            maxRowsPerPage = (int)((pageSize.Height - margins) / itemHeight);

            pageCount = (int)Math.Ceiling((double)viewModel.TotalSaleProducts.Count() / maxRowsPerPage);

            int lastPageMargins = 100 + margins;
            int startLastPage = (pageCount - 1) * maxRowsPerPage;
            int countItemsOnLastPage = viewModel.TotalSaleProducts.Count - 1 - startLastPage;
            int lastPageHeight = countItemsOnLastPage * itemHeight;
            if (lastPageHeight > pageSize.Height - lastPageMargins)
            {
                pageCount++;
            }
        }

        private static IList<TotalSaleProduct> GetRange(IList<TotalSaleProduct> items, int start, int end)
        {
            List<TotalSaleProduct> saleProductItems = new List<TotalSaleProduct>();
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
        private static IList<Sale> GetRange(IList<Sale> items, int start, int end)
        {
            List<Sale> saleProductItems = new List<Sale>();
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
            if (isLastPage)
            {
                DayViewLastPrintPage page = new DayViewLastPrintPage(viewModel.Day, GetRange(viewModel.TotalSaleProducts, start, end), viewModel.TotalSaleProducts, pageSize, pageNumber);
                page.Arrange(new Rect(pageSize));
                return new DocumentPage(page);
            }
            else
            {
                DayViewPrintPage page = new DayViewPrintPage(viewModel.Day, GetRange(viewModel.TotalSaleProducts, start, end), pageSize, pageNumber);
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

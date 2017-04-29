using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Globalization;

namespace KioskVerwaltung.Printing
{
    public class MonthPrintPaginator : DocumentPaginator
    {
        private MonthStatisticViewModel viewModel;
        private Size pageSize;
        private int pageCount;
        private int maxRowsPerPage;

        public MonthPrintPaginator(MonthStatisticViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.pageSize = new Size(800, 1120);
            PaginateSaleProductItems();
        }

        private void PaginateSaleProductItems()
        {
            int margins = 180;
            int itemHeight = 35;
            maxRowsPerPage = (int)((pageSize.Height - margins) / itemHeight);

            pageCount = (int)Math.Ceiling((double)viewModel.TotalSaleProducts.Count() / maxRowsPerPage);

            int lastPageMargins = 100 + margins;
            int startLastPage = (pageCount - 1) * maxRowsPerPage;
            int countItemsOnLastPage = viewModel.TotalSaleProducts.Count - 1 - startLastPage;
            int lastPageHeight = countItemsOnLastPage * itemHeight;
            if (lastPageHeight >= pageSize.Height - lastPageMargins)
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

        #region DocumentPaginator Members

        public override DocumentPage GetPage(int pageNumber)
        {
            // Compute the range of SaleProdcut items to display
            int start = pageNumber * maxRowsPerPage;
            int end = start + maxRowsPerPage;

            bool isLastPage = (pageNumber + 1 == pageCount);
            if (isLastPage)
            {
                MonthViewLastPrintPage page = new MonthViewLastPrintPage(viewModel.Month, GetRange(viewModel.TotalSaleProducts, start, end), viewModel.TotalSaleProducts, pageSize, pageNumber);
                page.Arrange(new Rect(pageSize));
                return new DocumentPage(page);
            }
            else
            {
                MonthViewPrintPage page = new MonthViewPrintPage(viewModel.Month, GetRange(viewModel.TotalSaleProducts, start, end), pageSize, pageNumber);
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
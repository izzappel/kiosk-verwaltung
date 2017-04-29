using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.Printing.Start
{
    public class StartPrintPaginator : DocumentPaginator
    {
        private StartViewModel viewModel;
        private Size pageSize;
        private int pageCount;
        private int printedExpiringProductsCount;
        private int maxRowsPerPage;

        public StartPrintPaginator(StartViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.pageSize = new Size(800, 1056);
            printedExpiringProductsCount = 0;
            PaginateProductItems();
        }

        private void PaginateProductItems()
        {
            int margins = 0;
            int itemHeight = 28;
            maxRowsPerPage = (int)((pageSize.Height - margins) / itemHeight);

            int countShortInStockProducts = viewModel.ProductsShortInStock.Count;
            int count = countShortInStockProducts;
            foreach (var product in viewModel.ExpiringProducts)
            {
                count++;
                foreach (var consignment in product.Consignments)
                {
                    count++;
                }
            }
            pageCount = (int)Math.Ceiling((double)count / maxRowsPerPage);

        }
        private IList<Product> GetRangeExpriringProducts(IList<Product> expiringItems, int start, int end)
        {
            List<Product> saleProductItems = new List<Product>();
            int count = 0;
            int i = start;

            foreach (var item in expiringItems)
            {
                Product product = new Product(item.Id, item.Name, item.Barcode, item.HasExpirationDate, item.HasConsignmentPrice, item.Price, new List<Consignment>());

                if (count == i)
                {
                    i++;
                }
                count++;

                foreach (var consignment in item.Consignments)
                {
                    if (count == i)
                    {
                        i++;
                        product.Consignments.Add(consignment);
                    }
                    count++;

                    if (count == end)
                    {
                        break;
                    }
                }


                if (product.Consignments.Count > 0)
                {
                    saleProductItems.Add(product);
                }

                if (count == end)
                {
                    break;
                }
            }
            return saleProductItems;
        }
        private IList<Product> GetRangeShortInStockProducts(IList<Product> shortInStockItems, int start, int end)
        {
            List<Product> saleProductItems = new List<Product>();
            for (int i = start; i < end; i++)
            {
                if (i >= shortInStockItems.Count())
                {
                    break;
                }
                saleProductItems.Add(shortInStockItems[i]);
            }
            return saleProductItems;
        }


        #region DocumentPaginator Members

        public override DocumentPage GetPage(int pageNumber)
        {
            // Compute the range of SaleProduct items to display
            int start = pageNumber * maxRowsPerPage;
            int end = start + maxRowsPerPage;

            IList<Product> expiringProducts = GetRangeExpriringProducts(viewModel.ExpiringProducts, start, end);
            IList<Product> shortInStockProducts = new List<Product>();
            if (expiringProducts.Count == 0)
            {
                start -= printedExpiringProductsCount;
                end -= printedExpiringProductsCount;
                shortInStockProducts = GetRangeShortInStockProducts(viewModel.ProductsShortInStock, start, end);
            }
            else
            {
                int count = end - start;
                start = 0;
                int expCount = 0;
                foreach (var product in expiringProducts)
                {
                    expCount++;
                    foreach (var consignment in product.Consignments)
                    {
                        expCount++;
                    }
                }
                printedExpiringProductsCount += expCount;
                end = count - expCount;
                shortInStockProducts = GetRangeShortInStockProducts(viewModel.ProductsShortInStock, start, end);
            }
            StartViewPage page = new StartViewPage(expiringProducts, shortInStockProducts, pageSize);

            page.Arrange(new Rect(pageSize));
            return new DocumentPage(page);
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
                    PaginateProductItems();
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

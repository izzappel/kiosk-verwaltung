using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    public class BasketItem
    {
        public Product Product { get; set; }
        public Consignment Consignment { get; set; }

        public SaleProduct SaleProduct { get; set; }

        public BasketItem(Product product, Consignment consignment, SaleProduct saleProduct)
        {
            Product = product;
            Consignment = consignment;
            SaleProduct = saleProduct;
        }
    }
}

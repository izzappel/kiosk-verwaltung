using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Xml;
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung.DataAccess
{
    public class DataAccess
    {
        public KioskDataSet KioskDataSet
        {
            get { return kioskDataSet; }
            private set
            {
                kioskDataSet = value;
            }
        }
        private KioskDataSet kioskDataSet;

        public IList<Product> Products
        {
            get { return products; }
            set { products = value; }
        }
        private IList<Product> products;


        public SaleDataSet SaleDataSet
        {
            get { return saleDataSet; }
            private set
            {
                saleDataSet = value;
            }
        }
        private SaleDataSet saleDataSet;

        public IList<Sale> Sales
        {
            get { return sales; }
            set { sales = value; }
        }
        private IList<Sale> sales;


        public SettingsDataSet SettingsDataSet
        {
            get { return settingsDataSet; }
            private set
            {
                settingsDataSet = value;
            }
        }
        private SettingsDataSet settingsDataSet;

        public IList<BarcodeSetting> BarcodeSettings
        {
            get { return barcodeSettings; }
            set { barcodeSettings = value; }
        }
        private IList<BarcodeSetting> barcodeSettings;


        private IList<Observer> observers;

        private DataAccess()
        {
            kioskDataSet = new KioskDataSet();
            products = new List<Product>();

            saleDataSet = new SaleDataSet();
            sales = new List<Sale>();

            settingsDataSet = new SettingsDataSet();
            barcodeSettings = new List<BarcodeSetting>();

            observers = new List<Observer>();
        }

        public string GetBarcode(string barcode)
        {
            if (barcode != null)
            {
                return barcode.Trim();
            }
            return string.Empty;
        }

        public void Attach(Observer observer)
        {
            if (!observers.Contains(observer)) { observers.Add(observer); }
        }
        public void Detach(Observer observer)
        {
            if (observers.Contains(observer)) { observers.Remove(observer); }
        }

        private void Notify()
        {
            UpdateProducts();
            UpdateSaleProducts();
            UpdateBarcodeSettings();
            foreach (var observer in observers)
            {
                observer.Udpate();
            }
        }
        public void UpdateProducts()
        {
            products.Clear();
            foreach (var row in kioskDataSet.Product)
            {
                List<Consignment> consignments = new List<Consignment>();
                foreach (var consignment in row.GetConsignmentRows())
                {
                    consignments.Add(new Consignment(consignment.Id, consignment.NumberOfContent, consignment.ExpirationDate, consignment.Price));
                }

                consignments = new List<Consignment>(consignments.OrderBy(c => c.ExpirationDate));
                
                products.Add(new Product(row.Id, row.Name, row.Barcode, row.HasExpirationDate, row.HasConsignmentPrice, row.Price, consignments));
            }
            
            products = new List<Product>(products.OrderBy(p => p.Name));
        }
        public void UpdateSaleProducts()
        {
            sales.Clear();
            foreach (var row in saleDataSet.Sale)
            {
                List<SaleProduct> saleProducts = new List<SaleProduct>();

                foreach (var saleProduct in row.GetSaleProductRows())
                {
                    saleProducts.Add(new SaleProduct(saleProduct.Id, saleProduct.ProductId, saleProduct.Name, saleProduct.Price, saleProduct.IsPaidByCreditCard, saleProduct.IsPrivate, saleProduct.Deduction, saleProduct.SellPrice));
                }
                sales.Add(new Sale(row.Id, row.Date, saleProducts));
            }
        }
        public void UpdateBarcodeSettings()
        {
            barcodeSettings.Clear();
            foreach (var row in settingsDataSet.BarcodeSetting)
            {
                BarcodeSetting setting = new BarcodeSetting(row.Id, row.Name, row.Value, row.Barcode, row.IsFixPrice);
                barcodeSettings.Add(setting);
            }

            barcodeSettings = new List<BarcodeSetting>(barcodeSettings.OrderBy(b => b.Name));
        }

        #region KioskDataSet
        public void ReadKioskFile(string filename)
        {
            try
            {
                kioskDataSet.Clear();
                kioskDataSet.ReadXml(filename);
                Notify();
            }
            catch (SecurityException securityException)
            {
                //Log exception
                throw securityException;
            }
            catch (XmlException xmlException)
            {
                //Log exception
                throw xmlException;
            }
        }

        public void SaveKioskToFile(string filename)
        {
            try
            {
                kioskDataSet.WriteXml(filename);
            }
            catch (SecurityException exception)
            {
                //Log exception
                throw exception;
            }
        }

        public Product GetProductById(int id)
        {
            foreach (var product in products)
            {
                if (product.Id == id) return product;
            }
            return null;
        }

        public void AddProduct(Product product)
        {
            products.Add(product);

            KioskDataSet.ProductRow row = kioskDataSet.Product.NewProductRow();
            row.Name = product.Name;
            row.Barcode = GetBarcode(product.Barcode);
            row.HasExpirationDate = product.HasExpirationDate;
            row.HasConsignmentPrice = product.HasConsignmentPrice;
            row.Price = product.Price;

            kioskDataSet.Product.AddProductRow(row);
            kioskDataSet.AcceptChanges();
            Notify();
        }

        public void EditProduct(Product product)
        {
            Product oldProduct = GetProductById(product.Id);
            oldProduct.Name = product.Name;
            oldProduct.Barcode = GetBarcode(product.Barcode);
            oldProduct.HasExpirationDate = product.HasExpirationDate;
            oldProduct.HasConsignmentPrice = product.HasConsignmentPrice;
            oldProduct.Price = product.Price;

            KioskDataSet.ProductRow row = kioskDataSet.Product.FindById(product.Id);
            row.Name = product.Name;
            row.Barcode = product.Barcode;
            row.HasExpirationDate = product.HasExpirationDate;
            row.HasConsignmentPrice = product.HasConsignmentPrice;
            row.Price = product.Price;

            kioskDataSet.AcceptChanges();
            Notify();
        }

        public void RemoveProduct(int productId)
        {
            Product product = GetProductById(productId);
            products.Remove(product);

            KioskDataSet.ProductRow row = kioskDataSet.Product.FindById(product.Id);
            if (row != null)
            {
                kioskDataSet.Product.RemoveProductRow(row);
            }

            kioskDataSet.AcceptChanges();
            Notify();
        }

        public void AddConsigment(int productId, Consignment consignment)
        {
            Product product = GetProductById(productId);
            product.Consignments.Add(consignment);

            KioskDataSet.ConsignmentRow row = kioskDataSet.Consignment.NewConsignmentRow();
            row.NumberOfContent = consignment.NumberOfContent;
            row.ExpirationDate = consignment.ExpirationDate;
            row.ProductId = productId;
            row.Price = consignment.Price;

            kioskDataSet.Consignment.AddConsignmentRow(row);
            kioskDataSet.AcceptChanges();
            Notify();
        }

        public void EditConsignment(int productId, Consignment consignment)
        {
            Product product = GetProductById(productId);
            if (product.Consignments.Contains(consignment))
            {
                //it's ok
                Consignment oldConsignment = GetConsignmentById(consignment.Id);
                oldConsignment.NumberOfContent = consignment.NumberOfContent;
                oldConsignment.ExpirationDate = consignment.ExpirationDate;
                oldConsignment.Price = consignment.Price;
            }

            KioskDataSet.ConsignmentRow row = kioskDataSet.Consignment.FindById(consignment.Id);
            row.NumberOfContent = consignment.NumberOfContent;
            row.ExpirationDate = consignment.ExpirationDate;
            row.ProductId = productId;
            row.Price = consignment.Price;

            kioskDataSet.AcceptChanges();
            Notify();
        }

        public void RemoveConsignment(int productId, int consignmentId)
        {
            Product product = GetProductById(productId);
            Consignment consignment = GetConsignmentById(consignmentId);
            if (consignment != null)
            {
                product.Consignments.Remove(consignment);
            }

            KioskDataSet.ConsignmentRow row = kioskDataSet.Consignment.FindById(consignment.Id);
            if (row != null)
            {
                kioskDataSet.Consignment.RemoveConsignmentRow(row);
            }

            kioskDataSet.AcceptChanges();
            Notify();
        }

        private Consignment GetConsignmentById(int consignmentId)
        {
            foreach (var product in products)
            {
                foreach (var consignment in product.Consignments)
                {
                    if (consignment.Id == consignmentId) return consignment;
                }
            }
            return null;
        }
        #endregion KioskDataSet

        #region SaleDataSet

        public void ReadSaleFile(string filename)
        {
            try
            {
                saleDataSet.Clear();
                saleDataSet.ReadXml(filename);

                //Für alte XMLs, welche keine SellPrice-Spalte enthalten
                foreach (var sale in saleDataSet.Sale)
                {
                    foreach (var saleProduct in sale.GetSaleProductRows())
                    {
                        if (saleProduct.SellPrice < 0) saleProduct.SellPrice = saleProduct.Price;
                    }
                }
                saleDataSet.AcceptChanges();


                Notify();
            }
            catch (SecurityException securityException)
            {
                //Log exception
                throw securityException;
            }
            catch (XmlException xmlException)
            {
                //Log exception
                throw xmlException;
            }
        }
        public IList<Sale> ReadSalesFromFile(string filename)
        {
            IList<Sale> sales = new List<Sale>();
            try
            {

                SaleDataSet dataSet = new KioskVerwaltung.DataAccess.SaleDataSet();
                dataSet.ReadXml(filename);

                //Für alte XMLs, welche keine SellPrice-Spalte enthalten
                foreach (var sale in dataSet.Sale)
                {
                    foreach (var saleProduct in sale.GetSaleProductRows())
                    {
                        if (saleProduct.SellPrice < 0) saleProduct.SellPrice = saleProduct.Price;
                    }
                }
                dataSet.AcceptChanges();

                foreach (var row in dataSet.Sale)
                {
                    List<SaleProduct> saleProducts = new List<SaleProduct>();

                    foreach (var saleProduct in row.GetSaleProductRows())
                    {
                        saleProducts.Add(new SaleProduct(saleProduct.Id, saleProduct.ProductId, saleProduct.Name, saleProduct.Price, saleProduct.IsPaidByCreditCard, saleProduct.IsPrivate, saleProduct.Deduction, saleProduct.SellPrice));
                    }
                    sales.Add(new Sale(row.Id, row.Date, saleProducts));
                }
            }
            catch (SecurityException securityException)
            {
                //Log exception
                throw securityException;
            }
            catch (XmlException xmlException)
            {
                //Log exception
                throw xmlException;
            }

            return sales;
        }
        public void SaveSaleToFile(string filename)
        {
            try
            {
                saleDataSet.WriteXml(filename);
            }
            catch (SecurityException exception)
            {
                //Log exception
                throw exception;
            }
        }

        public void AddSale(Sale sale)
        {
            sales.Add(sale);

            SaleDataSet.SaleRow row = saleDataSet.Sale.NewSaleRow();
            row.Date = sale.Date;
            saleDataSet.Sale.AddSaleRow(row);

            saleDataSet.AcceptChanges();
            Notify();
        }
        public void AddSaleProduct(int saleId, SaleProduct saleProduct)
        {
            Sale sale = GetSaleById(saleId);
            sale.SaleProducts.Add(saleProduct);

            SaleDataSet.SaleProductRow row = saleDataSet.SaleProduct.NewSaleProductRow();
            row.Name = saleProduct.Name;
            row.ProductId = saleProduct.ProductId;
            row.Price = saleProduct.Price;
            row.IsPaidByCreditCard = saleProduct.IsPaidByCreditCard;
            row.IsPrivate = saleProduct.IsPrivate;
            row.Deduction = saleProduct.Deduction;
            row.SellPrice = saleProduct.SellPrice;
            row.SaleId = saleId;

            saleDataSet.SaleProduct.AddSaleProductRow(row);
            saleDataSet.AcceptChanges();
            Notify();
        }

        public void RemoveSaleProduct(int saleId, int saleProductId)
        {
            Sale sale = GetSaleById(saleId);
            SaleProduct saleProuct = GetSaleProductById(saleProductId);
            sale.SaleProducts.Remove(saleProuct);

            SaleDataSet.SaleProductRow row = saleDataSet.SaleProduct.FindById(saleProductId);
            if (row != null)
            {
                saleDataSet.SaleProduct.RemoveSaleProductRow(row);
            }
            saleDataSet.AcceptChanges();
            Notify();
        }
        public void EditSaleProduct(int saleId, SaleProduct saleProduct)
        {
            Sale sale = GetSaleById(saleId);
            SaleProduct oldSaleProduct = GetSaleProductById(saleProduct.Id);
            oldSaleProduct.Name = saleProduct.Name;
            oldSaleProduct.ProductId = saleProduct.ProductId;
            oldSaleProduct.Price = saleProduct.Price;
            oldSaleProduct.IsPaidByCreditCard = saleProduct.IsPaidByCreditCard;
            oldSaleProduct.IsPrivate = saleProduct.IsPrivate;
            oldSaleProduct.Deduction = saleProduct.Deduction;
            oldSaleProduct.SellPrice = saleProduct.SellPrice;

            SaleDataSet.SaleProductRow row = saleDataSet.SaleProduct.FindById(saleProduct.Id);
            row.Name = saleProduct.Name;
            row.ProductId = saleProduct.ProductId;
            row.Price = saleProduct.Price;
            row.IsPaidByCreditCard = saleProduct.IsPaidByCreditCard;
            row.IsPrivate = saleProduct.IsPrivate;
            row.Deduction = saleProduct.Deduction;
            row.SellPrice = saleProduct.SellPrice;

            saleDataSet.AcceptChanges();
            Notify();
        }

        private Sale GetSaleById(int saleId)
        {
            foreach (var sale in sales)
            {
                if (sale.Id == saleId) return sale;
            }
            return null;
        }
        private SaleProduct GetSaleProductById(int saleProductId)
        {
            foreach (var sale in sales)
            {
                foreach (var saleProduct in sale.SaleProducts)
                {
                    if (saleProduct.Id == saleProductId) return saleProduct;
                }

            }
            return null;
        }
        #endregion SaleDataSet

        #region SettingsDataSet

        public void ReadSettingsFile(string filename)
        {
            try
            {
                settingsDataSet.Clear();
                settingsDataSet.ReadXml(filename);

                Notify();
            }
            catch (SecurityException securityException)
            {
                //Log exception
                throw securityException;
            }
            catch (XmlException xmlException)
            {
                //Log exception
                throw xmlException;
            }
        }

        public void SaveSettingsFile(string filename)
        {
            try
            {
                settingsDataSet.WriteXml(filename);
            }
            catch (SecurityException exception)
            {
                //Log exception
                throw exception;
            }
        }

        public void AddBarcodeSetting(BarcodeSetting setting)
        {
            barcodeSettings.Add(setting);

            SettingsDataSet.BarcodeSettingRow row = settingsDataSet.BarcodeSetting.NewBarcodeSettingRow();
            row.Name = setting.Name;
            row.Value = setting.Value;
            row.Barcode = GetBarcode(setting.Barcode);
            row.IsFixPrice = setting.IsFixPrice;
            settingsDataSet.BarcodeSetting.AddBarcodeSettingRow(row);

            settingsDataSet.AcceptChanges();
            Notify();
        }
        public void EditBarcodeSetting(BarcodeSetting setting)
        {
            BarcodeSetting oldSetting = barcodeSettings.FirstOrDefault(s => s.Id == setting.Id);
            oldSetting.Name = setting.Name;
            oldSetting.Value = setting.Value;
            oldSetting.Barcode = GetBarcode(setting.Barcode);
            oldSetting.IsFixPrice = setting.IsFixPrice;

            SettingsDataSet.BarcodeSettingRow row = settingsDataSet.BarcodeSetting.FindById(setting.Id);
            row.Name = setting.Name;
            row.Value = setting.Value;
            row.Barcode = GetBarcode(setting.Barcode);
            row.IsFixPrice = setting.IsFixPrice;

            settingsDataSet.AcceptChanges();
            Notify();
        }

        public void RemoveBarcodeSettings(int settingId)
        {
            BarcodeSetting setting = barcodeSettings.FirstOrDefault(s => s.Id == settingId);
            barcodeSettings.Remove(setting);

            SettingsDataSet.BarcodeSettingRow row = settingsDataSet.BarcodeSetting.FindById(setting.Id);
            settingsDataSet.BarcodeSetting.RemoveBarcodeSettingRow(row);

            settingsDataSet.AcceptChanges();
            Notify();
        }

        #endregion SettingsDataSet

        #region Singleton
        private static DataAccess instance;
        public static DataAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataAccess();
                }
                return instance;
            }
        }
        #endregion Singleton
    }
}

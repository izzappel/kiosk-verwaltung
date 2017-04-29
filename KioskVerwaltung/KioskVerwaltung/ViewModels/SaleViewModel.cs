using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using KioskVerwaltung.BusinessObjects;
using System.Collections.ObjectModel;
using System.IO;
using KioskVerwaltung.Printing;
using System.Drawing.Printing;

namespace KioskVerwaltung
{
    public class SaleViewModel : Observer, INotifyPropertyChanged
    {
        public Sale SaleToday
        {
            get { return saleToday; }
            set
            {
                saleToday = value;
                OnPropertyChanged("SaleToday");
            }
        }
        public ObservableCollection<Sale> Sales
        {
            get { return sales; }
            set
            {
                sales = value;
                OnPropertyChanged("Sales");
            }
        }


        public ObservableCollection<BasketItem> BasketItems
        {
            get { return basketItems; }
            set
            {
                basketItems = value;
                OnPropertyChanged("BasketItems");
            }
        }


        public bool IsBasketPaidByCreditCard
        {
            get { return isBasketPaidByCreditCard; }
        }
        public bool IsBasketPrivate
        {
            get { return isBasketPrivate; }
        }
        public double BasketTotal
        {
            get { return GetBasketTotal(); }
        }

        private Sale saleToday;
        private ObservableCollection<Sale> sales;
        private bool isBasketPaidByCreditCard;
        private bool isBasketPrivate;

        private ObservableCollection<BasketItem> basketItems;

        private DataAccess.DataAccess dataAccess;
        public SaleViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            dataAccess.Attach(this);

            basketItems = new ObservableCollection<BasketItem>();
            sales = new ObservableCollection<Sale>(dataAccess.Sales);
            InitializeSaleToday();
            UpdateSaleToday();
        }

        private void InitializeSaleToday()
        {
            Sale sale = sales.FirstOrDefault(s => s.Date.Date.Equals(DateTime.Today));
            if (sale == null)
            {
                sale = new Sale(DateTime.Today);
                AddSale(sale);
            }
        }

        public void AddSale(Sale sale)
        {
            dataAccess.AddSale(sale);
        }
        public void AddSaleProduct(SaleProduct saleProduct)
        {
            dataAccess.AddSaleProduct(saleToday.Id, saleProduct);
        }

        public void DecrementConsignment(int productId, Consignment consignment)
        {
            if (consignment != null)
            {
                consignment.NumberOfContent--;
                dataAccess.EditConsignment(productId, consignment);
            }
        }
        public void IncrementConsignment(int productId, Consignment consignment)
        {
            if (consignment != null)
            {
                consignment.NumberOfContent++;
                dataAccess.EditConsignment(productId, consignment);
            }
        }
        private Consignment GetLatestConsignment(Product product)
        {
            Consignment latest = null;
            foreach (var consignment in product.Consignments)
            {
                if (latest == null) { latest = consignment; }
                if (product.HasExpirationDate)
                {
                    if ((consignment.ExpirationDate <= latest.ExpirationDate && consignment.NumberOfContent > 0) || (consignment.ExpirationDate > latest.ExpirationDate && latest.NumberOfContent == 0))
                    {
                        latest = consignment;
                    }
                }

                if (product.HasConsignmentPrice)
                {
                    if ((consignment.Price <= latest.Price && consignment.NumberOfContent > 0) || (consignment.Price > latest.Price && latest.NumberOfContent == 0))
                    {
                        latest = consignment;
                    }
                }
            }
            return latest;
        }

        public void ClearBasket()
        {
            foreach (var basketItem in basketItems)
            {
                AddSaleProduct(basketItem.SaleProduct);
            }
            basketItems.Clear();

            isBasketPaidByCreditCard = false;
            isBasketPrivate = false;

            OnPropertyChanged("IsBasketPaidByCreditCard");
            OnPropertyChanged("IsBasketPrivate");
            OnPropertyChanged("BasketTotal");
            OnPropertyChanged("BasketItems");
        }

        public bool ScanProduct(string barcode)
        {
            if (barcode.Equals(Properties.Settings.Default.ClearBasketBarcode))
            {
                ClearBasket();
                return true;
            }

            IList<Product> products = dataAccess.Products;
            foreach (var product in products)
            {
                if (product.Barcode.Equals(barcode))
                {
                    Consignment consignment = GetLatestConsignment(product);
                    if (consignment == null)
                    {
                        consignment = new Consignment(0, 0, DateTime.Now, 0);
                        dataAccess.AddConsigment(product.Id, consignment);
                    }

                    consignment = GetLatestConsignment(dataAccess.GetProductById(product.Id));

                    double price = product.Price;
                    if (product.HasConsignmentPrice)
                    {
                        price = consignment.Price;
                    }
                    SaleProduct saleProduct = new SaleProduct(0, product.Id, product.Name, price, isBasketPaidByCreditCard, isBasketPrivate, "", price);


                    BasketItem basketItem = new BasketItem(product, consignment, saleProduct);
                    basketItems.Add(basketItem);

                    DecrementConsignment(product.Id, consignment);

                    OnPropertyChanged("BasketTotal");
                    OnPropertyChanged("BasketItems");

                    return true;
                }
            }

            return HandleSettingsBarcode(barcode);
        }
        private bool HandleSettingsBarcode(string barcode)
        {
            IList<BarcodeSetting> settings = dataAccess.BarcodeSettings;
            foreach (var setting in settings)
            {
                if (setting.Barcode.Equals(barcode))
                {
                    if (basketItems.Count > 0)
                    {
                        BasketItem basketItem = basketItems.Last();
                        SaleProduct saleProduct = basketItem.SaleProduct;
                        saleProduct.Deduction = setting.Name;
                        if (setting.IsFixPrice)
                        {
                            saleProduct.SellPrice = setting.Value;
                        }
                        else
                        {
                            saleProduct.SellPrice = Math.Round(10 * ((1 - setting.Value) * saleProduct.Price), MidpointRounding.AwayFromZero) / 10;
                        }

                        OnPropertyChanged("BasketTotal");
                        OnPropertyChanged("BasketItems");
                    }
                    return true;
                }
            }
            return false;
        }

        public void RemoveSaleProduct(BasketItem basketItem)
        {
            basketItems.Remove(basketItem);

            IncrementConsignment(basketItem.Product.Id, basketItem.Consignment);
            
            OnPropertyChanged("BasketTotal");
            OnPropertyChanged("BasketItems");
        }

        public void CleanForClosing()
        {
            foreach (var basketItem in basketItems)
            {
                IncrementConsignment(basketItem.Product.Id, basketItem.Consignment);
            }
        }

        public void SetIsBasketPaidByCreditCard(bool isBasketPaidByCreditCard)
        {
            foreach (var basketItem in basketItems)
            {
                SaleProduct saleProduct = basketItem.SaleProduct;
                saleProduct.IsPaidByCreditCard = isBasketPaidByCreditCard;
                saleProduct.IsPrivate = false;
            }
            this.isBasketPaidByCreditCard = isBasketPaidByCreditCard;
            this.isBasketPrivate = false;

            OnPropertyChanged("IsBasketPaidByCreditCard");
            OnPropertyChanged("IsBasketPrivate");
        }
        public void SetIsBasketPrivate(bool isBasketPrivate)
        {
            foreach (var basketItem in basketItems)
            {
                SaleProduct saleProduct = basketItem.SaleProduct;
                saleProduct.IsPrivate = isBasketPrivate;
                saleProduct.IsPaidByCreditCard = false;
            }
            this.isBasketPrivate = isBasketPrivate;
            this.isBasketPaidByCreditCard = false;

            OnPropertyChanged("IsBasketPaidByCreditCard");
            OnPropertyChanged("IsBasketPrivate");
        }

        public void SetIsPaidByCreditCardToSaleProduct(bool isPaidByCreditCard, SaleProduct saleProduct)
        {
            saleProduct.IsPaidByCreditCard = isPaidByCreditCard;
            saleProduct.IsPrivate = (isPaidByCreditCard == true) ? false : saleProduct.IsPrivate;
            dataAccess.EditSaleProduct(saleToday.Id, saleProduct);

            OnPropertyChanged("Sales");
            OnPropertyChanged("SaleToday");
        }
        public void SetIsPrivateToSaleProduct(bool isPrivate, SaleProduct saleProduct)
        {
            saleProduct.IsPrivate = isPrivate;
            saleProduct.IsPaidByCreditCard = (isPrivate == true) ? false : saleProduct.IsPrivate;
            dataAccess.EditSaleProduct(saleToday.Id, saleProduct);

            OnPropertyChanged("Sales");
            OnPropertyChanged("SaleToday");
        }

        public void DeleteSaleProduct(SaleProduct saleProduct)
        {
            dataAccess.RemoveSaleProduct(saleToday.Id, saleProduct.Id);

            OnPropertyChanged("Sales");
            OnPropertyChanged("SaleToday");
        }

        private double GetBasketTotal()
        {
            double total = 0;

            foreach (var basketItem in basketItems)
            {
                SaleProduct saleProduct = basketItem.SaleProduct;
                total += saleProduct.SellPrice;
            }
            return total;
        }

        public void Udpate()
        {
            sales = new ObservableCollection<Sale>(dataAccess.Sales);
            OnPropertyChanged("Sales");

            UpdateSaleToday();
        }
        private void UpdateSaleToday()
        {
            saleToday = sales.FirstOrDefault(s => s.Date.Date.Equals(DateTime.Today));
            OnPropertyChanged("SaleToday");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

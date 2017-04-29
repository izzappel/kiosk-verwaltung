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
    public class SaleViewModel : Observer, INotifyPropertyChanged, Receiver
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
        public ObservableCollection<SaleProduct> ShoppingBasket
        {
            get { return shoppingBasket; }
            set
            {
                shoppingBasket = value;
                OnPropertyChanged("ShoppingBasket");
            }
        }

        public ObservableCollection<Invoker> Invokers
        {
            get { return invokers; }
            set
            {
                invokers = value;
                OnPropertyChanged("Invokers");
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
        private ObservableCollection<SaleProduct> shoppingBasket;
        private bool isBasketPaidByCreditCard;
        private bool isBasketPrivate;

        private ObservableCollection<Invoker> invokers;

        private DataAccess.DataAccess dataAccess;
        public SaleViewModel()
        {
            dataAccess = DataAccess.DataAccess.Instance;
            dataAccess.Attach(this);

            shoppingBasket = new ObservableCollection<SaleProduct>();
            invokers = new ObservableCollection<Invoker>();
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

        public void DecrementConsignment(int productId)
        {
            Consignment consignment = GetLatestConsignment(dataAccess.GetProductById(productId));
            if (consignment != null)
            {
                consignment.NumberOfContent--;
                dataAccess.EditConsignment(productId, consignment);
            }
            else
            {
                consignment = new Consignment(0, -1, DateTime.Now);
                dataAccess.AddConsigment(productId, consignment);
            }
        }
        public void IncrementConsignment(int productId)
        {
            Consignment consignment = GetLatestConsignment(dataAccess.GetProductById(productId));
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
                if (consignment.ExpirationDate < latest.ExpirationDate && latest.NumberOfContent != 0)
                {
                    latest = consignment;
                }
            }
            return latest;
        }

        public void ClearBasket()
        {
            shoppingBasket.Clear();
            foreach (var invoker in invokers)
            {
                invoker.ExecuteCommand();
            }
            invokers.Clear();

            isBasketPaidByCreditCard = false;
            isBasketPrivate = false;
            
            OnPropertyChanged("IsBasketPaidByCreditCard");
            OnPropertyChanged("IsBasketPrivate");
            OnPropertyChanged("BasketTotal");
            OnPropertyChanged("Invokers");
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
                    SaleProduct saleProduct = new SaleProduct(0, product.Name, product.Price, isBasketPaidByCreditCard, isBasketPrivate, "");
                    AddSaleProductToBasket(product, saleProduct);
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
                    if (invokers.Count > 0)
                    {
                        Invoker invoker = invokers.Last();
                        SaleProduct saleProduct = (invoker.Command as AddSaleProductCommand).SaleProduct;
                        saleProduct.Deduction = setting.Name;
                        if (setting.IsFixPrice)
                        {
                            saleProduct.Price = setting.Value;
                        }
                        else
                        {
                            saleProduct.Price = Math.Round(10*((1 - setting.Value) * saleProduct.Price), MidpointRounding.AwayFromZero)/10;
                        }

                        OnPropertyChanged("BasketTotal");
                        OnPropertyChanged("Invokers");
                    }
                    return true;
                }
            }
            return false;
        }

        public void AddSaleProductToBasket(Product product, SaleProduct saleProduct)
        {
            Command command = new AddSaleProductCommand(this, product.Id, saleProduct);
            Invoker invoker = new Invoker();

            invoker.Command = command;
            invokers.Add(invoker);

            //DecrementConsignment(product.Id);

            shoppingBasket.Add(saleProduct);
            
            OnPropertyChanged("BasketTotal");
            OnPropertyChanged("Invokers");
        }
        public void RemoveSaleProduct(Invoker invoker)
        {
            AddSaleProductCommand addSaleProductCommand = invoker.Command as AddSaleProductCommand;
            shoppingBasket.Remove(addSaleProductCommand.SaleProduct);

            //IncrementConsignment(((AddSaleProductCommand)invoker.Command).ProductId);
            invokers.Remove(invoker);
            OnPropertyChanged("BasketTotal");
            OnPropertyChanged("Invokers");
        }
        public void SetIsBasketPaidByCreditCard(bool isBasketPaidByCreditCard)
        {
            if (!IsBasketPrivate)
            {
                foreach (var saleProduct in shoppingBasket)
                {
                    saleProduct.IsPaidByCreditCard = isBasketPaidByCreditCard;
                }
                this.isBasketPaidByCreditCard = isBasketPaidByCreditCard;

                OnPropertyChanged("IsBasketPaidByCreditCard");
            }
        }
        public void SetIsBasketPrivate(bool isBasketPrivate)
        {
            foreach (var saleProduct in shoppingBasket)
            {
                saleProduct.IsPrivate = isBasketPrivate;
                saleProduct.IsPaidByCreditCard = false;
            }
            this.isBasketPrivate = isBasketPrivate;
            this.isBasketPaidByCreditCard = false;

            OnPropertyChanged("IsBasketPaidByCreditCard");
            OnPropertyChanged("IsBasketPrivate");
        }
       
        private double GetBasketTotal()
        {
            double total = 0;
            foreach (var saleProduct in shoppingBasket)
            {
                total += saleProduct.Price;
            }
            return total;
        }


        public PrintDocument CreateDailyAccounting()
        {
            Printer printer = new Printer();
            return printer.CreateSalePrintDocument(SaleToday);
        }

        public void Action(Command command)
        {
            Type commandType = command.GetType();
            if(commandType ==  typeof(AddSaleProductCommand)) {
                AddSaleProductCommand addSaleProductCommand = command as AddSaleProductCommand;
                AddSaleProduct(addSaleProductCommand.SaleProduct);
                DecrementConsignment(addSaleProductCommand.ProductId);
            }
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

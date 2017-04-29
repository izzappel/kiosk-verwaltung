using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KioskVerwaltung.BusinessObjects
{
    public class Consignment
    {
        public int Id { get; set; }

        public int NumberOfContent { get; set; }
        public DateTime ExpirationDate { get; set; }
        public String ExpirationDateString 
        {
            get 
            { 
                if(ExpirationDate.Equals(DateTime.MinValue)) 
                {
                    return "kein Ablaufdatum";
                }
                return ExpirationDate.ToShortDateString(); 
            }
        }
        public double Price { get; set; }
        
        public Consignment()
        {
        }

        public Consignment(int id, int numberOfContent, DateTime expirationDate, double price)
        {
            this.Id = id;
            this.NumberOfContent = numberOfContent;
            this.ExpirationDate = expirationDate;
            this.Price = price;
        }

    }
}

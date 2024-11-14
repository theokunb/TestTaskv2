using System;
using System.Collections.Generic;

namespace TestTaskv2.Entity
{
    public class PurchaseData : BaseEntity
    {
        public string PurchaseNumber { get; set; }
        public string PurchaseObjectInfo { get; set; }
        public float Price { get; set; }
        public DateTime DocPublishDate { get; set; }
        public IList<Customer> Customers { get; set; }
    }
}

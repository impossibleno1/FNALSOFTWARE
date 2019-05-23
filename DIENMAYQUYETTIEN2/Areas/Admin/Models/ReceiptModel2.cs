using DIENMAYQUYETTIEN2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DIENMAYQUYETTIEN2.Areas.Admin.Models
{
    public class ReceiptModel2
    {
        public int ID { get; set; }
        public string BillCode { get; set; }
        public string CustomerID { get; set; }
        public string CustomerP { get; set; }
        public System.DateTime Date { get; set; }
        public string Shipper { get; set; }
        public string Note { get; set; }
        public string Method { get; set; }
        public int Period { get; set; }
        public int GrandTotal { get; set; }
        public int Taken { get; set; }
        public int Remain { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<InstallmentBillDetail> InstallmentBillDetails { get; set; }
    }
}
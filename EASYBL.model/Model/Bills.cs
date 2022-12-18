using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.model.Model
{
    public class Bills
    {
        [Key]
        public int Id { get; set; }

        public int BillNo { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhoneNumber { get; set; }

        public DateTime RecivedDate { get; set; }   

        public DateTime DeliveryDate { get; set; }

        public decimal Amount { get; set; }

        public int TotalQuantity { get; set; }
          
        public int UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.model.Model
{
    public class Items
    {
        [Key]
        public int Id { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemPrice { get; set; }
        public int BillNo { get; set; } 
        public string ItemName { get; set; }
        public int UserId { get; set; }
    }
}

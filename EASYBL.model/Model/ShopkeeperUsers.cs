using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.model.Model
{
    public class ShopkeeperUsers
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    

        public string Address { get; set; } 

        public string Email { get; set; }

        public string Number { get; set; }

        public string GstNo { get; set; } 
        public string Password { get; set; }

        public int CurrentBillNo { get; set; }
    }
}

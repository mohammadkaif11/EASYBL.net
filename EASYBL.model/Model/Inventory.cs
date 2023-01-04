﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.model.Model
{
    [Table("Inventory")]
    public class Inventory
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string Name { get; set; }   
        public decimal Price { get; set; }  
    }
}

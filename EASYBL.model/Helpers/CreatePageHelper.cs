using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.model.Helpers
{
    public class CreatePageHelper
    {
        public int billNo { get; set; }

        public string Name { get; set; }

        public string Address { get; set; } 

        public string Number { get; set; }      
        
        public List<string> Pages { get; set;}
    }
}

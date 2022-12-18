using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.model.Helpers
{
    public class BillResponseDto
    {
        public Bills BillObject { get; set; }

        public List<Items> ItemObject { get; set; } 
    }
}

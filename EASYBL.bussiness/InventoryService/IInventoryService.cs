using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.bussiness.InventoryService
{
    public interface IInventoryService
    {
        Inventory Add(Inventory inventory); 

        Inventory Update(Inventory inventory,int user_id);  

        Inventory Delete(int id,int user_id);  

        List<Inventory> Get(int userId);

        Inventory InventoryGetById(int id ,int userId);
    }
}

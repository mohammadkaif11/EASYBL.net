using EASYBL.data.IReposistoryBase;
using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.bussiness.InventoryService
{
    public class InventoryService : IInventoryService
    {
        private readonly IReposistoryBase<Inventory> _reposistory;
        public InventoryService(IReposistoryBase<Inventory> reposistory)
        {
            _reposistory = reposistory;
        }
        public Inventory Add(Inventory inventory)
        {
            _reposistory.Insert(inventory);
            return inventory;
        }

        public Inventory Delete(int id, int userid)
        {
            var _inventory = _reposistory.FindByCondition(x => x.Id == id && x.UserId == userid).FirstOrDefault();
            if (_inventory != null)
            {
                _reposistory.Delete(_inventory.Id);
            }

            return _inventory;
        }

        public List<Inventory> Get(int userId)
        {
            var _inventory = _reposistory.FindByCondition(x => x.UserId == userId).ToList();
            return _inventory;
        }

        public Inventory InventoryGetById(int id, int userId)
        {
            var _inventory = _reposistory.FindByCondition(x => x.Id == id && x.UserId == userId).FirstOrDefault();

            return _inventory;
        }

        public Inventory Update(Inventory inventory, int user_id)
        {
            var _inventory = _reposistory.FindByCondition(x => x.Id == inventory.Id && x.UserId == user_id).FirstOrDefault();
            if (_inventory != null)
            {
                _inventory.Price = inventory.Price;
                _inventory.Name = inventory.Name;
                _reposistory.Save();
            }

            return _inventory;
        }
    }
}

using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.bussiness.UserService
{
    public interface IUserService
    {
        ShopkeeperUsers Register(ShopkeeperUsers dto);
        ShopkeeperUsers IsRegistered(string Password,string Email);  
        bool IsEmailandName(string Email ,string Name);
        void UpdateBillNo(int UserId);
        ShopkeeperUsers GetById(int id);
    }
}

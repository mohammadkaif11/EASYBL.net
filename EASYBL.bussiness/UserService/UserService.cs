using EASYBL.data.IReposistoryBase;
using EASYBL.data.ReposistoryBase;
using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.bussiness.UserService
{
    public class UserService : IUserService
    {
        private readonly IReposistoryBase<ShopkeeperUsers> reposistoryBase;
        public UserService(IReposistoryBase<ShopkeeperUsers> _reposistoryBase) {
         reposistoryBase = _reposistoryBase;
        }

        public ShopkeeperUsers GetById(int id)
        {
           return reposistoryBase.GetById(id);    
        }

        public bool IsEmailandName(string Email, string Name)
        {
            var users = reposistoryBase.FindByCondition(x => x.Name == Name && x.Email == Email).FirstOrDefault();
            if (users != null)
            {
                return true;
            }
            return false;
        }

        public ShopkeeperUsers IsRegistered(string Password, string Email)
        {
           var users=reposistoryBase.FindByCondition(x=>x.Password== Password || x.Email==Email).FirstOrDefault();
            if(users!=null)
            {
                return users;
            }
            return null;
        }

        public ShopkeeperUsers Register(ShopkeeperUsers dto)
        {
           reposistoryBase.Insert(dto);
           return dto; 
        }

        public void UpdateBillNo(int UserId)
        {
            var User=reposistoryBase.GetById(UserId);
            if (User != null)
            {
                User.CurrentBillNo= User.CurrentBillNo+1;
                reposistoryBase.Update(User);            
            }
        }
    }
}

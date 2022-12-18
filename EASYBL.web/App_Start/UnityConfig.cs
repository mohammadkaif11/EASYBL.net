using EASYBL.bussiness.BillService;
using EASYBL.bussiness.UserService;
using EASYBL.data.IReposistoryBase;
using EASYBL.data.ReposistoryBase;
using EASYBL.model.Model;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace EASYBL.web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<IBillService, BillService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IReposistoryBase<Items>, ReposistoryBase<Items>>();
            container.RegisterType< IReposistoryBase<Bills>, ReposistoryBase<Bills>>();
            container.RegisterType<IReposistoryBase<ShopkeeperUsers>, ReposistoryBase<ShopkeeperUsers>>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
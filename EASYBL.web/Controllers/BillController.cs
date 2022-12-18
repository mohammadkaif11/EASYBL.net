using EASYBL.bussiness.BillService;
using EASYBL.bussiness.UserService;
using EASYBL.model.Helpers;
using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;

namespace EASYBL.web.Controllers
{
    [Authorize]
    public class BillController : Controller
    {
        private readonly IBillService billService;
        private readonly IUserService userService;
        public BillController(IBillService billService, IUserService userService)
        {
            this.billService = billService;
            this.userService = userService;
        }

        public ActionResult Index()
        {
            try
            {
                var a =  User.Identity.Name;
                var Id= Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("Login", "Account");
                }
                var User_data = userService.GetById(Id);
                if (User_data != null)
                {
                    CreatePageHelper createPageHelper = new CreatePageHelper()
                    {
                        Name= User_data.Name,   
                        Address= User_data.Address, 
                        billNo=User_data.CurrentBillNo,
                        Number=User_data.Number,    
                    };
                    return View(createPageHelper);
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult SendBill(Bills BillObjectDto, List<Items> ListObjectDto)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return Json("Provide Crendials", JsonRequestBehavior.AllowGet);
                }

                if (BillObjectDto.CustomerName == null || BillObjectDto.CustomerPhoneNumber == null || BillObjectDto.RecivedDate == null || BillObjectDto.DeliveryDate == null)
                {
                    return Json("Phone number and name are Required Field", JsonRequestBehavior.AllowGet);
                }
                else if (ListObjectDto.Count <= 0)
                {
                    return Json("Add atleast one item", JsonRequestBehavior.AllowGet);

                }
                foreach (var item in ListObjectDto)
                {
                    if (item.ItemName == null || item.ItemQuantity <= 0 || item.ItemPrice <= 0)
                    {
                        return Json("Add non empty items ", JsonRequestBehavior.AllowGet);
                    }
                }
                billService.CreateBill(BillObjectDto, ListObjectDto, Id);

                return Json("Entry Added SuccessFully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("Some Error Occured", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Data(int page)
        {
            try
            {

                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("Login", "Account");
                }

                if(page==0 || page == null)
                {
                    page = 1;
                }

                var BillData= billService.GetBills(Id, page).ToList();

                return View(BillData);
            }
            catch (Exception ex)
            {
                return View();
            }

        }
            
        //BillNo Get By id
        [Route("getById/{id}")]
        public ActionResult BillGetById(int id)
        {
           try
            {
                var a = User.Identity.Name;
                var User_id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("Login", "Account");
                }

                var BillData = billService.GetBillsByBillNo(id, User_id);

                return View(BillData);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        
        [HttpPost]
        public ActionResult UpdateBill(BillResponseDto billResponseDto)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToRoute(new { action = "BillGetById", controller = "Bill", id = 1 });
                }
                var billObjectResponse = billService.UpdateBills(billResponseDto, Id);
                return RedirectToRoute(new { action = "BillGetById", controller = "Bill", id = 1 });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");

            }

        }

        //Bill Delete ById
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var a = User.Identity.Name;
                var User_id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("Login", "Account");
                }
                billService.DeleteBill(id, User_id);

                return RedirectToRoute(new { action = "Data", controller = "Bill", page = 1 });
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult Filter(DateTime dateTime,string name,int billNo)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("Login", "Account");
                }
               
                var BillData = billService.FilterData(dateTime, name,billNo,Id).ToList();

                return View(BillData);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
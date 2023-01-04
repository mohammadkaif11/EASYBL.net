using EASYBL.bussiness.BillService;
using EASYBL.bussiness.InventoryService;
using EASYBL.bussiness.UserService;
using EASYBL.model.Helpers;
using EASYBL.model.Model;
using Microsoft.Ajax.Utilities;
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
        private readonly IInventoryService inventoryService;
        public BillController(IInventoryService inventoryService, IBillService billService, IUserService userService)
        {
            this.billService = billService;
            this.userService = userService;
            this.inventoryService = inventoryService;
        }

        public ActionResult Index()
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }
                var User_data = userService.GetById(Id);
                var inventoryItem = inventoryService.Get(Id);
                if (User_data != null)
                {
                    CreatePageHelper createPageHelper = new CreatePageHelper()
                    {
                        Name = User_data.Name,
                        Address = User_data.Address,
                        billNo = User_data.CurrentBillNo,
                        Number = User_data.Number,
                    };
                    ViewBag.data = inventoryItem;
                    return View(createPageHelper);
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
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
                else if (ListObjectDto == null)
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
                    return RedirectToAction("ErrorPage", "Error");
                }

                if (page == 0 || page == null)
                {
                    page = 1;
                }

                var BillData = billService.GetBills(Id, page).ToList();
                if (BillData == null)
                {
                    return RedirectToAction("Index", "Main");

                }

                return View(BillData);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
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
                    return RedirectToAction("ErrorPage", "Error");
                }

                var BillData = billService.GetBillsByBillNo(id, User_id);
                if (BillData == null)
                {
                    return RedirectToAction("Index", "Main");
                }

                return View(BillData);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
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
                    return RedirectToAction("ErrorPage", "Error");
                }
                var billObjectResponse = billService.UpdateBills(billResponseDto, Id);
                if (billObjectResponse == null)
                {
                    return RedirectToAction("Index", "Main");

                }
                return RedirectToRoute(new { action = "BillGetById", controller = "Bill", id = billResponseDto.BillObject.BillNo });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");

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
                    return RedirectToAction("ErrorPage", "Error");
                }
                billService.DeleteBill(id, User_id);

                return RedirectToRoute(new { action = "Data", controller = "Bill", page = 1 });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public ActionResult Filter()
        {
            try
            {
                var a = User.Identity.Name;
                var User_id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        [HttpPost]
        public ActionResult Filter(FilterObject filterObject)
        {
            try
            {
                var a = User.Identity.Name;
                var User_id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }
                var ObjectResponse = billService.FilterData(filterObject, User_id);
                if (ObjectResponse != null)
                {
                    return View(ObjectResponse);
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        [HttpGet]
        public JsonResult MonthReview()
        {
            try
            {
                var a = User.Identity.Name;
                var User_id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return Json("Some Error Occured", JsonRequestBehavior.AllowGet);
                }
                var response = billService.MonthReview(User_id);

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Some Error Occured", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
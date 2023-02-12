using EASYBL.bussiness.BillService;
using EASYBL.bussiness.InventoryService;
using EASYBL.bussiness.UserService;
using EASYBL.model.Helpers;
using EASYBL.model.Model;
using Mailjet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using Mailjet.Client.Resources;
using Mailjet.Client.Resources.SMS;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Send = Mailjet.Client.Resources.Send;

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
        public async Task<JsonResult> SendBill(Bills BillObjectDto, List<Items> ListObjectDto,bool isEmail)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                var user=userService.GetById(Id);   
                if (string.IsNullOrEmpty(a))
                {
                    return Json("Provide Crendials", JsonRequestBehavior.AllowGet);
                }


                if (BillObjectDto.CustomerName == null || BillObjectDto.CustomerPhoneNumber == null || BillObjectDto.RecivedDate == null || BillObjectDto.DeliveryDate == null)
                {
                    return Json("Phone number and name are Required Field", JsonRequestBehavior.AllowGet);
                }
                if (!BillObjectDto.CustomerPhoneNumber.Contains("@") && isEmail==true)
                {
                    return Json("Enter valid Email", JsonRequestBehavior.AllowGet);
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
                var SendingTemplate = GenrateHtml(BillObjectDto, ListObjectDto);
                if(isEmail==true) {
                    var result = await sendEmail(user.Email, user.Name, BillObjectDto.CustomerName, BillObjectDto.CustomerPhoneNumber, SendingTemplate);
                    if (result >= 1)
                    {
                        return Json("Entry added successFully and sended to user with email", JsonRequestBehavior.AllowGet);
                    }
                    return Json("Entry added successFully and not sended to user with email", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = await sendSms(user.Name, BillObjectDto.CustomerName, BillObjectDto.CustomerPhoneNumber, SendingTemplate);
                    if (data >= 1)
                    {
                        return Json("Entry added successFully and sended to user with phoneNumber", JsonRequestBehavior.AllowGet);
                    }
                    return Json("Entry added successFully and not sended to user with phoneNumber", JsonRequestBehavior.AllowGet);

                }
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

        //Send Email function
        public static async Task<int> sendEmail(string shopemail,string shopName, string customerName, string customerEmail, string data)
        {
            MailjetClient client = new MailjetClient("1f6af4c21948d6b3983096f764365223", "7c7706bf24fbf949951af9336ada75cd");
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, shopemail)
               .Property(Send.FromName,shopName)
               .Property(Send.Subject, "Bill")
               .Property(Send.TextPart, "Thanks for visting hear")
               .Property(Send.HtmlPart, data)
               .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", customerEmail},
                 {"Name" ,customerName}
                          }
                   });
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
   
        //Genrate email Template
        public static string GenrateHtml(Bills BillObjectDto, List<Items> ListObjectDto)
        {

            var result = "<h3>Bill No -" + BillObjectDto.BillNo +" : "+"Date -"+ BillObjectDto.RecivedDate+ "</h3>";
            result += "<h3>Customer Name - " + BillObjectDto.CustomerName + "</h3>";
            result += "<h3>Customer Email - " + BillObjectDto.CustomerPhoneNumber + "</h3>";
            result += "<ul>";
            result += "<li>Name : Price : Quantity</li>";

            foreach (var item in ListObjectDto)
            {
                result += "<li>"+item.ItemName+" : "+item.ItemPrice+" : "+item.ItemQuantity+"</li>";
            }
            result += "</ul>";
            result += "<h3>TotalAmount - " + BillObjectDto.Amount + "</h3>";
            result += "<h3>TotalQuantity - " + BillObjectDto.TotalQuantity + "</h3>";
            result += "</br>";
            result += "<h3>Thanks for visting</h3>";

            return result;
        }

        //Send sms function
        public static async Task<int> sendSms(string shopName, string customerName, string customerPhone, string data)
        {
            MailjetClient client = new MailjetClient("a7846fd1c7134e128c666ec6b9aa40d3");

            MailjetRequest request = new MailjetRequest
            {
                Resource = SMS.Resource,
            }
            .Property(SMS.From, shopName)
            .Property(SMS.To,customerPhone)
            .Property(SMS.Text, data);

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
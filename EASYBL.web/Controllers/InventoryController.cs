using EASYBL.bussiness.InventoryService;
using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EASYBL.web.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly IInventoryService inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }
        // GET: Inventory
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
                var inventoryList = inventoryService.Get(Id);

                return View(inventoryList);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }
                var inventoryList = inventoryService.InventoryGetById(id, Id);
                return View(inventoryList);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
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

        // POST: Inventory/Create
        [HttpPost]
        public ActionResult Create(Inventory inventoryDto)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }
                inventoryDto.UserId = Id;
                inventoryService.Add(inventoryDto);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }
                var inventoryList = inventoryService.InventoryGetById(id, Id);
                if (inventoryList == null)
                {
                    return RedirectToAction("Index");
                }
                return View(inventoryList);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Inventory inventoryDto)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }
                var inventoryList = inventoryService.Update(inventoryDto, Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }
                var inventoryList = inventoryService.InventoryGetById(id, Id);
                if (inventoryList == null)
                {
                    return RedirectToAction("Index");
                }
                return View(inventoryList);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Inventory inventoryDto)
        {
            try
            {
                var a = User.Identity.Name;
                var Id = Int32.Parse(a);
                if (string.IsNullOrEmpty(a))
                {
                    return RedirectToAction("ErrorPage", "Error");
                }

                var inventoryList = inventoryService.Delete(inventoryDto.Id, Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }
    }
}

using EASYBL.bussiness.UserService;
using EASYBL.data.IReposistoryBase;
using EASYBL.model.Helpers;
using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.bussiness.BillService
{
    public class BillService : IBillService
    {
        private readonly IReposistoryBase<Bills> billRepo;
        private readonly IReposistoryBase<Items> itemsRepo;
        private readonly IUserService userService;
        public BillService(IReposistoryBase<Items> itemsRepo, IReposistoryBase<Bills> _billRepo, IUserService userService)
        {
            this.billRepo = _billRepo;
            this.userService = userService;
            this.itemsRepo = itemsRepo;
        }

        public void CreateBill(Bills BillDto, List<Items> itemDto, int id)
        {
            if (BillDto != null || itemDto != null)
            {

                var User_data = userService.GetById(id);
                decimal totalPrice = 0;
                var totalQuantity = 0;
                BillDto.BillNo = User_data.CurrentBillNo;
                BillDto.UserId = id;

                if (itemDto != null)
                {
                    foreach (var item in itemDto)
                    {
                        totalPrice += item.ItemPrice * item.ItemQuantity;
                        totalQuantity += item.ItemQuantity;
                        item.BillNo = User_data.CurrentBillNo;
                        item.UserId = id;
                    }
                }
                BillDto.DeliveryDate = DateTime.Now.Date;
                BillDto.RecivedDate = DateTime.Now.Date;
                //Insert Items
                BillDto.TotalQuantity = totalQuantity;
                BillDto.Amount = totalPrice;

                //Insert BillData
                billRepo.Insert(BillDto);
                if (itemDto != null)
                {
                    itemsRepo.InsertRange(itemDto);
                }

                //Update BillNo
                userService.UpdateBillNo(id);
            }
        }

        public void DeleteBill(int BillNo, int UserId)
        {
            var BillObject = billRepo.FindByCondition(x => x.BillNo == BillNo && x.UserId == UserId).FirstOrDefault();
            var Items = itemsRepo.FindByCondition(x => x.UserId == UserId && x.BillNo == BillNo).ToList();
            if (BillObject != null)
            {
                billRepo.Delete(BillObject.Id);
            }
            if (Items != null)
            {
                itemsRepo.DeleteRange(Items);
            }
        }

        public IQueryable<BillResponseDto> FilterData(FilterObject filterObject, int userId)
        {
            List<BillResponseDto> result = new List<BillResponseDto>();
            var Bills = billRepo.FindByCondition(x => x.UserId == userId).ToList();
            var items = itemsRepo.FindByCondition(x => x.UserId == userId).ToList();

            if (filterObject.date != null)
            {
                Bills = Bills.Where(x => x.RecivedDate.Date.Equals(filterObject.date)).ToList();
            }
            if (filterObject.name != null)
            {
                Bills = Bills.Where(x => x.CustomerName.Equals(filterObject.name)).ToList();
            }
            if (filterObject.billNo != null)
            {
                Bills = Bills.Where(x => x.BillNo.Equals(filterObject.billNo)).ToList();
            }

            foreach (var bill in Bills)
            {
                BillResponseDto obj = new BillResponseDto();
                var item = items.Where(x => x.BillNo == bill.BillNo).ToList();
                if (item != null)
                {
                    decimal totalPrice = 0;
                    var TotalItem = item.Sum(x => x.ItemQuantity);
                    foreach (var data in items)
                    {
                        totalPrice += data.ItemQuantity * data.ItemPrice;
                    }
                    bill.TotalQuantity = TotalItem;
                    bill.Amount = totalPrice;
                    bill.RecivedDate = bill.RecivedDate.Date;
                    obj.BillObject = bill;
                    obj.ItemObject = item;

                    result.Add(obj);
                }
            }

            return result.AsQueryable();

        }

        public IQueryable<BillResponseDto> GetBills(int UserId, int PageNo)
        {
            List<BillResponseDto> result = new List<BillResponseDto>();

            var Bills = billRepo.FindByCondition(x => x.UserId == UserId).ToList().OrderByDescending(x => x.RecivedDate).ThenByDescending(x => x.Id);
            var Items = itemsRepo.FindByCondition(x => x.UserId == UserId).ToList();
            var TotaPage = Bills.Count() / 2;
            if (PageNo <= 0)
            {
                PageNo = 1;
            }
            if (TotaPage > 0 && TotaPage <= 15)
            {
                TotaPage = 1;
            }

            if (Bills.Count() % 15 != 0)
            {
                TotaPage = TotaPage + 1;
            }
            if (PageNo > TotaPage)
            {
                PageNo = TotaPage;
            }
            var billsData = Bills.Skip((PageNo - 1) * 10).Take(10).ToList();

            foreach (var bill in billsData)
            {
                BillResponseDto obj = new BillResponseDto();
                var item = Items.Where(x => x.BillNo == bill.BillNo).ToList();
                decimal totalPrice = 0;
                var TotalItem = item.Sum(x => x.ItemQuantity);
                foreach (var data in item)
                {
                    totalPrice += data.ItemQuantity * data.ItemPrice;
                }
                bill.TotalQuantity = TotalItem;
                bill.Amount = totalPrice;
                bill.RecivedDate = bill.RecivedDate.Date;
                obj.BillObject = bill;
                obj.ItemObject = item;

                result.Add(obj);
            }

            return result.AsQueryable();
        }

        public BillResponseDto GetBillsByBillNo(int BillNo, int UserId)
        {
            BillResponseDto result = new BillResponseDto();

            var Bill = billRepo.FindByCondition(x => x.BillNo == BillNo && x.UserId == UserId).FirstOrDefault();
            var Items = itemsRepo.FindByCondition(x => x.BillNo == BillNo && x.UserId == UserId).ToList();

            decimal totalPrice = 0;
            var TotalItem = Items.Sum(x => x.ItemQuantity);
            foreach (var data in Items)
            {
                totalPrice += data.ItemQuantity * data.ItemPrice;
            }

            Bill.TotalQuantity = TotalItem;
            Bill.Amount = totalPrice;
            result.BillObject = Bill;
            result.ItemObject = Items;

            return result;
        }

        public BillResponseDto GetBillsById(int Id, int UserId)
        {
            BillResponseDto result = new BillResponseDto();

            var Bill = billRepo.FindByCondition(x => x.Id == Id && x.UserId == UserId).FirstOrDefault();
            var Items = itemsRepo.FindByCondition(x => x.BillNo == Bill.BillNo && x.UserId == UserId).ToList();

            var TotalItem = 0;
            decimal totalPrice = 0;
            if (Items != null)
            {
                TotalItem = Items.Sum(x => x.ItemQuantity);
                foreach (var data in Items)
                {
                    totalPrice += data.ItemQuantity * data.ItemPrice;
                }
            }

            if (Bill != null)
            {
                Bill.TotalQuantity = TotalItem;
                Bill.Amount = totalPrice;
                result.BillObject = Bill;
                result.ItemObject = Items;

            }

            return result;
        }

        //algorithm for data analysis
        public decimal[] MonthReview(int UserId)
        {
            var AllBills=billRepo.FindByCondition(x=>x.UserId== UserId).ToList();
            var Month = DateTime.Now.Month;
            var SizeofArray = DateTime.Now.Day;
            var bills = AllBills.Where(x => x.RecivedDate.Month == Month).OrderBy(x => x.RecivedDate.Date).ToList();
            decimal[] result = new decimal[SizeofArray+1];
            if (bills.Count > 0)
            {
                var currentdate = bills[0].RecivedDate.Date;
                decimal amount = 0;
                foreach (var item in bills)
                {
                    if (currentdate.Date != item.RecivedDate.Date)
                    {
                        int index = currentdate.Date.Day;
                        result[index] = amount;
                        amount = 0;
                        amount += item.Amount;
                        currentdate = item.RecivedDate.Date;
                    }
                    else
                    {
                        amount += item.Amount;
                        currentdate=item.RecivedDate.Date;  
                    }
                }
                if (amount != 0)
                {
                    int index = currentdate.Date.Day;
                    result[index] = amount;
                }
            }


            return result;
        }

        public BillResponseDto UpdateBills(BillResponseDto billResponseDto, int UserId)
        {
            var BillObject = billRepo.FindByCondition(x => x.Id == billResponseDto.BillObject.Id && x.UserId == UserId && x.BillNo == billResponseDto.BillObject.BillNo).FirstOrDefault();
            var items = itemsRepo.FindByCondition(x => x.UserId == UserId && x.BillNo == BillObject.BillNo).ToList();

            if (items != null)
            {
                //deleteRange
                itemsRepo.DeleteRange(items);
            }

            decimal totalPrice = 0;
            var totalQuantity = 0;


            if (items != null)
            {
                foreach (var item in billResponseDto.ItemObject)
                {
                    totalPrice += item.ItemPrice * item.ItemQuantity;
                    totalQuantity += item.ItemQuantity;
                    item.BillNo = BillObject.BillNo;
                    item.UserId = UserId;
                }
                BillObject.CustomerName = billResponseDto.BillObject.CustomerName;
                BillObject.CustomerPhoneNumber = billResponseDto.BillObject.CustomerPhoneNumber;
                BillObject.DeliveryDate = billResponseDto.BillObject.RecivedDate;
                BillObject.RecivedDate = billResponseDto.BillObject.RecivedDate;
            }

            //Insert Items
            BillObject.TotalQuantity = totalQuantity;
            BillObject.Amount = totalPrice;

            //Insert BillData
            billRepo.Update(BillObject);
            if (billResponseDto.ItemObject.Count > 0)
            {
                itemsRepo.InsertRange(billResponseDto.ItemObject);
            }

            return billResponseDto;
        }

    }
}

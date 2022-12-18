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
        public BillService(IReposistoryBase<Items> itemsRepo,IReposistoryBase<Bills> _billRepo, IUserService userService)
        {
            this.billRepo = _billRepo;
            this.userService = userService;
            this.itemsRepo= itemsRepo;
        }

        public void CreateBill(Bills BillDto, List<Items> itemDto, int id)
        {
            var User_data=userService.GetById(id);
            decimal totalPrice = 0;
            var totalQuantity = 0;
            BillDto.BillNo = User_data.CurrentBillNo;
            BillDto.UserId = id;

            foreach (var item in itemDto)
            {
                totalPrice += item.ItemPrice * item.ItemQuantity;
                totalQuantity += item.ItemQuantity;
                item.BillNo= User_data.CurrentBillNo;   
                item.UserId= id;  
            }
            //Insert Items
            BillDto.TotalQuantity = totalQuantity;
            BillDto.Amount= totalPrice;

            //Insert BillData
            billRepo.Insert(BillDto);
            itemsRepo.InsertRange(itemDto);

            //Update BillNo
            userService.UpdateBillNo(id);
        }

        public IQueryable<BillResponseDto> GetBills(int UserId,int PageNo)
        {
            List<BillResponseDto> result= new List<BillResponseDto>();

            var Bills = billRepo.FindByCondition(x => x.UserId == UserId).ToList().OrderByDescending(x=>x.RecivedDate).ThenBy(x=>x.Id);
            var Items = itemsRepo.FindByCondition(x => x.UserId == UserId).ToList();
            var TotaPage = Bills.Count() / 2;
            if (PageNo <= 0)
            {
                PageNo = 1;
            }
            if (TotaPage > 0 && TotaPage<=15)
            {
                TotaPage = 1;
            }

            if (Bills.Count() % 15 != 0)
            {
                TotaPage=TotaPage + 1;
            }
            if(PageNo > TotaPage)
            {
                PageNo=TotaPage;   
            }
            var billsData=Bills.Skip((PageNo-1)*2).Take(2).ToList(); 

            foreach (var bill in billsData)
            {
               BillResponseDto obj= new BillResponseDto();  
               var item=Items.Where(x=>x.BillNo==bill.BillNo).ToList();
               decimal totalPrice = 0;
               var TotalItem=item.Sum(x=>x.ItemQuantity);
                foreach (var data in item)
                {
                    totalPrice += data.ItemQuantity * data.ItemPrice;
                }
                bill.TotalQuantity = TotalItem;
                bill.Amount= totalPrice;
                bill.RecivedDate = bill.RecivedDate.Date;
                obj.BillObject = bill;
                obj.ItemObject=item;

                result.Add(obj);
            }

            return result.AsQueryable();
        }

        public BillResponseDto GetBillsByBillNo(int BillNo,int UserId)
        {
            BillResponseDto result = new BillResponseDto();

            var Bill = billRepo.FindByCondition(x => x.BillNo == BillNo && x.UserId==UserId).FirstOrDefault();
            var Items = itemsRepo.FindByCondition(x => x.BillNo == BillNo && x.UserId==UserId).ToList();

            
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

        public BillResponseDto UpdateBills(BillResponseDto billResponseDto)
        {
            throw new NotImplementedException();
        }
    }
}

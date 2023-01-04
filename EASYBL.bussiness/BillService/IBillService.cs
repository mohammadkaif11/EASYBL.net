using EASYBL.model.Helpers;
using EASYBL.model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.bussiness.BillService
{
    public interface IBillService
    {
        void CreateBill(Bills BillDto,List<Items> itemDto,int id);

        IQueryable<BillResponseDto> GetBills(int UserId,int pageNo);

        BillResponseDto GetBillsById(int BillId,int UserId);

        BillResponseDto GetBillsByBillNo(int BillId, int UserId);

        BillResponseDto UpdateBills(BillResponseDto billResponseDto,int UserId);

        void DeleteBill(int BillNo,int UserId);

        IQueryable<BillResponseDto> FilterData(FilterObject filterObject,int userId);

        decimal[] MonthReview(int UserId);
    }
}

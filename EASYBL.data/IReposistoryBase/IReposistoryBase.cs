using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.data.IReposistoryBase
{
    public interface IReposistoryBase<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void InsertRange(IEnumerable<T> obj);    
        void Update(T obj);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> obj);    
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);

    }

}

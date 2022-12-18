using EASYBL.data.DataContext;
using EASYBL.data.IReposistoryBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EASYBL.data.ReposistoryBase
{
    public class ReposistoryBase<T> : IReposistoryBase<T> where T : class
    {
        private ApplicationDataContext _context = null;
        private DbSet<T> table = null;

        public ReposistoryBase()
        {
            this._context = new ApplicationDataContext();
            table = _context.Set<T>();
        }

        public ReposistoryBase(ApplicationDataContext _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return table.Where(expression).ToList();
        }

        public void InsertRange(IEnumerable<T> obj)
        {
           table.AddRange(obj);
            _context.SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> obj)
        {
            table.RemoveRange(obj);
            _context.SaveChanges(); 
        }
    }
}

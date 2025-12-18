using System;
using Common.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Services
{
    public class BaseService<T> where T : BaseEntity
    {
        private AppDbContext db { get; set; }
        private DbSet<T> Items { get; set; }

        public BaseService()
        {
            db = new AppDbContext();
            Items = db.Set<T>();
        }

        public int Count(Expression<Func<T, bool>> filter = null)
        {
            var query = Items.AsQueryable();
            if (filter != null)
                query = query.Where(filter);

            return query.Count();
        }

        /*public List<T> GetAll() 
        {
            return Items.ToList();
        }*/

        public List<T> GetAll(Expression<Func<T, bool>> filter = null, string orderBy = null, bool sortAsc = false, int page = 1, int pageSize = int.MaxValue)
        {
            var query = Items.AsQueryable();
            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (sortAsc)
                    query = query.OrderBy(e => EF.Property<object>(e, orderBy));
                else
                    query = query.OrderByDescending(e => EF.Property<object>(e, orderBy));
            }

            query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

            return query.ToList();
        }

        public T GetById(int id) 
        {
            return  Items.FirstOrDefault(x => x.Id == id);
        }

        public void Save(T item) 
        {
            if (item.Id > 0)
                Items.Update(item);
            else
                Items.Add(item);

            db.SaveChanges();
        }

        public void Delete(T item) 
        {
            Items.Remove(item);
            db.SaveChanges();
        }
    }
}

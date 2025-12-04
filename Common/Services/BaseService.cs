using System;
using Common.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public List<T> GetAll() 
        {
            return Items.ToList();
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

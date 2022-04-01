using MyNotes.CoreLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyNotes.EntityLayer;

namespace MyNotes.DataAccessLayer
{
    public class Repository<T> : BaseRepository, IRepository<T> where T:class
    {

        private readonly MyNotesContext _db;
        private DbSet<T> objSet;

        public Repository()
        {
            _db = BaseRepository.CreateContext();
            objSet = _db.Set<T>();
        }

        public int Delete(T entity)
        {
            objSet.Remove(entity);
            return Save();
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return objSet.FirstOrDefault(predicate);
        }

        public int Insert(T entity)
        {
            objSet.Add(entity);
            if (entity is BaseEntity o) //Alttaki ile aynı şeyi kısa yoldan yapıyor
            {
                DateTime now=DateTime.Now;
                //BaseEntity o = entity as BaseEntity; //T nesnesinde sadece BaseEntity değerlerine ulaş
                o.ModifiedUserName = "system";
                o.ModifiedOn= now;
                o.CreatedOn=now;
                //o.ModifiedOn= DateTime.Now;
                //o.CreatedOn=DateTime.Now;
                //Milisaniyelik farkı 0'a indirdik bu işlem için değil ama hassas işler için
            }

            return Save();
        }

        public List<T> List() //Bir çok programda list yerine GetAll Kulanılır 
        {
            return objSet.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> predicate)
        {
            return objSet.Where(predicate).ToList();
        }

        public IQueryable<T> QList()
        {
            return objSet.AsQueryable();
        }

        public int Save()
        {
           return _db.SaveChanges();
        }

        public int Update(T entity)
        {
            if (entity is BaseEntity o)
            {
                o.ModifiedUserName = "system";
                o.ModifiedOn= DateTime.Now;
            }

            return Save();
        }
    }
}

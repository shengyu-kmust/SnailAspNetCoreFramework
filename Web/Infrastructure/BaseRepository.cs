using AutoMapper;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApplicationCore.Enum;
namespace Web.Infrastructure
{
    public class BaseRepository<T> where T:BaseEntity
    {
        public DatabaseContext db;

        public BaseRepository(DatabaseContext db)
        {
            this.db = db;
        }
        public virtual List<T> AllValid()
        {
            return db.Set<T>().Where(a => a.IsValid == (int) ValidOrNot.Valid).ToList();
        }

        public virtual List<T> All()
        {
            return db.Set<T>().ToList();
        }

        public virtual T FirstOrDefault(Expression<Func<T,bool>> expression)
        {
            return db.Set<T>().FirstOrDefault(expression);
        }

        public virtual List<T> Where(Expression<Func<T, bool>> expression)
        {
            return db.Set<T>().Where(expression).ToList();
        }
        public virtual void Add(T entity)
        {
            db.Set<T>().Add(entity);
            db.SaveChanges();
        }
        public virtual void SetInvalid(int id)
        {
            var entity=db.Set<T>().FirstOrDefault(a => a.Id == id);
            if (entity != null)
            {
                entity.IsValid = (int) ValidOrNot.Invalid;
                db.SaveChanges();
            }
        }

        public virtual void Delete(int id)
        {
            var entity=db.Set<T>().FirstOrDefault(a => a.Id == id);
            if (entity != null)
            {
                db.Set<T>().Remove(entity);
                db.SaveChanges();
            }
        }

        public virtual void Update(object dto,int id)
        {
            var entity = db.Set<T>().FirstOrDefault(a => a.Id == id);
            if (entity != null)
            {
                Mapper.Map(dto, entity);
            }
            db.SaveChanges();
        }
    }
}

// Decompiled with JetBrains decompiler
// Type: CSDB.DataAccess.Implementation.DbRepository`1
// Assembly: CSDB.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2DB40F69-A4CC-40F0-82B0-03FE7F8C67C3
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.DataAccess.dll

using CSDB.DataAccess.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CSDB.DataAccess.Implementation
{
  public class DbRepository<T> : IRepository<T> where T : class
  {
    protected Microsoft.EntityFrameworkCore.DbSet<T> DbSet;
    protected DbContext context;

    public DbRepository(DbContext dataContext)
    {
      this.context = dataContext;
      this.DbSet = dataContext.Set<T>();
    }

    public DbRepository(Microsoft.EntityFrameworkCore.DbSet<T> dbSet) => this.DbSet = dbSet;

    public void Insert(T entity)
    {
      this.DbSet.Add(entity);
      this.context.Entry<T>(entity).State = EntityState.Added;
      this.context.SaveChanges();
    }

    public void Delete(T entity)
    {
      this.DbSet.Attach(entity);
      this.context.Entry<T>(entity).State = EntityState.Deleted;
      this.DbSet.Remove(entity);
      this.context.Entry<T>(entity).State = EntityState.Deleted;
      this.context.SaveChanges();
    }

    public IEnumerable<T> SearchFor(Expression<Func<T, bool>> predicate) => (IEnumerable<T>) this.DbSet.Where<T>(predicate);

    public IEnumerable<T> GetAll() => (IEnumerable<T>) this.DbSet;

    public T GetById(int id) => this.context.Set<T>().Find((object) id);

    public T GetById(string id) => this.DbSet.Find((object) id);

    public bool Update(T entity)
    {
      EntityEntry<T> entityEntry = this.context.Entry<T>(entity);
      object primaryKey = this.GetPrimaryKey(entity);
      if (entityEntry.State != EntityState.Detached)
        return false;
      T entity1 = this.DbSet.Find(primaryKey);
      if ((object) entity1 != null)
      {
        this.context.Entry<T>(entity1).CurrentValues.SetValues((object) entity);
      }
      else
      {
        this.DbSet.Attach(entity);
        entityEntry.State = EntityState.Modified;
      }
      this.context.SaveChanges();
      return true;
    }

    public bool BulkUpdate(IEnumerable<T> entList)
    {
      PropertyInfo property = typeof (T).GetProperty("ObjectState");
      foreach (T ent in entList)
      {
        object obj = property.GetValue((object) ent, (object[]) null);
        switch (obj == null ? 0 : (int) obj)
        {
          case 1:
            this.Insert(ent);
            continue;
          case 2:
            this.Update(ent);
            continue;
          case 3:
            this.Delete(ent);
            continue;
          default:
            continue;
        }
      }
      return true;
    }

    public bool BulkUpdateList(List<T> entList)
    {
      PropertyInfo property = typeof (T).GetProperty("ObjectState");
      foreach (T ent in entList)
      {
        object obj = property.GetValue((object) ent, (object[]) null);
        switch (obj == null ? 0 : (int) obj)
        {
          case 1:
            this.Insert(ent);
            continue;
          case 2:
            this.Update(ent);
            continue;
          case 3:
            this.Delete(ent);
            continue;
          default:
            continue;
        }
      }
      return true;
    }

    public IQueryable<T> AsQueryable() => throw new NotImplementedException();

    public T Single(Expression<Func<T, bool>> predicate) => throw new NotImplementedException();

    public T SingleOrDefault(Expression<Func<T, bool>> predicate) => throw new NotImplementedException();

    public T First(Expression<Func<T, bool>> predicate) => throw new NotImplementedException();

    public T FirstOrDefault() => this.DbSet.First<T>();

    public T FirstOrDefault(Expression<Func<T, bool>> predicate) => (T) this.DbSet.Where<T>(predicate);

    public void Attach(T entity) => throw new NotImplementedException();

    private object GetPrimaryKey(T entry) => ((IEnumerable<PropertyInfo>) entry.GetType().GetProperties()).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (prop => Attribute.IsDefined((MemberInfo) prop, typeof (KeyAttribute)))).GetValue((object) entry, (object[]) null);
  }
}

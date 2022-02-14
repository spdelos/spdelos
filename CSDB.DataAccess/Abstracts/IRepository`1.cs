// Decompiled with JetBrains decompiler
// Type: CSDB.DataAccess.Abstracts.IRepository`1
// Assembly: CSDB.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2DB40F69-A4CC-40F0-82B0-03FE7F8C67C3
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.DataAccess.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSDB.DataAccess.Abstracts
{
  public interface IRepository<T> where T : class
  {
    IQueryable<T> AsQueryable();

    IEnumerable<T> GetAll();

    IEnumerable<T> SearchFor(Expression<Func<T, bool>> predicate);

    T Single(Expression<Func<T, bool>> predicate);

    T SingleOrDefault(Expression<Func<T, bool>> predicate);

    T First(Expression<Func<T, bool>> predicate);

    T FirstOrDefault();

    T FirstOrDefault(Expression<Func<T, bool>> predicate);

    T GetById(int id);

    T GetById(string id);

    void Insert(T entity);

    bool Update(T entity);

    bool BulkUpdate(IEnumerable<T> entList);

    void Delete(T entity);

    void Attach(T entity);
  }
}
